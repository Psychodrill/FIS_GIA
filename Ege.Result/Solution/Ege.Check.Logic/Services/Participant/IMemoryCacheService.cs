namespace Ege.Check.Logic.Services.Participant
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public interface IMemoryCacheService
    {
        [NotNull]
        Task RefreshMemoryCache(
            bool refreshRegionSettings = true,
            bool refreshAnswerCriteria = true,
            bool refreshAvailableRegions = true,
            bool refreshCancelledExams = true,
            bool refreshSubjectsAndExams = true);

        KeyValuePair<DateTimeOffset, bool> GetLastRefreshTime();
    }
}