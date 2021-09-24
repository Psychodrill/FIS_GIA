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
using System.Xml.Linq;

namespace Fbs.Web.Administration.Accounts.Administrators
{
    public partial class AccountKeyList : BasePage
    {
        private const string LoginQueryKey = "login";

        public string Login
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString[LoginQueryKey]))
                    return string.Empty;
                return Request.QueryString[LoginQueryKey];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle = String.Format("Ключи доступа “{0}”", Login); 
        }
    }
}
