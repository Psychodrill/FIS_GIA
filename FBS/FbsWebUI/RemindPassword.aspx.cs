using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Fbs.Core;
using Fbs.Utility;

namespace Fbs.Web
{
    public partial class RemindPassword : BasePage
    {
        private const string SuccessUri = "/RemindPasswordSuccess.aspx";

        private string mLogin;
        private string Login
        {
            get
            {
                if (mLogin == null)
                    mLogin= IntrantAccount.GetRemindAccountLogin(Email);
                return mLogin;
            }
        }

        private string Email
        {
            get
            {
                return txtEmail.Text.Trim();
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

        protected void btnRemind_Click(object sender, EventArgs e)
        {

            // Проверю валидаторы
            if (!Page.IsValid)
                return;

            // Если пользователь существует
            if (Login != null && (CurrentUserAccount != null || CurrentIntrantAccount != null))
            {
                // Изменю пароль текущему пользователю 
                Account.ChangePassword(Login, CurrentPassword);
                // Отпралю уведомление о изменении пароля
                SendEmailNotification();
            }

            // Выполню действия после попытки изменения пароля
            ProcessSuccess();
        }


        private void SendEmailNotification()
        {
            // Подготовлю email сообщение 
            EmailTemplate template = new EmailTemplate(EmailTemplateTypeEnum.RemindPassword);
            EmailMessage message = template.ToEmailMessage();

            if (IsUser)
            {
                message.To = CurrentUserAccount.Email;
                message.Params = Utility.CollectEmailMetaVariables(CurrentUserAccount, CurrentPassword, Utility.GetSeverPath(Request));
            }
            else
            {
                message.To = CurrentIntrantAccount.Email;
                message.Params = Utility.CollectEmailMetaVariables(CurrentIntrantAccount, CurrentPassword, Utility.GetSeverPath(Request));
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
