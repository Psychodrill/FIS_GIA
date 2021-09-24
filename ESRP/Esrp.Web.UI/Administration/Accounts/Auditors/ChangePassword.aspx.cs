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
using Esrp.Utility;
using System.Security.Cryptography;
using Esrp.Web.Extentions;
using System.Text;

namespace Esrp.Web.Administration.Accounts.Auditors
{
    public partial class ChangePassword : BasePage
    {
        private const string SuccessUri = "/Administration/Accounts/Auditors/Edit.aspx?login={0}";
        private const string ErrorUserNotFound = "Пользователь \"{0}\" не найден";
        private const string LoginQueryKey = "login";

        AuditorAccount mCurrentUser;
        public AuditorAccount CurrentUser
        {
            get
            {
                if (mCurrentUser == null)
                    mCurrentUser = AuditorAccount.GetAuditorAccount(Login);

                if (mCurrentUser == null)
                    throw new NullReferenceException(String.Format(ErrorUserNotFound, Login));

                return mCurrentUser;
            }
        }

        public string Login
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString[LoginQueryKey]))
                    throw new NullReferenceException(String.Format(ErrorUserNotFound, ""));

                return Request.QueryString[LoginQueryKey];
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            // Проверю валидность контролов страницы
            if (!Page.IsValid)
                return;

            //string password = txtPassword.Text;
            // Обновлю пароль пользователя
            //AuditorAccount.ChangePassword(CurrentUser.Login, password);

            // Подготовлю email сообщение 
            EmailTemplate template = new EmailTemplate(EmailTemplateTypeEnum.AdminChangePassword);
            EmailMessage message = template.ToEmailMessage();
            message.To = CurrentUser.Email;
            string toEncrypt = String.Format("{0};{1};{2};useOld", this.Login, DateTime.Now.ToString(), this.CurrentUser.PasswordHash);

            RSACryptoServiceProvider Rsa = Helper.GetCurrentRsaProvider(true);
            toEncrypt = Uri.EscapeDataString(Convert.ToBase64String(Rsa.Encrypt(Encoding.Default.GetBytes(toEncrypt), false)));
            message.Params = Utility.CollectEmailMetaVariables(CurrentUser, "", Utility.GetSeverPath(Request), String.Format("{0}/WebUserAccount/ResetPassword.aspx?userId={1}", Utility.GetSeverPath(this.Request), toEncrypt));
            // Отправлю уведомление
             TaskManager.SendEmail(message);

          
            // Выполню действия после успешной регистрации
            ProcessSuccess();
        }

        private void ProcessSuccess()
        {
            this.pwdChangePanel.Visible = false;
            this.pwdChangeSuccessPanel.Visible = true;
        }
    }
}
