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

namespace Fbs.Web.Profile
{
    public partial class AccountKeyCreate : BasePage
    {
        private const string SuccessUri = "/Profile/AccountKeyCreateSuccess.aspx?key={0}";

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                string key = AccountKey.CreateAccountKey(ConvertToDate(txtDateFrom.Text),
                    ConvertToDate(txtDateTo.Text), cbIsActive.Checked);

                Response.Redirect(String.Format(SuccessUri, key), true);
            }
        }

        private DateTime? ConvertToDate(string value)
        {
            DateTime date;
            return DateTime.TryParse(value, out date) ? (DateTime?)date : null;
        }
    }
}
