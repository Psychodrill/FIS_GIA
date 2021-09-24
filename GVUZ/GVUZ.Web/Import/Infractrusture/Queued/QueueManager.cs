using System;
using System.Configuration;
using System.Linq;
using System.Timers;
using System.Xml.Linq;
using FogSoft.Helpers;
using GVUZ.Model.Entrants;

namespace GVUZ.Web.Import.Infractrusture.Queued
{
    public class QueueManager<T, TResult, QueueType>
    {
        private const int COLLECTOR_PERIOD_MS = 60000;
        private const int PROCESSOR_WAIT_TIMEOUT_SEC = 1200;
        private int _queuedFactoryProcessorThreadCount;
        private QueuedFactoryProcessor<T, TResult>[] _processors;

        public static object locker = new object(); 
        private static QueueManager<T, TResult, QueueType> _instance;
        public static QueueManager<T, TResult, QueueType> Instance
        {
            get
            {
                lock (locker)
                {
                    if (_instance == null)
                    {
                        _instance = new QueueManager<T, TResult, QueueType>();
                        _instance.Initialize();
                    }
                    return _instance;
                }
            }
        }

        private void Initialize()
        {
            /* Извлекаем кол-во потоков для обработки очереди по каждому из институтов */
            string key = string.Format("QueuedFactoryProcessorThreadCount.{0}", typeof(QueueType).Name);
            var app = ConfigurationManager.AppSettings[key];
            _queuedFactoryProcessorThreadCount = app != null ? Convert.ToInt32(app) : 1;

            /* Извлекаем верхнюю границу массива для обработки институтов */
            var institutionsCount = 0;
            using (var entities = new EntrantsEntities())
            {
                institutionsCount = entities.Institution.Max(c => c.InstitutionID);
            }

            if (institutionsCount == 0)
                throw new ApplicationException("Ошибка извлечения верхней границы массива для менеджера очередей");

            _processors = new QueuedFactoryProcessor<T, TResult>[institutionsCount];

            /* Запускаем периодическую задачу, которая будет делать Dispose для долго не используемых процессоров
             * чтобы не забивать память очередями вузов, которые давно перестали обращаться */
            var collector = new Timer(COLLECTOR_PERIOD_MS);
            collector.Elapsed += (sender, args) => DisposeTimeoutProcessors();
            collector.Enabled = true;
        }

        /// <summary>
        /// Периодическая работа по уничтожению долго неиспользуемых процессоров
        /// </summary>
        public void DisposeTimeoutProcessors()
        {
            try
            {
                /* Берем процессоры с пустыми очередями и с временем последнего обращения большего установленного */
                for (var i = 0; i < _processors.Length; i++)
                {
                    if (_processors[i] != null &&
                        _processors[i].RunningThreadsCount == 0 &&
                        _processors[i].LastTimeActivity <= DateTime.Now.AddSeconds(-PROCESSOR_WAIT_TIMEOUT_SEC))
                    {
                        _processors[i].Dispose();
                        _processors[i] = null;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.ErrorFormat(ex.Message);
            }
        }
        
        public QueuedFactoryProcessor<T, TResult> this[int institutionId]
        {
            get
            {
                lock (locker)
                {
                    if (_processors[institutionId] == null)
                        _processors[institutionId] =
                            new QueuedFactoryProcessor<T, TResult>(_queuedFactoryProcessorThreadCount);
                    return _processors[institutionId];
                }
            }
            set { _processors[institutionId] = value; }
        }

        /// <summary>
        /// Получить информацию по всем процессорам и очередям в менеджере
        /// </summary>
        /// <returns></returns>
        public XElement GetReport()
        {
            var report = new XElement(typeof(QueueType).Name);
            for (int i = 0; i < _processors.Length; i++)
            {
                if (_processors[i] == null) continue;
                report.Add(new XElement("InstitutionID", i,
                    new XAttribute("InQueue", _processors[i].InQueueCount),
                    new XAttribute("Running", _processors[i].RunningThreadsCount),
                    new XAttribute("Wait", _processors[i].WaitThreadsCount)));
            }
            return report;
        }
    }
}