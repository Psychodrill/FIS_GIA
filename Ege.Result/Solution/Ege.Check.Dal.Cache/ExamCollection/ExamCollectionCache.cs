namespace Ege.Check.Dal.Cache.ExamCollection
{
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    internal class ExamCollectionCache : BaseLockableCache<ExamCollectionCacheModel, ParticipantCacheModel>,
                                         IExamCollectionCache
    {
        public ExamCollectionCache(
            [NotNull] ICacheFailureHelper failureHelper,
            [NotNull] ICacheSettingsProvider settings,
            [NotNull] ICacheLockAcquirer acquirer) 
            : base(failureHelper, settings, acquirer)
        {
        }

        protected override string GetKeyString([NotNull] ParticipantCacheModel key)
        {
            return string.Format("Exams{0}.{1}", key.RegionId, key.Code);
        }
    }
}