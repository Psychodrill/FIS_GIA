using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Esrp.Core;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using Esrp.Utility;
using Esrp.Web.Extentions;

namespace Esrp.Web.WebUserAccount
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        private const string SuccessUri = "/RemindPasswordSuccess.aspx";

        private string mLogin;
        private string Login
        {
            get
            {
                if (mLogin == null)
                    mLogin = this.Page.User.Identity.Name;
                return mLogin;
            }
        }

        

        // TODO: Рефакторинг.
        private UserAccount mCurrentUserAccount;
        private UserAccount CurrentUserAccount
        {
            get
            {
                if (!IsUser)
                    return null;
                if (mCurrentUserAccount == null)
                    mCurrentUserAccount = UserAccount.GetUserAccount(Login);
                return mCurrentUserAccount;
            }
        }

        private IntrantAccount mCurrentIntrantAccount;
        private IntrantAccount CurrentIntrantAccount
        {
            get
            {
                if (IsUser)
                    return null;
                if (mCurrentIntrantAccount == null)
                    mCurrentIntrantAccount = IntrantAccount.GetIntrantAccount(Login);
                return mCurrentIntrantAccount;
            }
        }

        private bool? mIsUser = null;
        private bool IsUser
        {
            get
            {
                if (mIsUser == null)
                    mIsUser = Account.GetType(Login) == typeof(UserAccount);
                return (bool)mIsUser;
            }
        }

        private string mCurrentPassword;
        private string CurrentPassword
        {
            get
            {
                if (String.IsNullOrEmpty(mCurrentPassword))
                    mCurrentPassword = Utility.GeneratePassword();
                return mCurrentPassword;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
            
            this.btnChange.Click += new EventHandler(btnChange_Click);
        }

        void btnChange_Click(object sender, EventArgs e)
        {
            if (this.IsValid)
            {
                // Если пользователь существует
                if (Login != null && (CurrentUserAccount != null || CurrentIntrantAccount != null))
                {
                    
                    //Account.ChangePassword(Login, CurrentPassword);
                    string toEncrypt = String.Format("{0};{1};{2};useOld", this.Login, DateTime.Now.ToString(), this.CurrentIntrantAccount == null ? this.CurrentUserAccount.PasswordHash : this.CurrentIntrantAccount.PasswordHash);
                    
                    RSACryptoServiceProvider Rsa = Helper.GetCurrentRsaProvider(true);
                    toEncrypt = Uri.EscapeDataString(Convert.ToBase64String(Rsa.Encrypt(Encoding.Default.GetBytes(toEncrypt), false)));
                    // Отпралю уведомление о изменении пароля
                    SendEmailNotification(String.Format("{0}/WebUserAccount/ResetPassword.aspx?userId={1}", Utility.GetSeverPath(this.Request), toEncrypt));
                }

                // Выполню действия после попытки изменения пароля
                ProcessSuccess();
            }
        }

        private void SendEmailNotification(string resetLink)
        {
            // Подготовлю email сообщение 
            EmailTemplate template = new EmailTemplate(EmailTemplateTypeEnum.ChangePassword);
            EmailMessage message = template.ToEmailMessage();

            if (IsUser)
            {
                message.To = CurrentUserAccount.Email;
                message.Params = Utility.CollectEmailMetaVariables(CurrentUserAccount, CurrentPassword, Utility.GetSeverPath(Request), resetLink);
            }
            else
            {
                message.To = CurrentIntrantAccount.Email;
                message.Params = Utility.CollectEmailMetaVariables(CurrentIntrantAccount, CurrentPassword, Utility.GetSeverPath(Request), resetLink);
            }

            // Отправлю email сообщение
            TaskManager.SendEmail(message);
        }

        private void ProcessSuccess()
        {
            // Перейду на страницу успеха
            Response.Redirect(SuccessUri, true);
        }
      
    }
}