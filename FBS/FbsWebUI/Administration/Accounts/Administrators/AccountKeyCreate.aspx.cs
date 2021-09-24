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
using Fbs.Core;

namespace Fbs.Web.Administration.Accounts.Administrators
{
    public partial class AccountKeyCreate : System.Web.UI.Page
    {
        private const string LoginQueryKey = "login";
        private const string SuccessUri = "/Administration/Accounts/Administrators/AccountKeyCreateSuccess.aspx?login={0}&key={1}";

        public string Login
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString[LoginQueryKey]))
                    return string.Empty;
                return Request.QueryString[LoginQueryKey];
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                string key = AccountKey.CreateAccountKey(Login ,ConvertToDate(txtDateFrom.Text),
                    ConvertToDate(txtDateTo.Text), cbIsActive.Checked);

                Response.Redirect(String.Format(SuccessUri, Login, key), true);
            }
        }

        private DateTime? ConvertToDate(string value)
        {
            DateTime date;
            return DateTime.TryParse(value, out date) ? (DateTime?)date : null;
        }
    }
}
