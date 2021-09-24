using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Fbs.Core;

namespace Fbs.Web.Administration.Certificates
{
    public partial class CommonNationalCertificateLoad : BasePage
    {
        private const string QueryKey = "id";
        private const string ErrorMessage = "Свидетельство не найдено";
        private const string SuccessUri = "/Administration/Certificates/CommonNationalCertificateLoads.aspx";

        private long CertificateId
        {
            get
            {
                long result = 0;
                if (Int64.TryParse(Request.QueryString[QueryKey], out result))
                    return result;
                throw new NullReferenceException(ErrorMessage);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CommonNationalCertificateContext.ExecuteLoadingTask(CertificateId, CurrentUser.ClietnLogin);
            Response.Redirect(SuccessUri, true);
        }
    }
}
