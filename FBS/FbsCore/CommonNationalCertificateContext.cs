using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Linq;

namespace Fbs.Core
{
    public partial class CommonNationalCertificateContext
    {
        private const string UpdateErrorMessage = "Ошибка добавления пакета";

        static private ThreadInstanceManager<CommonNationalCertificateContext> mInstanceManager =
                new ThreadInstanceManager<CommonNationalCertificateContext>(CreateInstance);

        static private CommonNationalCertificateContext CreateInstance()
        {
            CommonNationalCertificateContext instance = new CommonNationalCertificateContext();
            instance.ObjectTrackingEnabled = false;
            instance.CommandTimeout = 0;
            return instance;
        }

        static internal CommonNationalCertificateContext Instance()
        {
            return mInstanceManager.Instance();
        }

        static internal void BeginLock()
        {
            mInstanceManager.BeginLock();
        }

        static internal void EndLock()
        {
            mInstanceManager.EndLock();
        }

        #region Текущий пользователь

        public static string ClientLogin
        {
            get;
            set;
            //get
            //{
            //    if (HttpContext.Current == null)
            //        return null;

            //    return HttpContext.Current.User.Identity.Name;
            //}
        }

        public static string ClientIp
        {
            get
            {
                if (HttpContext.Current == null)
                    return "127.0.0.1";
                return HttpContext.Current.Request.UserHostAddress;
            }
        }

        #endregion

        public static XElement CheckCommonNationalExamCertificateByNumberForXml(string number, string checkSubjectMarks, string participantid, string userlogin, string ip, bool shouldCheckMarks)
        {
            CommonNationalCertificateContext.BeginLock();
            try
            {
                XElement xml = null;
                CommonNationalCertificateContext.Instance().CheckCommonNationalExamCertificateByNumberForXml(number, checkSubjectMarks, participantid, userlogin, ip, shouldCheckMarks, ref xml);

                return xml;
            }
            finally
            {
                CommonNationalCertificateContext.EndLock();
            }
        }

        public static XElement CheckCommonNationalExamCertificateByPassportForXml(string passportSeria, string passportNumber, string checkSubjectMarks, string userlogin, string ip, bool shouldWriteLog)
        {
            CommonNationalCertificateContext.BeginLock();
            try
            {
                XElement xml = null;
                CommonNationalCertificateContext.Instance().CheckCommonNationalExamCertificateByPassportForXml(passportSeria, passportNumber, checkSubjectMarks, userlogin, ip, shouldWriteLog, ref xml);

                return xml;
            }
            finally
            {
                CommonNationalCertificateContext.EndLock();
            }
        }

        public static XElement SearchCommonNationalExamCertificateCheckByOuterId(long? batchId)
        {
            CommonNationalCertificateContext.BeginLock();
            try
            {
                XElement xml = null;
                CommonNationalCertificateContext.Instance().SearchCommonNationalExamCertificateCheckByOuterId(batchId, ref xml);

                return xml;
            }
            finally
            {
                CommonNationalCertificateContext.EndLock();
            }
        }

        /// <summary>
        /// Добавление пакетной проверки (по регистрационному номеру)
        /// </summary>
        /// <param name="data">Текст пакетной проверки (без фильтра)</param>
        /// <param name="filter">Заголовок-фильтр (для сокращённого формата пакета)</param>
        /// <returns>Публичный идентификатор добавленной проверки</returns>
        public static long UpdateCheckBatch(string data, string filter, string clientLogin, int type, long? batchId, int? year)
        {
            CommonNationalCertificateContext.BeginLock();
            try
            {
                long? id = null;
                CommonNationalCertificateContext.Instance().UpdateCommonNationalExamCertificateCheckBatch(ref id,
                    clientLogin, ClientIp, data, filter, type, batchId, year);

                if (id == null)
                    throw new NullReferenceException(UpdateErrorMessage);

                return Convert.ToInt64(id);
            }
            finally
            {
                CommonNationalCertificateContext.EndLock();
            }
        }

