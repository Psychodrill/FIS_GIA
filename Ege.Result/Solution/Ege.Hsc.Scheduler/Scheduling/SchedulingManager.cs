namespace Ege.Hsc.Scheduler.Scheduling
{
    using System;
    using Common.Logging;
    using Ege.Check.Common.Async;
    using Ege.Dal.Common.Factory;
    using Ege.Hsc.Dal.Store.Repositories;
    using Ege.Hsc.Scheduler.Configuration;
    using Ege.Hsc.Scheduler.Jobs;
    using Ege.Hsc.Scheduler.Retrievers;
    using JetBrains.Annotations;
    using Quartz;

    internal class SchedulingManager : ISchedulingManager
    {
        [NotNull] private readonly IJobCronStringConfiguration _jobCronStringConfiguration;

        [NotNull] private readonly ILog _logger;
        [NotNull] private readonly IScheduler _scheduler;
        [NotNull] private readonly IDbConnectionFactory _connectionFactory;
        [NotNull] private readonly IStateRepository _state;

        public SchedulingManager(
            [NotNull] ISchedulerRetriever schedulerRetriever,
            [NotNull] IJobCronStringConfiguration jobCronStringConfiguration, 
            [NotNull]IStateRepository state, 
            [NotNull]IDbConnectionFactory connectionFactory)
        {
            _jobCronStringConfiguration = jobCronStringConfiguration;
            _state = state;
            _connectionFactory = connectionFactory;
            _scheduler = schedulerRetriever.GetScheduler();
            var logger = LogManager.GetLogger(GetType());
            if (logger == null)
            {
                throw new NullReferenceException(string.Format("Can not create logger for type {0}", GetType()));
            }
            _logger = logger;
        }

        public void StartScheduler()
        {
            Console.WriteLine("StartScheduler");

            using (var connection = _connectionFactory.CreateHscSync())
            {
// ReSharper disable AccessToDisposedClosure
                // the lambda will be run inline; it won't be used outside the using statement
                var resetEntities = AsyncHelper.RunSync(() => _state.ResetState(connection));
                _logger.TraceFormat("Reset state for {0} entities", resetEntities);
// ReSharper restore AccessToDisposedClosure
            }
            
            var jobs = new []
                {
                    /*new JobData(typeof(DownloadBlanksJob), "DownloadBlanks"),
                    new JobData(typeof(LoadServersJob), "LoadServers"), 
                    new JobData(typeof(LoadBlanksJob), "LoadBlanks"),
                    new JobData(typeof(ZipBlanksJob), "ZipBlanks"), 
                    new JobData(typeof(CheckServerAvailabilityJob), "CheckServerAvailability"), 
                    new JobData(typeof(DeleteOldZipsJob), "DeleteOldZips"), */
                    new JobData(typeof(LoadCompositionPageCountJob), "LoadCompositionPageCount"),
                    new JobData(typeof(FixPageCountInconsistenciesJob), "FixPageCountInconsistencies"), 
                    //new JobData(typeof(UpdateBlankCacheJob), "UpdateBlankCache"), 
                };
            foreach (var job in jobs)
            {
                if (job == null)
                {
                    continue;
                }
                ScheduleJob(job.Type, job.GroupName);
            }

            _scheduler.Start();
        }

        public void StopScheduler()
        {
            _logger.Trace("Ege.Hsc.Scheduler shutdown...");
            _scheduler.Shutdown();
            _logger.Trace("Ege.Hsc.Scheduler shutdowned");
        }

        private void ScheduleJob([NotNull] Type jobType, [NotNull] string groupName)
        {
            var jobName = jobType.Name;
            _logger.TraceFormat("Schedule job {0}", jobName);
            var cronString = _jobCronStringConfiguration.GetCronString(jobName);
            _logger.TraceFormat("Job {0} cron string: {1}", jobName, cronString);
            if (string.IsNullOrWhiteSpace(cronString))
            {
                _logger.WarnFormat("job {0} cron string is null or empty!", jobName);
                return;
            }
            // ReSharper disable PossibleNullReferenceException
            var job = JobBuilder.Create(jobType)
                                .WithIdentity(jobName, groupName)
                                .Build();
            var trigger = TriggerBuilder.Create()
                                        .WithIdentity(jobName + "Trigger", groupName)
                                        .WithCronSchedule(cronString)
                                        .StartNow()
                                        .Build();
            // ReSharper restore PossibleNullReferenceException
            _scheduler.ScheduleJob(job, trigger);
        }

        private class JobData
        {
            public JobData([NotNull] Type type, [NotNull] string groupName)
            {
                Type = type;
                GroupName = groupName;
            }

            [NotNull]
            public Type Type { get; private set; }

            [NotNull]
            public string GroupName { get; private set; }
        }
    }
}