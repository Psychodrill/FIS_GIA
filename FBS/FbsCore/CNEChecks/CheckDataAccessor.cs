namespace Fbs.Core.CNEChecks
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Transactions;

    using System.Data.Linq;
    using System.Linq;
    
    using Fbs.Core.Shared;
    using Fbs.Core.UICheckLog;

    /// <summary>
    /// The check data accessor.
    /// </summary>
    public class CheckDataAccessor
    {
        #region Constants

        private const int CmdTimeout = 90;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Проверка по номеру сертификата и ФИО
        /// </summary>
        /// <param name="login">
        /// логин проверяющего 
        /// </param>
        /// <param name="CNENumber">
        /// номер свидетельства 
        /// </param>
        /// <param name="firstName">
        /// имя владельца свидетельства 
        /// </param>
        /// <param name="lastName">
        /// фамилия владельца свидетельства 
        /// </param>
        /// <param name="patronymicName">
        /// отчетсво владельца свидетельства  
        /// </param>
        /// <param name="subjectMarks">
        /// указанные в проверке оценки 
        /// </param>
        /// <returns>
        /// Информация о проверке 
        /// </returns>
        public static List<CNEInfo> CheckByCNENumer(
            string login, 
            string CNENumber, 
            string firstName, 
            string lastName, 
            string patronymicName, 
            string subjectMarks)
        {
            return CheckByCNENumer(login, "127.0.0.1", CNENumber, firstName, lastName, patronymicName, subjectMarks);
        }

        public static CNEInfo CheckByCNENumer2(
            string login,
            string CNENumber,
            string firstName,
            string lastName,
            string patronymicName,
            string subjectMarks)
        {
            return CheckByCNENumer2(login, "127.0.0.1", CNENumber, firstName, lastName, patronymicName, subjectMarks);
        }

        public static string GetMarksFromBatchCheck(string certNumber, long batchId, string login)
        {
            var certData = GetCertResult(batchId, certNumber, login);
            if (certData == null || certData.Count < 1)
            {
                throw new Exception(string.Format("не найдена проверка с номером {0} для пользователя {1} по сертификату {2}", batchId, login, certNumber));
            }

            return certData[0].Marks.FormMarksString();
        }

        // Проверка по типографскому номеру и ФИО

        // Проверка по номеру паспорта и ФИО
        /// <summary>
        /// The check by passport.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="passportSeria">
        /// The passport seria.
        /// </param>
        /// <param name="passportNumber">
        /// The passport number.
        /// </param>
        /// <param name="firstName">
        /// The first name.
        /// </param>
        /// <param name="lastName">
        /// The last name.
        /// </param>
        /// <param name="patronymicName">
        /// The patronymic name.
        /// </param>
        /// <returns>
        /// </returns>
        public static List<CNEInfo> CheckByPassport(
            string login,
            string passportSeria,
            string passportNumber,
            string firstName,
            string lastName,
            string patronymicName)
        {
            int eventId = CheckLogDataAccessor.AddPassportCheckEvent(
                login, lastName, firstName, patronymicName, passportSeria, passportNumber);

            // Сначала получаем список сертификатов по номеру и серии паспорта и ФИО
            DataTable CNEsRaw = CheckByPassportRaw(
                login, passportSeria, passportNumber, firstName, lastName, patronymicName);
            if (CNEsRaw.Rows.Count == 0)
            {
                return null;
            }

            var Result = new List<CNEInfo>();

            // Т.к. хранимая процедура поиска по серии/номеру документа возвращает не все необходимые данные, 
            // то для каждого найденного сертификата выполняем поиск по номеру 
            // сертификата (используя соответствующую хранимую процедуру)
            foreach (DataRow CNERow in CNEsRaw.Rows)
            {
                string certificateId = CNERow["CertificateId"].ToString();

                if (eventId != 0)
                {
                    CheckLogDataAccessor.UpdateCheckEvent(eventId.ToString(), certificateId);
                }

                string CNENumber = CNERow["CertificateNumber"].ToString();
                string FirstName = CNERow["FirstName"].ToString();
                string LastName = CNERow["LastName"].ToString();
                string PatronimicName = CNERow["PatronymicName"].ToString();

                //Вот зачем это тут делали совсем неясно ((((
                //if ((!string.IsNullOrEmpty(firstName))
                //    && (!firstName.Equals(FirstName, StringComparison.CurrentCultureIgnoreCase)))
                //{
                //    continue;
                //}

                //if ((!string.IsNullOrEmpty(patronymicName))
                //    && (!patronymicName.Equals(PatronimicName, StringComparison.CurrentCultureIgnoreCase)))
                //{
                //    continue;
                //}

                //if ((!string.IsNullOrEmpty(lastName))
                //    && (!lastName.Equals(LastName, StringComparison.CurrentCultureIgnoreCase)))
                //{
                //    continue;
                //}

                // Выполняем поиск по номеру сертификата (вообще говоря, это коряво, но хитро)
                var newInfo = CheckByCNENumer(login, CNENumber, FirstName, LastName, PatronimicName, null);
                if (newInfo != null)
                {
                    Result.AddRange(newInfo);
                }
            }

            return Result;
        }


        // Проверка по номеру паспорта и ФИО
        /// <summary>
        /// The check by passport.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="passportSeria">
        /// The passport seria.
        /// </param>
        /// <param name="passportNumber">
        /// The passport number.
        /// </param>
        /// <param name="firstName">
        /// The first name.
        /// </param>
        /// <param name="lastName">
        /// The last name.
        /// </param>
        /// <param name="patronymicName">
        /// The patronymic name.
        /// </param>
        /// <returns>
        /// </returns>
        public static List<CNEInfo> CheckByPassport2(
            string login, 
            string passportSeria, 
            string passportNumber, 
            string firstName, 
            string lastName, 
            string patronymicName)
        {
            int eventId = CheckLogDataAccessor.AddPassportCheckEvent(
                login, lastName, firstName, patronymicName, passportSeria, passportNumber);

            // Сначала получаем список сертификатов по номеру и серии паспорта и ФИО
            DataTable CNERaw = CheckByPassportRaw2(
                login, passportSeria, passportNumber, firstName, lastName, patronymicName);
            if (CNERaw.Rows.Count == 0)
            {
                return null;
            }

            string certificateId = CNERaw.Rows[0]["CertificateId"].ToString();

            if (eventId != 0)
            {
                CheckLogDataAccessor.UpdateCheckEvent(eventId.ToString(), certificateId);
            }

            return CNERaw.ToCertificatesCollection(true);
        }

        /// <summary>
        /// The check by typh numer.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="typographicNumber">
        /// The typographic number.
        /// </param>
        /// <param name="firstName">
        /// The first name.
        /// </param>
        /// <param name="lastName">
        /// The last name.
        /// </param>
        /// <param name="patronymicName">
        /// The patronymic name.
        /// </param>
        /// <returns>
        /// </returns>
        public static List<CNEInfo> CheckByTyphNumer(
            string login, string typographicNumber, string firstName, string lastName, string patronymicName)
        {
            int eventId = CheckLogDataAccessor.AddTNCheckEvent(
                login, lastName, firstName, patronymicName, typographicNumber);
            DataTable CNERaw = CheckByTyphNumerRaw(login, typographicNumber, firstName, lastName, patronymicName);
            if (CNERaw.Rows.Count == 0)
            {
                return null;
            }

            //Вот зачем это тут делали совсем неясно ((((
            //if ((!string.IsNullOrEmpty(firstName))
            //    &&
            //    (!firstName.Equals(CNERaw.Rows[0]["FirstName"].ToString(), StringComparison.CurrentCultureIgnoreCase)))
            //{
            //    return null;
            //}

            //if ((!string.IsNullOrEmpty(patronymicName))
            //    &&
            //    (!patronymicName.Equals(
            //        CNERaw.Rows[0]["PatronymicName"].ToString(), StringComparison.CurrentCultureIgnoreCase)))
            //{
            //    return null;
            //}

            //if ((!string.IsNullOrEmpty(lastName))
            //    && (!lastName.Equals(CNERaw.Rows[0]["LastName"].ToString(), StringComparison.CurrentCultureIgnoreCase)))
            //{
            //    return null;
            //}

            string certificateId = CNERaw.Rows[0]["CertificateId"].ToString();

            if (eventId != 0)
            {
                CheckLogDataAccessor.UpdateCheckEvent(eventId.ToString(), certificateId);
            }

            return CNERaw.ToCertificatesCollection(true);
        }

        public static List<CNEInfo> CheckByTyphNumer2(
            string login, string typographicNumber, string firstName, string lastName, string patronymicName)
        {
            int eventId = CheckLogDataAccessor.AddTNCheckEvent(
                login, lastName, firstName, patronymicName, typographicNumber);
            DataTable CNERaw = CheckByTyphNumerRaw2(login, typographicNumber, firstName, lastName, patronymicName);
            if (CNERaw.Rows.Count == 0)
            {
                return null;
            }

            //Вот зачем это тут делали совсем неясно ((((
            //if ((!string.IsNullOrEmpty(firstName))
            //    &&
            //    (!firstName.Equals(CNERaw.Rows[0]["FirstName"].ToString(), StringComparison.CurrentCultureIgnoreCase)))
            //{
            //    return null;
            //}

            //if ((!string.IsNullOrEmpty(patronymicName))
            //    &&
            //    (!patronymicName.Equals(
            //        CNERaw.Rows[0]["PatronymicName"].ToString(), StringComparison.CurrentCultureIgnoreCase)))
            //{
            //    return null;
            //}

            //if ((!string.IsNullOrEmpty(lastName))
            //    && (!lastName.Equals(CNERaw.Rows[0]["LastName"].ToString(), StringComparison.CurrentCultureIgnoreCase)))
            //{
            //    return null;
            //}

            string certificateId = CNERaw.Rows[0]["CertificateId"].ToString();

            if (eventId != 0)
            {
                CheckLogDataAccessor.UpdateCheckEvent(eventId.ToString(), certificateId);
            }

            return CNERaw.ToCertificatesCollection(true);
        }

        /// <summary>
        /// The get cert result.
        /// </summary>
        /// <param name="batchId">
        /// The batch id.
        /// </param>
        /// <param name="certNumber">
        /// The cert number.
        /// </param>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <returns>
        /// </returns>
        public static List<CNEInfo> GetCertResult(long batchId, string certNumber, string login)
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();

            using (var scope = new TransactionScope())
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[dbo].[SearchCommonNationalExamCertificateCheck]";

                        cmd.Parameters.AddWithValue("login", login);
                        cmd.Parameters.AddWithValue("batchId", batchId);
                        if (!string.IsNullOrEmpty(certNumber))
                        {
                            cmd.Parameters.AddWithValue("certNumber", certNumber);
                        }

                        var table = new DataTable();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            table.Load(reader);
                        }

                        var result = new List<CNEInfo>();
                        CNEInfo item;
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            DataRow row = table.Rows[i];
                            item = new CNEInfo();

                            item.LastName = GetStringValue(row["Surname"]);
                            item.FirstName = GetStringValue(row["Name"]);
                            item.PatronymicName = GetStringValue(row["SecondName"]);
                            item.PassportSeria = GetStringValue(row["DocumentSeries"]);
                            item.PassportNumber = GetStringValue(row["DocumentNumber"]);
                            item.CertificateNumber = GetStringValue(row["CertificateNumber"]);
                            item.TypographicNumber = GetStringValue(row["TypographicNumber"]);
                            item.Year = GetStringValue(row["UseYear"]);
                            item.Status = GetStringValue(row["StatusName"]);

                            item.CertificateDeny = Convert.ToInt16(row["Cancelled"]);
                            item.CertificateNewNumber = GetStringValue(row["DenyNewCertificateNumber"]);
                            item.CertificateDenyComment = GetStringValue(row["DenyComment"]);
                            item.UniqueIHEaFCheck = string.IsNullOrEmpty(row["UniqueIHEaFCheck"].ToString())
                                                        ? 0
                                                        : Convert.ToInt16(row["UniqueIHEaFCheck"]);

                            item.Marks.Add(
                                "Русский язык", 
                                GetStringValue(row["RussianMark"]), 
                                GetStringValue(row["RussianHasAppeal"]));
                            item.Marks.Add(
                                "Математика", 
                                GetStringValue(row["MathematicsMark"]), 
                                GetStringValue(row["MathematicsHasAppeal"]));
                            item.Marks.Add(
                                "Физика", GetStringValue(row["PhysicsMark"]), GetStringValue(row["PhysicsHasAppeal"]));
                            item.Marks.Add(
                                "Химия", GetStringValue(row["ChemistryMark"]), GetStringValue(row["ChemistryHasAppeal"]));
                            item.Marks.Add(
                                "Биология", GetStringValue(row["BiologyMark"]), GetStringValue(row["BiologyHasAppeal"]));
                            item.Marks.Add(
                                "История", 
                                GetStringValue(row["RussiaHistoryMark"]), 
                                GetStringValue(row["RussiaHistoryHasAppeal"]));
                            item.Marks.Add(
                                "География", 
                                GetStringValue(row["GeographyMark"]), 
                                GetStringValue(row["GeographyHasAppeal"]));
                            item.Marks.Add(
                                "Английский язык", 
                                GetStringValue(row["EnglishMark"]), 
                                GetStringValue(row["EnglishHasAppeal"]));
                            item.Marks.Add(
                                "Немецкий язык", 
                                GetStringValue(row["GermanMark"]), 
                                GetStringValue(row["GermanHasAppeal"]));
                            item.Marks.Add(
                                "Французский язык", 
                                GetStringValue(row["FranchMark"]), 
                                GetStringValue(row["FranchHasAppeal"]));
                            item.Marks.Add(
                                "Обществознание", 
                                GetStringValue(row["SocialScienceMark"]), 
                                GetStringValue(row["SocialScienceHasAppeal"]));
                            item.Marks.Add(
                                "Литература", 
                                GetStringValue(row["LiteratureMark"]), 
                                GetStringValue(row["LiteratureHasAppeal"]));
                            item.Marks.Add(
                                "Испанский язык", 
                                GetStringValue(row["SpanishMark"]), 
                                GetStringValue(row["SpanishHasAppeal"]));
                            item.Marks.Add(
                                "Информатика и ИКТ", 
                                GetStringValue(row["InformationScienceMark"]), 
                                GetStringValue(row["InformationScienceHasAppeal"]));

                            result.Add(item);
                        }

                        return result;
                    }
                }
            }
        }

        #endregion

        #region Methods

        private static List<CNEInfo> CheckByCNENumer(
            string login, 
            string ip, 
            string CNENumber, 
            string firstName, 
            string lastName, 
            string patronymicName, 
            string subjectMarks)
        {
            int eventId = CheckLogDataAccessor.AddCNENumberCheckEvent(
                login, lastName, firstName, patronymicName, CNENumber);
            DataTable CNERaw = CheckByCNENumerRaw(
                login, ip, CNENumber, firstName, lastName, patronymicName, subjectMarks);

            if (CNERaw.Rows.Count == 0)
            {
                return null;
            }

            //Вот зачем это тут делали совсем неясно ((((
            //if ((!string.IsNullOrEmpty(firstName))
            //    &&
            //    (!firstName.Equals(CNERaw.Rows[0]["FirstName"].ToString(), StringComparison.CurrentCultureIgnoreCase)))
            //{
            //    return null;
            //}

            //if ((!string.IsNullOrEmpty(patronymicName))
            //    &&
            //    (!patronymicName.Equals(
            //        CNERaw.Rows[0]["PatronymicName"].ToString(), StringComparison.CurrentCultureIgnoreCase)))
            //{
            //    return null;
            //}

            //if ((!string.IsNullOrEmpty(lastName))
            //    && (!lastName.Equals(CNERaw.Rows[0]["LastName"].ToString(), StringComparison.CurrentCultureIgnoreCase)))
            //{
            //    return null;
            //}

            string certificateId = CNERaw.Rows[0]["CertificateId"].ToString();

            if (eventId != 0)
            {
                CheckLogDataAccessor.UpdateCheckEvent(eventId.ToString(), certificateId);
            }

            return CNERaw.ToCertificatesCollection(true);
        }

        private static CNEInfo CheckByCNENumer2(
            string login,
            string ip,
            string CNENumber,
            string firstName,
            string lastName,
            string patronymicName,
            string subjectMarks)
        {
            int eventId = CheckLogDataAccessor.AddCNENumberCheckEvent(
                login, lastName, firstName, patronymicName, CNENumber);
            DataTable CNERaw = CheckByCNENumerRaw2(
                login, ip, CNENumber, firstName, lastName, patronymicName, subjectMarks);

            if (CNERaw.Rows.Count == 0)
            {
                return null;
            }

            string certificateId = CNERaw.Rows[0]["CertificateId"].ToString();

            if (eventId != 0)
            {
                CheckLogDataAccessor.UpdateCheckEvent(eventId.ToString(), certificateId);
            }

            var Result = CNERaw.ToCertificatesCollection(true);
            return Result[0];
        }

        private static DataTable CheckByCNENumerRaw(
            string login, 
            string ip, 
            string CNENumber, 
            string firstName, 
            string lastName, 
            string patronymicName, 
            string subjectMarks)
        {
            var Result = new DataTable();
            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandTimeout = CmdTimeout;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = "dbo.CheckCommonNationalExamCertificateByNumber";

                Cmd.Parameters.AddWithValue("number", Utils.GetNullString(CNENumber));
                Cmd.Parameters.AddWithValue("checkFirstName", Utils.GetNullString(firstName));
                Cmd.Parameters.AddWithValue("checkLastName", Utils.GetNullString(lastName));
                Cmd.Parameters.AddWithValue("checkPatronymicName", Utils.GetNullString(patronymicName));
                Cmd.Parameters.AddWithValue("checkSubjectMarks", Utils.GetNullString(subjectMarks));

                Cmd.Parameters.AddWithValue("ip", ip);
                Cmd.Parameters.AddWithValue("login", login);

                Result.Load(Cmd.ExecuteReader());
            }
            var items = Result.AsEnumerable().Where(x => x.Field<Boolean>("PatronymicNameIsCorrect") && x.Field<Boolean>("FirstNameIsCorrect") && x.Field<Boolean>("LastNameIsCorrect"));
            if (items.Count() > 0)
                Result = items.CopyToDataTable();
            else
                Result = new DataTable();
            return Result;
        }

        private static DataTable CheckByCNENumerRaw2(
            string login,
            string ip,
            string CNENumber,
            string firstName,
            string lastName,
            string patronymicName,
            string subjectMarks)
        {
            var Result = new DataTable();
            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandTimeout = CmdTimeout;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = "dbo.SingleCheck_LicenseNumberFio";

                Cmd.Parameters.AddWithValue("senderType", 2);
                Cmd.Parameters.AddWithValue("licenseNumber", Utils.GetNullString(CNENumber));
                Cmd.Parameters.AddWithValue("surname", Utils.GetNullString(lastName));
                Cmd.Parameters.AddWithValue("name", Utils.GetNullString(firstName));
                Cmd.Parameters.AddWithValue("secondName", Utils.GetNullString(patronymicName));
                Cmd.Parameters.AddWithValue("login", login);

                Result.Load(Cmd.ExecuteReader());
            }
            //var items = Result.AsEnumerable().Where(x => x.Field<Boolean>("PatronymicNameIsCorrect") && x.Field<Boolean>("FirstNameIsCorrect") && x.Field<Boolean>("LastNameIsCorrect"));
            var items = Result.AsEnumerable();
            if (items.Count() > 0)
                Result = items.CopyToDataTable();
            else
                Result = new DataTable();
            return Result;
        }

        private static DataTable CheckByPassportRaw(
         string login,
         string passportSeria,
         string passportNumber,
         string firstName,
         string lastName,
         string patronymicName)
        {
            var Result = new DataTable();
            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandTimeout = CmdTimeout;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = "dbo.SearchCommonNationalExamCertificate";

                Cmd.Parameters.AddWithValue("passportSeria", Utils.GetNullString(passportSeria));
                Cmd.Parameters.AddWithValue("passportNumber", Utils.GetNullString(passportNumber));
                Cmd.Parameters.AddWithValue("firstName", Utils.GetNullString(firstName));
                Cmd.Parameters.AddWithValue("lastName", Utils.GetNullString(lastName));
                Cmd.Parameters.AddWithValue("patronymicName", Utils.GetNullString(patronymicName));

                Cmd.Parameters.AddWithValue("ip", "127.0.0.1");
                Cmd.Parameters.AddWithValue("login", login);

                Result.Load(Cmd.ExecuteReader());
            }

            return Result;
        }  
   
        private static DataTable CheckByPassportRaw2(
            string login, 
            string passportSeria, 
            string passportNumber, 
            string firstName, 
            string lastName, 
            string patronymicName)
        {
            var Result = new DataTable();
            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandTimeout = CmdTimeout;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = "dbo.SingleCheck_FioDocumentNumberSeries";

                Cmd.Parameters.AddWithValue("senderType", 2);
                Cmd.Parameters.AddWithValue("documentSeries", Utils.GetNullString(passportSeria));
                Cmd.Parameters.AddWithValue("documentNumber", Utils.GetNullString(passportNumber));
                Cmd.Parameters.AddWithValue("surname", Utils.GetNullString(lastName));
                Cmd.Parameters.AddWithValue("name", Utils.GetNullString(firstName));
                Cmd.Parameters.AddWithValue("secondName", Utils.GetNullString(patronymicName));
                Cmd.Parameters.AddWithValue("login", login);

                Result.Load(Cmd.ExecuteReader());
            }

            return Result;
        }

        private static DataTable CheckByTyphNumerRaw(
            string login, string typographicNumber, string firstName, string lastName, string patronymicName)
        {
            var Result = new DataTable();
            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandTimeout = CmdTimeout;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = "dbo.CheckCommonNationalExamCertificateByNumber";

                Cmd.Parameters.AddWithValue("checkTypographicNumber", Utils.GetNullString(typographicNumber));
                Cmd.Parameters.AddWithValue("checkFirstName", Utils.GetNullString(firstName));
                Cmd.Parameters.AddWithValue("checkLastName", Utils.GetNullString(lastName));
                Cmd.Parameters.AddWithValue("checkPatronymicName", Utils.GetNullString(patronymicName));

                Cmd.Parameters.AddWithValue("ip", "127.0.0.1");
                Cmd.Parameters.AddWithValue("login", login);

                Result.Load(Cmd.ExecuteReader());
            }
            var items = Result.AsEnumerable().Where(x => x.Field<Boolean>("PatronymicNameIsCorrect") && x.Field<Boolean>("FirstNameIsCorrect") && x.Field<Boolean>("LastNameIsCorrect"));
            if (items.Count() > 0)
                Result = items.CopyToDataTable();
            else 
                Result = new DataTable();
            return Result;
        }

        private static DataTable CheckByTyphNumerRaw2(
            string login, string typographicNumber, string firstName, string lastName, string patronymicName)
        {
            var Result = new DataTable();
            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandTimeout = CmdTimeout;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = "dbo.SingleCheck_TypographicNumberFio";

                Cmd.Parameters.AddWithValue("senderType", 2);
                Cmd.Parameters.AddWithValue("typographicNumber", Utils.GetNullString(typographicNumber));
                Cmd.Parameters.AddWithValue("surname", Utils.GetNullString(lastName));
                Cmd.Parameters.AddWithValue("name", Utils.GetNullString(firstName));
                Cmd.Parameters.AddWithValue("secondName", Utils.GetNullString(patronymicName));
                Cmd.Parameters.AddWithValue("login", login);

                Result.Load(Cmd.ExecuteReader());
            }
            //var items = Result.AsEnumerable().Where(x => x.Field<Boolean>("PatronymicNameIsCorrect") && x.Field<Boolean>("FirstNameIsCorrect") && x.Field<Boolean>("LastNameIsCorrect"));
            var items = Result.AsEnumerable();
            if (items.Count() > 0)
                Result = items.CopyToDataTable();
            else
                Result = new DataTable();
            return Result;
        }

        private static string GetStringValue(object val)
        {
            if (val == DBNull.Value)
            {
                return string.Empty;
            }
            else
            {
                return val.ToString();
            }
        }

        #endregion
    }
}