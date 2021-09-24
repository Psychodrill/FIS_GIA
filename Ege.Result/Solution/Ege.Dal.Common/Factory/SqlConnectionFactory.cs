namespace Ege.Dal.Common.Factory
{
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Factory;
    using JetBrains.Annotations;
    using global::Common.Logging;

    public class SqlConnectionFactory : IDbConnectionFactory, IConnectionFactory<SqlConnection>
    {
        [NotNull] private static readonly ILog Logger = LogManager.GetLogger<SqlConnectionFactory>();
        [NotNull] private readonly IConnectionStringProvider _connectionString;

        public SqlConnectionFactory([NotNull] IConnectionStringProvider connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<SqlConnection> CreateAsync()
        {
            return await CreateAsync<SqlConnection>(_connectionString.CheckEge());
        }

        public async Task<SqlConnection> CreateHscAsync()
        {
            return await CreateAsync<SqlConnection>(_connectionString.Hsc());
        }

        async Task<DbConnection> IConnectionFactory<DbConnection>.CreateAsync()
        {
            return await CreateAsync();
        }

        async Task<DbConnection> IConnectionFactory<DbConnection>.CreateHscAsync()
        {
            return await CreateHscAsync();
        }

        public DbConnection CreateSync()
        {
            return CreateSync(_connectionString.CheckEge());
        }

        public DbConnection CreateHscSync()
        {
            return CreateSync(_connectionString.Hsc());
        }

        [NotNull]
        private DbConnection CreateSync([NotNull] string connectionString)
        {
            var result = new SqlConnection(connectionString);
            result.Open();
            return result;
        }

        [NotNull]
        private async Task<T> CreateAsync<T>([NotNull] string connectionString)
            where T : DbConnection, new()
        {
            var result = new T {ConnectionString = connectionString};
            await result.OpenAsync();
            //var csb = new SqlConnectionStringBuilder(connectionString) {UserID = null, Password = null};
            Logger.TraceFormat("SQL Server connection {0} created", result.Database);
            return result;
        }
    }
}
