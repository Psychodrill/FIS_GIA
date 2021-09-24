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
using Esrp.Core;

namespace Esrp.Web.Administration.Accounts.Users
{
    public partial class AccountKeyCreate : System.Web.UI.Page
    {
        #region Constants & Fields

        private const string LoginQueryKey = "login";
        private const string SuccessUri = "/Administration/Accounts/Users/AccountKeyCreateSuccess.aspx?login={0}&key={1}&UserKey={2}";
        private const string ErrorUserNotFound = "Пользователь \"{0}\" не найден";
        private string mCurrentUserLogin;

        #endregion

        #region Properties

        public string Login
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString[LoginQueryKey]))
                    return string.Empty;
                return Request.QueryString[LoginQueryKey];
            }
        }

        /// <summary>
        /// Gets CurrentUser.
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// </exception>
        public String CurrentUserLogin
        {
            get
            {
                if (this.mCurrentUserLogin == null)
                {
                    this.mCurrentUserLogin = AdministratorAccount.GetAdministratorAccountForce(this.Login).Login;
                }

                if (this.mCurrentUserLogin == null)
                {
                    this.mCurrentUserLogin = UserAccount.GetUserAccount(this.Login).Login;
                }

                if (this.mCurrentUserLogin == null)
                {
                    throw new NullReferenceException(string.Format(ErrorUserNotFound, this.Login));
                }

                return this.mCurrentUserLogin;
            }
        }

        #endregion

        #region Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath!=null)
                {
                    if (Request.UrlReferrer.LocalPath.Contains("ListIS.aspx"))
                    {
                        Session["BackLink.HRef"] = Request.UrlReferrer.ToString();
                    }
                    BackLink.HRef = (string)Session["BackLink.HRef"];
                }
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                string key = AccountKey.CreateAccountKey(Login, txtDateFrom.Value,
                    txtDateTo.Value, cbIsActive.Checked);

                Response.Redirect(String.Format(SuccessUri, Login, key, GetUserKeyCode()), true);
            }
        }

        private DateTime? ConvertToDate(string value)
        {
            DateTime date;
            return DateTime.TryParse(value, out date) ? (DateTime?)date : null;
        }

        protected String GetUserKeyCode()
        {
            if (String.IsNullOrEmpty(Request["UserKey"]))
                return "";
            return Request["UserKey"];
        }

        #endregion
    }
}
