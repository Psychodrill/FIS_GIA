namespace Ege.Hsc.Logic.Requests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Logging;
    using Ege.Check.Common;
    using Ege.Check.Common.Config;
    using Ege.Check.Common.Hash;
    using Ege.Check.Logic.Models.Requests;
    using Ege.Dal.Common.Factory;
    using Ege.Hsc.Dal.Entities;
    using Ege.Hsc.Dal.Store.Repositories;
    using Ege.Hsc.Logic.Blanks;
    using Ege.Hsc.Logic.Csv;
    using Ege.Hsc.Logic.Models;
    using Ege.Hsc.Logic.Models.Requests;
    using global::Ninject;
    using JetBrains.Annotations;

    class RequestService : IRequestService
    {
        [NotNull] private readonly IFioHasher _fioHasher;
        [NotNull] private readonly IBlankRequestRepository _repository;
        [NotNull] private readonly IDbConnectionFactory _connectionFactory;
        [NotNull] private readonly IBlankZipper _zipper;
        [NotNull] private readonly IRequestCsvParser _csvParser;
        [NotNull] private readonly IEqualityComparer<ParticipantBlankRequest> _requestEqualityComparer;
        [NotNull] private readonly IMapper<RequestData, RequestStatus> _requestDataMapper;
        [NotNull] private readonly IMapper<ParticipantBlankRequest, RequestedParticipant> _requestMapper;
        [NotNull] private readonly IMapper<ParticipantBlankRequest, string> _nameExtractor;
        [NotNull] private readonly IFilePathHelper _filePathHelper;
        [NotNull]private readonly IConfigReaderHelper _configReader;

        [NotNull]private static readonly ILog Logger = LogManager.GetLogger<RequestService>();

        public RequestService(
            [NotNull] IFioHasher fioHasher, 
            [NotNull] IBlankRequestRepository repository, 
            [NotNull] IDbConnectionFactory connectionFactory, 
            [NotNull] IBlankZipper zipper,
            [NotNull] IRequestCsvParser csvParser,
            [NotNull][Named(ParticipantBlankRequestEqualityComparer.Characteristic)] IEqualityComparer<ParticipantBlankRequest> requestEqualityComparer, 
            [NotNull] IMapper<RequestData, RequestStatus> requestDataMapper, 
            [NotNull] IMapper<ParticipantBlankRequest, RequestedParticipant> requestMapper, 
            [NotNull] IMapper<ParticipantBlankRequest, string> nameExtractor, 
            [NotNull] IFilePathHelper filePathHelper,
            [NotNull] IConfigReaderHelper configReader)
        {
            _fioHasher = fioHasher;
            _repository = repository;
            _connectionFactory = connectionFactory;
            _zipper = zipper;
            _csvParser = csvParser;
            _requestEqualityComparer = requestEqualityComparer;
            _requestDataMapper = requestDataMapper;
            _requestMapper = requestMapper;
            _nameExtractor = nameExtractor;
            _filePathHelper = filePathHelper;
            _configReader = configReader;
        }

        public async Task<SingleParticipantRequestResult> ProcessSingleParticipant(ParticipantBlankRequest request, UserReference user)
        {
            var hash = _fioHasher.GetHash(request.Surname, request.FirstName, request.Patronymic);
            RequestedParticipant participant;
            Guid requestId;
            using (var connection = await _connectionFactory.CreateHscAsync())
            {
                if (connection == null)
                {
                    throw new InvalidOperationException("IDbConnectionFactory::CreateHscAsync returned null");
                }
                participant = await _repository.GetRequestedParticipant(connection, hash, request.Document.AddLeadingZeroes());
                if (participant == null)
                {
                    return new SingleParticipantRequestResult { Status = SingleParticipantRequestStatus.NotFound };
                }
                if (participant.IsCollision)
                {
                    return new SingleParticipantRequestResult { Status = SingleParticipantRequestStatus.Collision };
                }
                if (participant.ProcessingUnfinished)
                {
                    return new SingleParticipantRequestResult {Status = SingleParticipantRequestStatus.NotYetDownloaded};
                }
                requestId = await _repository.AddSingleParticipantRequest(connection, user, participant.Id);
            }
            participant.Name = _nameExtractor.Map(request);
            await _zipper.Zip(new BlankRequest {Id = requestId, Participants = new[] {participant}});
            return new SingleParticipantRequestResult
            {
                Status = participant.HasErrors
                        ? SingleParticipantRequestStatus.HasErrors
                        : SingleParticipantRequestStatus.Success,
                Id = requestId,
            };
        }

        public async Task<RequestStatusPage> GetRequests(UserReference user, int skip, int take)
        {
            using (var connection = await _connectionFactory.CreateHscAsync())
            {
                if (connection == null)
                {
                    throw new InvalidOperationException("IDbConnectionFactory::CreateHscAsync returned null");
                }
                var data = await _repository.GetRequestsData(connection, user, skip, take);
                if (data == null || data.Page == null)
                {
                    throw new InvalidOperationException(string.Format("IBlankRequestRepository::GetRequests returned null : {0}", data));
                }
                return new RequestStatusPage
                {
                    Count = data.Count,
                    Page = data.Page.Select(_requestDataMapper.Map).ToList(),
                };
            }
        }

        public async Task<Guid> CreateRequest(UserReference user, string note, IEnumerable<Task<Stream>> csvs)
        {
            IEnumerable<ParticipantBlankRequest> requests = Enumerable.Empty<ParticipantBlankRequest>();
            foreach (var csv in csvs)
            {
                requests = requests.Concat(_csvParser.Parse(await csv));
            }
            requests = requests.Distinct(_requestEqualityComparer);
            using (var connection = await _connectionFactory.CreateHscAsync())
            {
                if (connection == null)
                {
                    throw new InvalidOperationException("IDbConnectionFactory::CreateHscAsync returned null");
                }
                return await _repository.CreateRequest(connection, user, note, requests.Select(_requestMapper.Map));
            }
        }

        public async Task<RequestPermission> IsRequestOwner(Guid requestId, UserReference user)
        {
            using (var connection = await _connectionFactory.CreateHscAsync())
            {
                if (connection == null)
                {
                    throw new InvalidOperationException("IDbConnectionFactory::CreateHscAsync returned null");
                }
                return await _repository.IsRequestOwner(connection, requestId, user);
            }
        }

        public async Task<int> DeleteZipsForOldRequests()
        {
            using (var connection = await _connectionFactory.CreateHscAsync())
            {
                if (connection == null)
                {
                    throw new InvalidOperationException("IDbConnectionFactory::CreateHscAsync returned null");
                }
                var oldRequestIds = await _repository.GetOldRequestIds(connection, _configReader.GetInt("ZipHoursToLive", string.Empty, 24));
                if (oldRequestIds == null || oldRequestIds.Ids == null)
                {
                    throw new InvalidOperationException(string.Format(
                        "IBlankRequestRepository::GetOldRequestIds returned null : {0}", oldRequestIds));
                }
                Logger.InfoFormat("{0} requested archives are old", oldRequestIds.Ids.Count);
                foreach (var id in oldRequestIds.Ids)
                {
                    var file = _filePathHelper.GetZipPath(id);
                    if (File.Exists(file))
                    {
                        Logger.InfoFormat("Deleting file {0}", file);
                        File.Delete(file);
                        Logger.InfoFormat("Deleted file {0}", file);
                    }
                    else
                    {
                        Logger.InfoFormat("File {0} does not exist", file);
                    }
                }
                return await _repository.SetDeletedStatusForOldRequests(connection, oldRequestIds);
            }
        }
    }
}
