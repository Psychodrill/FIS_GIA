namespace Ege.Check.Dal.Store.Repositories.Appeals
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public interface IAppealRepository
    {
        Task<AppealCollectionCacheModel> GetAppeals([NotNull] DbConnection connection,
                                                    KeyValuePair<ParticipantCacheModel, int> participantExam);
    }
}