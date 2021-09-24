using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Esrp.Core;
using Esrp.Core.Users;

namespace Esrp.Web.Administration.Accounts.Users
{
    public partial class AuthenticationHistory : BasePage
    {
        #region Constants & Fields

        private const string LoginQueryKey = "login";
        private const string ErrorUserNotFound = "Пользователь \"{0}\" не найден";
        private const string TitleFormat = "История аутентификаций “{0}”";
        private string mCurrentUserLogin;
        private OrgUser mCurrentOrgUser;

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

        public OrgUser CurrentOrgUser
        {
            get
            {
                if (mCurrentOrgUser == null)
                    mCurrentOrgUser = OrgUserDataAccessor.Get(this.Login);

                if (mCurrentOrgUser == null)
                    throw new NullReferenceException(String.Format("Пользователь \"{0}\" не найден",
                        Login));

                return mCurrentOrgUser;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
			// Теперь и для других можно смотреть
            // Проверю соответствие типа запрашиваемого аккаунта
            //if (Account.GetType(Login) != typeof(UserAccount))
            //    throw new NullReferenceException(String.Format(ErrorUserNotFound, Login));
               
            // Установлю заголовок страницы
            this.PageTitle = string.Format(TitleFormat, Login);

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


    }
}
