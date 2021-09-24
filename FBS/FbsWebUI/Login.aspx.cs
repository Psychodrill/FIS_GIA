using System;
using System.Text;
using System.Web.Security;
using Fbs.Core;
using Fbs.Utility;
using Fbs.Web.CheckAuthService;

namespace Fbs.Web
{
    public partial class Login : BasePage
    {
        private const string FormLoginKey = "login";
        private const string FormPasswordKey = "password";
        private const string FormPersistantKey = "persistant";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[LoginControl.KeySession]!=null)
            {
                lblError.Text = Session[LoginControl.KeySession].ToString();
                Session[LoginControl.KeySession] = null;
            }
        }

    }
}
