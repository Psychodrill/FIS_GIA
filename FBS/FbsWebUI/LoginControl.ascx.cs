namespace Fbs.Web
{
    using System;
    using System.Text;
    using System.Web.Security;
    using System.Web.UI;

    using Esrp;

    using Fbs.Core;
    using Fbs.Utility;
    using Fbs.Web.CheckAuthService;
    using Fbs.Web.Helpers;

    /// <summary>
    /// The login control.
    /// </summary>
    public partial class LoginControl : UserControl
    {
        #region Constants

        /// <summary>
        /// The key session.
        /// </summary>
        public const string KeySession = "SessionAuth";

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets SessionAuthentication.
        /// </summary>
        public string SessionAuthentication
        {
            get
            {
                if (this.Session[KeySession] == null)
                {
                    return string.Empty;
                }

                return this.Session[KeySession].ToString();
            }

            set
            {
                this.Session[KeySession] = value;
            }
        }

        #endregion

        #region Properties

        private string ReturnUrl
        {
            get
            {
                if (this.Request.QueryString["ReturnUrl"] == null)
                {
                    return string.Empty;
                }

                return this.Request.QueryString["ReturnUrl"];
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The page_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var logger = new MemoryLogger();
            try
            {
                var esrp = new EsrpClient { SystemId = SystemId.Fbs, EsrpUrl = Config.UrlEsrp };
                this.hlRegistration.NavigateUrl = string.Format("{0}/Registration.aspx", esrp.EsrpUrl);
                this.hlRemindPassword.NavigateUrl = string.Format("{0}/RemindPassword.aspx", esrp.EsrpUrl);
                esrp.OnAuthorized += this.esrp_OnAuthorized;
                esrp.OnAuthenticated += this.esrp_OnAuthenticated;
                esrp.OnNotAuthenticated += this.esrp_OnNotAuthenticated;
                esrp.Parse(this.Request);
            }
            catch (Exception ex)
            {
                logger.WriteError(ex);
            }

            string log = logger.GetLog();
            if (!string.IsNullOrEmpty(log))
            {
                this.SessionAuthentication = log;
                this.Response.Redirect("/Login.aspx");
            }
        }

        /// <summary>
        /// The btn login_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
            {
                return;
            }

            string redirectUri = string.Format(
                "{0}://{1}{2}", this.Request.Url.Scheme, this.Request.Url.Authority, this.Request.Url.AbsolutePath);
            var esrp = new EsrpClient { SystemId = SystemId.Fbs, EsrpUrl = Config.UrlEsrp };
            esrp.Login(redirectUri, this.Response);
        }

        private void Authenticate(string userName, bool isPersistant)
        {
            // Получу сотояние аккаунта с заданными параметрами
            Account.VerifyStateEnum verifyState = Account.Verify(userName);

            switch (verifyState)
            {
                case Account.VerifyStateEnum.InvalidIp:

                    // IP запрещен
                    // Запишу варнинг в лог
                    this.PublishForbiddenIpWarning(userName);

                    // Выполню редирект на страницу с описанием
                    this.Response.Redirect(Config.ForbiddenIpPageUrl, true);
                    break;
                case Account.VerifyStateEnum.InvalidLogin:

                    // Пользователь не найден
                    // cvInvalidCredentials.IsValid = false;
                    break;
                case Account.VerifyStateEnum.Valid:

                    // Пользователь существует и ip находится в списке разрешенных
                    // Авторизую пользователя
                    FormsAuthentication.SetAuthCookie(userName, isPersistant);

                    // Установлю флаг успешной проверки
                    // AccountIpChecker.SetCheckPassed();
                    // Определю страницу перехода 
                    if (string.IsNullOrEmpty(this.ReturnUrl))
                    {
                        // Обычный вход в систему. определю страницу перехода в зависимости от 
                        // типа пользователя.
                        this.Response.Redirect(RedirectManager.GetCurrentAuthUrl(userName), true);
                    }
                    else
                    {
                        // Пользователь запрашивает конкретную защищенную страницу. перекину его туда.
                        this.Response.Redirect(this.ReturnUrl, true);
                    }

                    break;
                default:
                    break;
            }

            if (verifyState != Account.VerifyStateEnum.Valid)
            {
                this.CheckLoginAttemptsInfo();
            }
        }

        /// <summary>
        /// проверка количества логинов
        /// </summary>
        private void CheckLoginAttemptsInfo()
        {
            if (LoginAttemptsInfo.GetDelay(this.Request.UserHostAddress) > 0)
            {
                this.Response.Redirect("~/LoginBlocked.aspx?ReturnUrl=" + this.ReturnUrl);
            }
        }

        private void PublishForbiddenIpWarning(string userName)
        {
            // Сформирую информацию
            var message = new StringBuilder("Попытка входа с запрещенного IP-адреса.");
            message.Append("\n");
            message.AppendFormat("Login: {0}", userName).Append("\n");
            message.AppendFormat("Password: {0}", "**********").Append("\n");
            message.AppendFormat("User ip: {0}", this.Request.UserHostAddress).Append("\n");

            // Запишу варнинг в лог
            LogManager.Warning(new ApplicationException(message.ToString()));
        }

        private void esrp_OnAuthenticated(string login)
        {
            this.SessionAuthentication = "Вы не имеете доступа к системе.";
            this.Response.Redirect("/Login.aspx");
        }

        private void esrp_OnAuthorized(string login)
        {
            var updatesData = new AccountDataUpdater(login);
            updatesData.ActualizeRegData();
            this.Authenticate(updatesData.LoginUser, false);
        }

        private void esrp_OnNotAuthenticated(string login)
        {
            this.Response.Redirect("/", true);
        }

        #endregion
    }
}