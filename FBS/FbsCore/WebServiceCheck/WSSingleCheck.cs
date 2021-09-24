using System.Collections.Generic;
using System.Text.RegularExpressions;
using Fbs.Core.CNEChecks;
using Fbs.Core.Shared;

namespace Fbs.Core.WebServiceCheck
{
    using System;
    using System.Xml;

    using Fbs.Utility;

    using FbsChecksClient;
    using FbsChecksClient.WSChecksReference;

    public class WSSingleCheck : WSBaseCheck
    {
        #region Constructors

        public WSSingleCheck(string currentLogin)
            : base(currentLogin)
        {
            checkType = WSCheckType.wsSingleCheck;
        }

        #endregion

        #region Public Methods

        public string SingleCheckNN(string login, string userHostAddress, string queryXml)
        {
            var returnXML = new XmlDocument();
            try
            {
                // Проверяем авторизацию
                this.CheckAccess();

                // проверить тип проверки (по сертификату или по паспорту). По паспорту - более приоритетная проверка
                var matches = Regex.Matches(queryXml, @"\<passportNumber\>.+\</passportNumber>");
                if (matches.Count > 0)
                {
                    this.searchType = WSSearchType.wsPassport;
                }
                else
                {
                    this.searchType = WSSearchType.wsCertNumber;
                }

                // Проверяем структуру входного XML-сообщения
                this.ValidateXMLDocumentNN(queryXml, this.searchType == WSSearchType.wsCertNumber);

                // Проверка ошибок в наполнении (в данных) XML-сообщения
                this.ValidateXMLDataNN(queryXml);

                var queryItems = this.inputMessage.QueryList;
                if (queryItems == null || queryItems.Count == 0)
                {
                    return returnXML.OuterXml;
                }

                QueryItem qItem = queryItems[0];

                var checkClient = new WSCheckClient();

                XmlDocument result;
                if (this.searchType == WSSearchType.wsPassport)
                {
                    if (this.FormResultNNForPassport(checkClient, login, qItem.QueryPassportSeria.Value, qItem.QueryPassportNumber.Value, qItem.Marks.Value, userHostAddress, out result) == WebServiceReplyCodes.UserIsBanned)
                    {
                        return this.FormError(BanErrorMessage);
                    }

                    return result.OuterXml;
                }

                if (this.FormResultNNForCert(checkClient, login, qItem.QueryCertificateNumber.Value, qItem.Marks.Value, userHostAddress, true, out result) == WebServiceReplyCodes.UserIsBanned)
                {
                    return this.FormError(BanErrorMessage);
                }

                return result.OuterXml;
            }
            catch (WSCheckException)
            {
                return this.Result.InnerXml;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                return "<error>server certificate check error. contact administrator for more information</error>";
            }
        }

        private WebServiceReplyCodes FormResultNNForCert(WSCheckClient checkClient, string login, string certNumber, string checkMarks, string host, bool shouldCheckMarks, out XmlDocument returnXml)
        {
            returnXml = new XmlDocument();
            XmlElement xml = null;

            if (checkClient.CheckCommonNationalExamCertificateByNumberForXml(
                        certNumber,
                        checkMarks,
                        null,
                        login,
                        host,
                        shouldCheckMarks,
                        ref xml) == (int)WebServiceReplyCodes.UserIsBanned)
            {
                return WebServiceReplyCodes.UserIsBanned;
            }
           
            XmlDocument resultXml = new XmlDocument();

            resultXml.LoadXml(string.Format("<root>{0}</root>", xml.InnerXml));
            var rootNodes = resultXml.SelectNodes("./root/check");
            if (rootNodes == null || rootNodes.Count == 0)
            {
                return WebServiceReplyCodes.Ok;
            }

            XmlNode checkResultsNode = returnXml.CreateElement("checkResults");

            // результаты денормализованы по оценкам. поэтому сведения о св-ве можно взять из первого узла, а оценки - суммарно по всем
            var certNode = this.AddNode(returnXml, checkResultsNode, "certificate");
            this.AddNode(
                returnXml,
                certNode,
                "passportSeria",
                rootNodes[0].SelectSingleNode("./PassportSeria").InnerText);
            this.AddNode(
                returnXml,
                certNode,
                "passportNumber",
                rootNodes[0].SelectSingleNode("./PassportNumber").InnerText);
            this.AddNode(
                returnXml,
                certNode,
                "certificateNumber",
                rootNodes[0].SelectSingleNode("./Number").InnerText);
            this.AddNode(
                returnXml,
                certNode,
                "typographicNumber",
                rootNodes[0].SelectSingleNode("./TypographicNumber").InnerText);
            this.AddNode(returnXml, certNode, "year", rootNodes[0].SelectSingleNode("./Year").InnerText);
            this.AddNode(returnXml, certNode, "status", rootNodes[0].SelectSingleNode("./Status").InnerText);
            this.AddNode(
                returnXml,
                certNode,
                "uniqueIHEaFCheck",
                rootNodes[0].SelectSingleNode("./UniqueIHEaFCheck").InnerText);
            this.AddNode(
                returnXml, certNode, "certificateDeny", rootNodes[0].SelectSingleNode("./IsDeny").InnerText);
            this.AddNode(
                returnXml,
                certNode,
                "certificateNewNumber",
                rootNodes[0].SelectSingleNode("./DenyNewcertificateNumber").InnerText);
            this.AddNode(
                returnXml,
                certNode,
                "certificateDenyComment",
                rootNodes[0].SelectSingleNode("./DenyComment").InnerText);

            // собрать оценки
            var marks = this.AddNode(returnXml, certNode, "marks");
            for (int i = 0; i < rootNodes.Count; i++)
            {
                var mark = this.AddNode(returnXml, marks, "mark");
                this.AddNode(returnXml, mark, "subjectName", rootNodes[i].SelectSingleNode("./SubjectName").InnerText);
                this.AddNode(returnXml, mark, "subjectMark", rootNodes[i].SelectSingleNode("./SubjectMark").InnerText);
                this.AddNode(returnXml, mark, "subjectAppeal", rootNodes[i].SelectSingleNode("./HasAppeal").InnerText);
            }

            returnXml.AppendChild(checkResultsNode);
            return WebServiceReplyCodes.Ok;
        }

