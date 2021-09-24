namespace Ege.Check.Dal.MemoryCache.CancelledParticipantExams
{
    using System.Collections.Generic;
    using Ege.Check.Logic.Models.Staff;
    using JetBrains.Annotations;

    public interface ICancelledParticipantExamMemoryCache
    {
        bool IsHidden([NotNull]string code, int regionId, int examId);

        void Put([NotNull]ICollection<CancelledParticipantExam> exams);
    }
}
