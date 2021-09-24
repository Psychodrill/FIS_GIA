namespace Ege.Check.Dal.MemoryCache.ExamInfo
{
    using System.Collections.Generic;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public interface IExamInfoMemoryCache
    {
        [NotNull]
        ExamInfoCacheModel Get(int subjectCode);

        void Put([NotNull] IDictionary<int, ExamInfoCacheModel> info);
    }
}