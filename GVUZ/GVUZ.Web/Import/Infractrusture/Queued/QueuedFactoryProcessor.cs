using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FogSoft.Helpers;

namespace GVUZ.Web.Import.Infractrusture.Queued
{
    public class QueuedFactoryProcessor<T, TResult> : IDisposable
    {
        private DateTime _lastTimeActivity = DateTime.Now;
        private List<Thread> _workers = new List<Thread>();
        private Queue<FactoryWorkItem<T, TResult>> _queue;
        private readonly AutoResetEvent _workEvent = new AutoResetEvent(false);

        public QueuedFactoryProcessor(int queuedFactoryProcessorThreadCount)
        {
            Initialize(queuedFactoryProcessorThreadCount);
        }

        /// <summary>
        /// Инициализация фабрики
        /// </summary>
        void Initialize(int queuedFactoryProcessorThreadCount)
        {
            _queue = new Queue<FactoryWorkItem<T, TResult>>();
            for (int i = 0; i < queuedFactoryProcessorThreadCount; i++)
            {
                var thread = new Thread(DoWork);
                _workers.Add(thread);
                thread.Start();
            }
        }

        /// <summary>
        /// То что делает работник фабрики - берет из очереди работу, 
        /// работает ее и сообщает о результате
        /// </summary>
        private void DoWork()
        {
            while (true)
            {
                FactoryWorkItem<T, TResult> work = null;
                _lastTimeActivity = DateTime.Now;
                
                lock (_queue)
                {
                    if (_queue.Count > 0)
                    {
                        work = _queue.Dequeue();
                        if (work == null) 
                            return;
                    }
                }

                if (work != null)
                {
                    /* Обработка */
                    try
                    {
                        TResult result = work.Action(work.ActionTask);
                        work.RaiseCompleteEvent(result);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Log.ErrorFormat("Ошибка в QueuedFactoryProcessor: {0}", ex.Message);
                        work.RaiseFailedEvent(ex.Message);
                    }
                }
                else
                    _workEvent.WaitOne();
            }
        }

        /// <summary>
        /// Кол-во объектов в очереди
        /// </summary>
        public int InQueueCount
        {
            get { return _queue.Count; }
        }

        /// <summary>
        /// Кол-во потоков находящихся в работе
        /// </summary>
        public int RunningThreadsCount
        {
            get { return _workers.Count(c => c.ThreadState == ThreadState.Running); }
        }

        /// <summary>
        /// Кол-во спящих потоков
        /// </summary>
        public int WaitThreadsCount
        {
            get { return _workers.Count(c => c.ThreadState != ThreadState.Running); }
        }

        /// <summary>
        /// Время последней активности
        /// </summary>
        public DateTime LastTimeActivity
        {
            get { return _lastTimeActivity; }
        }

        /// <summary>
        /// Добавить объект в очередь на обработку
        /// </summary>
        /// <param name="work"></param>
        public void InsertWorkInQueque(FactoryWorkItem<T, TResult> work)
        {
            lock (_queue)
            {
                /* Добавляем работу в очередь и сообщаем всем спящим
                * потокам о том, что можно попробовать ее взять в обработку */
                _queue.Enqueue(work);
                _workEvent.Set();
            }
        }

        public void Dispose()
        {
            foreach (var thread in _workers)
            {
                thread.Abort();
                thread.Join();
            }
            _workers.Clear();
            _workers = null;
        }
    }
}