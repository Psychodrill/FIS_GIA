namespace Ege.Hsc.Scheduler.Jobs
{
    using System;
    using Common.Logging;
    using JetBrains.Annotations;
    using Quartz;

    internal abstract class BaseJob : IJob
    {
        [NotNull] protected readonly ILog Logger;

        protected BaseJob()
        {
            var logger = LogManager.GetLogger(GetType());
            if (logger == null)
            {
                throw new NullReferenceException(string.Format("Can not create logger for {0}", GetType()));
            }
            Logger = logger;
        }

        public void Execute(IJobExecutionContext context)
        {
            var jobName = GetType().Name;
            try
            {
                Logger.Trace(string.Format("Job {0} started", jobName));
                InnerExecute(context);
                Logger.Trace(string.Format("Job {0} finished", jobName));
            }
            catch (Exception exception)
            {
                Logger.Error(string.Format("Job {0} threw exception.", jobName), exception);
            }
            
        }

        protected abstract void InnerExecute(IJobExecutionContext context);
    }
}