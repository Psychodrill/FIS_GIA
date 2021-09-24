namespace Fbs.Web.ViewBase
{
    using System;
    using System.Configuration;
    using System.Data;

    /// <summary>
    /// общая функциональность страниц результатов проверок
    /// </summary>
    public class CertificateCheckResultBase : BasePage
    {
        /// <summary>
        /// получить ссылку на сертификат при проверке
        /// </summary>
        /// <param name="dataItem"> </param>
        /// <param name="certificateItem">
        ///   обьект для биндинга на грид
        /// </param>
        /// <returns>
        /// ссылка на сертификат (с доп инфой)
        /// </returns>
        protected string GetCertificateLink(object certificateItem)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["EnableOpenedFbs"]))
            {
                // Открытая версия приложения
                var itemCasted = certificateItem as DataRowView;
                if (itemCasted == null)
                {
                    return string.Empty;
                }

                var isDenyRaw = itemCasted["IsDeny"];
                var isDeny = !(isDenyRaw is DBNull || Convert.ToBoolean(Convert.ToInt32(isDenyRaw)) == false);

                return Convert.ToBoolean(Convert.ToInt32(itemCasted["IsExist"]))
                           ? string.Format(
                               "<span{2}><nobr>{0}</nobr> {1}</span>",
                               string.Format(
                                   "<a href=\"/Certificates/CommonNationalCertificates/CheckResultByPassportForOpenedFbs.aspx?number={0}&SubjectMarks={1}\">{2}</a>",
                                   itemCasted["CertificateNumber"], 
                                   Request.QueryString["SubjectMarks"],
                                   string.IsNullOrEmpty(itemCasted["CertificateNumber"].ToString()) ? "Нет свидетельства" : itemCasted["CertificateNumber"]),
                               isDeny ? "<span style=\"color:Red\">(аннулировано)</span>" : string.Empty,
                               isDeny ? string.Format(" title='Свидетельство №{0} аннулировано по следующей причине: {1}'", itemCasted["CertificateNumber"], Convert.ToString(itemCasted["DenyComment"])) : string.Empty)
                           : "<span style=\"color:Red\" title='Свидетельство не найдено'>Не&nbsp;найдено</span>";
            }
            else
            {
                var itemCasted = certificateItem as DataRowView;
                if (itemCasted == null)
                {
                    return string.Empty;
                }

                var isDenyRaw = itemCasted["IsDeny"];
                bool isDeny = !(isDenyRaw is DBNull || Convert.ToBoolean(Convert.ToInt32(isDenyRaw)) == false);

                return Convert.ToBoolean(Convert.ToInt32(itemCasted["IsExist"]))
                           ? string.Format(
                               "<span{2}><nobr>{0}</nobr> {1}</span>",
                               string.Format(
                                   "<a href=\"/Certificates/CommonNationalCertificates/CheckResult.aspx?number={0}&LastName={1}&participantid={3}&year={4}\">{2}</a>",
                                   itemCasted["CertificateNumber"].Equals("Нет свидетельства") ? "" : itemCasted["CertificateNumber"],
                                   itemCasted["LastName"],
                                   string.IsNullOrEmpty(itemCasted["CertificateNumber"].ToString()) ? "Нет свидетельства" : itemCasted["CertificateNumber"],
                                   itemCasted["ParticipantID"],
                                   itemCasted["Year"]),
                               isDeny ? "<span style=\"color:Red\">(аннулировано)</span>" : string.Empty,
                               isDeny ? string.Format(
                                         " title='Свидетельство №{0} аннулировано по следующей причине: {1}'",
                                        itemCasted["CertificateNumber"],
                                        Convert.ToString(itemCasted["DenyComment"]))
                                    : string.Empty)
                           : "<span style=\"color:Red\" title='Свидетельство не найдено'>Не&nbsp;найдено</span>";
            }
        }
    }
}