using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Xml;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace FbsService.FbsCheck
{
    public class CNEFormCheckMonitorTask : Task
    {
        class Search : TaskStatus
        {
            private const string CreateTempFormNumberStatement =
                    "create table #CommonNationalExamCertificateFormNumberRange \r\n" +
                    "    ( \r\n" +
                    "    NumberFrom nvarchar(255) \r\n" +
                    "    , NumberTo nvarchar(255) \r\n" +
                    "    ) ";
            private const string CreateTempFormStatement =
                    "create table #CommonNationalExamCertificateForm \r\n" +
                    "    ( \r\n" +
                    "    CheckingFormId uniqueidentifier \r\n" +
                    "    , Number nvarchar(255) \r\n" +
                    "    , CertificateNumber nvarchar(255) \r\n" +
                    "    , LastName nvarchar(255) \r\n" +
                    "    , FirstName nvarchar(255) \r\n" +
                    "    , PatronymicName nvarchar(255) \r\n" +
                    "    , PassportSeria nvarchar(255) \r\n" +
                    "    , PassportNumber nvarchar(255) \r\n" +
                    "    , IsBlank bit \r\n" +
                    "    , IsDuplicate bit \r\n" +
                    "    , IsDeny bit \r\n" +
                    "    , IsValid bit \r\n" +
                    "    ) ";
            private const string CreateTempFormSubjectStatement =
                    "create table #CommonNationalExamCertificateSubjectForm \r\n" +
                    "    ( \r\n" +
                    "    CheckingFormId uniqueidentifier \r\n" +
                    "    , SubjectCode nvarchar(255) \r\n" +
                    "    , Mark numeric(5,1) \r\n" +
                    "    ) ";

            private const string DropTempFormNumberStatement =
                    "drop table #CommonNationalExamCertificateFormNumberRange ";
            private const string DropTempFormStatement =
                    "drop table #CommonNationalExamCertificateForm ";
            private const string DropTempFormSubjectStatement =
                    "drop table #CommonNationalExamCertificateSubjectForm ";

            private const string InsertTempFormNumberStatement =
                    "insert into #CommonNationalExamCertificateFormNumberRange \r\n" +
                    "values (@numberFrom, @numberTo)";
            private const string InsertTempFormStatement =
                    "insert into #CommonNationalExamCertificateForm \r\n" +
                    "values (@checkingFormId, @number, @certificateNumber, @lastName, @firstName, @patronymicName, " +
                    "@passportSeria, @passportNumber, @isBlank, @isDuplicate, @isDeny, @isValid)";
            private const string InsertTempFormSubjectStatement =
                    "insert into #CommonNationalExamCertificateSubjectForm \r\n" +
                    "values (@checkingFormId, @subjectCode, @mark)";

            private const string CommitSPName =
                    "dbo.UpdateCommonNationalExamCertificateForm";

            static string InputDirectory = ConfigurationManager.AppSettings["InputFormDirectory"];
            static string OutputFormFileNameFormat = ConfigurationManager.AppSettings["OutputFormFileNameFormat"];

            private static TimeSpan FileReadDelay = TimeSpan.Parse("00:00:03");

            private static Hashtable SubjectCodes = new Hashtable {
                    { 1, "Russian" }, 
                    { 2, "Mathematics" }, 
                    { 3, "Physics" }, 
                    { 4, "Chemistry" },
                    { 6, "Biology" }, 
                    { 7, "RussiaHistory" }, 
                    { 8, "Geography" }, 
                    { 9, "English" }, 
                    { 10, "German" }, 
                    { 11, "Franch" }, 
                    { 12, "SocialScience" }, 
                    { 18, "Literature" }, 
                    { 13, "Spanish" }, 
                    { 5, "InformationScience" }};
            
            protected internal override string GetStatusCode()
            {
                return "search";
            }

            private void BeginParsing(SqlConnection connection)
            {
                using (SqlCommand cmdCreate = connection.CreateCommand())
                {
                    cmdCreate.CommandText = CreateTempFormNumberStatement;
                    cmdCreate.ExecuteNonQuery();
                }
                using (SqlCommand cmdCreate = connection.CreateCommand())
                {
                    cmdCreate.CommandText = CreateTempFormStatement;
                    cmdCreate.ExecuteNonQuery();
                }
                using (SqlCommand cmdCreate = connection.CreateCommand())
                {
                    cmdCreate.CommandText = CreateTempFormSubjectStatement;
                    cmdCreate.ExecuteNonQuery();
                }
            }

            private void EndParsing(SqlConnection connection)
            {
                using (SqlCommand cmdDrop = connection.CreateCommand())
                {
                    cmdDrop.CommandText = DropTempFormSubjectStatement;
                    cmdDrop.ExecuteNonQuery();
                }
                using (SqlCommand cmdDrop = connection.CreateCommand())
                {
                    cmdDrop.CommandText = DropTempFormStatement;
                    cmdDrop.ExecuteNonQuery();
                }
                using (SqlCommand cmdDrop = connection.CreateCommand())
                {
                    cmdDrop.CommandText = DropTempFormNumberStatement;
                    cmdDrop.ExecuteNonQuery();
                }
            }

            private void AddFormNumber(SqlConnection connection, string numberFrom, string numberTo)
            {
                using (SqlCommand cmdInsert = connection.CreateCommand())
                {
                    SqlParameter[] parameters = new SqlParameter[] 
                            {
                            new SqlParameter("@numberFrom", System.Data.SqlDbType.NVarChar, 255),
                            new SqlParameter("@numberTo", System.Data.SqlDbType.NVarChar, 255),
                            };

                    cmdInsert.CommandText = InsertTempFormNumberStatement;
                    cmdInsert.Parameters.AddRange(parameters);

                    cmdInsert.Parameters["@numberFrom"].Value = numberFrom;
                    cmdInsert.Parameters["@numberTo"].Value = numberTo;
                    cmdInsert.ExecuteNonQuery();
                }
            }

            private void AddForm(SqlConnection connection, Guid checkFormId, string number, string certificateNumber, 
                string lastName, string firstName, string patronymicName, string passportSeria, 
                string passportNumber, bool isBlank, bool isDuplicate, bool isDeny, bool? isValid)
            {
                using (SqlCommand cmdInsert = connection.CreateCommand())
                {
                    SqlParameter[] parameters = new SqlParameter[] 
                    {
                    new SqlParameter("@checkingFormId", System.Data.SqlDbType.UniqueIdentifier),
                    new SqlParameter("@number", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@certificateNumber", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@lastName", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@firstName", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@patronymicName", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@passportSeria", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@passportNumber", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@isBlank", System.Data.SqlDbType.Bit),
                    new SqlParameter("@isDuplicate", System.Data.SqlDbType.Bit),
                    new SqlParameter("@isDeny", System.Data.SqlDbType.Bit),
                    new SqlParameter("@isValid", System.Data.SqlDbType.Bit),
                    };

                    cmdInsert.CommandText = InsertTempFormStatement;
                    cmdInsert.Parameters.AddRange(parameters);

                    cmdInsert.Parameters["@checkingFormId"].Value = checkFormId;
                    cmdInsert.Parameters["@number"].Value = number;
                    if (certificateNumber != null)
                        cmdInsert.Parameters["@certificateNumber"].Value = certificateNumber;
                    else
                        cmdInsert.Parameters["@certificateNumber"].Value = DBNull.Value;
                    if (lastName != null)
                        cmdInsert.Parameters["@lastName"].Value = lastName;
                    else
                        cmdInsert.Parameters["@lastName"].Value = DBNull.Value;
                    if (firstName != null)
                        cmdInsert.Parameters["@firstName"].Value = firstName;
                    else
                        cmdInsert.Parameters["@firstName"].Value = DBNull.Value;
                    if (patronymicName != null)
                        cmdInsert.Parameters["@patronymicName"].Value = patronymicName;
                    else
                        cmdInsert.Parameters["@patronymicName"].Value = DBNull.Value;
                    if (passportSeria != null)
                        cmdInsert.Parameters["@passportSeria"].Value = passportSeria;
                    else
                        cmdInsert.Parameters["@passportSeria"].Value = DBNull.Value;
                    if (passportNumber != null)
                        cmdInsert.Parameters["@passportNumber"].Value = passportNumber;
                    else
                        cmdInsert.Parameters["@passportNumber"].Value = DBNull.Value;
                    cmdInsert.Parameters["@isBlank"].Value = isBlank;
                    cmdInsert.Parameters["@isDuplicate"].Value = isDuplicate;
                    cmdInsert.Parameters["@isDeny"].Value = isDeny;
                    if (isValid != null)
                        cmdInsert.Parameters["@isValid"].Value = isValid;
                    else
                        cmdInsert.Parameters["@isValid"].Value = DBNull.Value;
                    cmdInsert.ExecuteNonQuery();
                }
            }

            private void AddFormSubject(SqlConnection connection, Guid checkFormId, string subjectCode, short mark)
            {
                using (SqlCommand cmdInsert = connection.CreateCommand())
                {
                    SqlParameter[] parameters = new SqlParameter[] 
                            {
                            new SqlParameter("@checkingFormId", System.Data.SqlDbType.UniqueIdentifier),
                            new SqlParameter("@subjectCode", System.Data.SqlDbType.NVarChar, 255),
                            new SqlParameter("@mark", System.Data.SqlDbType.Float),
                            };

                    cmdInsert.CommandText = InsertTempFormSubjectStatement;
                    cmdInsert.Parameters.AddRange(parameters);

                    cmdInsert.Parameters["@checkingFormId"].Value = checkFormId;
                    cmdInsert.Parameters["@subjectCode"].Value = subjectCode;
                    cmdInsert.Parameters["@mark"].Value = mark;
                    cmdInsert.ExecuteNonQuery();
                }
            }

            private void CommitParsing(SqlConnection connection, string regionCode)
            {
                using (SqlCommand cmdCommit = connection.CreateCommand())
                {
                    SqlParameter[] parameters = new SqlParameter[] 
                            {
                            new SqlParameter("@regionCode", System.Data.SqlDbType.NVarChar, 255),
                            };
                    cmdCommit.CommandType = CommandType.StoredProcedure;
                    cmdCommit.CommandText = CommitSPName;
                    cmdCommit.CommandTimeout = 0;
                    cmdCommit.Parameters.AddRange(parameters);
                    cmdCommit.Parameters["@regionCode"].Value = regionCode;
                    cmdCommit.ExecuteNonQuery();
                }
            }

            private bool StoreForm(string fileName, out string regionCode)
            {
                regionCode = null;
                using (SqlConnection connection = new SqlConnection(
                        global::FbsService.Properties.Settings.Default.FbsCheckConnectionString))
                {
                    connection.Open();
                    try
                    {
                        BeginParsing(connection);
                        try
                        {
                            XmlReaderSettings settings = new XmlReaderSettings();
                            settings.IgnoreProcessingInstructions = true;
                            settings.IgnoreComments = true;
                            settings.IgnoreWhitespace = true;
                            FileStream stream = null;
                            try
                            {
                                stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                            }
                            catch
                            {
                                return false;
                            }
                            try
                            {
                                using (XmlReader reader = XmlReader.Create(stream, settings))
                                {
                                    string number = null;
                                    Guid checkFormId = new Guid();
                                    string certificateNumber = null;
                                    string lastName = null;
                                    string firstName = null;
                                    string patronymicName = null;
                                    string passportSeria = null;
                                    string passportNumber = null;
                                    bool isBlank = false;
                                    bool isDuplicate = false;
                                    bool isDeny = false;
                                    bool? isValid = null;

                                    while (reader.Read())
                                    {
                                        if (reader.NodeType == XmlNodeType.Element &&
                                                string.Compare(reader.Name, "CertificatesRequest", true) == 0)
                                        {
                                            regionCode = reader.GetAttribute("Region");
                                            while (reader.Read())
                                            {
                                                if (reader.NodeType == XmlNodeType.Element &&
                                                        string.Compare(reader.Name, "AllRegistered", true) == 0)
                                                    while (reader.Read())
                                                    {
                                                        if (reader.NodeType == XmlNodeType.Element &&
                                                                string.Compare(reader.Name, "SerNumRange", true) == 0)
                                                            AddFormNumber(connection, reader.GetAttribute("From"),
                                                                    reader.GetAttribute("To"));
                                                        if (reader.NodeType == XmlNodeType.EndElement &&
                                                                string.Compare(reader.Name, "AllRegistered", true) == 0)
                                                            break;
                                                    }

                                                if (reader.NodeType == XmlNodeType.Element &&
                                                        string.Compare(reader.Name, "Used", true) == 0)
                                                    while (reader.Read())
                                                    {
                                                        if (number != null && (string.Compare(reader.Name, "Cert", true) == 0
                                                                || string.Compare(reader.Name, "Used", true) == 0))
                                                        {
                                                            AddForm(connection, checkFormId, number, certificateNumber, lastName,
                                                                    firstName, patronymicName, passportSeria, passportNumber, isBlank,
                                                                    isDuplicate, isDeny, isValid);
                                                            number = null;
                                                        }

                                                        if (reader.NodeType == XmlNodeType.Element &&
                                                                string.Compare(reader.Name, "Cert", true) == 0)
                                                        {
                                                            number = null;
                                                            certificateNumber = null;
                                                            lastName = null;
                                                            firstName = null;
                                                            patronymicName = null;
                                                            passportSeria = null;
                                                            passportNumber = null;
                                                            isBlank = false;
                                                            isDuplicate = false;
                                                            isDeny = false;
                                                            isValid = null;

                                                            checkFormId = Guid.NewGuid();
                                                            number = reader.GetAttribute("SerNum");
                                                            switch (reader.GetAttribute("State"))
                                                            {
                                                                case "B":
                                                                    isBlank = true;
                                                                    break;
                                                                case "C":
                                                                    isValid = null;
                                                                    break;
                                                                case "A":
                                                                    isValid = true;
                                                                    break;
                                                                case "D":
                                                                    isValid = false;
                                                                    break;
                                                            }
                                                            if (reader.GetAttribute("Cancel") == "1")
                                                                isDeny = true;
                                                            if (reader.GetAttribute("Dup") == "1")
                                                                isDuplicate = true;
                                                        }

                                                        if (reader.NodeType == XmlNodeType.Element &&
                                                                string.Compare(reader.Name, "RegNum", true) == 0)
                                                            certificateNumber = ToCertificateNumber(
                                                                    reader.ReadElementContentAsString());
                                                        if (reader.NodeType == XmlNodeType.Element &&
                                                                string.Compare(reader.Name, "Student", true) == 0)
                                                        {
                                                            lastName = reader.GetAttribute("LName");
                                                            firstName = reader.GetAttribute("FName");
                                                            patronymicName = reader.GetAttribute("Patr");
                                                            passportSeria = reader.GetAttribute("DocSer");
                                                            passportNumber = reader.GetAttribute("DocNum");
                                                        }

                                                        if (reader.NodeType == XmlNodeType.Element &&
                                                                string.Compare(reader.Name, "Marks", true) == 0)
                                                            while (reader.Read())
                                                            {
                                                                if (reader.NodeType == XmlNodeType.Element &&
                                                                        string.Compare(reader.Name, "Mark", true) == 0)
                                                                    AddFormSubject(connection, checkFormId,
                                                                            Convert.ToString(SubjectCodes[
                                                                                Convert.ToInt32(reader.GetAttribute("DiscCode"))]),
                                                                            Convert.ToInt16(reader.GetAttribute("Rate")));

                                                                if (reader.NodeType == XmlNodeType.EndElement &&
                                                                        string.Compare(reader.Name, "Marks", true) == 0)
                                                                    break;
                                                            }

                                                        if (reader.NodeType == XmlNodeType.EndElement &&
                                                                string.Compare(reader.Name, "Used", true) == 0)
                                                            break;
                                                    }

                                                if (reader.NodeType == XmlNodeType.EndElement &&
                                                        string.Compare(reader.Name, "CertificatesRequest", true) == 0)
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                            finally
                            {
                                stream.Dispose();
                                stream = null;
                            }

                            CommitParsing(connection, regionCode);
                        }
                        finally
                        {
                            EndParsing(connection);
                        }
                    }
                    catch
                    {
                        connection.Close();
                        throw;
                    }
                }
                return true;
            }

            private string ToCertificateNumber(string formNumber)
            {
                return Regex.Replace(formNumber, @"(?<region>\d{2})-(?<number>\d{7})-(?<seria>\d{2})-(?<year>\d{2})",
                        "${region}-${number}${seria}-${year}");
            }

            private string FromCertificateNumber(string certificateNumber)
            {
                return Regex.Replace(certificateNumber, 
                        @"(?<region>\d{2})-(?<number>\d{7})(?<seria>\d{2})-(?<year>\d{2})",
                        "${region}-${number}-${seria}-${year}");
            }

            private void StoreResult(string regionCode, string resultFileName)
            {
                if (File.Exists(resultFileName))
                    File.Delete(resultFileName);
                using (XmlWriter writer = XmlWriter.Create(resultFileName))
                {
                    writer.WriteStartDocument();
                    try
                    {
                        writer.WriteStartElement("CertificatesResult");

                        try
                        {
                            writer.WriteAttributeString("Year", DateTime.Now.Year.ToString());
                            writer.WriteAttributeString("Region", regionCode);

                            writer.WriteStartElement("MistakenRegistered");
                            writer.WriteEndElement();

                            writer.WriteStartElement("Used");
                            try
                            {
                                foreach (CNEForm form in CNEForm.GetActiveForms(regionCode))
                                {
                                    writer.WriteStartElement("Cert");
                                    try
                                    {
                                        writer.WriteAttributeString("SerNum", form.Number);
                                        writer.WriteAttributeString("RegNum", 
                                                FromCertificateNumber(form.CertificateNumber));
                                        if (form.IsValid)
                                            writer.WriteAttributeString("Result", "1");
                                        else
                                        {
                                            writer.WriteAttributeString("Result", "0");
                                            writer.WriteStartElement("Comment");
                                            try
                                            {
                                                if (!form.IsCertificateExist)
                                                    writer.WriteString("Cвидетельство с указанным номером не найдено.");
                                                else if (form.IsCertificateDeny)
                                                    writer.WriteString("Сертификат аннулирован.");
                                                else
                                                    writer.WriteString("Некорректные данные свидетельства.");
                                            }
                                            finally
                                            {
                                                writer.WriteEndElement();
                                            }
                                        }
                                    }
                                    finally
                                    {
                                        writer.WriteEndElement();
                                    }
                                }
                            }
                            finally
                            {
                                writer.WriteEndElement();
                            }
                        }
                        finally
                        {
                            writer.WriteEndElement();
                        }
                    }
                    finally
                    {
                        writer.WriteEndDocument();
                    }
                    writer.Flush();
                }
            }

            private void ConfirmExecute(string fileName)
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);
            }

            private void ClearResult(string resultFieldName)
            {
                if (File.Exists(resultFieldName))
                    File.Delete(resultFieldName);
            }

            private void ExecuteFile(string fileName)
            {
                if (File.GetLastWriteTime(fileName).Add(FileReadDelay) >= DateTime.Now)
                    return;
                string regionCode = null;
                ClearResult(GetResultFileName(fileName));
                if (!StoreForm(fileName, out regionCode))
                    return;
                StoreResult(regionCode, GetResultFileName(fileName));
                ConfirmExecute(fileName);
            }

            private string GetResultFileName(string fileName)
            {
                return string.Format(OutputFormFileNameFormat, Path.GetFileNameWithoutExtension(fileName));
            }

            protected internal override void Execute()
            {
                foreach (string fileName in Directory.GetFiles(InputDirectory, "*.xml"))
                    try
                    {
                        ExecuteFile(fileName);
                    }
                    catch (Exception ex)
                    {
                        LogException(ex);
                    }
            }
        }

        protected override string GetTaskCode()
        {
            return "CommonNationalExamCertificateFormCheckMonitor";
        }

        protected internal override TaskStatus GetStatus(string code)
        {
            return new Search();
        }
    }
}
