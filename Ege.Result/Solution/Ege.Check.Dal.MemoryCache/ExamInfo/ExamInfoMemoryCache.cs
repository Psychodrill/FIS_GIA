namespace Ege.Check.Dal.MemoryCache.ExamInfo
{
    using System.Collections.Generic;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public class ExamInfoMemoryCache : IExamInfoMemoryCache
    {
        [NotNull] private static volatile IDictionary<int, ExamInfoCacheModel> _cache =
            new Dictionary<int, ExamInfoCacheModel>();

        public ExamInfoCacheModel Get(int subjectCode)
        {
            ExamInfoCacheModel result;
            if (!_cache.TryGetValue(subjectCode, out result) || result == null)
            {
                result = new ExamInfoCacheModel
                    {
                        PartB = new TaskBInfoCacheModel[0],
                        WithCriteria = new TaskWithCriteriaInfoCacheModel[0],
                    };
            }
            return result;
        }

        public void Put(IDictionary<int, ExamInfoCacheModel> info)
        {
            _cache = info;
        }
    }
}