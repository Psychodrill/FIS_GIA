namespace Ege.Check.Logic.Services.Participant.Appeals
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public interface IAppealService
    {
        [NotNull]
        Task<AppealCollectionCacheModel> GetAppeals(KeyValuePair<ParticipantCacheModel, int> participantExam);
    }
}