namespace Ege.Check.Logic.Services.Participant.Participants
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.CacheFactory;
    using Ege.Check.Dal.Cache.Participants;
    using Ege.Check.Dal.Store.Repositories.Participants;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Dal.Common.Factory;
    using JetBrains.Annotations;

    internal class ParticipantService : IParticipantService
    {
        [NotNull] private readonly IParticipantCache _cache;
        [NotNull] private readonly ICacheFactory _cacheFactory;
        [NotNull] private readonly IDbConnectionFactory _connectionFactory;
        [NotNull] private readonly IParticipantRepository _repository;

        public ParticipantService(
            [NotNull] IParticipantRepository repository,
            [NotNull] IParticipantCache cache,
            [NotNull] IDbConnectionFactory connectionFactory,
            [NotNull] ICacheFactory cacheFactory)
        {
            _repository = repository;
            _cache = cache;
            _connectionFactory = connectionFactory;
            _cacheFactory = cacheFactory;
        }

        public async Task<ParticipantServiceResult> Get(string hash, string code, string document, int regionId)
        {
            if (code != null && document != null)
            {
                throw new ArgumentException("code and document can not be both set");
            }
            var cacheConnection = _cacheFactory.GetCache();
            var withRequestedHash = _cache.Get(cacheConnection, hash);

            if (withRequestedHash == null)
            {
                using (var connection = await _connectionFactory.CreateAsync())
                {
                    withRequestedHash = await _repository.GetByHash(connection, hash);
                }
                _cache.Put(cacheConnection, hash, withRequestedHash);
            }
            if (withRequestedHash == null)
            {
                throw new InvalidOperationException("IParticipantRepository::GetByHash returned null");
            }

            ParticipantCacheModel result = null;
            bool collision = false;
            if (withRequestedHash.Participants == null)
            {
            }
            else if (code != null)
            {
                result =
                    withRequestedHash.Participants.FirstOrDefault(
                        p => p != null && code.Equals(p.Code) && p.RegionId == regionId);
            }
            else if (document != null)
            {
                var withSameDocument = withRequestedHash.Participants.Where(p => p != null && document.Equals(p.Document) && p.RegionId == regionId);
                using (var enumerator = withSameDocument.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        result = enumerator.Current;
                        collision = enumerator.MoveNext();
                    }
                }
            }
            // если в кэше есть данные, то они всегда актуальные - если запрошенного пользователя нет в коллекции, пришедшей из кэша, его и вообще нет - в БД не лезем
            return new ParticipantServiceResult
            {
                Collision = collision,
                Participant = result,
            };
        }


        public async Task<string> GetCodeByRbdId(Guid rbdId)
        {
            using (var connection = await _connectionFactory.CreateAsync())
            {
                return await _repository.GetCodeByRbdId(connection, rbdId);
            }
        }
    }
}
