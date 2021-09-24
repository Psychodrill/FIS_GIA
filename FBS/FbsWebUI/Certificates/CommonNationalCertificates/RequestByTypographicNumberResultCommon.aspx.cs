using System.Collections.Generic;

namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System.Web.UI.WebControls;

    /// <summary>
    /// Результат поиска по ФИО и типографскому номеру свидетельства
    /// </summary>
    public partial class RequestByTypographicNumberResultCommon : BasePage, ICheckResultCommonBase
    {
        /// <summary>
        /// Возвращает источник данных для поиска
        /// </summary>
        /// <returns>Источник данных для поиска по ФИО и типографскому номеру свидетельства</returns>
        public SqlDataSource GetQuerySource()
        {
            return dsSearch_FIOTypographicNumber;
        }

        public Dictionary<string, string> GetNotFoundPrintData()
        {
           return new Dictionary<string, string>
                {
                    {"TypographicNumber", Request.QueryString["TypographicNumber"]},
                    {"GivenName", Request.QueryString["PatronymicName"]},
                    {"FirstName", Request.QueryString["FirstName"]},
                    {"LastName", Request.QueryString["LastName"]}
                };
        }
    }
}