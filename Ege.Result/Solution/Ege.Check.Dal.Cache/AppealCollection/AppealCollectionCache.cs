namespace Ege.Check.Dal.Cache.AppealCollection
{
    using System.Collections.Generic;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    internal class AppealCollectionCache :
        BaseLockableCache<AppealCollectionCacheModel, KeyValuePair<ParticipantCacheModel, int>>, IAppealCollectionCache
    {
        public AppealCollectionCache(
            [NotNull] ICacheFailureHelper failureHelper, 
            [NotNull] ICacheSettingsProvider settings,
            [NotNull] ICacheLockAcquirer acquirer)
            : base(failureHelper, settings, acquirer)
        {
        }

        protected override string GetKeyString(KeyValuePair<ParticipantCacheModel, int> key)
        {
            return string.Format("Appeals{0}.{1}.{2}", key.Key.RegionId, key.Key.Code, key.Value);
        }
    }
}