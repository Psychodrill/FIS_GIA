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
    public partial class AccountKeyCreateSuccess : System.Web.UI.Page
    {
        #region Constants & Fields

        private const string AccountKeyCodeQueryKey = "key";
        private const string LoginQueryKey = "login";
        private const string ErrorUserNotFound = "Пользователь \"{0}\" не найден";
        private string mCurrentUserLogin;

        #endregion

        #region Properties

        public string AccountKeyCode
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString[AccountKeyCodeQueryKey]))
                    return string.Empty;
                return Request.QueryString[AccountKeyCodeQueryKey];
            }
        }

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
        public string CurrentUserLogin
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

        protected String GetUserKeyCode()
        {
            if (String.IsNullOrEmpty(Request["UserKey"]))
                return "";
            return Request["UserKey"];
        }

        #endregion

    }
}
