using System;
using System.Threading;
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

namespace Esrp.Web
{
    public partial class Logout : BasePage
    {
        private const string RedirectUri = "/";

        protected void Page_Load(object sender, EventArgs e)
        {
            // делаю логаут
            FormsAuthentication.SignOut();
            // перекидывваю пользователя на заданную страницу
            Response.Redirect(RedirectUri, true);
        }
    }
}