        /// <summary>
        /// Добавление пакетной проверки (по регистрационному номеру)
        /// </summary>
        /// <param name="data">Текст пакетной проверки (без фильтра)</param>
        /// <param name="filter">Заголовок-фильтр (для сокращённого формата пакета)</param>
        /// <returns>Публичный идентификатор добавленной проверки</returns>
        public static long UpdateCheckBatch(string data, string filter, string clientLogin)
        {
            return UpdateCheckBatch(data, filter, clientLogin, 0, null, null);
        }



        /// <summary>
        /// Добавление пакетной проверки (по паспортным данным или типографскому номеру)
        /// </summary>
        /// <param name="data">Текст пакетной проверки (без фильтра)</param>
        /// <param name="filter">Заголовок-фильтр (для сокращённого формата пакета)</param>
        /// <param name="IsTypographicNumber">признак проверки по типографскому номеру</param>
        /// <param name="year"></param>
        /// <returns>Публичный идентификатор добавленной проверки</returns>
        public static long UpdateRequestBatch(string data, string filter, bool IsTypographicNumber, string year, string clientLogin)
        {
            CommonNationalCertificateContext.BeginLock();
            try
            {
                long? id = null;
                CommonNationalCertificateContext.Instance().UpdateCommonNationalExamCertificateRequestBatch(ref id,
                    clientLogin, ClientIp, data, filter, IsTypographicNumber, year);

                if (id == null)
                {
                    throw new NullReferenceException(UpdateErrorMessage);
                }

                return Convert.ToInt64(id);
            }
            finally
            {
                CommonNationalCertificateContext.EndLock();
            }
        }

        public static void ExecuteLoadingTask(long id, string clientLogin)
        {
            CommonNationalCertificateContext.BeginLock();
            try
            {
                CommonNationalCertificateContext.Instance().ExecuteCommonNationalExamCertificateLoadingTask(id,
                    clientLogin, ClientIp);
            }
            finally
            {
                CommonNationalCertificateContext.EndLock();
            }
        }

        public static void ExecuteDenyLoadingTask(long id, string clientLogin)
        {
            CommonNationalCertificateContext.BeginLock();
            try
            {
                CommonNationalCertificateContext.Instance().ExecuteCommonNationalExamCertificateDenyLoadingTask(id,
                   clientLogin, ClientIp);
            }
            finally
            {
                CommonNationalCertificateContext.EndLock();
            }
        }


        #region  Обработка списка поступивших

        private const string CreateTempEntrant =
            @"create table #Entrant
            (
                [Year] int,
            LastName nvarchar(255),
            FirstName nvarchar(255),
            PatronymicName nvarchar(255),
            CertificateNumber nvarchar(255),
            PassportNumber nvarchar(255),
            PassportSeria nvarchar(255),
            GIFOCategoryName nvarchar(255),
            DirectionCode nvarchar(255),
            SpecialtyCode nvarchar(255)
            )";

        private const string InsertTempEntrant =
            @"INSERT INTO #Entrant (
                [Year],
                LastName,
                FirstName,
                PatronymicName,
                CertificateNumber,
                PassportNumber,
                PassportSeria,
                GIFOCategoryName,
                DirectionCode,
                SpecialtyCode
            )
            VALUES (
                @Year,
                @LastName,
                @FirstName,
                @PatronymicName,
                @CertificateNumber,
                @PassportNumber,
                @PassportSeria,
                @GIFOCategoryName,
                @DirectionCode,
                @SpecialtyCode
            )";

        private const string DropTempEntrant = "drop table #Entrant";

        private const string UpdateTempEntrant = "dbo.UpdateEntrant";

        public static void UpdateEntrant(Entrant[] entrants)
        {
            using (SqlConnection connection = new SqlConnection(
                    global::Fbs.Core.Properties.Settings.Default.FbsConnectionString))
            {
                connection.Open();
                try
                {
                    BeginUpdateEntrant(connection);
                    try
                    {
                        foreach (Entrant entrant in entrants)
                            AddEntrant(connection, entrant);
                        UpdateEntrantsData(connection);
                    }
                    finally
                    {
                        EndUpdateEntrant(connection);
                    }
                }
                catch
                {
                    connection.Close();
                    throw;
                }
            }
        }

