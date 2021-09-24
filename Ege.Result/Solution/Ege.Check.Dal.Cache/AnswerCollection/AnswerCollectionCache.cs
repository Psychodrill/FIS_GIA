namespace Ege.Check.Dal.Cache.AnswerCollection
{
    using System.Collections.Generic;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    internal class AnswerCollectionCache :
        BaseLockableCache<AnswerCollectionCacheModel, KeyValuePair<ParticipantCacheModel, int>>, 
        IAnswerCollectionCache
    {
        public AnswerCollectionCache(
            [NotNull] ICacheFailureHelper failureHelper,
            [NotNull] ICacheSettingsProvider settings,
            [NotNull] ICacheLockAcquirer acquirer)
            : base(failureHelper, settings, acquirer)
        {
        }

        protected override string GetKeyString(KeyValuePair<ParticipantCacheModel, int> key)
        {
            return string.Format("ExamAnswers{0}.{1}.{2}", key.Key.RegionId, key.Key.Code, key.Value);
        }
    }
}