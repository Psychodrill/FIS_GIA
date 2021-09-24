using System;
using System.Linq;
using System.Threading;

namespace GVUZ.AppExport.Services
{
    public class ApplicationExportRequestMonitor
    {
        private readonly IApplicationExportRequestRepository _repository;
        private readonly CancellationToken _token;
        private readonly TimeSpan _pollInterval;
        private readonly int _maxBatchSize;

        public ApplicationExportRequestMonitor(CancellationToken token, IApplicationExportRequestRepository repository, TimeSpan pollInterval, int maxBatchSize)
        {
            _token = token;
            _repository = repository;
            _pollInterval = pollInterval;
            _maxBatchSize = maxBatchSize;
        }

        public event EventHandler<ApplicationExportRequestMonitorEventArgs> RequestCreated;
        
        public void Run()
        {
            _repository.ResetIncomplete();
            do
            {
                Guid[] items = _repository.FetchNewId(_maxBatchSize).ToArray();
                if (items.Length > 0 && !_token.IsCancellationRequested)
                {
                    OnRequestCreated(items);
                }
            } while (!_token.WaitHandle.WaitOne(_pollInterval));
        }

        private void OnRequestCreated(Guid[] items)
        {
            var handler = RequestCreated;
            if (handler != null)
            {
                handler(this, new ApplicationExportRequestMonitorEventArgs(items));
            }
        }
    }
}