        private static void BeginUpdateEntrant(SqlConnection connection)
        {
            using (SqlCommand cmdCreate = connection.CreateCommand())
            {
                cmdCreate.CommandText = CreateTempEntrant;
                cmdCreate.ExecuteNonQuery();
            }
        }

        private static void AddEntrant(SqlConnection connection, Entrant entrant)
        {
            using (SqlCommand cmdInsert = connection.CreateCommand())
            {
                cmdInsert.Parameters.AddWithValue("@Year", DBNull.Value);
                cmdInsert.Parameters.AddWithValue("@LastName", entrant.LastName);
                cmdInsert.Parameters.AddWithValue("@FirstName", entrant.FirstName);
                cmdInsert.Parameters.AddWithValue("@PatronymicName",
                    GetPreparedDBValue(entrant.PatronymicName));
                cmdInsert.Parameters.AddWithValue("@CertificateNumber",
                    GetPreparedDBValue(entrant.CertificateNumber));
                cmdInsert.Parameters.AddWithValue("@PassportNumber", entrant.PassportNumber);
                cmdInsert.Parameters.AddWithValue("@PassportSeria",
                    GetPreparedDBValue(entrant.PassportSeria));
                cmdInsert.Parameters.AddWithValue("@GIFOCategoryName",
                    GetPreparedDBValue(entrant.GIFOCategoryName));
                cmdInsert.Parameters.AddWithValue("@DirectionCode",
                    GetPreparedDBValue(entrant.DirectionCode));
                cmdInsert.Parameters.AddWithValue("@SpecialtyCode",
                    GetPreparedDBValue(entrant.SpecialtyCode));

                cmdInsert.CommandText = InsertTempEntrant;
                cmdInsert.ExecuteNonQuery();
            }
        }

        private static void UpdateEntrantsData(SqlConnection connection)
        {
            using (SqlCommand cmdCommit = connection.CreateCommand())
            {
                cmdCommit.CommandType = CommandType.StoredProcedure;
                cmdCommit.Parameters.AddWithValue("@login", ClientLogin);
                cmdCommit.Parameters.AddWithValue("@ip", ClientIp);
                cmdCommit.CommandText = UpdateTempEntrant;
                cmdCommit.ExecuteNonQuery();
            }
        }

        private static void EndUpdateEntrant(SqlConnection connection)
        {
            using (SqlCommand cmdDrop = connection.CreateCommand())
            {
                cmdDrop.CommandText = DropTempEntrant;
                cmdDrop.ExecuteNonQuery();
            }
        }

        #endregion

        #region Обработка списка поступивших и забравших документы

        private const string UpdateTempEntrantRenunciation = "dbo.UpdateEntrantRenunciation";

        private const string CreateTempEntrantRenunciation =
            @"create table #EntrantRenunciation
            (
                [Year] int,
            LastName nvarchar(255),
            FirstName nvarchar(255),
            PatronymicName nvarchar(255),
            PassportNumber nvarchar(255),
            PassportSeria nvarchar(255)
            )";

