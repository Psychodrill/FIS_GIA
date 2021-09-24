using System;
using System.Configuration;
using System.Reflection;
using System.Threading;
using FogSoft.Helpers;
using GVUZ.ServiceModel.Import.Core.Packages;
using GVUZ.ServiceModel.Import.Package;
using log4net;
using System.Diagnostics;

namespace GVUZ.ServiceModel.Import.AppCheckProcessor
{
    /// <summary>
    /// Обработка пакетов импорта
    /// </summary>
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

            //сбрасываем все статусы, которые были в процессинге, но почему-то не завершились
            using (ImportEntities dbContext = new ImportEntities())
            {
                dbContext.ExecuteStoreCommand(@"UPDATE ImportPackage SET CheckStatusID = {0} WHERE CheckStatusID = {1}
												UPDATE ImportPackage SET StatusID = {0} WHERE StatusID = {1}",
                    (int)PackageStatus.PlacedInQueue, (int)PackageStatus.Processing);
            }

            _isStopping = false;

            int checkThreadCount = ConfigurationManager.AppSettings["CheckProcessorThreadcount"].To(1);
            _checkingThreads = new Thread[checkThreadCount];
            for (var i = 0; i < checkThreadCount; i++)
            {
                _checkingThreads[i] = new Thread(CheckApplicationsJob);
                _checkingThreads[i].Start();
            }

            //Юсупов: возможно, не актуально
            Thread.Sleep(1000); //ждём, пока проснётся первый тред, чтобы одновременно в базу не ломиться

            int importThreadCount = ConfigurationManager.AppSettings["ImportProcessorThreadcount"].To(1);
            _importingThreads = new Thread[importThreadCount];
            for (var i = 0; i < importThreadCount; i++)
            {
                _importingThreads[i] = new Thread(ImportApplicationsJob);
                _importingThreads[i].Start();
            }
        }

        // Roman.N.Bukin - тестирование и отладка процедур сервиса из WinForms Application
        //public static void StartWinForms()
        //{
        //    DateTime dt1 = DateTime.Now;
        //    //сбрасываем все статусы, которые были в процессинге, но почему-то не завершились
        //    using (ImportEntities dbContext = new ImportEntities())
        //    {
        //        dbContext.ExecuteStoreCommand(@"UPDATE ImportPackage SET CheckStatusID = {0} WHERE CheckStatusID = {1}
								//				UPDATE ImportPackage SET StatusID = {0} WHERE StatusID = {1}",
        //            (int)PackageStatus.PlacedInQueue, (int)PackageStatus.Processing);
        //    }

        //    //_isStopping = false;

        //    //int checkThreadCount = ConfigurationManager.AppSettings["CheckProcessorThreadcount"].To(1);
        //    //_checkingThreads = new Thread[checkThreadCount];
        //    //for (var i = 0; i < checkThreadCount; i++)
        //    //{
        //    //    _checkingThreads[i] = new Thread(CheckApplicationsJob);
        //    //    _checkingThreads[i].Start();
        //    //}
        //    //CheckApplicationsJob();
        //    //PackageManager.TryCheckNextPackage();

        //    //Юсупов: возможно, не актуально
        //    //Thread.Sleep(1000); //ждём, пока проснётся первый тред, чтобы одновременно в базу не ломиться

        //    //int importThreadCount = ConfigurationManager.AppSettings["ImportProcessorThreadcount"].To(1);
        //    //_importingThreads = new Thread[importThreadCount];
        //    //for (var i = 0; i < importThreadCount; i++)
        //    //{
        //    //    _importingThreads[i] = new Thread(ImportApplicationsJob);
        //    //    _importingThreads[i].Start();
        //    //}
        //    //ImportApplicationsJob();

        //    DateTime dt2 = DateTime.Now;
        //    Debug.WriteLine("UPDATE ImportPackage: " + dt2.Subtract(dt1).ToString());
        //    try
        //    {
        //        PackageManager.TryProcessNextPackage();
        //        DateTime dt3 = DateTime.Now;
        //        Debug.WriteLine("TryProcessNextPackage: " + dt3.Subtract(dt2).ToString());
                
        //        //если ничего нет, то уходим в спячку
        //        PackageManager.TryCheckNextPackage();
        //        DateTime dt4 = DateTime.Now;
        //        Debug.WriteLine("TryCheckNextPackage: " + dt4.Subtract(dt3).ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("Package process error: ", ex);
        //        System.Diagnostics.Debug.WriteLine("Package process error: " + ex);
        //    }
        //}

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
       /// Метод проверки заявлений из пакетов в ФБС
       /// </summary>
        private static void CheckApplicationsJob()
        {
            int threadNumber = Interlocked.Increment(ref _checkingThreadNumber);
            Log.DebugFormat("Поток проверки в ФБС № {0} запущен", threadNumber);
            bool doWait = true;
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
                        doWait = !PackageManager.TryCheckNextPackage();
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
            Log.DebugFormat("Поток импорта пакетов № {0} запущен", threadNumber);
            bool doWait = true;
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
                        //если ничего нет, то уходим в спячку
                        doWait = !PackageManager.TryProcessNextPackage();
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