        private WebServiceReplyCodes FormResultNNForPassport(WSCheckClient checkClient, string login, string passportSeria, string passportNumber, string checkMarks, string host, out XmlDocument returnXml)
        {
            returnXml = new XmlDocument();
            XmlElement xml = null;

            if (checkClient.CheckCommonNationalExamCertificateByPassportForXml(
                passportSeria,
                passportNumber,
                checkMarks,
                login,
                host,
                true,
                ref xml) == (int)WebServiceReplyCodes.UserIsBanned)
            {
                return WebServiceReplyCodes.UserIsBanned;
            }


            XmlDocument resultXml = new XmlDocument();

            resultXml.LoadXml(string.Format("<root>{0}</root>", xml.InnerXml));
            XmlNodeList rootNodes = resultXml.SelectNodes("./root/check");
            if (rootNodes == null || rootNodes.Count == 0)
            {
                return WebServiceReplyCodes.Ok;
            }

            XmlNode checkResultsNode = returnXml.CreateElement("checkResults");
            foreach (XmlNode rootNode in rootNodes)
            {
                string certNumber = rootNode.SelectSingleNode("./CertificateNumber").InnerText;
                XmlDocument formResultNnForCert;
                if(this.FormResultNNForCert(checkClient, login, certNumber, checkMarks, host, false, out formResultNnForCert) == WebServiceReplyCodes.UserIsBanned)
                {
                    return WebServiceReplyCodes.UserIsBanned;
                }

                var certificates = formResultNnForCert.SelectNodes("./checkResults/certificate");
                if (certificates == null || certificates.Count == 0)
                {
                    var exception = new Exception(string.Format("для паспорта {0}-{1} вторичный поиск не нашел сертификат {2}", passportSeria, passportNumber, certNumber));
                    LogManager.Error(exception);
                    throw exception;
                }

                var foundCert = certificates[0];

                XmlDocumentFragment xfrag = returnXml.CreateDocumentFragment();
                xfrag.InnerXml = "<certificate>" + foundCert.InnerXml + "</certificate>";
                checkResultsNode.AppendChild(xfrag);
            }

            returnXml.AppendChild(checkResultsNode);
            return WebServiceReplyCodes.Ok;
        }

        private XmlNode AddNode(XmlDocument doc, XmlNode node, string name)
        {
            XmlNode newNode = doc.CreateElement(name);
            node.AppendChild(newNode);
            return newNode;
        }

        private XmlNode AddNode(XmlDocument doc, XmlNode node, string name, string value)
        {
            XmlNode newNode = doc.CreateElement(name);
            newNode.InnerText = value;
            node.AppendChild(newNode);
            return newNode;
        }

