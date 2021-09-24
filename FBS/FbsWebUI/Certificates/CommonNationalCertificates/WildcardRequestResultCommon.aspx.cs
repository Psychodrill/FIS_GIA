using System.Collections.Generic;

namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System.Web.UI.WebControls;

    /// <summary>
    /// Результат поиска по неполным данным
    /// </summary>
    public partial class WildcardRequestResultCommon : BasePage, ICheckResultCommonBase
    {
        /// <summary>
        /// Возвращает источник данных для поиска
        /// </summary>
        /// <returns>Источник данных для поиска по неполным данным</returns>
        public SqlDataSource GetQuerySource()
        {
            return dsSearch_Wildcard;
        }

        public Dictionary<string, string> GetNotFoundPrintData()
        {
            /*LastName={0}&FirstName={1}&PatronymicName={2}&DocSeries={3}&DocNumber={4}&Year={5}&TypographicNumber={6}&Number={7}*/
            return new Dictionary<string, string>
                {
                    {"CertNumber", Request.QueryString["Number"]},
                    {"GivenName", Request.QueryString["PatronymicName"]},
                    {"FirstName", Request.QueryString["FirstName"]},
                    {"LastName", Request.QueryString["LastName"]},
                    {"PassportNumber", Request.QueryString["DocNumber"]},
                    {"Series", Request.QueryString["DocSeries"]},
                    {"TypographicNumber", Request.QueryString["TypographicNumber"]}
                };
        }
    }
}