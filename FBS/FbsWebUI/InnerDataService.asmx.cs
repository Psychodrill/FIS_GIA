namespace Fbs.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.IO;
    using System.Security.Authentication;
    using System.Text;
    using System.Web;
    using System.Web.Services;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using Fbs.Core;
    using Fbs.Core.WebServiceCheck;
    using Fbs.Utility;
    using Fbs.Web.Administration.IPCheck;
    using Fbs.Web.CheckAuthService;

    using FbsServices;

    using FbsWebViewModel.CNEC;

    /// <summary>
    /// Summary description for InnerDataService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
        // [System.Web.Script.Services.ScriptService]
    public class InnerDataService : WebService
    {
        #region Fields

        private readonly CNECService cnecService = new CNECService();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The cne last update date.
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="AuthenticationException">
        /// </exception>
        [WebMethod]
        public DateTime CNELastUpdateDate()
        {
            this.CheckOuterAddress();
            var service = new ReportingService();
            return service.CNELastUpdateDate();
        }

        /// <summary>
        /// The check common national exam certificate by number for xml.
        /// </summary>
        /// <param name="number">
        /// The number.
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
        /// <param name="shouldCheckMarks">
        /// The should check marks.
        /// </param>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// The check common national exam certificate by number for xml.
        /// </returns>
        /// <exception cref="AuthenticationException">
        /// </exception>
        [WebMethod]
        public int CheckCommonNationalExamCertificateByNumberForXml(
            string number, string checkSubjectMarks, string participantid, string login, string ip, bool shouldCheckMarks, ref XElement xml)
        {
            this.CheckOuterAddress();

            var updatesData = new AccountDataUpdater(login);
            updatesData.ActualizeRegData();
            if (Account.IsBanned(login))
            {
                // код бана
                xml = null;
                return (int)WebServiceReplyCodes.UserIsBanned;
            }

            xml = (new WSBatchCheck(login)).CheckCommonNationalExamCertificateByNumberForXml(
                number, checkSubjectMarks, participantid, login, ip, shouldCheckMarks);

            return (int)WebServiceReplyCodes.Ok;
        }

        [WebMethod]
        public int SelectCheckHystoryCount(string login, int type)
        {
            this.CheckOuterAddress();
            return this.cnecService.SelectCheckHystoryCount(login, (CheckType)type);
        }

        [WebMethod]
        public string SelectCheckHystory(
            string login, int startRowIndex, int maxRowCount, int type)
        {
            this.CheckOuterAddress();
            List<HistoryCheckCertificateView> result = this.cnecService.SelectCheckHystory(login, startRowIndex, maxRowCount, (CheckType)type);
            var output = new StringWriter(new StringBuilder());
            var serializer = new XmlSerializer(typeof(List<HistoryCheckCertificateView>));
            serializer.Serialize(output, result);
            return output.ToString();
        }

        /// <summary>
        /// The check common national exam certificate by passport for xml.
        /// </summary>
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
        /// The check common national exam certificate by passport for xml.
        /// </returns>
        /// <exception cref="AuthenticationException">
        /// </exception>
        [WebMethod]
        public int CheckCommonNationalExamCertificateByPassportForXml(
            string passportSeria, 
            string passportNumber, 
            string checkSubjectMarks, 
            string login, 
            string ip, 
            bool shouldWriteLog,
            ref XElement xml)
        {
            this.CheckOuterAddress();

            var updatesData = new AccountDataUpdater(login);
            updatesData.ActualizeRegData();
            if (Account.IsBanned(login))
            {
                // код бана
                xml = null;
                return (int)WebServiceReplyCodes.UserIsBanned;
            }

            xml = (new WSBatchCheck(login)).CheckCommonNationalExamCertificateByPassportForXml(
                passportSeria, passportNumber, checkSubjectMarks, login, ip, shouldWriteLog);

            return (int)WebServiceReplyCodes.Ok;
        }

        /// <summary>
        /// Получение списка сертификатот по фамилии и паспортным данным в закрытой фбс
        /// </summary>
        /// <param name="currentCertificateNumber">
        /// Номер сертификата, который нужно исключить из списка других сертификатов
        /// </param>
        /// <param name="passportNumber">
        /// Номер паспорта
        /// </param>
        /// <param name="passportSeria">
        /// TСерия пасспорта
        /// </param>
        /// <param name="lastName">
        /// Фамилия
        /// </param>
        /// <param name="firstName">
        /// Имя
        /// </param>
        /// <param name="patronymicName">
        /// Отчество
        /// </param>
        /// <returns>
        /// Список сертификатов
        /// </returns>
        [WebMethod]
        public string GetCertificateByFioAndPassport(
            string currentCertificateNumber, 
            string passportNumber, 
            string passportSeria, 
            string lastName, 
            string firstName, 
            string patronymicName)
        {
            this.CheckOuterAddress();

            List<HistoryCertificateView> result = this.cnecService.GetCertificateForUser(
                lastName, firstName, patronymicName, passportNumber, passportSeria, currentCertificateNumber);

            var output = new StringWriter(new StringBuilder());
            var serializer = new XmlSerializer(typeof(List<HistoryCertificateView>));
            serializer.Serialize(output, result);
            return output.ToString();
        }

        /// <summary>
        /// получить данные отчета по загруженным сертификатам
        /// </summary>
        /// <returns>
        /// данные в виде xml
        /// </returns>
        [WebMethod]
        public string ReportCertificateLoadShortTVF()
        {
            this.CheckOuterAddress();

            var service = new ReportingService();
            DataSet dataSet = service.ReportCertificateLoadShortTVF();
            var ser = new XmlSerializer(typeof(DataSet));
            var sb = new StringBuilder();
            using (TextWriter tw = new StringWriter(sb))
            {
                ser.Serialize(tw, dataSet);
                tw.Close();
            }

            return sb.ToString();
        }

        /// <summary>
        /// The search common national exam certificate check by outer id.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="batchId">
        /// The batch id.
        /// </param>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// The search common national exam certificate check by outer id.
        /// </returns>
        /// <exception cref="AuthenticationException">
        /// </exception>
        [WebMethod]
        public int SearchCommonNationalExamCertificateCheckByOuterId(string login, long batchId, ref XElement xml)
        {
            this.CheckOuterAddress();
            var updatesData = new AccountDataUpdater(login);
            updatesData.ActualizeRegData();
            if (Account.IsBanned(login))
            {
                return (int)WebServiceReplyCodes.UserIsBanned;
            }

            xml = (new WSBatchCheck(login)).SearchCommonNationalExamCertificateCheckByOuterId(batchId);

            return (int)WebServiceReplyCodes.Ok;
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
        /// сериализованные проверки свидетельств организацией
        /// </returns>
        [WebMethod]
        public string SelectCNECCheckHystoryByOrgId(
            long orgId, 
            int startRow, 
            int maxRow, 
            string sortBy, 
            bool isUniqueCheck, 
            int sortorder, 
            string typeCheck, 
            string family,
            string dats = null,
            string datf = null)
        {
            this.CheckOuterAddress();
            try
            {
                List<HistoryCheckCertificateForOrganizationView> result =
                    this.cnecService.SelectCNECCheckHystoryByOrgId(
                        orgId, startRow, maxRow, sortBy, isUniqueCheck, sortorder, typeCheck, family, dats, datf);

                return this.ServializeList(result);
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                throw;
            }
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
        [WebMethod]
        public int CountCNECCheckHystoryByOrgId(long orgId, bool isUniqueCheck, string typeCheck, string family, string dats = null, string datf = null)
        {
            this.CheckOuterAddress();
            try
            {
                return this.cnecService.CountCNECCheckHystoryByOrgId(orgId, isUniqueCheck, typeCheck, family, dats, datf);
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// The batch check.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="batchId">
        /// The batch Id.
        /// </param>
        /// <param name="year">
        /// The year.
        /// </param>
        /// <returns>
        /// The batch check.
        /// </returns>
        [WebMethod]
        public Guid? StartBatchCheck(string login, string data, int type, long batchId, int? year)
        {
            this.CheckOuterAddress();
            var updatesData = new AccountDataUpdater(login);
            updatesData.ActualizeRegData();
            if (Account.IsBanned(login))
            {
                // флаг бана
                return Guid.Empty;
            }

           return (new WSBatchCheck(login)).StartCheck(data, type, batchId, year);
        }

        #endregion

        #region Methods

        private void CheckOuterAddress()
        {
            string address = HttpContext.Current.Request.UserHostAddress;
            if (!IPChecker.CheckOuterSite(address))
            {
                LogManager.Warning(string.Format("Отсутствуют права доступа.{0}", address));
                throw new AuthenticationException(string.Format("Отсутствуют права доступа.{0}", address));
            }
        }

        private string ServializeList<T>(List<T> result) where T : class
        {
            var output = new StringWriter(new StringBuilder());
            var serializer = new XmlSerializer(typeof(List<T>));
            serializer.Serialize(output, result);
            return output.ToString();
        }

        #endregion
    }
}