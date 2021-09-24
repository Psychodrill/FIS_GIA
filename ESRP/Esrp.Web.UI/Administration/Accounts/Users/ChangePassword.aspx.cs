using System;
using Esrp.Core;
using Esrp.Utility;
using System.Web;
using Esrp.Core.Users;

namespace Esrp.Web.Administration.Accounts.Users
{
    public partial class ChangePassword : BasePage
    {
        #region Constant & Fields
        private const string SuccessUri = "/Administration/Accounts/Users/ChangePasswordSuccess.aspx?login={0}&UserKey={1}";
        private const string ErrorUserNotFound = "Пользователь \"{0}\" не найден";

        private UserAccount mCurrentUser;
        private OrgUser mCurrentOrgUser;

        #endregion

        #region Properties

        public UserAccount CurrentUser
        {
            get
            {
                if (mCurrentUser == null)
                    mCurrentUser = UserAccount.GetUserAccount(Login);

                if (mCurrentUser == null)
                    throw new NullReferenceException(String.Format("Пользователь \"{0}\" не найден",
                        Login));

                return mCurrentUser;
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

        public string Login
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString["login"]))
                    throw new NullReferenceException(String.Format(ErrorUserNotFound, ""));

                return Request.QueryString["login"];
            }
        }

        #endregion

        #region Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath != null)
                {
                    if (Request.UrlReferrer.LocalPath.Contains("ListIS.aspx"))
                    {
                        Session["BackLink.HRef"] = Request.UrlReferrer.ToString();
                    }
                    BackLink.HRef = (string)Session["BackLink.HRef"];
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            // Проверю валидность контролов страницы
            if (!Page.IsValid)
                return;


            string password = txtPassword.Text;
            // Обновлю пароль пользователя
            UserAccount.ChangePassword(CurrentUser.Login, password);

            
            // Выполню действия после смены пароля
            ProcessSuccess();
        }

        private void ProcessSuccess()
        {
            // Перейти на страницу редактирования профиля
            Response.Redirect(String.Format(SuccessUri, CurrentUser.Login, GetUserKeyCode()), true);
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
