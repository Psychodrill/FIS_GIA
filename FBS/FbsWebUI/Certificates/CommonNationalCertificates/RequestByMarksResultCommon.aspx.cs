using System.Collections.Generic;

namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System.Web.UI.WebControls;

    /// <summary>
    /// Результат запроса по ФИО и баллам по предметам
    /// </summary>
    public partial class RequestByMarksResultCommon : BasePage, ICheckResultCommonBase
    {
        /// <summary>
        /// Возвращает источник данных для поиска
        /// </summary>
        /// <returns>Источник данных для поиска по ФИО и баллам по предметам</returns>
        public SqlDataSource GetQuerySource()
        {
            return dsSearch_FIOSubjectMarks;
        }

        public Dictionary<string, string> GetNotFoundPrintData()
        {
            return new Dictionary<string, string>
                {
                    {"SubjectMarks", Request.QueryString["SubjectMarks"]},
                    {"GivenName", Request.QueryString["PatronymicName"]},
                    {"FirstName", Request.QueryString["FirstName"]},
                    {"LastName", Request.QueryString["LastName"]}
                };
        }
    }
}