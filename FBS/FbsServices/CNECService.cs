namespace FbsServices
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Xml;
    using System.Xml.Serialization;

    using Fbs.Core.CNEChecks;

    using FbsChecksClient;
    using FbsWebViewModel.CNEC;
    using Fbs.Core.Organizations;

    /// <summary>
    /// Тип проверки
    /// </summary>
    public enum CheckType
    {
        /// <summary>
        /// Запрос по регистрационному номеру и ФИО
        /// </summary>
        CNENumber = 0, 

        /// <summary>
        /// Запрос по ФИО и баллам по предметам
        /// </summary>
        Marks = 1, 

        /// <summary>
        /// Запрос по ФИО, номеру и серии документа
        /// </summary>
        Passport = 2, 

        /// <summary>
        /// Запрос по типографскому номеру и ФИО
        /// </summary>
        Typographic = 3,

        /// <summary>
        /// Запрос по номеру и баллам из открытой ФБС
        /// </summary>
        CNENumberOpen = 4,

        /// <summary>
        /// Запрос по паспорту и баллам из открытой ФБС
        /// </summary>
        PassportOpen = 5, 
    }

    /// <summary>
    /// Сервис для работы с егэ сертификатами
    /// </summary>
    public class CNECService
    {
        #region Properties

        /// <summary>
        /// Настройка web.config 
        /// </summary>
        private bool IsOpenFbs
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableOpenedFbs"]);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add check batch result.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <param name="batchid">
        /// The batchid.
        /// </param>
        /// <returns>
        /// </returns>
        public DataTable AddCheckBatchResult(XmlElement xml, long batchid)
        {
            var result = new DataTable();

            using (var cmd = new Command("dbo.usp_cne_AddCheckBatchResult"))
            {
                cmd.Parameters.Add("@xml", SqlDbType.Xml).Value =
                    new SqlXml(new XmlTextReader(new StringReader(string.Format("<root>{0}</root>", xml.InnerXml))));
                cmd.Parameters.Add("@batchid", SqlDbType.BigInt).Value = batchid;
                SqlDataReader reader = cmd.ExecuteReader();
                result.Load(reader);
            }

            return result;
        }

        /// <summary>
        /// Количество проверкок свидетельств организацией (включая проверки через веб-сервис удаленных проверок, в том числе и через ФИС ЕГЭ и приема)
        /// </summary>
        /// <param name="orgId">
        /// Идентификатор организации 
        /// </param>
        /// <param name="isUniqueCheck">
        /// Какие проверки выбирать(уникальные/все) 
        /// </param>
        /// <param name="typeCheck">
        /// Тип проверки(фильтр)
        /// </param>
        /// <param name="family">
        /// Фамилия(фильтр)
        /// </param>
        /// <returns>
        /// Количество проверкок свидетельств организацией 
        /// </returns>
        public int CountCNECCheckHystoryByOrgId(long orgId, bool isUniqueCheck, string typeCheck, string family, string dats, string datf)
        {
            // если вызов в открытой фбс
            if (this.IsOpenFbs)
            {
                var checkClient = new WSCheckClient();

                return checkClient.CountCNECCheckHystoryByOrgId(orgId, isUniqueCheck, typeCheck, family, dats, datf);
            }

            using (var cmd = new Command("dbo.SelectCNECCheckHystoryByOrgIdCount"))
            {
                cmd.Parameters.Add("@idorg", SqlDbType.Int).Value = orgId;
                cmd.Parameters.Add("@isUnique", SqlDbType.Bit).Value = isUniqueCheck;
                cmd.Parameters.Add("@TypeCheck", SqlDbType.NChar).Value = typeCheck;
                cmd.Parameters.Add("@LastName", SqlDbType.NChar).Value = family;
                cmd.Parameters.Add("@dats", SqlDbType.DateTime).Value = dats;
                cmd.Parameters.Add("@datf", SqlDbType.DateTime).Value = datf;
                return (int)cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Удаление пакетной проверки по id
        /// </summary>
        /// <param name="id">
        /// Идентификатор проверки
        /// </param>
        public void DeleteCheckFromCommonNationalExamCertificateCheckBatch(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id");
            }

            using (var cmd = new Command("dbo.DeleteCheckFromCommonNationalExamCertificateCheckBatchById"))
            {
                cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = id;
                cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Удаление пакетной проверки по id
        /// </summary>
        /// <param name="id">
        /// Идентификатор проверки
        /// </param>
        public void DeleteCheckFromCommonNationalExamCertificateRequestBatch(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id");
            }

            using (var cmd = new Command("dbo.DeleteCheckFromCommonNationalExamCertificateRequestBatchById"))
            {
                cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = id;
                cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// список свидетельств для пользователя
        /// </summary>
        /// <param name="lastName">
        /// Фамилия
        /// </param>
        /// <param name="firstName">
        /// Имя
        /// </param>
        /// <param name="patronymicName">
        /// Отчество
        /// </param>
        /// <param name="passportNumber">
        /// Номер паспорта
        /// </param>
        /// <param name="passportSeria">
        /// Серия паспорта
        /// </param>
        /// <param name="currentCertificateNumber">
        /// номер сертификата
        /// </param>
        /// <returns>
        /// список свидетельств
        /// </returns>
        public List<HistoryCertificateView> GetCertificateForUser(
            string lastName, 
            string firstName, 
            string patronymicName, 
            string passportNumber, 
            string passportSeria, 
            string currentCertificateNumber)
        {
            // если вызов в открытой фбс
            if (this.IsOpenFbs)
            {
                var checkClient = new WSCheckClient();

                var xml = checkClient.GetCertificateByFioAndPassport(
                    currentCertificateNumber, passportNumber, passportSeria, lastName, firstName, patronymicName);
                var output = new StringReader(xml);
                var serializer = new XmlSerializer(typeof(List<HistoryCertificateView>));
                return serializer.Deserialize(output) as List<HistoryCertificateView>;
            }

            /*if (string.IsNullOrEmpty(currentCertificateNumber))
            {
                throw new ArgumentException("certificateId");
            }

            if (string.IsNullOrEmpty(passportNumber))
            {
                throw new ArgumentException("passportNumber");
            }*/

            var result = new List<HistoryCertificateView>();

            using (var cmd = new Command("dbo.GetCertificateByFioAndPassport"))
            {
                cmd.Parameters.Add("@LastName", SqlDbType.NChar).Value = lastName;
                cmd.Parameters.Add("@PassportNumber", SqlDbType.NChar).Value = passportNumber;
                cmd.Parameters.Add("@PassportSeria", SqlDbType.NChar).Value = string.IsNullOrEmpty(passportSeria) ? string.Empty : passportSeria;
                cmd.Parameters.Add("@CurrentCertificateNumber", SqlDbType.NChar).Value = currentCertificateNumber;

                for (SqlDataReader reader = cmd.ExecuteReader(); reader.Read();)
                {
                    var item = new HistoryCertificateView
                        {
                            Status = reader["Status"] == DBNull.Value ? null : reader["Status"].ToString(), 
                            Year = reader["Year"] == DBNull.Value ? null : reader["Year"].ToString(), 
                            Certificate =
                                new Link
                                    {
                                        Text = reader["Number"] == DBNull.Value ? null : reader["Number"].ToString(), 
                                        Url =
                                            string.Format(
                                                "/Certificates/CommonNationalCertificates/CheckResult.aspx?number={0}", 
                                                reader["Number"] == DBNull.Value ? null : reader["Number"].ToString())
                                    }, 
                            Marks = reader["Marks"] == DBNull.Value ? null : reader["Marks"].ToString()
                        };

                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// The get data for print.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="ip">
        /// The ip.
        /// </param>
        /// <param name="number">
        /// The number.
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
        public DataTable GetDataForPrint(
            string login, string ip, string number, string firstName, string lastName, string patronymicName)
        {
            var result = new DataTable();

            if (this.IsOpenFbs)
            {
                var checkClient = new WSCheckClient();
                XmlElement xmlElement = null;
                checkClient.CheckCommonNationalExamCertificateByNumberForXml(
                    number, string.Empty, null, login, ip, false, ref xmlElement);

                var dataSet = new DataSet();
                dataSet.ReadXml(
                    new XmlTextReader(new StringReader(string.Format("<root>{0}</root>", xmlElement.InnerXml))));

                if (dataSet.Tables.Count > 0)
                {
                    return dataSet.Tables[0];
                }

                return null;
            }

            using (var cmd = new Command("dbo.CheckCommonNationalExamCertificateByNumber"))
            {
                cmd.Parameters.Add("@number", SqlDbType.NChar).Value = number;
                cmd.Parameters.Add("@checkFirstName", SqlDbType.NChar).Value = firstName;
                cmd.Parameters.Add("@checkLastName", SqlDbType.NChar).Value = lastName;
                cmd.Parameters.Add("@checkPatronymicName", SqlDbType.NChar).Value = patronymicName;
                cmd.Parameters.Add("@login", SqlDbType.NChar).Value = login;
                cmd.Parameters.Add("@ip", SqlDbType.NChar).Value = ip;
                result.Load(cmd.ExecuteReader());
            }

            return result;
        }

        /// <summary>
        /// выбрать уникальные проверки сертификата вузами с учетом пейджинга
        /// </summary>
        /// <param name="certificateId">
        /// id сертификата
        /// </param>
        /// <param name="startRow">
        /// начальная запись
        /// </param>
        /// <param name="maxRow">
        /// кол-во
        /// </param>
        /// <returns>
        /// список проверок
        /// </returns>
        public List<CNECCheckHistoryView> SelectCNECCheckHystory(Guid certificateId, int startRow, int maxRow)
        {
            if (certificateId == null)
            {
                throw new ArgumentException("certificateId");
            }


            var result = new List<CNECCheckHistoryView>();

            if (this.IsOpenFbs)
            {
                // todo: Implement and call WSCheckClient method
                return result;
            }

            using (var cmd = new Command("dbo.SearchCommonNationalExamCertificateCheckHistory"))
            {
                cmd.Parameters.Add("@certificateId", SqlDbType.UniqueIdentifier).Value = certificateId;
                cmd.Parameters.AddPaging(startRow, maxRow);
                for (SqlDataReader r = cmd.ExecuteReader(); r.Read();)
                {
                    var orgId = (int)r["OrganizationId"];
                    CNECCheckHistoryView existingOrg = result.FirstOrDefault(h => h.OrganizationId == orgId);
                    if (existingOrg == null)
                    {
                        existingOrg = new CNECCheckHistoryView
                            {
                                OrganizationId = orgId, 
                                OrganizationFullName = (string)r["OrganizationFullName"], 
                                CertificateId = (Guid)r["CertificateId"], 
                                CheckEntries = new List<CNECCheckHystoryEntryView>()
                            };

                        result.Add(existingOrg);
                    }

                    var cnecCheckHystoryEntryView = new CNECCheckHystoryEntryView(existingOrg)
                        {
                           Date = (DateTime)r["Date"] 
                        };
                    var isBatch = (bool)r["CheckType"];
                    cnecCheckHystoryEntryView.CheckType = isBatch ? CNECCheckType.Batch : CNECCheckType.Interactive;
                    existingOrg.CheckEntries.Add(cnecCheckHystoryEntryView);
                }
            }

            return result;
        }

        /// <summary>
        /// Все проверки свидетельств организацией (включая проверки через веб-сервис удаленных проверок, в том числе и через ФИС ЕГЭ и приема)
        /// c поддержкой пейджинга, сортировки и уникальности проверок
        /// </summary>
        /// <param name="orgId">
        /// Идентификатор организации 
        /// </param>
        /// <param name="startRow">
        /// начальная запись 
        /// </param>
        /// <param name="maxRow">
        /// количество выбираемых записей 
        /// </param>
        /// <param name="sortBy">
        /// Поле по которому сортируем 
        /// </param>
        /// <param name="isUniqueCheck">
        /// Какие проверки выбирать(уникальные/все) 
        /// </param>
        /// <param name="sortorder">
        /// Порядок сортировки 
        /// </param>
        /// <param name="typeCheck">
        /// Тип проверки(фильтр)
        /// </param>
        /// <param name="family">
        /// Фамилия(фильтр)
        /// </param>
        /// <returns>
        /// Все проверки свидетельств организацией
        /// </returns>
        public List<HistoryCheckCertificateForOrganizationView> SelectCNECCheckHystoryByOrgId(
            long orgId, 
            int startRow, 
            int maxRow, 
            string sortBy, 
            bool isUniqueCheck, 
            int sortorder, 
            string typeCheck, 
            string family,
            string dats,
            string datf)
        {
            // если вызов в открытой фбс
            if (this.IsOpenFbs)
            {
                var checkClient = new WSCheckClient();

                var xml = checkClient.SelectCNECCheckHystoryByOrgId(
                    orgId, startRow, maxRow, sortBy, isUniqueCheck, sortorder, typeCheck, family, dats, datf);
                var list = this.DeserializeList<HistoryCheckCertificateForOrganizationView>(xml);
                foreach (var view in list)
                {
                    view.Certificate.Url =
                        string.Format(
                            "/Certificates/CommonNationalCertificates/CheckResultForOpenedFbs.aspx?number={0}&check={1}",
                            view.Number,
                            CheckUtil.GetCheckHash(HttpContext.Current.User.Identity.Name, view.Number));
                }

                return list;
            }

            if (orgId <= 0)
            {
                return null;
            }

            var result = new List<HistoryCheckCertificateForOrganizationView>();
            using (var cmd = new Command("dbo.SelectCNECCheckHystoryByOrgId"))
            {
                cmd.Parameters.Add("@idorg", SqlDbType.Int).Value = orgId;
                cmd.Parameters.Add("@ps", SqlDbType.Int).Value = startRow;
                cmd.Parameters.Add("@pf", SqlDbType.Int).Value = maxRow;
                cmd.Parameters.Add("@fld", SqlDbType.NChar).Value = sortBy;
                cmd.Parameters.Add("@so", SqlDbType.Int).Value = sortorder;
                cmd.Parameters.Add("@isUnique", SqlDbType.Bit).Value = isUniqueCheck;
                cmd.Parameters.Add("@TypeCheck", SqlDbType.NChar).Value = typeCheck;
                cmd.Parameters.Add("@dats", SqlDbType.DateTime).Value = dats;
                cmd.Parameters.Add("@datf", SqlDbType.DateTime).Value = datf;
                cmd.Parameters.Add("@LastName", SqlDbType.NChar).Value = family;

                for (SqlDataReader reader = cmd.ExecuteReader(); reader.Read();)
                {
                    string number = reader["CertificateNumber"] == DBNull.Value
                                        ? null
                                        : reader["CertificateNumber"].ToString();
                    var item = new HistoryCheckCertificateForOrganizationView
                        {
                            Marks = reader["Marks"] == DBNull.Value ? null : reader["Marks"].ToString(), 
                            NumberRow = Convert.ToInt32(reader["rn"]),
                            Number = number,
                            Id = Convert.ToInt32(reader["Id"]), 
                            DateCheck = (DateTime)reader["CreateDate"], 
                            Certificate =
                                new Link
                                    {
                                        Text = number, 
                                        Url = string.Format("/Certificates/CommonNationalCertificates/CheckResult.aspx?number={0}", number)
                                    }, 
                            CountCheck =
                                reader["UniqueIHEaFCheck"] == DBNull.Value
                                    ? string.Empty
                                    : reader["UniqueIHEaFCheck"].ToString(), 
                            Document = reader["PassportData"] == DBNull.Value ? null : reader["PassportData"].ToString(), 
                            FirstName = reader["FirstName"] == DBNull.Value ? null : reader["FirstName"].ToString(), 
                            LastName = reader["LastName"] == DBNull.Value ? null : reader["LastName"].ToString(), 
                            PatronymicName =
                                reader["PatronymicName"] == DBNull.Value ? null : reader["PatronymicName"].ToString(), 
                            Status = reader["Status"] == DBNull.Value ? null : reader["Status"].ToString(), 
                            TypeCheck = reader["TypeCheck"] == DBNull.Value ? null : reader["TypeCheck"].ToString(), 
                            TypographicNumber =
                                reader["TypographicNumber"] == DBNull.Value
                                    ? null
                                    : reader["TypographicNumber"].ToString(), 
                            Year = reader["year"] == DBNull.Value ? null : reader["year"].ToString()
                        };
                    item.PrintNote = new Link
                        {
                            Text =
                                 "Распечатать справку"
                        };

                    result.Add(item);
                }
            }

            return result;
        }

        private List<T> DeserializeList<T>(string xml)
        {
            var output = new StringReader(xml);
            var serializer = new XmlSerializer(typeof(List<T>));
            return serializer.Deserialize(output) as List<T>;
        }

        /// <summary>
        /// получить число вузову, которые проверяли сертификат
        /// </summary>
        /// <param name="certificateId">
        /// id сертификата
        /// </param>
        /// <returns>
        /// число вузов
        /// </returns>
        public int SelectCNECCheckHystoryCount(Guid certificateId)
        {
            if (certificateId == null)
            {
                throw new ArgumentException("certificateId");
            }

            if (this.IsOpenFbs)
            {
                // todo: Implement and call WSCheckClient method
                return 0;
            }


            using (var cmd = new Command("dbo.SearchCommonNationalExamCertificateCheckHistory"))
            {
                cmd.Parameters.Add("@certificateId", SqlDbType.UniqueIdentifier).Value = certificateId;
                return (int)cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Выборка истории о итерационных проверках по типу с поддержкой пейджинга
        /// </summary>
        /// <param name="login">
        /// Логин текущего пользователя
        /// </param>
        /// <param name="type">
        /// Тип проверки
        /// </param>
        /// <returns>
        /// Выборка истории о итерационных проверках по типу
        /// </returns>
        public int SelectCheckHystoryCount(
            string login, CheckType type)
        {
            // если вызов в открытой фбс
            if (this.IsOpenFbs)
            {
                var checkClient = new WSCheckClient();
                return checkClient.SelectCheckHystoryCount(login, (int)type);
            }

            using (var cmd = new Command("dbo.GetNEWebUICheckLog"))
            {
                cmd.Parameters.Add("@login", SqlDbType.NChar).Value = login;
                cmd.Parameters.Add("@showCount", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@TypeCode", SqlDbType.NChar).Value = type.ToString();
                return (int)cmd.ExecuteScalar();
            }
        }


        /// <summary>
        /// Выборка истории о итерационных проверках по типу с поддержкой пейджинга
        /// </summary>
        /// <param name="login">
        /// Логин текущего пользователя
        /// </param>
        /// <param name="startRowIndex">
        /// С какой записи
        /// </param>
        /// <param name="maxRowCount">
        /// Сколько записей
        /// </param> 
        /// </param>
        /// <param name="type">
        /// Тип проверки
        /// </param>
        /// <returns>
        /// Выборка истории о итерационных проверках по типу
        /// </returns>
        public List<HistoryCheckCertificateView> SelectCheckHystory(
            string login, int startRowIndex, int maxRowCount, CheckType type)
        {
            // если вызов в открытой фбс
            if (this.IsOpenFbs)
            {
                var checkClient = new WSCheckClient();

                var xml = checkClient.SelectCheckHystory(
                    login, startRowIndex, maxRowCount, (int)type);
                var openResult = this.DeserializeList<HistoryCheckCertificateView>(xml);
                foreach (var view in openResult)
                {
                    view.LinkResult.Url = this.GetResult(view, type);
                }

                return openResult;
            }

            var result = new List<HistoryCheckCertificateView>();
            using (var cmd = new Command("dbo.GetNEWebUICheckLog"))
            {
                cmd.Parameters.Add("@login", SqlDbType.NChar).Value = login;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = startRowIndex;
                cmd.Parameters.Add("@maxRowCount", SqlDbType.Int).Value = maxRowCount;
                cmd.Parameters.Add("@showCount", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@TypeCode", SqlDbType.NChar).Value = type.ToString();
                for (SqlDataReader reader = cmd.ExecuteReader(); reader.Read();)
                {
                    var item = new HistoryCheckCertificateView
                        {
                            Marks = reader["Marks"] == DBNull.Value ? null : reader["Marks"].ToString(), 
                            Login = reader["login"] == DBNull.Value ? null : reader["login"].ToString(), 
                            LastName = reader["LastName"] == DBNull.Value ? null : reader["LastName"].ToString(), 
                            FirstName = reader["FirstName"] == DBNull.Value ? null : reader["FirstName"].ToString(), 
                            PatronymicName =
                                reader["PatronymicName"] == DBNull.Value ? null : reader["PatronymicName"].ToString(), 
                            DateCreate = (DateTime)reader["EventDate"], 
                            Status = Convert.ToInt32(reader["CheckCertificate"]) == 1 ? "Найдено" : "Не найдено", 
                            LinkResult = new Link { Text = "Результат" }, 
                            TypographicNumber =
                                reader["TypographicNumber"] == DBNull.Value
                                    ? null
                                    : reader["TypographicNumber"].ToString(), 
                            Id = reader["Id"] == DBNull.Value ? (long?)null : Convert.ToInt64(reader["id"]), 
                            PassportNumber =
                                reader["PassportNumber"] == DBNull.Value ? null : reader["PassportNumber"].ToString(), 
                            YearCertificate =
                                reader["YearCertificate"] == DBNull.Value
                                    ? (int?)null
                                    : Convert.ToInt32(reader["YearCertificate"]), 
                            PassportSeria =
                                reader["PassportSeria"] == DBNull.Value ? null : reader["PassportSeria"].ToString(), 
                            CNENumber = reader["CNENumber"] == DBNull.Value ? null : reader["CNENumber"].ToString()
                        };

                    item.LinkResult.Url = this.GetResult(item, type);

                    result.Add(item);
                }
            }

            return result;
        }

        #region Интерактивные проверки - новые методы

        public int GetCheckHistoryCommonPageCount(string login, CommonCheckType checkType, CertificateCheckMode checkMode)
        {
            using (var cmd = new Command("[dbo].[GetCheckHistoryPagesCount]"))
            {
                cmd.Parameters.Add(new SqlParameter("@login", SqlDbType.VarChar)).Value = login ?? (object)DBNull.Value;
                cmd.Parameters.Add(new SqlParameter("@rowsOnPageCount", SqlDbType.Int)).Value = DBNull.Value;
                cmd.Parameters.Add(new SqlParameter("@senderType", SqlDbType.Int)).Value = (int) checkMode;
                cmd.Parameters.Add(new SqlParameter("@checkType", SqlDbType.Int)).Value = (int) checkType;

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public List<HistoryCheckViewCommon> GetCheckHistoryCommonPaged(string login, CommonCheckType checkType, CertificateCheckMode checkMode,
                                                                       int startRowIndex, int pageSize)
        {
            using (var cmd = new Command("[dbo].[GetCheckHistoryPaged]"))
            {
                cmd.Parameters.Add(new SqlParameter("@login", SqlDbType.VarChar)).Value = login ?? (object) DBNull.Value;
                cmd.Parameters.Add(new SqlParameter("@startRowIndex", SqlDbType.Int)).Value = startRowIndex;
                cmd.Parameters.Add(new SqlParameter("@rowsOnPageCount", SqlDbType.Int)).Value = pageSize;
                cmd.Parameters.Add(new SqlParameter("@senderType", SqlDbType.Int)).Value = (int) checkMode;
                cmd.Parameters.Add(new SqlParameter("@checkType", SqlDbType.Int)).Value = (int)checkType;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    List<HistoryCheckViewCommon> results = null;

                    while (reader.Read())
                    {
                        if (results == null)
                        {
                            results = new List<HistoryCheckViewCommon>(pageSize);
                        }

                        results.Add(new HistoryCheckViewCommon
                            {
                                BatchId = reader.Get<long?>("BatchId"),
                                CheckId = reader.Get<long>("CheckId"),
                                CreateDate = reader.Get<DateTime>("CreateDate"),
                                FirstName = reader.Get<string>("FirstName"),
                                LastName = reader.Get<string>("LastName"),
                                Login = reader.Get<string>("Login"),
                                OrganizationId = reader.Get<long?>("OrganizationId"),
                                OrganizationName = reader.Get<string>("OrganizationName"),
                                OwnerId = reader.Get<long>("OwnerId"),
                                PatronymicName = reader.Get<string>("PatronymicName"),
                                RowNumber = reader.Get<long>("RowNumber"),
                                Status = reader.Get<string>("Status")
                            });
                    }

                    return results;
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Получение ссылки на результат
        /// </summary>
        /// <param name="item">
        /// Данные по проверке из которых формируем ссылку
        /// </param>
        /// <param name="type">
        /// Тип проверки
        /// </param>
        /// <returns>
        /// Ссылка на результат проверки
        /// </returns>
        private string GetResult(HistoryCheckCertificateView item, CheckType type)
        {
            string enable;
            switch (type)
            {
                case CheckType.CNENumberOpen:
                        enable =
                            string.Format(
                                "/Certificates/CommonNationalCertificates/CheckResultForOpenedFbs.aspx?number={0}&SubjectMarks={1}&check={2}", 
                                item.CNENumber, 
                                item.Marks,
                                CheckUtil.GetCheckHash(HttpContext.Current.User.Identity.Name, item.CNENumber));
                        break;
                case CheckType.CNENumber:    
                        enable =
                            string.Format(
                                "/Certificates/CommonNationalCertificates/CheckResult.aspx?number={0}&LastName={1}&FirstName={2}&PatronymicName={3}&SubjectMarks={4}&Ev={5}", 
                                item.CNENumber, 
                                item.LastName, 
                                item.FirstName, 
                                item.PatronymicName, 
                                item.Marks, 
                                item.Id);
                    break;
                case CheckType.Marks:
                    enable =
                        string.Format(
                            "/Certificates/CommonNationalCertificates/RequestByMarksResult.aspx?LastName={0}&FirstName={1}&PatronymicName={2}&SubjectMarks={3}&Ev={4}", 
                            item.LastName, 
                            item.FirstName, 
                            item.PatronymicName, 
                            item.Marks, 
                            item.Id);
                    break;
                case CheckType.PassportOpen:
                        enable =
                            string.Format(
                                "/Certificates/CommonNationalCertificates/RequestByPassportResultForOpenedFbs.aspx?Series={0}&Number={1}&SubjectMarks={2}&ShouldWriteLogs=false", 
                                item.PassportSeria, 
                                item.PassportNumber, 
                                item.Marks, 
                                item.Id);
                    break;
                    case CheckType.Passport:
                        enable =
                            string.Format(
                                "/Certificates/CommonNationalCertificates/RequestByPassportResult.aspx?LastName={0}&FirstName={1}&PatronymicName={2}&Series={3}&Number={4}&Year={5}&Ev={6}", 
                                item.LastName, 
                                item.FirstName, 
                                item.PatronymicName, 
                                item.PassportSeria, 
                                item.PassportNumber, 
                                item.YearCertificate, 
                                item.Id);
                    break;
                case CheckType.Typographic:
                    enable =
                        string.Format(
                            "/Certificates/CommonNationalCertificates/RequestByTypographicNumberResult.aspx?LastName={0}&FirstName={1}&PatronymicName={2}&TypographicNumber={3}&Ev={4}", 
                            item.LastName, 
                            item.FirstName, 
                            item.PatronymicName, 
                            item.TypographicNumber, 
                            item.Id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return enable;
        }

        #endregion
    }
}