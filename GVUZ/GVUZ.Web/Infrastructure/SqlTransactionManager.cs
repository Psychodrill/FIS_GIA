using System.Data;
using System.Data.SqlClient;

namespace GVUZ.Web.Infrastructure
{
    public class SqlTransactionManager : ITransactionManager
    {
        public static readonly ITransactionManager Instance = new SqlTransactionManager();

        private const string TransactionContextKey = "FIS.Transaction";
        private const string ConnectionContextKey = "FIS.Connection";

        private SqlTransactionManager()
        {
        }

        private System.Collections.IDictionary Context
        {
            get { return System.Web.HttpContext.Current.Items; }
        }

        public bool Activated
        {
            get
            {
                return Context.Contains(TransactionContextKey) ||
                       Context.Contains(ConnectionContextKey);
            }
        }

        private SqlConnection GetCurrentConnection()
        {
            SqlConnection cn = Context[ConnectionContextKey] as SqlConnection;

            if (cn == null)
            {
                cn = ConnectionProvider.CreateConnection();
                Context[ConnectionContextKey] = cn;
            }

            return cn;
        }

        private SqlTransaction GetCurrentTransaction()
        {
            SqlTransaction tx = Context[TransactionContextKey] as SqlTransaction;

            if (tx == null)
            {
                tx = GetCurrentConnection().BeginTransaction(IsolationLevel.ReadCommitted);
                Context[TransactionContextKey] = tx;
            }

            return tx;
        }

        public SqlCommand CreateCommand(CommandType commandType, string commandText = null)
        {
            return new SqlCommand(commandText, GetCurrentConnection(), GetCurrentTransaction())
                {
                    CommandType = commandType, CommandTimeout = 120
                };
        }

        public SqlBulkCopy CreateBulkCopy()
        {
            return new SqlBulkCopy(GetCurrentConnection(), SqlBulkCopyOptions.Default, GetCurrentTransaction());
        }

        public void Commit()
        {
            DisposeTransaction(true);
        }

        public void Rollback()
        {
            DisposeTransaction(false);
        }

        private void DisposeTransaction(bool commit)
        {
            SqlTransaction current = Context[TransactionContextKey] as SqlTransaction;

            if (current != null)
            {
                try
                {
                    if (commit)
                    {
                        current.Commit();
                    }
                    else
                    {
                        current.Rollback();
                    }
                }
                finally
                {
                    current.Dispose();
                    Context.Remove(TransactionContextKey);
                }
            }
        }

        public void DisposeConnection()
        {
            DisposeTransaction(false);

            SqlConnection current = Context[ConnectionContextKey] as SqlConnection;

            if (current != null)
            {
                try
                {
                    if (current.State == ConnectionState.Open)
                    {
                        current.Close();    
                    }
                }
                finally
                {
                    current.Dispose();
                    Context.Remove(ConnectionContextKey);
                }
            }
        }
    }
}