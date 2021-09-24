namespace Ege.Check.Dal.Store.Repositories.PagesCount
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Models.Servers;
    using JetBrains.Annotations;

    public interface IPagesCountRepository
    {
        [NotNull]
        Task Merge(
            [NotNull]SqlConnection connection,
            [NotNull]IEnumerable<KeyValuePair<KeyValuePair<int, ExamMemoryCacheModel>, ICollection<PageCountData>>> received);
    }
}