        private const string InsertTempEntrantRenunciation =
            @"INSERT INTO #EntrantRenunciation (
                [Year],
                LastName,
                FirstName,
                PatronymicName,
                PassportNumber,
                PassportSeria
            )
            VALUES (
                @Year,
                @LastName,
                @FirstName,
                @PatronymicName,
                @PassportNumber,
                @PassportSeria
            )";

        private const string DropTempEntrantRenunciation = "drop table #EntrantRenunciation";

        public static void UpdateEntrantRenunciation(EntrantRenunciation[] entrants)
        {
            using (SqlConnection connection = new SqlConnection(
                    global::Fbs.Core.Properties.Settings.Default.FbsConnectionString))
            {
                connection.Open();
                try
                {
                    BeginUpdateEntrantRenunciation(connection);
                    try
                    {
                        foreach (EntrantRenunciation entrant in entrants)
                            AddEntrantRenunciation(connection, entrant);
                        UpdateEntrantRenunciationsData(connection);
                    }
                    finally
                    {
                        EndUpdateEntrantRenunciation(connection);
                    }
                }
                catch
                {
                    connection.Close();
                    throw;
                }
            }
        }

        private static void BeginUpdateEntrantRenunciation(SqlConnection connection)
        {
            using (SqlCommand cmdCreate = connection.CreateCommand())
            {
                cmdCreate.CommandText = CreateTempEntrantRenunciation;
                cmdCreate.ExecuteNonQuery();
            }
        }

        private static void AddEntrantRenunciation(SqlConnection connection, EntrantRenunciation entrant)
        {
            using (SqlCommand cmdInsert = connection.CreateCommand())
            {
                cmdInsert.Parameters.AddWithValue("@Year", DBNull.Value);
                cmdInsert.Parameters.AddWithValue("@LastName", entrant.LastName);
                cmdInsert.Parameters.AddWithValue("@FirstName", entrant.FirstName);
                cmdInsert.Parameters.AddWithValue("@PatronymicName",
                    GetPreparedDBValue(entrant.PatronymicName));
                cmdInsert.Parameters.AddWithValue("@PassportNumber", entrant.PassportNumber);
                cmdInsert.Parameters.AddWithValue("@PassportSeria",
                    GetPreparedDBValue(entrant.PassportSeria));

                cmdInsert.CommandText = InsertTempEntrantRenunciation;
                cmdInsert.ExecuteNonQuery();
            }
        }

        private static void UpdateEntrantRenunciationsData(SqlConnection connection)
        {
            using (SqlCommand cmdCommit = connection.CreateCommand())
            {
                cmdCommit.CommandType = CommandType.StoredProcedure;
                cmdCommit.Parameters.AddWithValue("@login", ClientLogin);
                cmdCommit.Parameters.AddWithValue("@ip", ClientIp);
                cmdCommit.CommandText = UpdateTempEntrantRenunciation;
                cmdCommit.ExecuteNonQuery();
            }
        }

        private static void EndUpdateEntrantRenunciation(SqlConnection connection)
        {
            using (SqlCommand cmdDrop = connection.CreateCommand())
            {
                cmdDrop.CommandText = DropTempEntrantRenunciation;
                cmdDrop.ExecuteNonQuery();
            }
        }

        #endregion

        /// <summary>
        /// Подготовка значения для передачи в запрос.
        /// </summary>
        /// <returns>
        /// Возвращает DBNull если в качестве параметра передана пустая (либо null) строка.
        /// </returns>
        private static object GetPreparedDBValue(string value)
        {
            return String.IsNullOrEmpty(value) ? (object)DBNull.Value : (object)value;
        }

        public static long UpdateEntrantCheckBatch(string data, string clientLogin)
        {
            CommonNationalCertificateContext.BeginLock();
            try
            {
                long? id = null;
                CommonNationalCertificateContext.Instance().UpdateEntrantCheckBatch(ref id,
                    clientLogin, ClientIp, data);

                if (id == null)
                    throw new NullReferenceException(UpdateErrorMessage);

                return Convert.ToInt64(id);
            }
            finally
            {
                CommonNationalCertificateContext.EndLock();
            }
        }

        public static long UpdateSchoolLeavingCertificateCheckBatch(string data, string clientLogin)
        {
            CommonNationalCertificateContext.BeginLock();
            try
            {
                long? id = null;
                CommonNationalCertificateContext.Instance().UpdateSchoolLeavingCertificateCheckBatch(ref id,
                    clientLogin, ClientIp, data);

                if (id == null)
                    throw new NullReferenceException(UpdateErrorMessage);

                return Convert.ToInt64(id);
            }
            finally
            {
                CommonNationalCertificateContext.EndLock();
            }
        }

        public static long UpdateCompetitionCertificateRequestBatch(string data, string clientLogin)
        {
            CommonNationalCertificateContext.BeginLock();
            try
            {
                long? id = null;
                CommonNationalCertificateContext.Instance().UpdateCompetitionCertificateRequestBatch(ref id,
                    clientLogin, ClientIp, data);

                if (id == null)
                    throw new NullReferenceException(UpdateErrorMessage);

                return Convert.ToInt64(id);
            }
            finally
            {
                CommonNationalCertificateContext.EndLock();
            }
        }
    }
}