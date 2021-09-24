using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;

namespace FbsService.Fbs
{
    public class SLCertificateDenyImportTask : Task
    {
        class Search : TaskStatus
        {
            static string LoadingDirectory = ConfigurationManager.AppSettings["SchoolLeavingCertificateDenyLoadingDirectory"];
            static string LogFileName = ConfigurationManager.AppSettings["SchoolLeavingCertificateDenyLoadingLog"];
            static long EditorAccountId = long.Parse(ConfigurationManager.AppSettings["LoaderAccountId"]);
            static string EditorIp = ConfigurationManager.AppSettings["LoaderIp"];
            
            private const string CreateTempImportStatement =
                    "create table #SchoolLeavingCertificateDeny \r\n" +
                    "    ( \r\n" +
                    "    CertificateNumberFrom nvarchar(255) \r\n" +
                    "    , CertificateNumberTo nvarchar(255) \r\n" +
                    "    , OrganizationName nvarchar(255) \r\n" +
                    "    , Comment ntext \r\n" +
                    "    , EditorAccountId bigint \r\n" +
                    "    , EditorIp nvarchar(255) \r\n" +
                    "    ) ";

            private const string DropTempImportStatement =
                    "drop table #SchoolLeavingCertificateDeny ";

            private const string InsertTempImportStatement =
                    "insert into #SchoolLeavingCertificateDeny \r\n" +
                    "values (@certificateNumberFrom, @certificateNumberTo, @organizationName \r\n" +
                    "   , @comment, @editorAccountId, @editorIp)";

            private const string CommitSPName =
                    "dbo.ImportSchoolLeavingCertificateDeny";

            protected internal override string GetStatusCode()
            {
                return "search";
            }

            private void WriteLog(string errorMessage, int? lineIndex)
            {
                using (FileStream stream = File.Open(LogFileName, FileMode.Append, FileAccess.Write,
                        FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding(1251)))
                    if (lineIndex != null)
                        writer.WriteLine(string.Format("{0:u}. Cтрока {2}: {1}", DateTime.Now, errorMessage, lineIndex));
                    else
                        writer.WriteLine(string.Format("{0:u}. {1}", DateTime.Now, errorMessage));
            }

            private void AddError(string errorMessage, int lineIndex)
            {
                WriteLog(errorMessage, lineIndex);
            }

            private void AddCompetitionCertificate(SqlConnection connection, string certificateNumberFrom,
                    string certificateNumberTo, string comment)
            {
                using (SqlCommand cmdInsert = connection.CreateCommand())
                {
                    SqlParameter[] addParameters = new SqlParameter[] 
                    {
                    new SqlParameter("@certificateNumberFrom", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@certificateNumberTo", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@organizationName", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@comment", System.Data.SqlDbType.NText),
                    new SqlParameter("@editorAccountId", System.Data.SqlDbType.BigInt),
                    new SqlParameter("@editorIp", System.Data.SqlDbType.NVarChar, 255),
                    };

                    cmdInsert.CommandText = InsertTempImportStatement;
                    cmdInsert.Parameters.AddRange(addParameters);

                    cmdInsert.Parameters["@certificateNumberFrom"].Value = certificateNumberFrom;
                    cmdInsert.Parameters["@certificateNumberTo"].Value = certificateNumberTo;
                    cmdInsert.Parameters["@organizationName"].Value = DBNull.Value;
                    cmdInsert.Parameters["@comment"].Value = comment;
                    cmdInsert.Parameters["@editorAccountId"].Value = EditorAccountId;
                    cmdInsert.Parameters["@editorIp"].Value = EditorIp;
                    cmdInsert.ExecuteNonQuery();
                }
            }

            private void BeginParsing(SqlConnection connection)
            {
                using (SqlCommand cmdCreate = connection.CreateCommand())
                {
                    cmdCreate.CommandText = CreateTempImportStatement;
                    cmdCreate.ExecuteNonQuery();
                }
            }

            private void EndParsing(SqlConnection connection)
            {
                if (connection.State != ConnectionState.Open)
                    return;
                using (SqlCommand cmdDrop = connection.CreateCommand())
                {
                    cmdDrop.CommandText = DropTempImportStatement;
                    cmdDrop.ExecuteNonQuery();
                }
            }

            private void CommitParsing(SqlConnection connection)
            {
                using (SqlCommand cmdCommit = connection.CreateCommand())
                {
                    cmdCommit.CommandType = CommandType.StoredProcedure;
                    cmdCommit.CommandText = CommitSPName;
                    cmdCommit.CommandTimeout = 0;
                    cmdCommit.ExecuteNonQuery();
                }
            }

            private void Parse(string fileName)
            {
                if (File.Exists(LogFileName))
                    File.Delete(LogFileName);

                using (FileStream sourceFileStream = File.Open(fileName, FileMode.Open, FileAccess.Read,
                        FileShare.None))
                using (StreamReader reader = new StreamReader(sourceFileStream, Encoding.GetEncoding(1251)))
                using (SqlConnection connection = new SqlConnection(
                        global::FbsService.Properties.Settings.Default.FbsWebConnectionString))
                {
                    connection.Open();
                    BeginParsing(connection);
                    try
                    {
                        string line;
                        int lineIndex = 0;
                        string errorMessage;

                        string certificateNumberFrom = null;
                        string certificateNumberTo = null;
                        string comment = null;

                        WriteLog("Начата обработка данных...", null);
                        while ((line = reader.ReadLine()) != null)
                        {
                            line = line.Trim();
                            lineIndex++;

                            if (string.IsNullOrEmpty(line))
                                continue;
                            string[] parts = line.Split('#');
                            errorMessage = null;

                            if (parts.Length != 3)
                                errorMessage = "Некорректное количество параметров";

                            if (errorMessage == null)
                            {
                                certificateNumberFrom = parts[0].Trim();
                                if (string.IsNullOrEmpty(certificateNumberFrom))
                                    errorMessage = "Не указан номер начала интервала";
                            }
                            if (errorMessage == null)
                            {
                                certificateNumberTo = parts[1].Trim();
                                if (string.IsNullOrEmpty(certificateNumberTo))
                                    errorMessage = "Не указан номер завершения интервала";
                            }
                            if (errorMessage == null)
                            {
                                comment = parts[2].Trim();
                                if (string.IsNullOrEmpty(comment))
                                    errorMessage = "Не указана причина аннулирования";
                            }

                            if (errorMessage != null)
                            {
                                AddError(errorMessage, lineIndex);
                                continue;
                            }

                            AddCompetitionCertificate(connection, certificateNumberFrom, 
                                    certificateNumberTo, comment);
                        }
                        WriteLog("Данные обработаны успешно.", null);

                        CommitParsing(connection);
                    }
                    finally
                    {
                        EndParsing(connection);
                    }
                }
            }

            protected internal override void Execute()
            {
                if (Directory.GetFiles(LoadingDirectory).Length == 0)
                    return;
                string fileName = Directory.GetFiles(LoadingDirectory)[0];
                Parse(fileName);
                File.Delete(fileName);
            }
        }

        protected override string GetTaskCode()
        {
            return "SchoolLeavingCertificateDenyImport";
        }

        protected internal override TaskStatus GetStatus(string code)
        {
            return new Search();
        }
    }
}
