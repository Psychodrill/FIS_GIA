namespace Ege.Check.Dal.Store.Repositories.Exams
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public interface ISubjectExamRepository
    {
        [NotNull]
        Task<KeyValuePair<IDictionary<int, SubjectMemoryCacheModel>, IDictionary<int, ExamMemoryCacheModel>>> GetAll(
            [NotNull] DbConnection connection);
    }
}