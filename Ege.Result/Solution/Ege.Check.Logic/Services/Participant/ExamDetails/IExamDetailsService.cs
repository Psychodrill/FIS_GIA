namespace Ege.Check.Logic.Services.Participant.ExamDetails
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Models.Response;
    using JetBrains.Annotations;

    public interface IExamDetailsService
    {
        [NotNull]
        Task<ExamResponse> GetExamDetails(KeyValuePair<ParticipantCacheModel, int> participantExam);
    }
}