using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;
using System.Configuration;
using System.Diagnostics;
using GVUZ.ImportService2016.Core.Main.Extensions;

namespace GVUZ.ImportService2016.Core.Main
{
    public class ProcessingManager
    {
        protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static Thread[] _checkingThreads;
        private static Thread[] _importingThreads;
        private static readonly ManualResetEvent _workEventP = new ManualResetEvent(false);
        private static readonly AutoResetEvent _workEventC = new AutoResetEvent(false);
        private static bool _isStopping = false;
        public static void Start()
        {
            //если вызвали второй раз, то делаем вначале стоп
            if (_checkingThreads != null)
                Stop();

            // сбрасываем все статусы, которые были в процессинге, но почему-то не завершились
            Repositories.ADOPackageRepository.ResetImportPackages();

            // TODO: Загружать статичные справончики (НСИ++)
            Repositories.ADOPackageRepository.GetStaticVocabularies();

            _isStopping = false;

            int thread_wait_time = 2000;
            string twts = System.Configuration.ConfigurationManager.AppSettings["ThreadWaitTime"].ToString();
            Console.WriteLine("ThreadWaitTime = {0}", twts);
            int.TryParse(twts, out thread_wait_time);
            Thread.Sleep(thread_wait_time); //ждём, пока проснётся первый тред, чтобы одновременно в базу не ломиться
            var count = System.Configuration.ConfigurationManager.AppSettings["ImportProcessorThreadcount"];
            int importThreadCount;
            if (!int.TryParse(count, out importThreadCount))
                importThreadCount = 8;

            count = System.Configuration.ConfigurationManager.AppSettings["CheckProcessorThreadcount"];
            int checkThreadCount;
            if (!int.TryParse(count, out checkThreadCount))
                checkThreadCount = 8;

            Log.InfoFormat("Установлено потоков на импорт пакетов: [{0}], на проверку пакетов: [{1}]", importThreadCount, checkThreadCount);

            _importingThreads = new Thread[importThreadCount];
            for (var i = 0; i < importThreadCount; i++)
            {
                Thread.Sleep(thread_wait_time); //ждём, пока проснётся первый тред, чтобы одновременно в базу не ломиться
                _importingThreads[i] = new Thread(ImportApplicationsJob);
                _importingThreads[i].Start();
            }

            _checkingThreads = new Thread[checkThreadCount];
            for (var i = 0; i < checkThreadCount; i++)
            {
                Thread.Sleep(thread_wait_time); //ждём, пока проснётся первый тред, чтобы одновременно в базу не ломиться
                _checkingThreads[i] = new Thread(CheckApplicationsJob);
                _checkingThreads[i].Start();
            }
        }

