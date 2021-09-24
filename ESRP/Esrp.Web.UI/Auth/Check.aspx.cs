using System;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Security;

using Esrp.Core;
using Esrp.Core.Systems;
using Esrp.Utility;

namespace Esrp.Web.Auth
{
	public partial class Check : BasePage
	{
		private class AuthData
		{
			public string BackUrl { get; set; }
			public int AuthRequest { get; set; }
			public int SystemID { get; set; }

			public AuthData(HttpRequest request)
			{
				string retPath = request.Headers["Referrer"];
				if (!String.IsNullOrEmpty(request["rp"]))
					retPath = HttpUtility.HtmlDecode(request["rp"]);
				BackUrl = retPath;
				int ra = 1;
				if(!String.IsNullOrEmpty(request["ra"]))
				{
					int.TryParse(request["ra"], out ra);
					if (ra < 0 || ra > 3) ra = 1;
				}
				if (!String.IsNullOrEmpty(request["sid"]))
				{
					int sid;
					int.TryParse(request["sid"], out sid);
					SystemID = sid;
				}
				else
					SystemID = -1;
				if (BackUrl == null) BackUrl = "";
				AuthRequest = ra;
			}

			public string GetBackUrl(string userLogin, int systemID, int status)
			{
				string sep = BackUrl.Contains("?") ? "&" : "?";

				var userGuid = Guid.Empty;
				if (!String.IsNullOrEmpty(userLogin))
					userGuid = UserAuthRequestCache.Cache.AddUser(userLogin, systemID);
				string redirectUrl = String.Format("{0}{1}esrpres={2}&l={3}&st={4}", BackUrl, sep, userGuid, userLogin, status);
				return redirectUrl;
			}

			public static AuthData CreateAuthData(HttpRequest request)
			{
				return new AuthData(request);
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			AuthData data = AuthData.CreateAuthData(Request);
			string userLogin = User.Identity.IsAuthenticated ? User.Identity.Name : "";
			if (data.AuthRequest == 2 || data.AuthRequest == 3) //нужен релогин
			{
				HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(""), new string[0]);
				FormsAuthentication.SignOut();
				if(data.AuthRequest == 3) //бежим назад
				{
					Response.Redirect(data.GetBackUrl("", 0, 0));
				}

			}
			else
			{
				if (!String.IsNullOrEmpty(userLogin))
				{
					Response.Redirect(data.GetBackUrl(HttpContext.Current.User.Identity.Name, data.SystemID, GeneralSystemManager.HasUserAccess(userLogin, (SystemKind)data.SystemID) ? 1 : 2));
					return;
				}
				else
				{
					if (data.AuthRequest == 0) //не требуется обязательная аутентификация
						Response.Redirect(data.GetBackUrl("", 0, 0));
				}				
			}
			base.OnLoad(e);
			DoLoad();
		}

		private const string FormLoginKey = "login";
		private const string FormPasswordKey = "password";
		private const string FormPersistantKey = "persistant";

		protected void DoLoad()
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
				case Account.VerifyStateEnum.OnRegistration:
					FormsAuthentication.SetAuthCookie(userName, isPersistant);
					Response.Redirect(RedirectManager.DocumentUploadForRegistrationUrl);
					break;

				case Account.VerifyStateEnum.Valid:
					// Пользователь существует и ip находится в списке разрешенных
					// Авторизую пользователя
					FormsAuthentication.SetAuthCookie(userName, isPersistant);

						if (userStatus == UserAccount.UserAccountStatusEnum.Registration)
							Response.Redirect(RedirectManager.DocumentUploadForRegistrationUrl);
						else if (userStatus == UserAccount.UserAccountStatusEnum.Consideration ||
								userStatus == UserAccount.UserAccountStatusEnum.Revision)
								Response.Redirect(RedirectManager.EditUserAccountRedirectUrl);

                    // Копылов Андрей - надо получить браузер пользователя (27.11.2017)
                    Account.SaveUserAgent(userName, Request.UserAgent);

                    // Установлю флаг успешной проверки
                    //AccountIpChecker.SetCheckPassed();
                    // Определю страницу перехода 
                    AuthData data = AuthData.CreateAuthData(Request);
					bool hasAccess = GeneralSystemManager.HasUserAccess(userName, (SystemKind) data.SystemID);
					Response.Redirect(data.GetBackUrl(userName, data.SystemID, hasAccess ? 1 : 2));
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
				Response.Redirect("~/LoginBlocked.aspx?ReturnUrl=" + "");
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

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			AuthData data = AuthData.CreateAuthData(Request);
			Response.Redirect(data.GetBackUrl("", 0, 0));
		}
	}
}