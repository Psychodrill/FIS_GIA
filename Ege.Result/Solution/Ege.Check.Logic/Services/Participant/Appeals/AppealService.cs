namespace Ege.Check.Logic.Services.Participant.Appeals
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.AppealCollection;
    using Ege.Check.Dal.Cache.CacheFactory;
    using Ege.Check.Dal.Store.Repositories.Appeals;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Dal.Common.Factory;
    using JetBrains.Annotations;

    internal class AppealService : IAppealService
    {
        [NotNull] private readonly IAppealCollectionCache _cache;
        [NotNull] private readonly ICacheFactory _cacheFactory;
        [NotNull] private readonly IDbConnectionFactory _connectionFactory;
        [NotNull] private readonly IAppealRepository _repository;

        public AppealService(
            [NotNull] IAppealRepository repository,
            [NotNull] IAppealCollectionCache cache,
            [NotNull] IDbConnectionFactory connectionFactory,
            [NotNull] ICacheFactory cacheFactory)
        {
            _repository = repository;
            _cache = cache;
            _connectionFactory = connectionFactory;
            _cacheFactory = cacheFactory;
        }

        public async Task<AppealCollectionCacheModel> GetAppeals(
            KeyValuePair<ParticipantCacheModel, int> participantExam)
        {
            var cacheConnection = _cacheFactory.GetCache();

            var appeals = _cache.Get(cacheConnection, participantExam);
            if (appeals == null)
            {
                using (var connection = await _connectionFactory.CreateAsync())
                {
                    appeals = await _repository.GetAppeals(connection, participantExam);
                }
                _cache.Put(cacheConnection, participantExam, appeals);
            }
            return appeals;
        }
    }
}