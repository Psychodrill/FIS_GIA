using System;
using System.Web.Security;
using Esrp;

namespace Fbs.Web
{
    public partial class Logout : BasePage
    {
        private const string RedirectUri = "/";

        protected void Page_Load(object sender, EventArgs e)
        {
            // делаю логаут
            FormsAuthentication.SignOut();

            string redirectUri = string.Format("{0}://{1}{2}", Request.Url.Scheme,
                Request.Url.Authority, RedirectUri);
            EsrpClient esrpClient = new EsrpClient()
            {
                SystemId = SystemId.Fbs,
                EsrpUrl = Config.UrlEsrp
            };
            esrpClient.Logout(redirectUri, Response);
        }
    }
}
