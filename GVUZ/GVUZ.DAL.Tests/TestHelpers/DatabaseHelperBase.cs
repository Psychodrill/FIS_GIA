using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace GVUZ.DAL.Tests.TestHelpers
{
    internal abstract class DatabaseHelperBase
    {
        protected abstract string TestDBConnectionString { get; }
        protected abstract string TestDBBackupPath { get; }
        protected abstract string TestBackupDataFileLogicalName { get; }
        protected abstract string TestBackupLogFileLogicalName { get; }
        protected abstract string MigrationsPath { get; }
        public abstract IEnumerable<string> GetOrderedMigrationsFromDB();

        public void CreatEmptyDatabase()
        {
            EnsureDatabaseNotExists();

            using (SqlConnection conn = new SqlConnection(Config.MasterDBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = String.Format("CREATE DATABASE {0}", TestDatabaseName);
                cmd.ExecuteNonQuery();
            }
        }

        public void CreateStructure()
        {
            RestoreFromBackup();
            ApplyMigrations(false);
        }

        public void TryCreateStructure()
        {
            RestoreFromBackup();
            ApplyMigrations(true);
        }

        public void DropDatabase()
        {
            using (SqlConnection conn = new SqlConnection(Config.MasterDBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = String.Format("ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE", TestDatabaseName);
                cmd.ExecuteNonQuery();
                cmd.CommandText = String.Format("DROP DATABASE {0}", TestDatabaseName);
                cmd.ExecuteNonQuery();
            }
        }

        public static string GetDatabaseName(string connectionString)
        {
            List<string> databaseKeyWords = new List<string>()
            {
                "database="
                ,"initial catalog="
                ,"database ="
                ,"initial catalog ="
            };

            string connectionStringLower = connectionString.ToLower();
            int databaseKeyWordIndex = -1;
            string foundKeyword = null;
            foreach (string keyword in databaseKeyWords)
            {
                databaseKeyWordIndex = connectionStringLower.IndexOf(keyword);
                if (databaseKeyWordIndex >= 0)
                {
                    foundKeyword = keyword;
                    break;
                }
            }
            if (databaseKeyWordIndex == -1)
                throw new SystemException();

            int commaIndex = connectionStringLower.IndexOf(";", databaseKeyWordIndex);
            int length = commaIndex - databaseKeyWordIndex - foundKeyword.Length;
            string result = connectionString.Substring(databaseKeyWordIndex + foundKeyword.Length, length);
            return result;
        }

        private void EnsureDatabaseNotExists()
        {
            try
            {
                DropDatabase();
            }
            catch (Exception ex)
            {
            }
        }

        private void RestoreFromBackup()
        {
            using (SqlConnection conn = new SqlConnection(Config.MasterDBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = String.Format("SELECT * FROM {0}.sys.database_files", TestDatabaseName);
                List<DatabaseFile> files = new List<DatabaseFile>();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        files.Add(new DatabaseFile(reader));
                    }
                }

                cmd.CommandText = String.Format(@"
RESTORE DATABASE {0} 
FROM  DISK = N'{1}' 
WITH  
MOVE N'{2}' TO N'{3}',  
MOVE N'{4}' TO N'{5}',  
FILE = 1,  
REPLACE", TestDatabaseName, TestDBBackupPath
        , TestBackupDataFileLogicalName, files.FirstOrDefault(x => x.IsDataFile).Path
        , TestBackupLogFileLogicalName, files.FirstOrDefault(x => x.IsLogFile).Path);

                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<string> AppliedMigrations { get { return _appliedMigrationsList; } }
        private List<string> _appliedMigrationsList = new List<string>();

        private void ApplyMigrations(bool ignoreExceptions)
        {
            _appliedMigrationsList.Clear();

            using (SqlConnection conn = new SqlConnection(TestDBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();

                IEnumerable<string> files = Directory.EnumerateFiles(MigrationsPath).Where(x => x.EndsWith(".sql"));
                IEnumerable<MigrationFile> orderedFiles = files.Select(x => new MigrationFile(x)).OrderBy(x => x.Order);

                foreach (MigrationFile migrationFile in orderedFiles)
                {
                    string migrationSql = migrationFile.Sql;
                    migrationSql = migrationSql.Replace("GOTO", "GTGT");
                    foreach (string sqlPart in migrationSql.Split(new string[] { "GO" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        cmd.CommandText = ApplyMigrationSqlReplacements(sqlPart.Replace("GTGT", "GOTO"));
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            if (!ignoreExceptions)
                                throw new Exception(String.Format("Не удалось выполнить скрипт {0}: {1}", migrationFile.Name, ex.Message), ex);
                        }
                    }
                    _appliedMigrationsList.Add(migrationFile.Name);
                }
            }
        }

        protected virtual string ApplyMigrationSqlReplacements(string sql)
        {
            return sql;
        }

        private string TestDatabaseName
        {
            get
            {
                return GetDatabaseName(TestDBConnectionString);
            }
        }

        private class MigrationFile
        {
            public int Order { get; private set; }
            public string Path { get; private set; }
            public string Name { get; private set; }
            public string Sql { get; private set; }

            public MigrationFile(string path)
            {
                Path = path;
                try
                {
                    Order = Int32.Parse(path.Split('\\').Last().Split('_')[0]);
                    Name = path.Split('\\').Last().Replace(".sql", "");
                }
                catch
                {
                    throw new Exception("Название миграционного скрипта имеет неверный формат");
                }
                Sql = File.ReadAllText(path);
            }
        }

        private class DatabaseFile
        {
            public string Name { get; private set; }
            public string Path { get; private set; }
            public string Desc { get; private set; }

            public bool IsDataFile { get { return Desc == "ROWS"; } }
            public bool IsLogFile { get { return Desc == "LOG"; } }

            public DatabaseFile(IDataReader reader)
            {
                Name = reader["name"].ToString();
                Path = reader["physical_name"].ToString();
                Desc = reader["type_desc"].ToString();
            }
        }
    }
}
