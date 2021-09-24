namespace Ege.Dal.Common.Factory
{
    using System.Configuration;
    using JetBrains.Annotations;

    public class ConfigConnectionStringProvider : IConnectionStringProvider
    {
        [NotNull] private readonly string _connectionName;
        [NotNull] private readonly string _hscConnectionName;

        public ConfigConnectionStringProvider()
            : this("CheckEge", "Hsc")
        {
        }

        public ConfigConnectionStringProvider([NotNull] string connectionName, [NotNull] string hscName)
        {
            _connectionName = connectionName;
            _hscConnectionName = hscName;
        }

        public string CheckEge()
        {
            return GetConnection(_connectionName);
        }

        public string Hsc()
        {
            return GetConnection(_hscConnectionName);
        }

        [NotNull]
        private static string GetConnection([NotNull] string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}