namespace Fbs.Core.WebServiceCheck
{
    using System;
    using System.Xml;

    using Fbs.Core.CNEChecks;
    using Fbs.Core.Shared;

    /// <summary>
    /// The ws base check.
    /// </summary>
    public abstract class WSBaseCheck
    {
        #region Constants and Fields

        protected XmlDocument Result; // Результирующий XML, который выдается в качестве ответа веб-сервиса

        protected string accountGroup; // Группа, к которой относится пользователь

        protected WSCheckType checkType; // Признак того, является ли проверка пакетной

        protected int errorCount; // Количество ошибок

        protected InputMessage inputMessage; // Входящее сообщение

        protected string login; // Логин пользователя от имени которого выполняется проверка

        protected WSSearchType searchType; // Тип поиска

        protected static string BanErrorMessage = "Ваш доступ к проверкам свидетельств временно заблокирован. Обратитесь к администратору";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WSBaseCheck"/> class.
        /// </summary>
        /// <param name="currentLogin">
        /// The current login.
        /// </param>
        public WSBaseCheck(string currentLogin)
        {
            this.login = currentLogin;

            // инициализируем ответный XML пустым XML файлом
            this.Result = new XmlDocument();
            this.Result.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><checkResults></checkResults>");

            this.errorCount = 0;
            this.checkType = WSCheckType.wsUnknown;
            this.searchType = WSSearchType.wsUnknown;
            this.inputMessage = new InputMessage();

            this.accountGroup = Account.GetGroup(this.login);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The append certificate info.
        /// </summary>
        /// <param name="CNE">
        /// The cne.
        /// </param>
        protected void AppendCertificateInfo(CNEInfo CNE)
        {
            XmlNode CertificateNode = this.Result.LastChild.AppendChild(this.Result.CreateElement("certificate"));
            CertificateNode.InnerXml = CNE.GetXML();
        }

        /// <summary>
        /// The append error.
        /// </summary>
        /// <param name="errorText">
        /// The error text.
        /// </param>
        protected void AppendError(string errorText)
        {
            this.AppendError(errorText, false);
        }

        /// <summary>
        /// The append error.
        /// </summary>
        /// <param name="errorText">
        /// The error text.
        /// </param>
        /// <param name="withException">
        /// The with exception.
        /// </param>
        /// <exception cref="WSCheckException">
        /// </exception>
        /// <exception cref="WSErrorCountException">
        /// </exception>
        protected void AppendError(string errorText, bool withException)
        {
            XmlNode ErrorsNode = this.Result.SelectSingleNode("//errors");
            if (ErrorsNode == null)
            {
                ErrorsNode = this.Result.LastChild.AppendChild(this.Result.CreateElement("errors"));
            }

            XmlNode ErrorNode = ErrorsNode.AppendChild(this.Result.CreateElement("error"));
            ErrorNode.InnerText = errorText;

            if (withException)
            {
                throw new WSCheckException(errorText);
            }

            this.errorCount++;
            if (this.errorCount > 30 && this.accountGroup != "UserRCOI")
            {
                throw new WSErrorCountException();
            }
        }

        /// <summary>
        /// The check access.
        /// </summary>
        protected void CheckAccess()
        {
            if (string.IsNullOrEmpty(this.login))
            {
                this.AppendError("Доступ закрыт", true);
            }
        }

        /// <summary>
        /// The flush errors.
        /// </summary>
        /// <exception cref="WSCheckException">
        /// </exception>
        protected void FlushErrors()
        {
            throw new WSCheckException();
        }

        protected string FormError(string errorMessage)
        {
            return string.Format(
                @"<?xml version=""1.0"" encoding=""utf-8""? <checkResults> <errors> <error>{0}</error> </errors> </checkResults>",
                errorMessage);
        }

        /// <summary>
        /// The get search type.
        /// </summary>
        /// <param name="queryXML">
        /// The query xml.
        /// </param>
        protected void GetSearchType(string queryXML)
        {
            // Тип поискового алгоритма определяется по наполненности полей первого(!) элемента 'query'
            try
            {
                this.searchType = this.inputMessage.QueryList[0].GetSearchType();
            }
            catch (WSQueryElementException ex)
            {
                this.AppendError(ex.Message, true);
            }
        }

        /// <summary>
        /// The validate xml data.
        /// </summary>
        /// <param name="queryXML">
        /// The query xml.
        /// </param>
        protected void ValidateXMLDataNN(string queryXML)
        {
            // Пробегаемся по всем элементам 'query' и проверяем, что имеются все данные для выполнения поиска
            // по ранее определенному алгоритму
            bool errorExists = false;
            foreach (QueryItem item in this.inputMessage.QueryList)
            {
                try
                {
                    item.CheckDataNN(this.searchType);
                }
                catch (WSQueryElementException ex)
                {
                    errorExists = true;
                    this.AppendError(ex.Message);
                }
            }

            if (errorExists)
            {
                this.FlushErrors();
            }
        }

        /// <summary>
        /// The validate xml data.
        /// </summary>
        /// <param name="queryXML">
        /// The query xml.
        /// </param>
        protected void ValidateXMLData(string queryXML)
        {
            // Пробегаемся по всем элементам 'query' и проверяем, что имеются все данные для выполнения поиска
            // по ранее определенному алгоритму
            bool errorExists = false;
            foreach (QueryItem item in this.inputMessage.QueryList)
            {
                try
                {
                    item.CheckData(this.searchType);
                }
                catch (WSQueryElementException ex)
                {
                    errorExists = true;
                    this.AppendError(ex.Message);
                }
            }

            if (errorExists)
            {
                this.FlushErrors();
            }
        }

        /// <summary>
        /// The validate xml document.
        /// </summary>
        /// <param name="queryXML">
        /// The query xml.
        /// </param>
        protected void ValidateXMLDocumentNN(string queryXML,bool isByCert)
        {
            // 1. ====================================================================================
            // Должен быть задан тип проверки
            if (this.checkType != WSCheckType.wsSingleCheck && this.checkType != WSCheckType.wsBatchCheck)
            {
                this.AppendError("Тип проверки не задан", true);
            }

            // 2. ====================================================================================
            // проверяем, что входная строка это валидный XML
            var QueryDoc = new XmlDocument();
            try
            {
                QueryDoc.LoadXml(queryXML);
            }
            catch
            {
                this.AppendError("Не удалось загрузить XML-документ. Входная строка была не в формате XML", true);
            }

            // 3. ====================================================================================
            // проверяем, что есть корневой обязательный элемент <items>
            XmlNodeList rootNodes = QueryDoc.SelectNodes("./items");
            if (rootNodes.Count != 1)
            {
                this.AppendError(
                    string.Format(
                        "Входной XML имеет неверный формат. Отсутствует корневой тэг 'items'. Найдено элементов: {0}",
                        rootNodes.Count),
                    true);
            }

            // 4. ====================================================================================
            // проверяем элементы <query> и <year>
            XmlNodeList queryNodes = QueryDoc.SelectNodes("./items/query");

            // 4.1. единичная проверка
            if (this.checkType == WSCheckType.wsSingleCheck)
            {
                if (queryNodes.Count != 1)
                {
                    this.AppendError(
                        string.Format(
                            "Входной XML имеет неверный формат. Единичная проверка. Отсутствует единичный элемент 'query'. Найдено: {0}",
                            queryNodes.Count),
                        true);
                }
            }

            // 4.2. пакетная проверка
            if (this.checkType == WSCheckType.wsBatchCheck)
            {
                // проверяем, что количество элементов 'query' не больше 5000
                if (queryNodes.Count > 5000 || queryNodes.Count <= 0)
                {
                    this.AppendError(
                        string.Format(
                            "Недопустимое количество элементов 'query' для пакетной проверки. Должно быть от 1 до 5000. Найдено: {0}",
                            queryNodes.Count),
                        true);
                }
            }

            // 5. ====================================================================================
            // Проверяем структуру тэгов 'query' и формируем объект InputMessage

            // 5.1. Проверка структуры элементов 'query' и инициализация inputMessage.QueryList
            // пробегаемся по всем элементам 'query' и проверяем что все обязательные вложенные элементы присутствуют
            // проверяется только(!) структура, не данные
            bool errorExists = false;
            for (int i = 0; i < queryNodes.Count; i++)
            {
                var item = new QueryItem(queryNodes[i], i);
                try
                {
                    if (isByCert)
                    {
                        item.CheckStructureCertNumber();
                    }
                    else
                    {
                        item.CheckStructurePassportNumber();
                    }

                    this.inputMessage.QueryList.Add(item);
                }
                catch (WSQueryElementException ex)
                {
                    this.AppendError(ex.Message);
                    errorExists = true;
                }
            }

            if (errorExists)
            {
                this.inputMessage = new InputMessage();
                this.FlushErrors();
            }
        }

        /// <summary>
        /// The validate xml document.
        /// </summary>
        /// <param name="queryXML">
        /// The query xml.
        /// </param>
        protected void ValidateXMLDocument(string queryXML)
        {
            // 1. ====================================================================================
            // Должен быть задан тип проверки
            if (this.checkType != WSCheckType.wsSingleCheck && this.checkType != WSCheckType.wsBatchCheck)
            {
                this.AppendError("Тип проверки не задан", true);
            }

            // 2. ====================================================================================
            // проверяем, что входная строка это валидный XML
            var QueryDoc = new XmlDocument();
            try
            {
                QueryDoc.LoadXml(queryXML);
            }
            catch
            {
                this.AppendError("Не удалось загрузить XML-документ. Входная строка была не в формате XML", true);
            }

            // 3. ====================================================================================
            // проверяем, что есть корневой обязательный элемент <items>
            XmlNodeList rootNodes = QueryDoc.SelectNodes("./items");
            if (rootNodes.Count != 1)
            {
                this.AppendError(
                    string.Format(
                        "Входной XML имеет неверный формат. Отсутствует корневой тэг 'items'. Найдено элементов: {0}", 
                        rootNodes.Count), 
                    true);
            }

            // 4. ====================================================================================
            // проверяем элементы <query> и <year>
            XmlNodeList queryNodes = QueryDoc.SelectNodes("./items/query");

            // 4.1. единичная проверка
            if (this.checkType == WSCheckType.wsSingleCheck)
            {
                if (queryNodes.Count != 1)
                {
                    this.AppendError(
                        string.Format(
                            "Входной XML имеет неверный формат. Единичная проверка. Отсутствует единичный элемент 'query'. Найдено: {0}", 
                            queryNodes.Count), 
                        true);
                }
            }

            // 4.2. пакетная проверка
            if (this.checkType == WSCheckType.wsBatchCheck)
            {
                // проверяем, что количество элементов 'query' не больше 5000
                if (queryNodes.Count > 5000 || queryNodes.Count <= 0)
                {
                    this.AppendError(
                        string.Format(
                            "Недопустимое количество элементов 'query' для пакетной проверки. Должно быть от 1 до 5000. Найдено: {0}", 
                            queryNodes.Count), 
                        true);
                }
            }

            // 5. ====================================================================================
            // Проверяем структуру тэгов 'query' и формируем объект InputMessage

            // 5.1. Проверка структуры элементов 'query' и инициализация inputMessage.QueryList
            // пробегаемся по всем элементам 'query' и проверяем что все обязательные вложенные элементы присутствуют
            // проверяется только(!) структура, не данные
            bool errorExists = false;
            for (int i = 0; i < queryNodes.Count; i++)
            {
                var item = new QueryItem(queryNodes[i], i);
                try
                {
                    item.CheckStructure();
                    this.inputMessage.QueryList.Add(item);
                }
                catch (WSQueryElementException ex)
                {
                    this.AppendError(ex.Message);
                    errorExists = true;
                }
            }

            if (errorExists)
            {
                this.inputMessage = new InputMessage();
                this.FlushErrors();
            }
        }

        /// <summary>
        /// The validate xml document.
        /// </summary>
        /// <param name="queryXML">
        /// The query xml.
        /// </param>
        protected void ValidateXMLDocumentForOpenedFbs(string queryXML)
        {
            // 1. ====================================================================================
            // Должен быть задан тип проверки
            if (this.checkType != WSCheckType.wsSingleCheck && this.checkType != WSCheckType.wsBatchCheck)
            {
                this.AppendError("Тип проверки не задан", true);
            }

            // 2. ====================================================================================
            // проверяем, что входная строка это валидный XML
            var QueryDoc = new XmlDocument();
            try
            {
                QueryDoc.LoadXml(queryXML);
            }
            catch
            {
                this.AppendError("Не удалось загрузить XML-документ. Входная строка была не в формате XML", true);
            }

            // 3. ====================================================================================
            // проверяем, что есть корневой обязательный элемент <items>
            XmlNodeList rootNodes = QueryDoc.SelectNodes("./items");
            if (rootNodes.Count != 1)
            {
                this.AppendError(
                    string.Format(
                        "Входной XML имеет неверный формат. Отсутствует корневой тэг 'items'. Найдено элементов: {0}",
                        rootNodes.Count),
                    true);
            }

            // 4. ====================================================================================
            // проверяем элементы <query> и <year>
            XmlNodeList queryNodes = QueryDoc.SelectNodes("./items/query");

            // 4.1. единичная проверка
            if (this.checkType == WSCheckType.wsSingleCheck)
            {
                if (queryNodes.Count != 1)
                {
                    this.AppendError(
                        string.Format(
                            "Входной XML имеет неверный формат. Единичная проверка. Отсутствует единичный элемент 'query'. Найдено: {0}",
                            queryNodes.Count),
                        true);
                }
            }

            // 4.2. пакетная проверка
            if (this.checkType == WSCheckType.wsBatchCheck)
            {
                // проверяем, что количество элементов 'query' не больше 5000
                if (queryNodes.Count > 5000 || queryNodes.Count <= 0)
                {
                    this.AppendError(
                        string.Format(
                            "Недопустимое количество элементов 'query' для пакетной проверки. Должно быть от 1 до 5000. Найдено: {0}",
                            queryNodes.Count),
                        true);
                }
            }

            // 5. ====================================================================================
            // Проверяем структуру тэгов 'query' и формируем объект InputMessage

            // 5.1. Проверка структуры элементов 'query' и инициализация inputMessage.QueryList
            // пробегаемся по всем элементам 'query' и проверяем что все обязательные вложенные элементы присутствуют
            // проверяется только(!) структура, не данные
            bool errorExists = false;
            for (int i = 0; i < queryNodes.Count; i++)
            {
                var item = new QueryItem(queryNodes[i], i);
                try
                {
                    item.CheckStructure();
                    this.inputMessage.QueryList.Add(item);
                }
                catch (WSQueryElementException ex)
                {
                    this.AppendError(ex.Message);
                    errorExists = true;
                }
            }

            if (errorExists)
            {
                this.inputMessage = new InputMessage();
                this.FlushErrors();
            }
        }

        /// <summary>
        /// The validate xml get result document.
        /// </summary>
        /// <param name="queryXML">
        /// The query xml.
        /// </param>
        protected void ValidateXMLGetResultDocument(string queryXML)
        {
            // 1. ====================================================================================
            // проверяем, что входная строка это валидный XML
            var QueryDoc = new XmlDocument();
            try
            {
                QueryDoc.LoadXml(queryXML);
            }
            catch
            {
                this.AppendError("Не удалось загрузить XML-документ. Входная строка была не в формате XML", true);
            }

            // 2. ====================================================================================
            // проверяем, что есть корневой обязательный элемент <items>
            XmlNodeList rootNodes = QueryDoc.SelectNodes("./items");
            if (rootNodes.Count != 1)
            {
                this.AppendError(
                    string.Format(
                        "Входной XML имеет неверный формат. Отсутствует корневой тэг 'items'. Найдено элементов: {0}", 
                        rootNodes.Count), 
                    true);
            }

            // 3. ====================================================================================
            // проверяем элементы <batchId>
            XmlNodeList queryNodes = QueryDoc.SelectNodes("./items/batchId");
            if (queryNodes.Count != 1)
            {
                this.AppendError(
                    string.Format(
                        "Входной XML имеет неверный формат. Отсутствует единичный элемент 'batchId'. Найдено: {0}", 
                        queryNodes.Count), 
                    true);
            }

            // 4. ====================================================================================
            // в качестве значения элемента ./items/batchId должен быть валидный Guid
            Guid resultGuid;
            if (!Utils.ParseGuid(queryNodes[0].InnerText.Trim(), out resultGuid))
            {
                this.AppendError(
                    "Входной XML имеет неверный формат. Значение в элементе 'batchId' должно быть Guid.", true);
            }
        }

        #endregion
    }
}