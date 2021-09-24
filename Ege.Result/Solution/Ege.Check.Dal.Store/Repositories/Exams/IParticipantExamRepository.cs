namespace Ege.Check.Dal.Store.Repositories.Exams
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public interface IParticipantExamRepository
    {
        [NotNull]
        Task<ExamCollectionCacheModel> GetByParticipant([NotNull] DbConnection connection,
                                                        [NotNull] ParticipantCacheModel participant);
    }
}