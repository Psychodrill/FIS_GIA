namespace Ege.Check.Dal.Store.Bulk.Load
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    internal class BulkLoader : IBulkLoader
    {
        public async Task LoadDataAsync(DataTable dt, string tableName, SqlConnection connection,
                                        SqlTransaction transaction = null)
        {
            using (var bcp = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
            {
                bcp.BulkCopyTimeout = 0;
                bcp.ColumnMappings.Clear();
                bcp.DestinationTableName = string.Format("[{0}]", tableName);
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    bcp.ColumnMappings.Add(new SqlBulkCopyColumnMapping(dataColumn.ColumnName, dataColumn.ColumnName));
                }
                await bcp.WriteToServerAsync(dt);
            }
        }
    }
}