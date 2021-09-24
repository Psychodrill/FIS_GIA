namespace Ege.Hsc.Scheduler.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ege.Dal.Common.Factory;
    using Ege.Hsc.Dal.Entities;
    using Ege.Hsc.Dal.Store.Repositories;
    using Ege.Hsc.Logic.Blanks;
    using JetBrains.Annotations;
    using Quartz;

    class ZipBlanksJob : AsyncJob
    {
        [NotNull]private readonly IDbConnectionFactory _connectionFactory;
        [NotNull]private readonly IBlankZipper _zipper;
        [NotNull]private readonly IBlankRequestRepository _blankRequestRepository;
        [NotNull]private readonly IBlankApplicationSettings _settings;

        public ZipBlanksJob(
            [NotNull] IDbConnectionFactory connectionFactory, 
            [NotNull] IBlankZipper zipper, 
            [NotNull] IBlankRequestRepository blankRequestRepository, 
            [NotNull] IBlankApplicationSettings settings)
        {
            _connectionFactory = connectionFactory;
            _zipper = zipper;
            _blankRequestRepository = blankRequestRepository;
            _settings = settings;
        }

        protected override async Task InnerExecuteAsync(IJobExecutionContext context)
        {
            ICollection<BlankRequest> requests;
            Logger.Trace("Retrieving requests awaiting zipping");
            using (var connection = await _connectionFactory.CreateHscAsync())
            {
                requests = await _blankRequestRepository.TopNotZippedAsync(connection, _settings.BatchBlankRequest());
            }
            if (requests == null)
            {
                throw new NullReferenceException("IBlankRequestRepository::TopNotZippedAsync returned null");
            }
            Logger.TraceFormat("Received {0} requests awaiting zipping", requests.Count);
            foreach (var request in requests)
            {
                if (request == null)
                {
                    throw new NullReferenceException("Null in collection returned by IBlankRequestRepository::TopNotZippedAsync");
                }
                Logger.TraceFormat("Making zip archive for request {0}", request.Id);
                await _zipper.Zip(request);
                using (var connection = await _connectionFactory.CreateHscAsync())
                {
                    await _blankRequestRepository.SetZipped(connection, request.Id);
                }
                Logger.TraceFormat("Made zip archive for request {0}", request.Id);
            }
        }
    }
}
