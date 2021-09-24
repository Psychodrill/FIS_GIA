using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Threading;
using Esrp.Core;
using Esrp.Utility;

namespace Esrp.Web.Personal.Profile
{
    public partial class ChangePassword : BasePage
    {

        private const string SuccessUri = "/Profile/ChangePasswordSuccess.aspx";
        private const string InvalidLoginErrorFormat = "Пользователь \"{0}\" не найден";

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Проверю валидаторы
            if (!Page.IsValid)
                return;

            string newPassword = txtNewPassword.Text.Trim();

            // Старый пароль правильный. изменю его на новый
            ChangeUserPassword(newPassword);

            // Выполню действия после успешного измения пароля
            ProcessSuccess(newPassword);
        }

        protected void cvWrongOldPassword_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // Проверю правильность старого пароля
            args.IsValid = IsCredentialsValid(args.Value.Trim());
        }

        // Проверка правильности логина и пароля пользователя
        private bool IsCredentialsValid(string password)
        {
						Account.VerifyStateEnum verifyState;        		
            verifyState = Account.Verify(CurrentUserName, password);

            return verifyState == Account.VerifyStateEnum.Valid;
        }

        private void ChangeUserPassword(string password)
        {
            Account.ChangePassword(CurrentUserName, password);
        }

        private void ProcessSuccess(string password)
        {
            // Подготовить email сообщение 
            EmailTemplate template = new EmailTemplate(EmailTemplateTypeEnum.ChangePassword);
            EmailMessage message = template.ToEmailMessage();

            // TODO: рефакторинг. 
            // Нужно общие поля вынести в класс Account и работать здесь с общей логикой.
            if (Account.GetType(CurrentUserName) == typeof(UserAccount))
            {
                // Получить текущего пользователя
                UserAccount user = UserAccount.GetUserAccount(CurrentUserName);
                if (user == null)
                    throw new NullReferenceException(
                        String.Format(InvalidLoginErrorFormat, CurrentUserName));

                message.To = user.Email;
                message.Params = Utility.CollectEmailMetaVariables(user, password, Utility.GetSeverPath(Request));
            }
            else
            {
                // Получить текущего пользователя
                IntrantAccount user = IntrantAccount.GetIntrantAccount(CurrentUserName);
                if (user == null)
                    throw new NullReferenceException(
                        String.Format(InvalidLoginErrorFormat, CurrentUserName));

                message.To = user.Email;
                message.Params = Utility.CollectEmailMetaVariables(user, password, Utility.GetSeverPath(Request));
            }

            // Отправлю email сообщение
            TaskManager.SendEmail(message);

            // Зашифрую и сохранию пароль в сессию
            Utility.SavePasswordToSession(password);

            // Перейду на страницу успеха
            Response.Redirect(SuccessUri, true);
        }
    }
}
