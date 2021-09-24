namespace Ege.Check.Dal.Store.Bulk.Load
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public interface IDataMerger
    {
        [NotNull]
        Task MergeData<TDto>([NotNull] string fullTableName, [NotNull] DbConnection connection,
                             DbTransaction transaction = null);
    }
}