        public string Check(string login, string queryXML)
        {
            try
            {
                // Проверяем авторизацию
                CheckAccess();

                // Проверяем структуру входного XML-сообщения
                ValidateXMLDocument(queryXML);

                // Определяем тип поиска
                GetSearchType(queryXML);

                // Проверка ошибок в наполнении (в данных) XML-сообщения
                ValidateXMLData(queryXML);

                CheckSingleQuery2(login);
            }
            catch (WSCheckException) { }

            return Result.OuterXml;
        }

        public string CheckForOpenedFbs(string login, string queryXML)
        {
            try
            {
                // Проверяем авторизацию
                CheckAccess();

                // Проверяем структуру входного XML-сообщения
                ValidateXMLDocumentForOpenedFbs(queryXML);

                // Определяем тип поиска
                GetSearchType(queryXML);

                // Проверка ошибок в наполнении (в данных) XML-сообщения
                ValidateXMLData(queryXML);

                CheckSingleQuery(login);
            }
            catch (WSCheckException)
            {
            }

            return Result.OuterXml;
        }

        public string GetQuerySample()
        {
            return
                "<items>" +
                    "<query>" +
                        "<firstName>Иван</firstName>" +
                        "<lastName>Иванов</lastName>" +
                        "<patronymicName>Иванович</patronymicName>" +
                        "<passportSeria>1234</passportSeria>" +
                        "<passportNumber>123456</passportNumber>" +
                        "<certificateNumber>12-123456789-12</certificateNumber>" +
                        "<typographicNumber>1234567</typographicNumber>" +
                    "</query>" +
                "</items>";
        }

        public string GetQuerySampleNN()
        {
            return
                "<items>" +
                    "<query>" +
                        "<certificateNumber>12-123456789-12</certificateNumber>" +
                        "<passportSeria>1234</passportSeria>" +
                        "<passportNumber>123456</passportNumber>" +
                        "<marks>" +
                        "<mark>" +
                        " <subjectName>1</subjectName>" +
                        " <subjectMark>65,0</subjectMark>" +
                        "</mark>" +
                        "<mark>" +
                        " <subjectName>2</subjectName>" +
                        " <subjectMark>36,5</subjectMark>" +
                        "</mark>" +
                        "</marks>" +
                    "</query>" +
                "</items>";
        }
        #endregion

        #region Private Methods

