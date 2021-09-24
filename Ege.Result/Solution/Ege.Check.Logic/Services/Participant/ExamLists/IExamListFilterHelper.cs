namespace Ege.Check.Logic.Services.Participant.ExamLists
{
    using System.Collections.Generic;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    interface IExamListFilterHelper
    {
        ICollection<ExamCacheModel> ApplyFilter(
            [NotNull]ParticipantCacheModel participant, 
            [NotNull]IEnumerable<ExamCacheModel> exams,
            [NotNull]IDictionary<int, RegionExamSettingCacheModel> regionSettings);
    }
}
