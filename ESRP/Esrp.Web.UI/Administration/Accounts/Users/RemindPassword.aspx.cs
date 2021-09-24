using System;
using Esrp.Core;
using Esrp.Utility;
using Esrp.Core.Users;
using System.Security.Cryptography;
using Esrp.Web.Extentions;
using System.Text; 

namespace Esrp.Web.Administration.Accounts.Users
{
    public partial class RemindPassword : System.Web.UI.Page
    {
        #region Constatns & Fields
        private const string SuccessUri = "/Administration/Accounts/Users/RemindPasswordSuccess.aspx?Login={0}&UserKey={1}";

        UserAccount mCurrentUser;
        OrgUser mCurrentOrgUser;
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
                    return string.Empty;

                return Request.QueryString["login"];
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
                    if (Request.UrlReferrer.LocalPath.Contains("List.aspx"))
                    {
                        Session["BackLink.HRef"] = Request.UrlReferrer.ToString();
                    }
                    BackLink.HRef = (string)Session["BackLink.HRef"];
                }
            }
        }

        protected void btnRemindPassword_Click(object sender, EventArgs e)
        {
            // Проверю активацию пользователя и валидность контролов страницы
            if (CurrentUser.Status == UserAccount.UserAccountStatusEnum.Deactivated || !Page.IsValid)
                return;

            // Сгенерирую новый пароль
            //string password = Utility.GeneratePassword();
            // Изменю пароль заданному пользователю 
            //Account.ChangePassword(CurrentUser.Login, password);

            // Подготовлю email сообщение 
            EmailTemplate template = new EmailTemplate(EmailTemplateTypeEnum.AdminChangePassword);
            EmailMessage message = template.ToEmailMessage();
            message.To = CurrentUser.Email;
            //Account.ChangePassword(Login, CurrentPassword);

            string toEncrypt = String.Format("{0};{1};{2}", this.Login, DateTime.Now.ToString(), this.CurrentUser.PasswordHash);
            RSACryptoServiceProvider Rsa = Helper.GetCurrentRsaProvider(true);

            toEncrypt = Uri.EscapeDataString(Convert.ToBase64String(Rsa.Encrypt(Encoding.Default.GetBytes(toEncrypt), false)));

            message.Params = Utility.CollectEmailMetaVariables(CurrentUser, "", Utility.GetSeverPath(Request), String.Format("{0}/WebUserAccount/ResetPassword.aspx?userId={1}", Utility.GetSeverPath(this.Request), toEncrypt));
            
            // Отправлю уведомление
            TaskManager.SendEmail(message);

            // Переход на страницу успеха
            Response.Redirect(String.Format(SuccessUri, Login, GetUserKeyCode()), true);
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
