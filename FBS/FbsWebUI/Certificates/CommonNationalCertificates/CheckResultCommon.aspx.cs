using System.Collections.Generic;

namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System.Web.UI.WebControls;

    /// <summary>
    /// Результат запроса по ФИО и регистрационному номеру свидетельства
    /// </summary>
    public partial class CheckResultCommon : BasePage, ICheckResultCommonBase
    {
        /// <summary>
        /// Возвращает источник данных для поиска
        /// </summary>
        /// <returns>Источник данных для поиска по ФИО и регистрационному номеру свидетельства</returns>
        public SqlDataSource GetQuerySource()
        {
            return dsSearch_FIOLicenseNumber;
        }

        public Dictionary<string, string> GetNotFoundPrintData()
        {
            return new Dictionary<string, string>
                {
                    {"CertNumber", Request.QueryString["number"]},
                    {"LastName", Request.QueryString["LastName"]},
                    {"FirstName", Request.QueryString["FirstName"]},
                    {"GivenName", Request.QueryString["PatronymicName"]}
                };
        }
    }
}