namespace Ege.Check.Dal.Cache.Participants
{
    using System;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    internal class ParticipantCache : BaseLockableCache<ParticipantCollectionCacheModel, string>, IParticipantCache
    {
        public ParticipantCache(
            [NotNull] ICacheFailureHelper failureHelper, 
            [NotNull] ICacheSettingsProvider settings,
            [NotNull] ICacheLockAcquirer acquirer)
            : base(failureHelper, settings, acquirer)
        {
        }

        protected override string GetKeyString(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            return key;
        }
    }
}