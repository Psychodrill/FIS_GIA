namespace FbsChecksClient
{
    using System;
    using System.ServiceModel;

    using Fbs.Utility;

    using FbsChecksClient.InnerDataReference;
    using FbsChecksClient.WSChecksReference;

    using UserCredentials = FbsChecksClient.WSChecksReference.UserCredentials;

    /// <summary>
    /// Класс обертка для веб референса 
    /// </summary>
    public class WSCheckClient
    {
        protected  WSChecksSoapClient CheckClient;

        protected InnerDataServiceSoapClient InnerDataClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="WSCheckClient"/> class.
        /// </summary>
        public WSCheckClient()
        {
            this.CheckClient = new WSChecksSoapClient("WSChecksSoap");
            this.InnerDataClient = new InnerDataServiceSoapClient("InnerDataServiceSoap");
        }

        /// <summary>
        /// Получение списка сертификатот по фамилии и паспортным данным в закрытой фбс
        /// </summary>
        /// <param name="currentCertificateNumber"> Номер сертификата, который нужно исключить из списка других сертификатов
        /// </param>
        /// <param name="passportNumber"> Номер паспорта </param>
        /// <param name="passportSeria"> TСерия пасспорта </param>
        /// <param name="lastName"> Фамилия </param>
        /// <param name="firstName"> Имя </param>
        /// <param name="patronymicName"> Отчество </param>
        /// <returns>
        /// Список сертификатов
        /// </returns>
        public string GetCertificateByFioAndPassport(string currentCertificateNumber, string passportNumber, string passportSeria, string lastName, string firstName, string patronymicName)
        {
            using (this.InnerDataClient.ThroughProxy())
            {
                return this.InnerDataClient.GetCertificateByFioAndPassport(
                    currentCertificateNumber, passportNumber, passportSeria, lastName, firstName, patronymicName);
            }
        }

        /// <summary>
        /// Searches the common national exam certificate check by outer id.
        /// </summary>
        /// <param name="userCredentials">
        /// The user Credentials.
        /// </param>
        /// <param name="batchId">
        /// The batch id.
        /// </param>
        /// <param name="xml">
        /// The XML.
        /// </param>
        /// <returns>
        /// статус ответа
        /// </returns>
        public int SearchCommonNationalExamCertificateCheckByOuterId(string login, long batchId, ref System.Xml.XmlElement xml)
        {
            using (this.InnerDataClient.ThroughProxy())
            {
                return this.InnerDataClient.SearchCommonNationalExamCertificateCheckByOuterId(login, batchId, ref xml);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="userCredentials"> The user credentials.
        /// </param>
        /// <param name="data"> The data. </param>
        /// <param name="type"> The type. </param>
        /// <param name="batchId"> The batch id. </param>
        /// <param name="year"> The year. </param>
        public Guid? StartBatchCheck(string login, string data, int type, long batchId, int? year)
        {
            using (this.InnerDataClient.ThroughProxy())
            {
                return this.InnerDataClient.StartBatchCheck(login, data, type, batchId, year);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="number">
        /// The number. 
        /// </param>
        /// <param name="checkSubjectMarks">
        /// The check subject marks. 
        /// </param>
        /// <param name="participantid">
        /// The participant id
        /// </param>
        /// <param name="login">
        /// The login. 
        /// </param>
        /// <param name="ip">
        /// The ip. 
        /// </param>
        /// <param name="shouldCheckMarks">
        /// The should check marks. 
        /// </param>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// статус операции
        /// </returns>
        public int CheckCommonNationalExamCertificateByNumberForXml(string number, string checkSubjectMarks, string participantid, string login, string ip, bool shouldCheckMarks, ref System.Xml.XmlElement xml)
        {
            using (this.InnerDataClient.ThroughProxy())
            {
                return this.InnerDataClient.CheckCommonNationalExamCertificateByNumberForXml(number, checkSubjectMarks, participantid, login, ip, shouldCheckMarks, ref xml);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="userCredentials">
        /// The user credentials. 
        /// </param>
        /// <param name="passportSeria">
        /// The passport seria. 
        /// </param>
        /// <param name="passportNumber">
        /// The passport number. 
        /// </param>
        /// <param name="checkSubjectMarks">
        /// The check subject marks. 
        /// </param>
        /// <param name="login">
        /// The login. 
        /// </param>
        /// <param name="ip">
        /// The ip. 
        /// </param>
        /// <param name="xml">
        /// The xml. 
        /// </param>
        /// <returns>
        /// статус ответа
        /// </returns>
        public int CheckCommonNationalExamCertificateByPassportForXml(string passportSeria, string passportNumber, string checkSubjectMarks, string login, string ip, bool shouldWriteLog, ref System.Xml.XmlElement xml)
        {
            using (this.InnerDataClient.ThroughProxy())
            {
                return this.InnerDataClient.CheckCommonNationalExamCertificateByPassportForXml(passportSeria, passportNumber, checkSubjectMarks, login, ip, shouldWriteLog, ref xml);
            }
        }

        public string GetBatchCheckResult(UserCredentials userCredentials, string xml)
        {
            using (this.CheckClient.ThroughProxy())
            {
               return this.CheckClient.GetBatchCheckResult(userCredentials, xml);
            }
        }

        public string ReportCertificateLoadShortTVF()
        {
            using (this.InnerDataClient.ThroughProxy())
            {
                return this.InnerDataClient.ReportCertificateLoadShortTVF();
            }
        }

        public DateTime CNELastUpdateDate()
        {
            using (this.InnerDataClient.ThroughProxy())
            {
                return this.InnerDataClient.CNELastUpdateDate();
            }
        }

        public string SelectCNECCheckHystoryByOrgId(long orgId, int startRow, int maxRow, string sortBy, bool isUniqueCheck, int sortorder, string typeCheck, string family, string dats, string datf)
        {
            using (this.InnerDataClient.ThroughProxy())
            {
                return this.InnerDataClient.SelectCNECCheckHystoryByOrgId(orgId, startRow, maxRow, sortBy, isUniqueCheck, sortorder, typeCheck, family, dats, datf);
            }
        }

        public int CountCNECCheckHystoryByOrgId(long orgId, bool isUniqueCheck, string typeCheck, string family, string dats, string datf)
        {
            using (this.InnerDataClient.ThroughProxy())
            {
                return this.InnerDataClient.CountCNECCheckHystoryByOrgId(orgId,isUniqueCheck, typeCheck, family, dats, datf);
            }
        }

        public int SelectCheckHystoryCount(string login, int type)
        {
            using (this.InnerDataClient.ThroughProxy())
            {
                return this.InnerDataClient.SelectCheckHystoryCount(login, type);
            }
        }

        public string SelectCheckHystory(string login, int startRowIndex, int maxRowCount, int type)
        {
            using (this.InnerDataClient.ThroughProxy())
            {
                return this.InnerDataClient.SelectCheckHystory(login,startRowIndex, maxRowCount, type);
            }
        }
    }
}
