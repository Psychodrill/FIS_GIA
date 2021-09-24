namespace Ege.Check.Logic.Services.Participant.ExamLists
{
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Models.Response;
    using JetBrains.Annotations;

    public interface IExamListService
    {
        [NotNull]
        Task<ExamListResponse> GetExamList([NotNull] ParticipantCacheModel participant);
    }
}