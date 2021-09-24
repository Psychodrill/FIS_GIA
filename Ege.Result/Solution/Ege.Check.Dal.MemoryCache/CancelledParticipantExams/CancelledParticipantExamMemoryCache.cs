namespace Ege.Check.Dal.MemoryCache.CancelledParticipantExams
{
    using System.Collections.Generic;
    using Ege.Check.Logic.Models.Staff;
    using JetBrains.Annotations;


    public class CancelledParticipantExamMemoryCache : ICancelledParticipantExamMemoryCache
    {
        public bool IsHidden(string code, int regionId, int examId)
        {
            return _exams.Contains(new CancelledParticipantExam
                {
                    Code = code,
                    RegionId = regionId,
                    ExamGlobalId = examId,
                });
        }

        [NotNull]
        private static volatile ISet<CancelledParticipantExam> _exams = new HashSet<CancelledParticipantExam>();

        public void Put(ICollection<CancelledParticipantExam> exams)
        {
            _exams = new HashSet<CancelledParticipantExam>(exams);
        }
    }
}