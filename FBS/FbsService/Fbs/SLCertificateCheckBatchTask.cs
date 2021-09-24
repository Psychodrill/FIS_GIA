using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace FbsService.Fbs
{
    public class SLCertificateCheckBatchTask : Task
    {
        class ParseData : TaskStatus
        {
            public const string StatusCode = "parse";

            private const string CreateTempCheckStatement =
                    "create table #SchoolLeavingCertificateCheck \r\n" +
                    "    ( \r\n" +
                    "    [Index] bigint \r\n" +
                    "    , CertificateNumber nvarchar(255) \r\n" +
                    "    ) ";

            private const string DropTempCheckStatement =
                    "drop table #SchoolLeavingCertificateCheck ";

            private const string InsertTempCheckStatement =
                    "insert into #SchoolLeavingCertificateCheck \r\n" +
                    "values (@index, @certificateNumber)";

            private const string CommitSPName =
                    "dbo.PrepareSchoolLeavingCertificateCheck";

            protected internal override string GetStatusCode()
            {
                return StatusCode;
            }

            private SLCertificateCheckBatch Batch
            {
                get
                {
                    return ((SLCertificateCheckBatchTask)Task).Batch;
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

            private void AddCheck(SqlConnection connection, long index, string certificateNumber)
            {
                using (SqlCommand cmdInsert = connection.CreateCommand())
                {
                    SqlParameter[] addCheckParameters = new SqlParameter[] 
                    {
                    new SqlParameter("@index", System.Data.SqlDbType.BigInt),
                    new SqlParameter("@certificateNumber", System.Data.SqlDbType.NVarChar, 255),
                    };

                    cmdInsert.CommandText = InsertTempCheckStatement;
                    cmdInsert.Parameters.AddRange(addCheckParameters);

                    cmdInsert.Parameters["@index"].Value = index;
                    cmdInsert.Parameters["@certificateNumber"].Value = certificateNumber;
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
                        string certificateNumber;
                        long index = 1;
                        foreach (string batchString in Batch.Batch.Split(
                                new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            certificateNumber = batchString;
                            AddCheck(connection, index, certificateNumber);
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
            public const string StatusCode = "check";

            private const string CheckSPName = "dbo.ExecuteSchoolLeavingCertificateCheck";

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

            private SLCertificateCheckBatch Batch
            {
                get
                {
                    return ((SLCertificateCheckBatchTask)Task).Batch;
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
            return "SchoolLeavingCertificateCheckBatch";
        }

        protected internal override TaskStatus GetStatus(string code)
        {
            if (code == ExecuteCheck.StatusCode)
                return new ExecuteCheck();
            else
                return new ParseData();
        }

        public long BatchId = 0;

        private SLCertificateCheckBatch mBatch;

        [XmlIgnore()]
        public SLCertificateCheckBatch Batch
        {
            get
            {
                if (mBatch == null)
                {
                    mBatch = SLCertificateCheckBatch.GetBatch(BatchId);
                }
                return mBatch;
            }
        }
    }
}
