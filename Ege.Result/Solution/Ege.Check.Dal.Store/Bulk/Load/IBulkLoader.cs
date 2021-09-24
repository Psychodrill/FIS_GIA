namespace Ege.Check.Dal.Store.Bulk.Load
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public interface IBulkLoader
    {
        [NotNull]
        Task LoadDataAsync([NotNull] DataTable dt, [NotNull] string tableName, [NotNull] SqlConnection connection,
                           SqlTransaction transaction = null);
    }
}