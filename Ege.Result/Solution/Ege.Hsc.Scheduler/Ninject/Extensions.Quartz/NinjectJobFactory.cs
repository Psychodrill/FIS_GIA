﻿namespace Ege.Hsc.Scheduler.Ninject.Extensions.Quartz
{
    using System;
    using System.Globalization;
    using Common.Logging;
    using JetBrains.Annotations;
    using global::Ninject;
    using global::Quartz;
    using global::Quartz.Spi;

    public class NinjectJobFactory : IJobFactory
    {
        private readonly IKernel _kernel;
        private readonly ILog _log;

        public NinjectJobFactory(IKernel kernel)
        {
            _kernel = kernel;
            _log = LogManager.GetLogger(GetType());
        }

        /// <summary>
        ///     Called by the scheduler at the time of the trigger firing, in order to
        ///     produce a <see cref="IJob" /> instance on which to call Execute.
        ///     Instance creation is delegated to the Ninject Kernel.
        /// </summary>
        /// <remarks>
        ///     It should be extremely rare for this method to throw an exception -
        ///     basically only the the case where there is no way at all to instantiate
        ///     and prepare the Job for execution.  When the exception is thrown, the
        ///     Ege.Hsc.Scheduler will move all triggers associated with the Job into the
        ///     <see cref="TriggerState.Error" /> state, which will require human
        ///     intervention (e.g. an application restart after fixing whatever
        ///     configuration problem led to the issue wih instantiating the Job.
        /// </remarks>
        /// <param name="bundle">
        ///     The TriggerFiredBundle from which the <see cref="IJobDetail" />
        ///     and other info relating to the trigger firing can be obtained.
        /// </param>
        /// <param name="scheduler"></param>
        /// <returns>the newly instantiated Job</returns>
        /// <throws>  SchedulerException if there is a problem instantiating the Job. </throws>
        public IJob NewJob([NotNull] TriggerFiredBundle bundle, [NotNull] IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;
            var jobType = jobDetail.JobType;
            try
            {
                if (_log.IsDebugEnabled)
                {
                    _log.Debug(string.Format(CultureInfo.InvariantCulture, "Producing instance of Job '{0}', class={1}",
                                             jobDetail.Key, jobType.FullName));
                }

                return _kernel.Get(jobType) as IJob;
            }
            catch (Exception e)
            {
                var se =
                    new SchedulerException(
                        string.Format(CultureInfo.InvariantCulture, "Problem instantiating class '{0}'",
                                      jobDetail.JobType.FullName), e);
                throw se;
            }
        }

        /// <summary>
        ///     Allows the the job factory to destroy/cleanup the job if needed.
        ///     No-op when using SimpleJobFactory.
        /// </summary>
        public void ReturnJob(IJob job)
        {
        }
    }
}