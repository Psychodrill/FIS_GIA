namespace Ege.Check.Dal.Store.Repositories.Answers
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public interface IAnswerCriteriaRepository
    {
        [NotNull]
        Task<ExamInfoCacheModel> GetByCode([NotNull] DbConnection connection, int subjectCode);

        [NotNull]
        Task SetByCode([NotNull] DbConnection connection, int subjectCode, ExamInfoCacheModel settings);

        [NotNull]
        Task<IDictionary<int, ExamInfoCacheModel>> GetAll([NotNull] DbConnection connection);
    }
}