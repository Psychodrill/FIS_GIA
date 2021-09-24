namespace Ege.Check.Logic.Services.Participant.ExamLists
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ege.Check.Dal.MemoryCache.CancelledParticipantExams;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    class ExamListFilterHelper : IExamListFilterHelper
    {
        [NotNull]private readonly ICancelledParticipantExamMemoryCache _cancelledExamsMemoryCache;

        public ExamListFilterHelper(
            [NotNull]ICancelledParticipantExamMemoryCache cancelledExamsMemoryCache)
        {
            _cancelledExamsMemoryCache = cancelledExamsMemoryCache;
        }

        private bool IsHiddenForRegion([NotNull]IDictionary<int, RegionExamSettingCacheModel> regionSettings, int examId)
        {
            RegionExamSettingCacheModel setting;
            return !regionSettings.TryGetValue(examId, out setting) || setting == null || !setting.ShowResult;
        }

        public ICollection<ExamCacheModel> ApplyFilter(
            ParticipantCacheModel participant, 
            IEnumerable<ExamCacheModel> exams,
            IDictionary<int, RegionExamSettingCacheModel> regionSettings)
        {
            if (participant.Code == null)
            {
                throw new ArgumentException("participant.Code is null");
            }
            var invisibleExams = new List<ExamCacheModel>();
            var orderedExams = exams
                .Where(e => e != null)
                .OrderBy(e => e.OralExamDate.HasValue ? (e.OralExamDate.Value > e.ExamDate ? e.OralExamDate.Value : e.ExamDate) : e.ExamDate)
                .ToList();
            ExamCacheModel showedComposition = null;
            foreach (var exam in orderedExams)
            {
                if (exam == null)
                {
                    continue;
                }
                if (orderedExams.Any(e => e != null && e.OralExamId == exam.ExamId))
                {
                    invisibleExams.Add(exam);
                }
                if (_cancelledExamsMemoryCache.IsHidden(participant.Code, participant.RegionId, exam.ExamId)
                    || exam.OralExamId.HasValue && _cancelledExamsMemoryCache.IsHidden(participant.Code, participant.RegionId, exam.OralExamId.Value))
                {
                    HideExam(exam);
                }
                const int examHasActualMarkInDatabase = 6;
                if ((exam.HasResult || exam.Status == examHasActualMarkInDatabase) && IsHiddenForRegion(regionSettings, exam.ExamId)
                    || (exam.OralExamId.HasValue && (exam.HasOralResult || exam.OralStatus == examHasActualMarkInDatabase) && 
                        IsHiddenForRegion(regionSettings, exam.OralExamId.Value)))
                {
                    HideExam(exam);
                    exam.IsHiddenForRegion = true;
                }
         /*       if (exam.IsComposition)
                {
                    ProcessComposition(exam, invisibleExams, ref showedComposition);
                }*/
                if (exam.Status != examHasActualMarkInDatabase)
                {
                    exam.HasResult = false;
                }
            }
            return invisibleExams.Count > 0
                ? orderedExams.Except(invisibleExams).ToList()
                : orderedExams;
        }

        private void HideExam([NotNull] ExamCacheModel exam)
        {
            exam.Mark5 = 0;
            exam.MinMark = 0;
            exam.TestMark = 0;
            exam.IsHidden = true;
        }

        private void ProcessComposition(
            [NotNull]ExamCacheModel exam,
            [NotNull]ICollection<ExamCacheModel> invisibleCompositions,
            ref ExamCacheModel showedComposition)
        {
            const int goodCompositionMark = 5;
            const int badCompositionMark = 2;
            const int noCompositionMark = 0;
            if (!exam.HasResult || exam.Mark5 == noCompositionMark)
            {
                exam.HasResult = false;
                if (showedComposition != null)
                {
                    invisibleCompositions.Add(exam);
                }
                else
                {
                    showedComposition = exam;
                }
            }
            else if (exam.Mark5 == goodCompositionMark)
            {
                if (showedComposition == null)
                {
                    showedComposition = exam;
                }
                else if (showedComposition.Mark5 != goodCompositionMark)
                {
                    invisibleCompositions.Add(showedComposition);
                    showedComposition = exam;
                }
                else
                {
                    invisibleCompositions.Add(exam);
                }
            }
            else if (exam.Mark5 == badCompositionMark)
            {
        /*        if (showedComposition != null)
                {
                    if (showedComposition.Mark5 != goodCompositionMark)
                    {
                        invisibleCompositions.Add(showedComposition);
                        showedComposition = exam;
                    }
                    else
                    {
                        invisibleCompositions.Add(exam);
                    }
                }
                else*/
                {
                    showedComposition = exam;
                }
            }
            else
            {
                invisibleCompositions.Add(exam);
            }
        }
    }
}
