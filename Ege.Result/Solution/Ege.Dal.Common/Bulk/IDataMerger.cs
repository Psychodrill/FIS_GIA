namespace Ege.Dal.Common.Bulk
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public interface IDataMerger
    {
        [NotNull]
        Task<int> MergeData<TDto>(
            [NotNull] DbConnection connection,
            DbTransaction transaction = null);
    }
}
