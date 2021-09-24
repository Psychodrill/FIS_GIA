namespace Ege.Check.Dal.Store.Repositories.Exams
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Staff;
    using JetBrains.Annotations;

    public interface IExamCancellationRepository
    {
        [NotNull]
        Task<CancelledExamsPage> GetCancelled(DbConnection connection, int? regionId, int take, int skip);

        [NotNull]
        Task<bool> SetCancellation(DbConnection connection, [NotNull]string participantCode, int regionId, int examId,
                                   bool isCancelled);

        [NotNull]
        Task<ICollection<CancelledParticipantExam>> GetCancelledParticipanExams(DbConnection connection);
    }
}