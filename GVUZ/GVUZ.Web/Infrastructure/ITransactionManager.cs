using System.Data;
using System.Data.SqlClient;

namespace GVUZ.Web.Infrastructure
{
    public interface ITransactionManager
    {
        SqlCommand CreateCommand(CommandType commandType, string commandText = null);
        SqlBulkCopy CreateBulkCopy();
    }
}