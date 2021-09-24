using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace FbsService.FbsCheck
{
    public class CNERequestBatchTask : Task
    {
        class ParseData : TaskStatus
        {
            private class InvalidRequestBatchParseParamException : Exception
            {
                private const string MessageFormat = "Incorrect parsing params in request batch {0}";

                public InvalidRequestBatchParseParamException(long batchId, Exception ex)
                    : base(string.Format(MessageFormat, batchId), ex)
                {
                }
            }

            public const string StatusCode = "parse";

            private const string CreateTempRequestStatement =
                    "create table #CommonNationalExamCertificateRequest \r\n" +
                    "    ( \r\n" +
                    "    [Index] bigint \r\n" +
                    "    , BatchId bigint \r\n" +
                    "    , LastName nvarchar(255) \r\n" +
                    "    , FirstName nvarchar(255) null\r\n" +
                    "    , PatronymicName nvarchar(255) null\r\n" +
                    "    , PassportSeria nvarchar(255) null\r\n" +
                    "    , PassportNumber nvarchar(255) null\r\n" +
                    "    , TypographicNumber nvarchar(255) null\r\n" +
                    "    ) ";

            private const string DropTempRequestStatement =
                    "drop table #CommonNationalExamCertificateRequest";

            private const string InsertTempRequestStatement =
                    "insert into #CommonNationalExamCertificateRequest \r\n" +
                    "values (@index, @batchId, @lastName, @firstName, @patronymicName, @passportSeria, @passportNumber, @typographicNumber)";

            private const string CommitSPName =
                    "dbo.PrepareCommonNationalExamCertificateRequest";

            protected internal override string GetStatusCode()
            {
                return StatusCode;
            }

            private CNERequestBatch Batch
            {
                get
                {
                    return ((CNERequestBatchTask)Task).Batch;
                }
            }

            private void BeginParsing(SqlConnection connection)
            {
                using (SqlCommand cmdCreate = connection.CreateCommand())
                {
                    cmdCreate.CommandText = CreateTempRequestStatement;
                    cmdCreate.ExecuteNonQuery();
                }
            }

            private void EndParsing(SqlConnection connection)
            {
                if (connection.State != ConnectionState.Open)
                    return;
                using (SqlCommand cmdDrop = connection.CreateCommand())
                {
                    cmdDrop.CommandText = DropTempRequestStatement;
                    cmdDrop.ExecuteNonQuery();
                }
            }

            private void AddRequest(SqlConnection connection, long index, string lastName, string firstName,
                    string patronymicName, string passportSeria, string passportNumber, string typographicNumber)
            {
                using (SqlCommand cmdInsert = connection.CreateCommand())
                {
                    SqlParameter[] addCheckParameters = new SqlParameter[] 
                    {
                    new SqlParameter("@index", System.Data.SqlDbType.BigInt),
                    new SqlParameter("@batchId", System.Data.SqlDbType.BigInt),
                    new SqlParameter("@lastName", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@firstName", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@patronymicName", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@passportSeria", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@passportNumber", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@typographicNumber", System.Data.SqlDbType.NVarChar, 255),
                    };

                    cmdInsert.CommandText = InsertTempRequestStatement;
                    cmdInsert.Parameters.AddRange(addCheckParameters);

                    cmdInsert.Parameters["@index"].Value = index;
                    cmdInsert.Parameters["@batchId"].Value = Batch.Id;
                    cmdInsert.Parameters["@lastName"].Value = lastName;
                    cmdInsert.Parameters["@firstName"].Value = firstName.ToDbFriendlyString();
                    cmdInsert.Parameters["@patronymicName"].Value = patronymicName.ToDbFriendlyString();
                    cmdInsert.Parameters["@passportSeria"].Value = passportSeria.ToDbFriendlyString();
                    cmdInsert.Parameters["@passportNumber"].Value = passportNumber.ToDbFriendlyString();
                    cmdInsert.Parameters["@typographicNumber"].Value = typographicNumber.ToDbFriendlyString();
                    cmdInsert.ExecuteNonQuery();
                }
            }

            private void CommitParsing(SqlConnection connection)
            {
                using (SqlCommand cmdCommit = connection.CreateCommand())
                {
                    SqlParameter[] commitParsingParameters = new SqlParameter[] 
                            {
                            new SqlParameter("@batchId", System.Data.SqlDbType.BigInt)
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
                using (var connection = new SqlConnection(Properties.Settings.Default.FbsCheckConnectionString))
                {
                    connection.Open();
                    BeginParsing(connection);
                    try
                    {
                        long index = 1;
                        foreach (string batchString in Batch.Batch.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
                            try
                            {
                                string lastName,
                                       firstName,
                                       patronymicName,
                                       passportSeria = null,
                                       passportNumber = null,
                                       typographicNumber = null;

                                string[] batchStringPart = batchString.Split('%');
                                if (batchStringPart.Length == 4) //типографский входной файл
                                {
                                    typographicNumber = batchStringPart[0].FullTrim();
                                    lastName = batchStringPart[1].FullTrim();
                                    firstName = batchStringPart[2].FullTrim();
                                    patronymicName = batchStringPart[3].FullTrim();
                                }
                                else if (batchStringPart.Length == 5) //не типографский входной файл
                                {
                                    lastName = batchStringPart[0].FullTrim();
                                    firstName = batchStringPart[1].FullTrim();
                                    patronymicName = batchStringPart[2].FullTrim();
                                    passportSeria = batchStringPart[3].FullTrim();
                                    passportNumber = batchStringPart[4].FullTrim();
                                }
                                else
                                    throw new Exception("Неверное число параметров при разборе строки запроса.");

                                AddRequest(connection, index, lastName, firstName, patronymicName, passportSeria,
                                           passportNumber, typographicNumber);
                                index++;
                            }
                            catch (Exception ex)
                            {
                                TaskManager.Instance().Logger.Error(new InvalidRequestBatchParseParamException(Batch.Id, ex));
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

            private const string CheckSPName = "dbo.ExecuteCommonNationalExamCertificateRequest";

            private void RequestData(SqlConnection connection)
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

            private CNERequestBatch Batch
            {
                get
                {
                    return ((CNERequestBatchTask)Task).Batch;
                }
            }

            protected internal override void Execute()
            {
                using (SqlConnection connection = new SqlConnection(
                        global::FbsService.Properties.Settings.Default.FbsCheckConnectionString))
                {
                    connection.Open();
                    RequestData(connection);
                }
                Batch.Executing = false;
                Batch.Update();
                FinishTask();
            }
        }

        protected override string GetTaskCode()
        {
            return "CommonNationalExamCertificateBatchRequest";
        }

        protected internal override TaskStatus GetStatus(string code)
        {
            if (code == ExecuteCheck.StatusCode)
                return new ExecuteCheck();
            else
                return new ParseData();
        }

        public long BatchId = 0;

        private CNERequestBatch mBatch;

        [XmlIgnore()]
        public CNERequestBatch Batch
        {
            get
            {
                if (mBatch == null)
                {
                    mBatch = CNERequestBatch.GetBatch(BatchId);
                }
                return mBatch;
            }
        }
    }
}
