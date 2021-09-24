using System.Collections.Generic;

namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System.Web.UI.WebControls;

    /// <summary>
    /// Результат поиска по ФИО и номеру документа
    /// </summary>
    public partial class RequestByPassportResultCommon : BasePage, ICheckResultCommonBase
    {
        /// <summary>
        /// Возвращает источник данных для поиска
        /// </summary>
        /// <returns>Источник данных для поиска по ФИО и номеру документа</returns>
        public SqlDataSource GetQuerySource()
        {
            return dsSearch_FIOPassport;
        }

        public Dictionary<string, string> GetNotFoundPrintData()
        {
            return new Dictionary<string, string>
                {
                    {"GivenName", this.Request.QueryString["secondName"]},
                    {"FirstName", Request.QueryString["name"]},
                    {"LastName", Request.QueryString["surname"]},
                    {"PassportNumber", Request.QueryString["documentNumber"]},
                    {"Series", Request.QueryString["documentSeries"]}
                };
        }
    }
}