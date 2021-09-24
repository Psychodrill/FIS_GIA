namespace Ege.Check.Logic.Services.Staff.ExamCancellation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.AnswerCollection;
    using Ege.Check.Dal.Cache.CacheFactory;
    using Ege.Check.Dal.Cache.ExamCollection;
    using Ege.Check.Dal.Store.Repositories.Exams;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Dal.Common.Factory;
    using JetBrains.Annotations;

    internal class ExamCancellationService : IExamCancellationService
    {
        [NotNull] private readonly IAnswerCollectionCache _answerCollectionCache;

        [NotNull] private readonly ICacheFactory _cacheFactory;
        [NotNull] private readonly IDbConnectionFactory _connectionFactory;
        [NotNull] private readonly IExamCollectionCache _examCollectionCache;
        [NotNull] private readonly IExamCancellationRepository _repository;

        public ExamCancellationService(
            [NotNull] IExamCancellationRepository repository,
            [NotNull] IExamCollectionCache examCollectionCache,
            [NotNull] IAnswerCollectionCache answerCollectionCache,
            [NotNull] IDbConnectionFactory connectionFactory,
            [NotNull] ICacheFactory cacheFactory)
        {
            _repository = repository;
            _examCollectionCache = examCollectionCache;
            _answerCollectionCache = answerCollectionCache;
            _connectionFactory = connectionFactory;
            _cacheFactory = cacheFactory;
        }

        public Task<bool> Cancel(string participantCode, int regionId, int examId)
        {
            return SetCancellation(participantCode, regionId, examId, true);
        }

        public Task<bool> Uncancel(string participantCode, int regionId, int examId)
        {
            return SetCancellation(participantCode, regionId, examId, false);
        }

        public async Task<CancelledExamsPage> Get(int? regionId, int take, int skip)
        {
            using (var connection = await _connectionFactory.CreateAsync())
            {
                return await _repository.GetCancelled(connection, regionId, take, skip);
            }
        }

        [NotNull]
        private async Task<bool> SetCancellation([NotNull]string participantCode, int regionId, int examId, bool status)
        {
            bool updated;
            using (var connection = await _connectionFactory.CreateAsync())
            {
                updated = await _repository.SetCancellation(connection, participantCode, regionId, examId, status);
            }
            if (updated)
            {
                var cacheConnection = _cacheFactory.GetCache();
                if (cacheConnection != null)
                {
                    var participant = new ParticipantCacheModel {Code = participantCode, RegionId = regionId};
                    _examCollectionCache.Put(cacheConnection, participant, null);
                    _answerCollectionCache.Put(cacheConnection, new KeyValuePair<ParticipantCacheModel, int>(participant, examId), null);
                }
            }
            return updated;
        }
    }
}