        private void CheckData(WSSearchType searchType, QueryItem queryItem)
        {
            bool error = false;
            string errorcomment = string.Empty;
            string errorMessage = string.Empty;

            //Проверка поля ФАМИЛИЯ
            string lastName = queryItem.QueryLastName.Value.Trim();

            if ((string.IsNullOrEmpty(lastName) ||
                 (!Regex.IsMatch(lastName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$") ||
                  lastName.StartsWith("-") || lastName.EndsWith("-"))))
            {
                errorMessage = string.Format(" {0}{1}", error ? "," : "", "Ф");
                errorcomment += errorMessage;
                error = true;
            }

            //Проверка поля ИМЯ
            string firstName = queryItem.QueryFirstName.Value.Trim();
            if (!string.IsNullOrEmpty(firstName) &&
                (!Regex.IsMatch(firstName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$") ||
                 firstName.StartsWith("-") || firstName.EndsWith("-")))
            {
                errorMessage = string.Format(" {0}{1}", error ? "," : "", "И");
                errorcomment += errorMessage;
                error = true;
            }

            //Проверка поля ОТЧЕСТВО
            string patronymicName = queryItem.QueryPatronymicName.Value.Trim();
            if (!string.IsNullOrEmpty(patronymicName) &&
                (!Regex.IsMatch(patronymicName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$")
                 || patronymicName.StartsWith("-") || patronymicName.EndsWith("-")))
            {
                errorMessage = string.Format(" {0}{1}", error ? "," : "", "О");
                errorcomment += errorMessage;
            }

            //Проверка поля НОМЕР СВИДЕТЕЛЬСТВА
            if (searchType == WSSearchType.wsCertNumber)
            {
                string number = queryItem.QueryCertificateNumber.Value.Trim();
                if (!Regex.IsMatch(number, @"^\d{2}-\d{9}-\d{2}$"))
                {
                    errorMessage = string.Format(" {0}{1}", error ? "," : "", "НС");
                    errorcomment += errorMessage;
                }
            }
            else if (searchType == WSSearchType.wsTypoNumber)
            {
                string typeNumber = queryItem.QueryTypographicNumber.Value.Trim();
                if (!Regex.IsMatch(typeNumber, @"^[0-9]{7,8}$"))
                {
                    errorMessage = string.Format(" {0}{1}", error ? "," : "", "ТН");
                    errorcomment += errorMessage;
                }
            }
            else if (searchType == WSSearchType.wsPassport)
            {
                // Проверка поля СЕРИЯ ПАСПОРТА
                string passportSeria = queryItem.QueryPassportSeria.Value.Trim();
                List<string> sErrors = DocumentCheck.DocSeriesCheck(passportSeria);
                if (sErrors.Count > 0)
                {
                    errorMessage = string.Format(" {0}{1}", error ? "," : "", "СП");
                    errorcomment += errorMessage;
                }

                // Проверка поля НОМЕР ПАСПОРТА
                string passportNumber = queryItem.QueryPassportNumber.Value.Trim();
                List<string> nErrors = DocumentCheck.DocNumberCheck(passportNumber);
                if (nErrors.Count > 0)
                {
                    errorMessage = string.Format(" {0}{1}", error ? "," : "", "НП");
                    errorcomment += errorMessage;
                }

            }

            if (!string.IsNullOrEmpty(errorcomment))
            {
                AppendError(string.Format("Ошибки:{0}", errorcomment));
                throw new WSCheckException();
            }
        }

        private void CheckSingleQuery(string login)
        {
            QueryItem qItem = inputMessage.QueryList[0];

            CheckData(searchType, qItem);

            // Проверка по номеру сертификата
            if (searchType == WSSearchType.wsCertNumber)
            {
                List<CNEInfo> Result = CheckDataAccessor.CheckByCNENumer(
                    login,
                    qItem.QueryCertificateNumber.Value,
                    qItem.QueryFirstName.Value,
                    qItem.QueryLastName.Value,
                    qItem.QueryPatronymicName.Value,
                    null);
                if (Result != null)
                {
                    foreach (CNEInfo CNE in Result)
                        AppendCertificateInfo(CNE);
                    return;
                }
            }

            // Проверка по типографскому номеру
            if (searchType == WSSearchType.wsTypoNumber)
            {
                List<CNEInfo> Result = CheckDataAccessor.CheckByTyphNumer(
                    login,
                    qItem.QueryTypographicNumber.Value,
                    qItem.QueryFirstName.Value,
                    qItem.QueryLastName.Value,
                    qItem.QueryPatronymicName.Value);
                if (Result != null)
                {
                    foreach (CNEInfo CNE in Result)
                        AppendCertificateInfo(CNE);
                    return;
                }
            }

            // Проверка по паспорту
            if (searchType == WSSearchType.wsPassport)
            {
                List<CNEInfo> Results = CheckDataAccessor.CheckByPassport(
                    login,
                    qItem.QueryPassportSeria.Value,
                    qItem.QueryPassportNumber.Value,
                    qItem.QueryFirstName.Value,
                    qItem.QueryLastName.Value,
                    qItem.QueryPatronymicName.Value);
                if (Results != null)
                {
                    foreach (CNEInfo CNE in Results)
                        AppendCertificateInfo(CNE);
                }
            }
        }

        private void CheckSingleQuery2(string login)
        {
            QueryItem qItem = inputMessage.QueryList[0];

            CheckData(searchType, qItem);

            // Проверка по номеру сертификата
            if (searchType == WSSearchType.wsCertNumber)
            {
                CNEInfo Result = CheckDataAccessor.CheckByCNENumer2(
                    login,
                    qItem.QueryCertificateNumber.Value,
                    qItem.QueryFirstName.Value,
                    qItem.QueryLastName.Value,
                    qItem.QueryPatronymicName.Value,
                    null);
                if (Result != null)
                {
                    AppendCertificateInfo(Result);
                    return;
                }
            }

            // Проверка по типографскому номеру
            if (searchType == WSSearchType.wsTypoNumber)
            {
                List<CNEInfo> Result = CheckDataAccessor.CheckByTyphNumer2(
                    login,
                    qItem.QueryTypographicNumber.Value,
                    qItem.QueryFirstName.Value,
                    qItem.QueryLastName.Value,
                    qItem.QueryPatronymicName.Value);
                if (Result != null)
                {
                    foreach (CNEInfo CNE in Result)
                        AppendCertificateInfo(CNE);
                    return;
                }
            }

            // Проверка по паспорту
            if (searchType == WSSearchType.wsPassport)
            {
                List<CNEInfo> Result = CheckDataAccessor.CheckByPassport2(
                    login,
                    qItem.QueryPassportSeria.Value,
                    qItem.QueryPassportNumber.Value,
                    qItem.QueryFirstName.Value,
                    qItem.QueryLastName.Value,
                    qItem.QueryPatronymicName.Value);
                if (Result != null)
                {
                    foreach (CNEInfo CNE in Result)
                        AppendCertificateInfo(CNE);
                    return;
                }
            }
        }
        #endregion


    }
}
