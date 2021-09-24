
namespace Fbs.Core.WebServiceCheck
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Transactions;
    using System.Xml;
    using System.Xml.Linq;

    using Fbs.Core.BatchCheck;
    using Fbs.Core.CNEChecks;
    using Fbs.Core.Shared;
    using Fbs.Core.UICheckLog;
    using Fbs.Utility;

    using FbsChecksClient;
    using FbsChecksClient.WSChecksReference;

    /// <summary>
    /// The ws batch check.
    /// </summary>
    public class WSBatchCheck : WSBaseCheck
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WSBatchCheck"/> class.
        /// </summary>
        /// <param name="currentLogin">
        /// The current login.
        /// </param>
        public WSBatchCheck(string currentLogin)
            : base(currentLogin)
        {
            this.checkType = WSCheckType.wsBatchCheck;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// GetQueryByPassportNumberSample
        /// </summary>
        /// <returns>
        /// sample
        /// </returns>
        public string GetBatchCheckQuerySampleNN()
        {
            return "<items>" + "<query>"
                  + "<passportSeria>1234</passportSeria>"
                  + "<passportNumber>123456</passportNumber>"
                  + "<certificateNumber>12-123456789-12</certificateNumber>"
                  + "<marks>" +
                        "<mark>" +
                        " <subjectName>1</subjectName>" +
                        " <subjectMark>65,0</subjectMark>" +
                        "</mark>" +
                        "<mark>" +
                        " <subjectName>2</subjectName>" +
                        " <subjectMark>36,5</subjectMark>" +
                        "</mark>" +
                    "</marks>"
                  + "</query>"
                  + "<query>"
                  + "<passportSeria>5678</passportSeria>"
                  + "<passportNumber>987654</passportNumber>"
                  + "<certificateNumber>12-123456789-12</certificateNumber>"
                  + "<marks>" +
                        "<mark>" +
                        " <subjectName>1</subjectName>" +
                        " <subjectMark>65,0</subjectMark>" +
                        "</mark>" +
                        "<mark>" +
                        " <subjectName>2</subjectName>" +
                        " <subjectMark>36,5</subjectMark>" +
                        "</mark>" +
                    "</marks>"
                  + "</query>"
                  + "<query>"
                  + "<passportSeria>7263</passportSeria>"
                  + "<passportNumber>902837</passportNumber>"
                  + "<certificateNumber>12-123456789-12</certificateNumber>"
                  + "<marks>" +
                        "<mark>" +
                        " <subjectName>1</subjectName>" +
                        " <subjectMark>65,0</subjectMark>" +
                        "</mark>" +
                        "<mark>" +
                        " <subjectName>2</subjectName>" +
                        " <subjectMark>36,5</subjectMark>" +
                        "</mark>" +
                    "</marks>"
                  + "</query>"
                  + "</items>";
        }


        /// <summary>
        /// The begin batch check.
        /// </summary>
        /// <param name="queryXML">
        /// The query xml.
        /// </param>
        /// <returns>
        /// The begin batch check.
        /// </returns>
        public string BeginBatchCheckNN(string queryXML)
        {
            try
            {
                // Проверяем авторизацию
                this.CheckAccess();

                // проверить тип проверки (по сертификату или по паспорту). По паспорту - более приоритетная проверка
                var matches = Regex.Matches(queryXML, @"\<passportNumber\>.+\</passportNumber>");
                if (matches.Count > 0)
                {
                    this.searchType = WSSearchType.wsPassport;
                }
                else
                {
                    this.searchType = WSSearchType.wsCertNumber;
                }

                // Проверяем структуру входного XML-сообщения
                this.ValidateXMLDocumentNN(queryXML, this.searchType == WSSearchType.wsCertNumber);

                Guid? batchId = null;

                // Проверка ошибок в наполнении (в данных) XML-сообщения
                this.ValidateXMLDataNN(queryXML);

                switch (this.searchType)
                {
                    case WSSearchType.wsPassport:
                        batchId = this.CheckPassportNumberQueryNN();
                        break;

                    case WSSearchType.wsCertNumber:
                        batchId = this.CheckCertificateNumberQueryNN();

                        break;
                }

                if (batchId == Guid.Empty)
                {
                    return this.FormError(BanErrorMessage);
                }

                if (!batchId.HasValue)
                {
                    this.AppendError("Пустой идентификатор пакета", true);
                }
                else
                {
                    this.AppendBatchId(batchId.Value);
                }
            }
            catch (WSCheckException)
            {
                // ошибка будет в this.Result.OuterXml
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                return this.FormError("server error. conact administrator for information");
            }

            return this.Result.OuterXml;
        }

        /// <summary>
        /// The begin batch check.
        /// </summary>
        /// <param name="queryXML">
        /// The query xml.
        /// </param>
        /// <returns>
        /// The begin batch check.
        /// </returns>
        public string BeginBatchCheck(string queryXML)
        {
            try
            {
                if (Account.IsBanned(this.login))
                {
                    return this.FormError(BanErrorMessage);
                }

                // Проверяем авторизацию
                this.CheckAccess();

                // Проверяем структуру входного XML-сообщения
                this.ValidateXMLDocument(queryXML);

                // Определяем тип поиска
                this.GetSearchType(queryXML);

                // Проверка ошибок в наполнении (в данных) XML-сообщения
                this.ValidateXMLData(queryXML);

                // Добавляем пакет в очередь на обработку и формируем результирующий XML
                this.BeginBatchCheck();
            }
            catch (WSCheckException)
            {
            }

            return this.Result.OuterXml;
        }

        /// <summary>
        /// The get query sample.
        /// </summary>
        /// <returns>
        /// The get query sample.
        /// </returns>
        public string GetQuerySample()
        {
            return "<items>" + "<query>" + "<firstName>Иван</firstName>" + "<lastName>Иванов</lastName>"
                   + "<patronymicName>Иванович</patronymicName>" + "<passportSeria>1234</passportSeria>"
                   + "<passportNumber>123456</passportNumber>"
                   + "<certificateNumber>12-123456789-12</certificateNumber>"
                   + "<typographicNumber>1234567</typographicNumber>" + "</query>" + "<query>"
                   + "<firstName>Сергей</firstName>" + "<lastName>Волков</lastName>"
                   + "<patronymicName>Александрович</patronymicName>" + "<passportSeria>5678</passportSeria>"
                   + "<passportNumber>987654</passportNumber>"
                   + "<certificateNumber>72-983746271-92</certificateNumber>"
                   + "<typographicNumber>9047832</typographicNumber>" + "</query>" + "<query>"
                   + "<firstName>Виктор</firstName>" + "<lastName>Смирнов</lastName>"
                   + "<patronymicName>Николаевич</patronymicName>" + "<passportSeria>7263</passportSeria>"
                   + "<passportNumber>902837</passportNumber>"
                   + "<certificateNumber>63-783940251-94</certificateNumber>"
                   + "<typographicNumber>7839461</typographicNumber>" + "</query>" + "</items>";
        }

        /// <summary>
        /// The get result.
        /// </summary>
        /// <param name="password"> </param>
        /// <param name="queryXML">
        ///   The query xml.
        /// </param>
        /// <returns>
        /// The get result.
        /// </returns>
        public string GetResultNN(string password, string queryXML)
        {
            var checkClient = new WSCheckClient();
            var userCredentials = new UserCredentials { Login = login, Password = password };
            var result = checkClient.GetBatchCheckResult(userCredentials, queryXML);
            return this.RemoveNames(result);
        }

        private string RemoveNames(string result)
        {
            var res = Regex.Replace(result, @"\<lastName\>.*?\</lastName\>", string.Empty);
            res = Regex.Replace(res, @"\<firstName\>.*?\</firstName\>", string.Empty);
            res = Regex.Replace(res, @"\<patronymicName\>.*?\</patronymicName\>", string.Empty);
            return res;
        }

        /// <summary>
        /// The get result.
        /// </summary>
        /// <param name="queryXML">
        /// The query xml.
        /// </param>
        /// <returns>
        /// The get result.
        /// </returns>
        public string GetResult(string queryXML)
        {
            try
            {
                if (Account.IsBanned(this.login))
                {
                    return this.FormError(BanErrorMessage);
                }

                // Проверяем авторизацию
                this.CheckAccess();

                // Проверяем структуру входного XML-сообщения
                this.ValidateXMLGetResultDocument(queryXML);

                // Проверяем, что идентификатор это валидный Guid
                Guid batchId = this.GetBatchUniqueId(queryXML);

                // Проверка статуса пакета
                WSSearchType searchType;
                if (this.CheckStatus(batchId, out searchType))
                {
                    // Если пакет обработан, то возвращаем результат
                    this.GetBatchResult(batchId, searchType);
                }
            }
            catch (WSCheckException)
            {
            }

            return this.Result.OuterXml;
        }

        public Guid StartCheck(string data, int type, long batchId, int? year)
        {
            var id = CommonNationalCertificateContext.UpdateCheckBatch(data, null, this.login, type, batchId, year);
            return this.GetBatchGUID(id);
        }

        public XElement CheckCommonNationalExamCertificateByNumberForXml(string number, string checkSubjectMarks, string participantid, string userlogin, string ip, bool shouldCheckMarks)
        {
            return CommonNationalCertificateContext.CheckCommonNationalExamCertificateByNumberForXml(number, checkSubjectMarks, participantid, userlogin, ip, shouldCheckMarks);
        }

        public XElement CheckCommonNationalExamCertificateByPassportForXml(string passportSeria, string passportNumber, string checkSubjectMarks, string userlogin, string ip, bool shouldWriteLog)
        {
            return CommonNationalCertificateContext.CheckCommonNationalExamCertificateByPassportForXml(passportSeria, passportNumber, checkSubjectMarks, userlogin, ip, shouldWriteLog);
        }

        public XElement SearchCommonNationalExamCertificateCheckByOuterId(long batchId)
        {
            return CommonNationalCertificateContext.SearchCommonNationalExamCertificateCheckByOuterId(batchId);
        }

        #endregion

        #region Methods


        protected void AppendBatchId(long id)
        {
            XmlNode CertificateNode = this.Result.LastChild.AppendChild(this.Result.CreateElement("batchId"));
            CertificateNode.InnerXml = id.ToString();
        }

        /// <summary>
        /// The append batch id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        protected void AppendBatchId(Guid id)
        {
            XmlNode CertificateNode = this.Result.LastChild.AppendChild(this.Result.CreateElement("batchId"));
            CertificateNode.InnerXml = id.ToString();
        }

        private void AppendStatus(int statusCode, string statusMessage)
        {
            XmlNode codeNode = this.Result.LastChild.AppendChild(this.Result.CreateElement("statusCode"));
            codeNode.InnerXml = statusCode.ToString();

            XmlNode messageNode = this.Result.LastChild.AppendChild(this.Result.CreateElement("statusMessage"));
            messageNode.InnerXml = statusMessage;
        }
        
        private void BeginBatchCheck()
        {
            Guid? batchId = null;

            switch (this.searchType)
            {
                case WSSearchType.wsPassport:
                    batchId = this.CheckPasswordQuery();
                    break;

                case WSSearchType.wsCertNumber:
                    batchId = this.CheckCertificateNumberQuery();
                    break;

                case WSSearchType.wsTypoNumber:
                    batchId = this.CheckTypographicNumberQuery();
                    break;
            }

            if (!batchId.HasValue)
            {
                this.AppendError("Пустой идентификатор пакета", true);
            }
            else
            {
                this.AppendBatchId(batchId.Value);
            }
        }


        private Guid? CheckPassportNumberQueryNN()
        {
            // Таблица с ошибками парсинга
            var resultData = new DataTable();

            using (StreamReader reader = this.inputMessage.GetPasspertNumberCSVNN())
            {
                var checkByCertificateNumber = new CheckByPassportAndSubjectValues();

                if (!checkByCertificateNumber.Parse(reader, this.accountGroup, ref resultData))
                {
                    DataRow row;
                    for (int i = 0; i < resultData.Rows.Count; i++)
                    {
                        row = resultData.Rows[i];
                        this.AppendError(
                            string.Format("Номер строки: {0}, ошибки: {1}", row["RowIndex"], row["Комментарий"]));
                    }

                    this.FlushErrors();
                    return null;
                }

                // Добавлю данные в бд
                reader.BaseStream.Position = 0;
                var batchText = reader.ReadToEnd();
                var checkClient = new WSCheckClient();
                return checkClient.StartBatchCheck(this.login, batchText, 2, 0, null);
            }
        }


        private Guid? CheckCertificateNumberQueryNN()
        {
            // Таблица с ошибками парсинга
            var resultData = new DataTable();

            using (StreamReader reader = this.inputMessage.GetCertificateNumberCSVNN())
            {
                var checkByCertificateNumber = new CheckByCertificateNumberAndSubjectValues();

                if (!checkByCertificateNumber.Parse(reader, this.accountGroup, ref resultData))
                {
                    DataRow row;
                    for (int i = 0; i < resultData.Rows.Count; i++)
                    {
                        row = resultData.Rows[i];
                        this.AppendError(
                            string.Format("Номер строки: {0}, ошибки: {1}", row["RowIndex"], row["Комментарий"]));
                    }

                    this.FlushErrors();
                    return null;
                }
                
                // Добавлю данные в бд
                reader.BaseStream.Position = 0;
                var batchText = reader.ReadToEnd();
                var checkClient = new WSCheckClient();
                return checkClient.StartBatchCheck(this.login, batchText, 1, 0, null);
            }
        }

        private Guid? CheckCertificateNumberQuery()
        {
            // Таблица с ошибками парсинга
            var resultData = new DataTable();

            using (StreamReader reader = this.inputMessage.GetCertificateNumberCSV())
            {
                var checkByCertificateNumber = new BatchCheckByCertificateNumber();

                if (!checkByCertificateNumber.Parse(reader, this.accountGroup, ref resultData))
                {
                    DataRow row;
                    for (int i = 0; i < resultData.Rows.Count; i++)
                    {
                        row = resultData.Rows[i];
                        this.AppendError(
                            string.Format("Номер строки: {0}, ошибки: {1}", row["RowIndex"], row["Комментарий"]));
                    }

                    this.FlushErrors();
                    return null;
                }
                else
                {
                    // Добавлю данные в бд
                    reader.BaseStream.Position = 0;
                    long id = CommonNationalCertificateContext.UpdateCheckBatch(reader.ReadToEnd(), null, this.login);

                    return this.GetBatchGUID(id);
                }
            }
        }

        private Guid? CheckPasswordQuery()
        {
            // Таблица с ошибками парсинга
            var resultData = new DataTable();

            using (StreamReader reader = this.inputMessage.GetPassportCSV())
            {
                var checkByPassport = new BatchCheckByPassport();
                if (!checkByPassport.Parse(reader, this.accountGroup, ref resultData))
                {
                    DataRow row;
                    for (int i = 0; i < resultData.Rows.Count; i++)
                    {
                        row = resultData.Rows[i];
                        this.AppendError(
                            string.Format("Номер строки: {0}, ошибки: {1}", row["RowIndex"], row["Комментарий"]));
                    }

                    this.FlushErrors();
                    return null;
                }
                else
                {
                    // Добавлю данные в бд
                    reader.BaseStream.Position = 0;
                    long id = CommonNationalCertificateContext.UpdateRequestBatch(
                        reader.ReadToEnd(),
                        // текст пакетной проверки
                        null,
                        false,
                        // пакетная проверка не является типографской
                        null,
                        // год для пакетной проверки
                        this.login);

                    return this.GetBatchGUID(id);
                }
            }
        }

        private bool CheckStatus(Guid batchId, out WSSearchType searchType)
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
                        cmd.CommandText = "[dbo].[GetBatchStatusById]";

                        cmd.Parameters.AddWithValue("batchUniqueId", batchId);
                        cmd.Parameters.AddWithValue("userLogin", this.login);

                        var isProcess = new SqlParameter("isProcess", SqlDbType.Bit);
                        isProcess.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(isProcess);

                        var isCorrect = new SqlParameter("isCorrect", SqlDbType.Bit);
                        isCorrect.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(isCorrect);

                        var isFound = new SqlParameter("isFound", SqlDbType.Bit);
                        isFound.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(isFound);

                        var searchTypePrm = new SqlParameter("searchType", SqlDbType.Int);
                        searchTypePrm.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(searchTypePrm);

                        cmd.ExecuteNonQuery();

                        searchType = (WSSearchType)searchTypePrm.Value;

                        if (!(bool)isFound.Value)
                        {
                            this.AppendStatus(0, "Не найден");
                            return false;
                        }

                        if (!(bool)isCorrect.Value)
                        {
                            AppendStatus(-1, "Возникла ошибка во время обработки");
                            return false;
                        }

                        if ((bool)isProcess.Value)
                        {
                            this.AppendStatus(1, "В обработке");
                            return false;
                        }
                        else
                        {
                            this.AppendStatus(2, "Обработан");
                            return true;
                        }
                    }
                }
            }
        }

        private Guid? CheckTypographicNumberQuery()
        {
            // Таблица с ошибками парсинга
            var resultData = new DataTable();

            using (StreamReader reader = this.inputMessage.GetTypographicNumberCSV())
            {
                var checkByTypographicNumber = new BatchCheckByTypographicNumber();

                if (!checkByTypographicNumber.Parse(reader, this.accountGroup, ref resultData))
                {
                    DataRow row;
                    for (int i = 0; i < resultData.Rows.Count; i++)
                    {
                        row = resultData.Rows[i];
                        this.AppendError(
                            string.Format("Номер строки: {0}, ошибки: {1}", row["RowIndex"], row["Комментарий"]));
                    }

                    this.FlushErrors();
                    return null;
                }
                else
                {
                    // Добавлю данные в бд
                    reader.BaseStream.Position = 0;

                    long id = CommonNationalCertificateContext.UpdateRequestBatch(
                        reader.ReadToEnd(),
                        // текст пакетной проверки
                        null,
                        true,
                        // пакетная проверка является типографской
                        null,
                        // год не нужен, так как типографский номер является уникальным
                        this.login);

                    return this.GetBatchGUID(id);
                }
            }
        }

        private Guid GetBatchGUID(long id)
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

                        cmd.CommandText =
                            @"
                        select
                            B.BatchUniqueId
                        from
                            BatchGUID B
                        where
                            B.Id = @Id AND B.WSSearchType = @searchType
                        ";
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@searchType", (int)this.searchType);

                        var uniqueId = (Guid?)cmd.ExecuteScalar();

                        if (uniqueId.HasValue)
                        {
                            scope.Complete();
                            return uniqueId.Value;
                        }
                        else
                        {
                            uniqueId = Guid.NewGuid();
                            cmd.CommandText =
                                @"
                            insert into BatchGUID (BatchUniqueId, Id, WSSearchType) 
                            values (@batchId, @id, @searchType)
                            ";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@batchId", uniqueId.Value);
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.Parameters.AddWithValue("@searchType", (int)this.searchType);
                            cmd.ExecuteNonQuery();
                            scope.Complete();
                            return uniqueId.Value;
                        }
                    }
                }
            }
        }

        private long GetBatchId(Guid batchGuid)
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

                        cmd.CommandText =
                            @"
                        select
                            B.Id
                        from
                            BatchGUID B
                        where
                            B.BatchUniqueId = @batchGuid
                        ";
                        cmd.Parameters.AddWithValue("@batchGuid", batchGuid);
                        var id = (long?)cmd.ExecuteScalar();

                        if (!id.HasValue)
                        {
                            this.AppendError(string.Format("Код пакета не найден {0}", batchGuid.ToString()), true);
                        }

                        return id.Value;
                    }
                }
            }
        }

        private void GetBatchResult(Guid batchGuid, WSSearchType searchType)
        {
            // В зависимости от типа проверки пакета (по паспорту, по номеру сертификата, по типографскому номеру)
            // вызываем нужную хранимую процедуру для выборки результатов проверки
            long batchId = this.GetBatchId(batchGuid);
            var result = new List<CNEInfo>();

            // Пакет проверялся по типографскому номеру или номеру/серии паспорта
            if (searchType == WSSearchType.wsTypoNumber || searchType == WSSearchType.wsPassport)
            {
                result = this.GetTypographicOrPassportNumberResult(batchId);
            }

            // Пакет проверялся номеру сертификата
            if (searchType == WSSearchType.wsCertNumber)
            {
                result = CheckDataAccessor.GetCertResult(batchId, null, this.login);
            }

            // Формируем выходное XML-сообщение
            foreach (CNEInfo item in result)
            {
                this.AppendCertificateInfo(item);
            }
        }

        private string GetStringValue(object val)
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

        private Guid GetBatchUniqueId(string queryXML)
        {
            var QueryDoc = new XmlDocument();
            QueryDoc.LoadXml(queryXML);
            XmlNodeList queryNodes = QueryDoc.SelectNodes("./items/batchId");
            Guid result;

            Utils.ParseGuid(queryNodes[0].InnerText.Trim(), out result);
            return result;
        }

        private List<CNEInfo> GetTypographicOrPassportNumberResult(long batchId)
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
                        cmd.CommandText = "[dbo].[SearchCommonNationalExamCertificateRequest]";

                        cmd.Parameters.AddWithValue("login", this.login);
                        cmd.Parameters.AddWithValue("batchId", batchId);
                        cmd.Parameters.AddWithValue("isExtended", true);

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
                            item = new CNEInfo
                                {
                                    LastName = this.GetStringValue(row["Surname"]),
                                    FirstName = this.GetStringValue(row["Name"]),
                                    PatronymicName = this.GetStringValue(row["SecondName"]),
                                    PassportSeria = this.GetStringValue(row["DocumentSeries"]),
                                    PassportNumber = this.GetStringValue(row["DocumentNumber"]),
                                    CertificateNumber = this.GetStringValue(row["CertificateNumber"]),
                                    TypographicNumber = this.GetStringValue(row["TypographicNumber"]),
                                    Year = this.GetStringValue(row["UseYear"]),
                                    Status = this.GetStringValue(row["StatusName"]),
                                    CertificateDeny = Convert.ToInt16(row["Cancelled"]),
                                    CertificateNewNumber = this.GetStringValue(row["DenyNewCertificateNumber"]),
                                    CertificateDenyComment = this.GetStringValue(row["DenyComment"]),
                                    UniqueIHEaFCheck =
                                        string.IsNullOrEmpty(row["UniqueIHEaFCheck"].ToString())
                                            ? 0
                                            : Convert.ToInt16(row["UniqueIHEaFCheck"])
                                };
                            item.Marks.Add(
                                "Русский язык",
                                this.GetStringValue(row["RussianMark"]),
                                this.GetStringValue(row["RussianHasAppeal"]));
                            item.Marks.Add(
                                "Математика",
                                this.GetStringValue(row["MathematicsMark"]),
                                this.GetStringValue(row["MathematicsHasAppeal"]));
                            item.Marks.Add(
                                "Физика",
                                this.GetStringValue(row["PhysicsMark"]),
                                this.GetStringValue(row["PhysicsHasAppeal"]));
                            item.Marks.Add(
                                "Химия",
                                this.GetStringValue(row["ChemistryMark"]),
                                this.GetStringValue(row["ChemistryHasAppeal"]));
                            item.Marks.Add(
                                "Биология",
                                this.GetStringValue(row["BiologyMark"]),
                                this.GetStringValue(row["BiologyHasAppeal"]));
                            item.Marks.Add(
                                "История",
                                this.GetStringValue(row["RussiaHistoryMark"]),
                                this.GetStringValue(row["RussiaHistoryHasAppeal"]));
                            item.Marks.Add(
                                "География",
                                this.GetStringValue(row["GeographyMark"]),
                                this.GetStringValue(row["GeographyHasAppeal"]));
                            item.Marks.Add(
                                "Английский язык",
                                this.GetStringValue(row["EnglishMark"]),
                                this.GetStringValue(row["EnglishHasAppeal"]));
                            item.Marks.Add(
                                "Немецкий язык",
                                this.GetStringValue(row["GermanMark"]),
                                this.GetStringValue(row["GermanHasAppeal"]));
                            item.Marks.Add(
                                "Французский язык",
                                this.GetStringValue(row["FranchMark"]),
                                this.GetStringValue(row["FranchHasAppeal"]));
                            item.Marks.Add(
                                "Обществознание",
                                this.GetStringValue(row["SocialScienceMark"]),
                                this.GetStringValue(row["SocialScienceHasAppeal"]));
                            item.Marks.Add(
                                "Литература",
                                this.GetStringValue(row["LiteratureMark"]),
                                this.GetStringValue(row["LiteratureHasAppeal"]));
                            item.Marks.Add(
                                "Испанский язык",
                                this.GetStringValue(row["SpanishMark"]),
                                this.GetStringValue(row["SpanishHasAppeal"]));
                            item.Marks.Add(
                                "Информатика и ИКТ",
                                this.GetStringValue(row["InformationScienceMark"]),
                                this.GetStringValue(row["InformationScienceHasAppeal"]));

                            result.Add(item);
                        }

                        return result;
                    }
                }
            }
        }

        #endregion
    }
}