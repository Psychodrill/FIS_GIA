using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GVUZ.DAL.Tests.TestHelpers
{
    internal class ESRPDatabaseHelper : DatabaseHelperBase
    {
        protected override string MigrationsPath
        {
            get { return Config.ESRPMigrationsFolderPath; }
        }

        protected override string TestBackupDataFileLogicalName
        {
            get { return Config.ESRPBackupDataFileLogicalName; }
        }

        protected override string TestBackupLogFileLogicalName
        {
            get { return Config.ESRPBackupLogFileLogicalName; }
        }

        protected override string TestDBBackupPath
        {
            get { return Config.ESRPBackupPath; }
        }

        protected override string TestDBConnectionString
        {
            get { return Config.ESRPConnectionString; }
        }

        public override IEnumerable<string> GetOrderedMigrationsFromDB()
        {
            List<string> result = new List<string>();
            using (SqlConnection conn = new SqlConnection(TestDBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = String.Format("SELECT MigrationName FROM Migrations ORDER BY CreateDate DESC");
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetString(0));
                    }
                }
            }
            return result;
        }
    }
}
