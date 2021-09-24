using System;
using System.Text;
using System.Web.Security;
using Esrp.Core;
using Esrp.Utility;

namespace Esrp.Web
{
    public partial class Login : BasePage
    {
        private const string FormLoginKey = "login";
        private const string FormPasswordKey = "password";
        private const string FormPersistantKey = "persistant";

        private string ReturnUrl
        {
            get
            {
                if (Request.QueryString["ReturnUrl"] == null)
                    return string.Empty;

                return Request.QueryString["ReturnUrl"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckLoginAttemptsInfo();

            // Спрячу чек если параметр DisableRememberMe установлен в True
            trRememberMe.Visible = !Config.DisableRememberMe;

            // Выйду если постбэк
            if (Page.IsPostBack)
                return;

            // Если логин или пароль не заданы в посте, то выйду из метода
            if (Request.Form[FormLoginKey] == null || Request.Form[FormPasswordKey] == null)
                return;

            // Означу контролы на случай, если возникнет ошибка
            txtLogin.Text = Request.Form[FormLoginKey];
            txtPassword.Text = Request.Form[FormPasswordKey];
            // Еще раз проверю параметр DisableRememberMe конфига
            cbPersistant.Checked = (!Config.DisableRememberMe && Request.Form[FormPersistantKey] == "on");

            // Проверю валидность контролов, т.к. значения были установлены
            Page.Validate();
            if (!Page.IsValid)
                return;

            Authenticate(txtLogin.Text.Trim(), txtPassword.Text.Trim(), cbPersistant.Checked);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            Authenticate(txtLogin.Text.Trim(), txtPassword.Text.Trim(), cbPersistant.Checked);
        }

        private void Authenticate(string userName, string password, bool isPersistant)
        {
            // Получу сотояние аккаунта с заданными параметрами
        		UserAccount.UserAccountStatusEnum userStatus;
            Account.VerifyStateEnum verifyState = Account.Verify(userName, password, out userStatus);

            switch (verifyState)
            {
                case Account.VerifyStateEnum.InvalidIp:
                    // IP запрещен
                    // Запишу варнинг в лог
                    PublishForbiddenIpWarning(userName);
                    // Выполню редирект на страницу с описанием
                    Response.Redirect(Config.ForbiddenIpPageUrl, true);
                    break;
                case Account.VerifyStateEnum.InvalidLogin:
                    // Пользователь не найден
                    cvInvalidCredentials.IsValid = false;
                    break;
                case Account.VerifyStateEnum.Valid:
                    // Пользователь существует и ip находится в списке разрешенных
                    // Авторизую пользователя
                    FormsAuthentication.SetAuthCookie(userName, isPersistant);
                    // Установлю флаг успешной проверки
                    //AccountIpChecker.SetCheckPassed();
                    // Определю страницу перехода 
                    if (string.IsNullOrEmpty(ReturnUrl))
                    {
						if(userStatus == UserAccount.UserAccountStatusEnum.Registration)
								Response.Redirect(RedirectManager.DocumentUploadForRegistrationUrl);
						else if (userStatus == UserAccount.UserAccountStatusEnum.Consideration ||
									userStatus == UserAccount.UserAccountStatusEnum.Revision)
								Response.Redirect(RedirectManager.EditUserAccountRedirectUrl);
						else
						// Обычный вход в систему. определю страницу перехода в зависимости от 
                        // типа пользователя.
							Response.Redirect(RedirectManager.GetCurrentAuthUrl(userName), true);
                    }
                    else
                    {
                        // Пользователь запрашивает конкретную защищенную страницу. перекину его туда.
                        Response.Redirect(ReturnUrl, true);
                    }
                    break;
								case Account.VerifyStateEnum.OnRegistration:
										Response.Redirect(RedirectManager.DocumentUploadForRegistrationUrl);
            				break;
                default:
                    break;
            }

            if (verifyState != Account.VerifyStateEnum.Valid)
            {
                CheckLoginAttemptsInfo();
            }
        }

        /// <summary>
        ///  проверка количества логинов
        /// </summary>
        private void CheckLoginAttemptsInfo()
        {
            if (LoginAttemptsInfo.GetDelay(Request.UserHostAddress) > 0)
            {
                Response.Redirect("~/LoginBlocked.aspx?ReturnUrl=" + ReturnUrl);
            }
        }

        private void PublishForbiddenIpWarning(string userName)
        {
            // Сформирую информацию
            StringBuilder message = new StringBuilder("Попытка входа с запрещенного IP-адреса.");
            message.Append("\n");
            message.AppendFormat("Login: {0}", userName).Append("\n");
            message.AppendFormat("Password: {0}", "**********").Append("\n");
            message.AppendFormat("User ip: {0}", Request.UserHostAddress).Append("\n");

            // Запишу варнинг в лог
            LogManager.Warning(new ApplicationException(message.ToString()));
        }
    }
}
