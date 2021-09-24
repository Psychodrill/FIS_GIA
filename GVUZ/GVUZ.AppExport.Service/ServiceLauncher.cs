using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GVUZ.AppExport.Services;
using log4net.Config;

namespace GVUZ.AppExport.Service
{
    public class ServiceLauncher
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private Task _task;
        private const int MaxThreads = 64;
        private const int DefaultMaxThreads = 4;
        
        public void Start()
        {
            XmlConfigurator.Configure();
            var p = InitProcessor();
            _task = Task.Factory.StartNew(p.Run, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
        }

        public void Stop()
        {
            _cts.Cancel(false);
            if (_task != null)
            {
                _task.Wait(TimeSpan.FromSeconds(5));
            }
        }

        private ExportProcessor InitProcessor()
        {
            var repository = new SqlApplicationExportRequestRepository(GetConnectionString());
            var monitor = new ApplicationExportRequestMonitor(_cts.Token, repository, GetPollInterval(), GetMaxBatchSize());
            var p = new ExportProcessor(_cts.Token, GetNumThreads(), repository, monitor, GetExportFileStorage());
            return p;
        }

        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["AppExport"].ConnectionString;
        }

        private DirectoryInfo GetExportFileStorage()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.AppExportFileStorage))
            {
                return new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FisAppExport"));
            }

            return new DirectoryInfo(Properties.Settings.Default.AppExportFileStorage);
        }

        private TimeSpan GetPollInterval()
        {
            TimeSpan poll = Properties.Settings.Default.AppExportDbPollInterval;

            if (poll != TimeSpan.Zero)
            {
                return poll;
            }

            return TimeSpan.FromSeconds(10);
        }

        private int GetNumThreads()
        {
            int numThreads = Properties.Settings.Default.AppExportThreadCount;
            if (numThreads >= 1 && numThreads <= MaxThreads)
            {
                return numThreads;
            }

            return DefaultMaxThreads;
        }

        private int GetMaxBatchSize()
        {
            return GetNumThreads()*4;
        }
    }
}