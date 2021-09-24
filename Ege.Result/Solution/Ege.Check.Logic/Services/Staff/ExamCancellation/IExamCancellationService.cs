namespace Ege.Check.Logic.Services.Staff.ExamCancellation
{
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Staff;
    using JetBrains.Annotations;

    public interface IExamCancellationService
    {
        [NotNull]
        Task<bool> Cancel([NotNull]string participantCode, int regionId, int examId);

        [NotNull]
        Task<bool> Uncancel([NotNull]string participantCode, int regionId, int examId);

        [NotNull]
        Task<CancelledExamsPage> Get(int? regionId, int take, int skip);
    }
}