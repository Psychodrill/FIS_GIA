using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace FbsService.Fbs
{
    public class CompetitionRequestBatchTask : Task
    {
        class ParseData : TaskStatus
        {
            public const string StatusCode = "parse";

            private const string CreateTempCheckStatement =
                    "create table #CompetitionCertificateRequest \r\n" +
                    "    ( \r\n" +
                    "    LastName nvarchar(255) \r\n" +
                    "    , FirstName nvarchar(255) \r\n" +
                    "    , PatronymicName nvarchar(255) \r\n" +
                    "    ) ";

            private const string DropTempCheckStatement =
                    "drop table #CompetitionCertificateRequest ";

            private const string InsertTempCheckStatement =
                    "insert into #CompetitionCertificateRequest \r\n" +
                    "values (@lastName, @firstName, @patronymicName)";

            private const string CommitSPName =
                    "dbo.PrepareCompetitionCertificateRequest";

            protected internal override string GetStatusCode()
            {
                return StatusCode;
            }

            private CompetitionRequestBatch Batch
            {
                get
                {
                    return ((CompetitionRequestBatchTask)Task).Batch;
                }
            }

            private void BeginParsing(SqlConnection connection)
            {
                using (SqlCommand cmdCreate = connection.CreateCommand())
                {
                    cmdCreate.CommandText = CreateTempCheckStatement;
                    cmdCreate.ExecuteNonQuery();
                }
            }

            private void EndParsing(SqlConnection connection)
            {
                if (connection.State != ConnectionState.Open)
                    return;
                using (SqlCommand cmdDrop = connection.CreateCommand())
                {
                    cmdDrop.CommandText = DropTempCheckStatement;
                    cmdDrop.ExecuteNonQuery();
                }
            }

            private void AddRequest(SqlConnection connection, string lastName, string firstName, string patronymicName)
            {
                using (SqlCommand cmdInsert = connection.CreateCommand())
                {
                    SqlParameter[] addCheckParameters = new SqlParameter[] 
                    {
                    new SqlParameter("@lastName", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@firstName", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@patronymicName", System.Data.SqlDbType.NVarChar, 255),
                    };

                    cmdInsert.CommandText = InsertTempCheckStatement;
                    cmdInsert.Parameters.AddRange(addCheckParameters);

                    cmdInsert.Parameters["@lastName"].Value = lastName;
                    cmdInsert.Parameters["@firstName"].Value = firstName;
                    cmdInsert.Parameters["@patronymicName"].Value = patronymicName;
                    cmdInsert.ExecuteNonQuery();
                }
            }

            private void CommitParsing(SqlConnection connection)
            {
                using (SqlCommand cmdCommit = connection.CreateCommand())
                {
                    SqlParameter[] commitParsingParameters = new SqlParameter[] 
                        {
                        new SqlParameter("@batchId", System.Data.SqlDbType.BigInt),
                        };

                    cmdCommit.CommandType = CommandType.StoredProcedure;
                    cmdCommit.CommandText = CommitSPName;
                    cmdCommit.CommandTimeout = 0;
                    cmdCommit.Parameters.AddRange(commitParsingParameters);
                    cmdCommit.Parameters["@batchId"].Value = Batch.Id;
                    cmdCommit.ExecuteNonQuery();
                }
            }

            protected internal override void Execute()
            {
                using (SqlConnection connection = new SqlConnection(
                        global::FbsService.Properties.Settings.Default.FbsWebConnectionString))
                {
                    connection.Open();
                    BeginParsing(connection);
                    try
                    {
                        string lastName;
                        string firstName;
                        string patronymicName;
                        int index = 1;
                        foreach (string batchString in Batch.Batch.Split(
                                new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            string[] batchStringPart = batchString.Split('%');
                            if (batchStringPart.Length > 0)
                                lastName = batchStringPart[0];
                            else
                                lastName = string.Empty;
                            if (batchStringPart.Length > 1)
                                firstName = batchStringPart[1];
                            else
                                firstName = string.Empty;
                            if (batchStringPart.Length > 2)
                                patronymicName = batchStringPart[2];
                            else
                                patronymicName = string.Empty;

                            AddRequest(connection, lastName, firstName, patronymicName);
                            index++;
                        }
                        CommitParsing(connection);
                    }
                    catch
                    {
                        connection.Close();
                        throw;
                    }
                    finally
                    {
                        EndParsing(connection);
                    }
                }
                SetStatus(ExecuteCheck.StatusCode);
            }
        }

        class ExecuteCheck : TaskStatus
        {
            public const string StatusCode = "request";

            private const string CheckSPName = "dbo.ExecuteCompetitionCertificateRequest";

            private void CheckData(SqlConnection connection)
            {
                using (SqlCommand cmdCheck = connection.CreateCommand())
                {
                    SqlParameter[] checkParsingParameters = new SqlParameter[] 
                            {
                            new SqlParameter("@batchId", System.Data.SqlDbType.BigInt),
                            };

                    cmdCheck.CommandType = CommandType.StoredProcedure;
                    cmdCheck.CommandText = CheckSPName;
                    cmdCheck.CommandTimeout = 0;
                    cmdCheck.Parameters.AddRange(checkParsingParameters);
                    cmdCheck.Parameters["@batchId"].Value = Batch.Id;
                    cmdCheck.ExecuteNonQuery();
                }
            }

            protected internal override string GetStatusCode()
            {
                return StatusCode;
            }

            private CompetitionRequestBatch Batch
            {
                get
                {
                    return ((CompetitionRequestBatchTask)Task).Batch;
                }
            }

            protected internal override void Execute()
            {
                using (SqlConnection connection = new SqlConnection(
                        global::FbsService.Properties.Settings.Default.FbsWebConnectionString))
                {
                    connection.Open();
                    CheckData(connection);
                }
                Batch.Executing = false;
                Batch.Update();
                FinishTask();
            }
        }

        protected override string GetTaskCode()
        {
            return "CompetitionCertificateRequestBatch";
        }

        protected internal override TaskStatus GetStatus(string code)
        {
            if (code == ExecuteCheck.StatusCode)
                return new ExecuteCheck();
            else
                return new ParseData();
        }

        public long BatchId = 0;

        private CompetitionRequestBatch mBatch;

        [XmlIgnore()]
        public CompetitionRequestBatch Batch
        {
            get
            {
                if (mBatch == null)
                {
                    mBatch = CompetitionRequestBatch.GetBatch(BatchId);
                }
                return mBatch;
            }
        }
    }
}
