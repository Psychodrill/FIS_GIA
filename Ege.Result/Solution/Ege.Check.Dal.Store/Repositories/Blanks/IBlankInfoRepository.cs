namespace Ege.Check.Dal.Store.Repositories.Blanks
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Servers;
    using JetBrains.Annotations;

    public interface IBlankInfoRepository
    {
        [NotNull]
        Task<ICollection<UpdatedBlankInfo>> UpdatePageCount([NotNull]DbConnection connection, int regionId, ICollection<PageCountData> pageCountData);

        [NotNull]
        Task<ICollection<UpdatedBlankInfo>> GetAllBlanksWithCompositionPageCount([NotNull] DbConnection connection);
    }
}
