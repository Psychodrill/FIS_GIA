namespace Ege.Check.Dal.Store.Repositories.Answers
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public interface IAnswerRepository
    {
        [NotNull]
        Task<AnswerCollectionCacheModel> GetExamAnswers([NotNull] DbConnection connection,
                                                        KeyValuePair<ParticipantCacheModel, int> participantExam);
    }
}