        // Roman.N.Bukin - тестирование и отладка процедур сервиса из WinForms Application
        public static void StartWinForms(int packageID, bool deleteBulk)
        {
            // сбрасываем все статусы, которые были в процессинге, но почему-то не завершились
             //GVUZ.ImportService2016.Core.Main.Repositories.ADOPackageRepository.ResetImportPackages();

            // Загружать статичные справончики (НСИ++)
            Repositories.ADOPackageRepository.GetStaticVocabularies();


            DateTime dt2 = DateTime.Now;
            try
            {
                int institutionId = 0;

                PackageManager.TryProcessNextPackage(ref institutionId, packageID, deleteBulk);
                DateTime dt3 = DateTime.Now;
                Debug.WriteLine("TryProcessNextPackage: " + dt3.Subtract(dt2).ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Package process error: " + ex);
                throw (ex);
            }
        }

        public static void CheckWinForms(int packageID)
        {
            DateTime dt2 = DateTime.Now;
            try
            {
                int institutionId = 0;

                PackageManager.TryCheckNextPackage(ref institutionId, packageID);
                DateTime dt3 = DateTime.Now;
                Debug.WriteLine("TryCheckNextPackage: " + dt3.Subtract(dt2).ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Package check error: " + ex);
                throw (ex);
            }
        }

        public static void Stop()
        {
            if (_checkingThreads == null)
                return;
            _isStopping = true;
            _workEventP.Set();
            _workEventC.Set();

            foreach (var checkingThread in _checkingThreads)
            {
                if (!checkingThread.Join(10000)) //ждём 10 секунд на завершение и прибиваем
                    checkingThread.Abort();
            }
            foreach (var importingThread in _importingThreads)
            {
                if (!importingThread.Join(10000)) //ждём 10 секунд на завершение и прибиваем
                    importingThread.Abort();
            }

            _checkingThreads = null;
            _importingThreads = null;
            GC.Collect();
        }

        //Юсупов: возможно, не актуально
        public static void KnockKnock()
        {
            //пришёл пакет, тук-тук
            _workEventP.Set();
        }

        private const int WORK_TIMEOUT = 60000;

        private static int _checkingThreadNumber;

       /// <summary>
       /// Метод проверки заявлений из пакетов в ФБС - сейчас не используется! 
       /// </summary>
        private static void CheckApplicationsJob()
        {
            int threadNumber = Interlocked.Increment(ref _checkingThreadNumber);
            Log.DebugFormat("Поток проверки в ФБС № {0} запущен", threadNumber);

            int institutionId = 0;
            bool doWait = false;
            try
            {
                while (true)
                {
                    bool isKnocked = false;
                    if (doWait) //если обработали пэкедж, не ждём и ищем следующий, иначе ждём
                        isKnocked = _workEventP.WaitOne(WORK_TIMEOUT);
                    if (_isStopping) return;
                    if (isKnocked) _workEventP.Reset();
                    Log.DebugFormat("[{0}] Package checking heartbeat event {1}", threadNumber, doWait);
                    try
                    {
                        //если ничего нет, то уходим в спячку
                        doWait = !PackageManager.TryCheckNextPackage(ref institutionId, 0);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Ошибка проверки в ФБС: ", ex);
                        doWait = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Fatal("Check Failed", ex);
                throw;
            }
        }

        private static int _importingThreadNumber;

        /// <summary>
        /// Метод импорта пакетов
        /// </summary>
        private static void ImportApplicationsJob()
        {
            int threadNumber = Interlocked.Increment(ref _importingThreadNumber);
            Log.InfoFormat("Поток импорта пакетов № {0} запущен", threadNumber);
            Debug.WriteLine("Поток импорта пакетов № {0} запущен", threadNumber);
            bool doWait = false;
            int sleepingCount =0;

            int institutionId = 0;
            try
            {
                while (true)
                {
                    bool isKnocked = false;
                    if (doWait) //если обработали пэкедж, не ждём и ищем следующий, иначе ждём
                        isKnocked = _workEventP.WaitOne(WORK_TIMEOUT);
                    if (_isStopping) return;
                    if (isKnocked) _workEventP.Reset();
                    Log.DebugFormat("[{0}] Package processing heartbeat event {1}", threadNumber, doWait);
                    try
                    {
                        // Если убираем логику, чтобы отрабатывать по данному институту, то раскоментарить строку
                        // institutionId=0;

                        //если ничего нет, то уходим в спячку
                        doWait = !PackageManager.TryProcessNextPackage(ref institutionId, 0, true);
                        if (doWait)
                        {
                            sleepingCount++;
                            if (sleepingCount * WORK_TIMEOUT > 3600000)
                            {
                                sleepingCount = 0;
                                Log.InfoFormat("[{0}] Поток спит уже целый час!", threadNumber);
                            }
                        }
                        else
                            sleepingCount = 0;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(String.Format("[{0}]", threadNumber) + " Package process error: ", ex);
                        doWait = true;
                    }
                }
            }
            catch (Exception ex) //сломалось всё что можно. Умираем.
            {
                Log.Fatal("Processing Failed", ex);
                throw;
            }
        }
    }
}
