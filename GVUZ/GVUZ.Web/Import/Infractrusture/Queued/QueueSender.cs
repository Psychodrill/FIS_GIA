using System;
using System.Configuration;
using System.Threading;
using FogSoft.Helpers;

namespace GVUZ.Web.Import.Infractrusture.Queued
{
    /// <summary>
    /// пример:
    /// new QueueSender<XElement, XElement, DoImportApplicationSingleService>().DoProcessWorkInQueque(_institutionId, package, DoWork);
    /// DoImportApplicationSingleService - под каждый тип будет создаваться свой Singeltone 
    /// менеджер сорержащий многопоточные процессоры обработки очереди
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="QueueType"></typeparam>
    public class QueueSender<T, TResult, QueueType>
    {
        /* предполагалось использовать для удаления элемента из очереди при 
         * отключении клиента */
        public Guid ticketId = Guid.NewGuid();
        public TResult _result;
        private readonly AutoResetEvent _workEvent = new AutoResetEvent(false);

        public TResult DoProcessWorkInQueque(int institutionId, T actionTask, Func<T, TResult> action)
        {
            /* Если процессор выключен - сразу выполняем задачу */
            var queuedFactoryProcessorIsOff = ConfigurationManager.AppSettings["QueuedFactoryProcessorIsOn"] != "true";
            if (queuedFactoryProcessorIsOff)
            {
                return action(actionTask);
            }

            var workItem = new FactoryWorkItem<T, TResult>()
                {
                    TicketId = ticketId,
                    Action = action,
                    ActionTask = actionTask
                };
            workItem.OnWorkCompleteEvent += OnWorkComplete;
            workItem.OnWorkFailedEvent += OnWorkFailed;
            QueueManager<T, TResult, QueueType>.Instance[institutionId].InsertWorkInQueque(workItem);
            _workEvent.WaitOne();

            if (_result == null)
                LogHelper.Log.ErrorFormat("Пустой ответ в очереди {0}", typeof(QueueType).Name);

            return _result;
        }

        private void OnWorkComplete(TResult result)
        {
            _result = result;
            Thread.Sleep(100);
            _workEvent.Set();
        }

        private void OnWorkFailed(string message)
        {
            _workEvent.Set();
        }
    }
}