using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace GVUZ.DAL.Tests.TestHelpers
{
    internal class GVUZDatabaseHelper : DatabaseHelperBase
    {
        protected override string MigrationsPath
        {
            get { return Config.GVUZMigrationsFolderPath; }
        }

        protected override string TestBackupDataFileLogicalName
        {
            get { return Config.GVUZBackupDataFileLogicalName; }
        }

        protected override string TestBackupLogFileLogicalName
        {
            get { return Config.GVUZBackupLogFileLogicalName; }
        }

        protected override string TestDBBackupPath
        {
            get { return Config.GVUZBackupPath; }
        }

        protected override string TestDBConnectionString
        {
            get { return Config.GVUZConnectionString; }
        }

        protected override string ApplyMigrationSqlReplacements(string sql)
        {
            string esrpDBName = GetDatabaseName(Config.ESRPConnectionString) + ".dbo.";
            string fbsDBName = GetDatabaseName(Config.FBSConnectionString);
            sql = sql
                .Replace("esrp.dbo.", esrpDBName)
                .Replace("ESRP.dbo.", esrpDBName)
                .Replace("FBS_2015_Debug", fbsDBName);
            return sql;
        }

        public override IEnumerable<string> GetOrderedMigrationsFromDB()
        {
            List<string> result = new List<string>();
            using (SqlConnection conn = new SqlConnection(TestDBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = String.Format("SELECT ScriptName FROM Migrations ORDER BY ScriptDate DESC");
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

        public void ExecSyncEntrant(int entrantId)
        {
            using (SqlConnection conn = new SqlConnection(TestDBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SyncEntrant";
                cmd.Parameters.AddWithValue("entrantId", entrantId);
                cmd.ExecuteNonQuery();
            }
        }

        public void ExecSyncEntrantMultiple(IEnumerable<int> entrantIds)
        {
            if (entrantIds == null)
                throw new ArgumentNullException("entrantIds");
            if (entrantIds.GroupBy(x => x).Any(x => x.Count() > 1))
                throw new ArgumentException("Список содержит дублирующиеся значения", "entrantIds");

            using (SqlConnection conn = new SqlConnection(TestDBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SyncEntrantMultiple";

                DataTable entrantIdsTable = new DataTable();
                entrantIdsTable.Columns.Add("id", typeof(int));
                foreach (int id in entrantIds)
                {
                    entrantIdsTable.Rows.Add(id);
                }

                cmd.Parameters.AddWithValue("entrantIds", entrantIdsTable);
                cmd.ExecuteNonQuery();
            }
        }

        public FindOlympicDiplomantRVIPersonsResult ExecFindOlympicDiplomantRVIPersons(PersonData personData)
        {
            using (SqlConnection conn = new SqlConnection(TestDBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "FindOlympicDiplomantRVIPersons";

                cmd.Parameters.Add(CreateSPParameter("firstName", SqlDbType.VarChar, ParameterDirection.Input, personData.FirstName));
                cmd.Parameters.Add(CreateSPParameter("lastName", SqlDbType.VarChar, ParameterDirection.Input, personData.LastName));
                cmd.Parameters.Add(CreateSPParameter("patronymic", SqlDbType.VarChar, ParameterDirection.Input, personData.Patronymic));
                cmd.Parameters.Add(CreateSPParameter("documentNumber", SqlDbType.VarChar, ParameterDirection.Input, personData.DocumentNumber));
                cmd.Parameters.Add(CreateSPParameter("documentSeries", SqlDbType.VarChar, ParameterDirection.Input, personData.DocumentSeries));
                cmd.Parameters.Add(CreateSPParameter("birthDate", SqlDbType.DateTime, ParameterDirection.Input, (personData.BirthDate == null) ? (object)DBNull.Value : (object)personData.BirthDate.Value));
                cmd.Parameters.Add(CreateSPParameter("statusId", SqlDbType.Int, ParameterDirection.Output, null));
                cmd.Parameters.Add(CreateSPParameter("statusName", SqlDbType.VarChar, ParameterDirection.Output, null));

                FindOlympicDiplomantRVIPersonsResult result = new FindOlympicDiplomantRVIPersonsResult();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.PersonIds.Add((int)reader["PersonId"]);
                    }
                }

                if (!(cmd.Parameters["statusId"].Value is int))
                    return null;

                result.StatusId = cmd.Parameters["statusId"].Value as int?;
                result.StatusName = cmd.Parameters["statusName"].Value as string;

                return result;
            }
        }

        public FindOlympicDiplomantRVIPersonsResult ExecFindOlympicDiplomantRVIPersonsMultiple(IEnumerable<PersonData> personDatas)
        {
            using (SqlConnection conn = new SqlConnection(TestDBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "FindOlympicDiplomantRVIPersonsMultiple";

                DataTable olympicDiplomantDataTable = new DataTable();
                olympicDiplomantDataTable.Columns.Add("Id", typeof(long));
                olympicDiplomantDataTable.Columns.Add("LastName", typeof(string));
                olympicDiplomantDataTable.Columns.Add("FirstName", typeof(string));
                olympicDiplomantDataTable.Columns.Add("Patronymic", typeof(string));
                olympicDiplomantDataTable.Columns.Add("DocumentNumber", typeof(string));
                olympicDiplomantDataTable.Columns.Add("DocumentSeries", typeof(string));
                olympicDiplomantDataTable.Columns.Add("BirthDate", typeof(DateTime));
                foreach (PersonData personData in personDatas)
                {
                    olympicDiplomantDataTable.Rows.Add(0, personData.LastName, personData.FirstName, personData.Patronymic, personData.DocumentNumber, personData.DocumentSeries, personData.BirthDate);
                }

                cmd.Parameters.AddWithValue("olympicDiplomantData", olympicDiplomantDataTable);
                cmd.Parameters.Add(CreateSPParameter("statusId", SqlDbType.Int, ParameterDirection.Output, null));
                cmd.Parameters.Add(CreateSPParameter("statusName", SqlDbType.VarChar, ParameterDirection.Output, null));

                FindOlympicDiplomantRVIPersonsResult result = new FindOlympicDiplomantRVIPersonsResult();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.PersonIds.Add((int)reader["PersonId"]);
                    }
                }

                if (!(cmd.Parameters["statusId"].Value is int))
                    return null;

                result.StatusId = cmd.Parameters["statusId"].Value as int?;
                result.StatusName = cmd.Parameters["statusName"].Value as string;

                return result;
            }
        }

        public void ExecSyncOlympicDiplomantMultiple(IEnumerable<long> olympicDiplomantIds)
        {
            if (olympicDiplomantIds == null)
                throw new ArgumentNullException("olympicDiplomantIds");
            if (olympicDiplomantIds.GroupBy(x => x).Any(x => x.Count() > 1))
                throw new ArgumentException("Список содержит дублирующиеся значения", "olympicDiplomantIds");

            using (SqlConnection conn = new SqlConnection(TestDBConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SyncOlympicDiplomantMultiple";

                DataTable olympicDiplomantIdsTable = new DataTable();
                olympicDiplomantIdsTable.Columns.Add("id", typeof(long));
                foreach (long id in olympicDiplomantIds)
                {
                    olympicDiplomantIdsTable.Rows.Add(id);
                }

                cmd.Parameters.AddWithValue("diplomantIds", olympicDiplomantIdsTable);
                cmd.ExecuteNonQuery();
            }
        }

        private SqlParameter CreateSPParameter(string name, SqlDbType sqlDbType, ParameterDirection direction, object value)
        {
            SqlParameter result = new SqlParameter();
            result.ParameterName = name;
            result.SqlDbType = sqlDbType;
            if ((result.SqlDbType == SqlDbType.VarChar) || (result.SqlDbType == SqlDbType.NVarChar))
            {
                result.Size = 1000;
            }
            result.Direction = direction;
            if ((result.Direction != ParameterDirection.Output) && (result.Direction != ParameterDirection.ReturnValue))
            {
                result.Value = value;
            }
            return result;
        }


        public class PersonData
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Patronymic { get; set; }
            public DateTime? BirthDate { get; set; }
            public string DocumentNumber { get; set; }
            public string DocumentSeries { get; set; }
        }

        public class FindOlympicDiplomantRVIPersonsResult
        {
            public FindOlympicDiplomantRVIPersonsResult()
            {
                PersonIds = new List<int>();
            }

            public int? StatusId { get; set; }
            public string StatusName { get; set; }

            public List<int> PersonIds { get; private set; }
        }
    }
}
