namespace Ege.Check.Logic.Services.Participant
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ege.Check.Common.Config;
    using Ege.Check.Common.Extensions;
    using Ege.Check.Dal.Cache.CacheFactory;
    using Ege.Check.Dal.Cache.LoadServices;
    using Ege.Check.Dal.Store.Repositories.Blanks;
    using Ege.Check.Logic.Models.Servers;
    using Ege.Dal.Common.Factory;
    using JetBrains.Annotations;

    class CacheUpdaterService : ICacheUpdaterService
    {
        [NotNull] private readonly IBlankInfoCacheUpdater _cacheUpdater;
        [NotNull] private readonly IBlankInfoRepository _repository;
        [NotNull] private readonly IDbConnectionFactory _connectionFactory;
        [NotNull] private readonly ICacheFactory _cacheFactory;
        [NotNull] private readonly IConfigReaderHelper _configReaderHelper;


        public CacheUpdaterService(
            [NotNull] IBlankInfoCacheUpdater cacheUpdater, 
            [NotNull] IBlankInfoRepository repository, 
            [NotNull]IDbConnectionFactory connectionFactory, 
            [NotNull]ICacheFactory cacheFactory, 
            [NotNull]IConfigReaderHelper configReaderHelper)
        {
            _cacheUpdater = cacheUpdater;
            _repository = repository;
            _connectionFactory = connectionFactory;
            _cacheFactory = cacheFactory;
            _configReaderHelper = configReaderHelper;
        }

        public async Task UpdateBlankCompositionPageCount()
        {
            var cache = _cacheFactory.GetCache();
            if (cache == null)
            {
                return;
            }
            ICollection<UpdatedBlankInfo> blanks;
            using (var connection = await _connectionFactory.CreateAsync())
            {
                if (connection == null)
                {
                    throw new Exception("CreateAsync returned null");
                }
                blanks = await _repository.GetAllBlanksWithCompositionPageCount(connection);
            }
            if (blanks == null)
            {
                throw new Exception("GetAllBlanksWithCompositionPageCount returned null");
            }
            foreach (var batch in blanks.ArrayBatch(_configReaderHelper.GetInt("CacheBatchSize", string.Empty, 100000)))
            {
                if (batch == null)
                {
                    continue;
                }
                await _cacheUpdater.UpdatePageCount(cache, batch);
            }
        }
    }
}
