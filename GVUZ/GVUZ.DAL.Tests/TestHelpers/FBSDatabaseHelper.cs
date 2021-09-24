using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GVUZ.DAL.Tests.TestHelpers
{
    internal class FBSDatabaseHelper : DatabaseHelperBase
    {
        protected override string MigrationsPath
        {
            get { return Config.FBSMigrationsFolderPath; }
        }

        protected override string TestBackupDataFileLogicalName
        {
            get { return Config.FBSBackupDataFileLogicalName; }
        }

        protected override string TestBackupLogFileLogicalName
        {
            get { return Config.FBSBackupLogFileLogicalName; }
        }

        protected override string TestDBBackupPath
        {
            get { return Config.FBSBackupPath; }
        }

        protected override string TestDBConnectionString
        {
            get { return Config.FBSConnectionString; }
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
