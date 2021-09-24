namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Fbs.Core;
    using Fbs.Core.Organizations;
    using Fbs.Core.Shared;
    using Fbs.Core.UICheckLog;
    using Fbs.Utility;
    using Fbs.Web.CheckAuthService;

    using WebControls;

    /// <summary>
    /// The check.
    /// </summary>
    public partial class Check : BasePage, IHistoryNavigator
    {
        #region Constants and Fields

        private const int MaxSubjectMark = 100;

        private const int MinSubjectMark = 0;

        private const string SearchUrl =
            "/Certificates/CommonNationalCertificates/CheckResultCommon.aspx?"
            + "number={0}&LastName={1}&FirstName={2}&PatronymicName={3}&Ev={4}";

        #endregion

        #region Methods

        /// <summary>
        /// The on load.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
            // Выйду если постбэк или нет объекта в сессии
            if (this.Page.IsPostBack || this.Session[UINavigation.SessionId] == null)
            {
                return;
            }

            var user = UserAccount.GetUserAccount(CurrentUser.ClietnLogin);
            if (!string.IsNullOrEmpty(this.Request.QueryString["embed"]) && user == null)
            {
                var updatesData = new AccountDataUpdater(CurrentUser.ClietnLogin);
                updatesData.ActualizeRegData();
                this.Authenticate(updatesData.LoginUser, false);
            }

            // Получу состояние контрола навигации
            var state = (UserNavigatorState)this.Session[UINavigation.SessionId];

            // Если происходит переход назад по экшену Back, то восстановлю сохраненные состояния 
            // контролов, иначе удалю сохраненные состояния 
            if (state.OkBack)
            {
                try
                {
                    StateManager.RestoreState(this.Page);
                }
                catch
                {
                    StateManager.ClearState();
                }
            }
            else
            {
                StateManager.ClearState();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["EnableOpenedFbs"]))
            {
                this.PageHideInLeftMenu();
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Проверить"
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// Параметры события
        /// </param>
        protected void BtnCheckClick(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
            {
                return;
            }

            
            // Сохраню состояния контролов
            StateManager.AddEntry(this.txtNumber.ID, this.txtNumber.Text.FullTrim());
            StateManager.AddEntry(this.txtLastName.ID, this.txtLastName.Text.FullTrim());
            StateManager.AddEntry(this.txtFirstName.ID, this.txtFirstName.Text.FullTrim());
            StateManager.AddEntry(this.txtPatronymicName.ID, this.txtPatronymicName.Text.FullTrim());
            StateManager.SaveState();

            // Зарегистритую событие
            string login = HttpContext.Current.User.Identity.Name;
            Organization org = OrganizationDataAccessor.GetByLogin(login);
            if ((org != null && org.DisableLog == false) || org == null)
            {
                int eventId = CheckLogDataAccessor.AddCNENumberCheckEvent(
                    this.User.Identity.Name, 
                    this.txtLastName.Text.FullTrim(), 
                    this.txtFirstName.Text.FullTrim(), 
                    this.txtPatronymicName.Text.FullTrim(), 
                    this.txtNumber.Text.FullTrim());

                // Перейду на страницу результатов (поиска).
                this.Response.Redirect(
                    string.Format(
                        SearchUrl, 
                        HttpUtility.UrlEncode(this.txtNumber.Text.FullTrim()), 
                        HttpUtility.UrlEncode(this.txtLastName.Text.FullTrim()), 
                        HttpUtility.UrlEncode(this.txtFirstName.Text.FullTrim()), 
                        HttpUtility.UrlEncode(this.txtPatronymicName.Text.FullTrim()), 
                        HttpUtility.UrlEncode(eventId.ToString())));
            }
            else
            {
                // Перейду на страницу результатов (поиска).
                this.Response.Redirect(
                    string.Format(
                        SearchUrl, 
                        HttpUtility.UrlEncode(this.txtNumber.Text.FullTrim()), 
                        HttpUtility.UrlEncode(this.txtLastName.Text.FullTrim()), 
                        HttpUtility.UrlEncode(this.txtFirstName.Text.FullTrim()), 
                        HttpUtility.UrlEncode(this.txtPatronymicName.Text.FullTrim()), 
                        string.Empty));
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Очистить"
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// параметры события
        /// </param>
        protected void BtnResetClick(object sender, EventArgs e)
        {
            this.Response.Redirect(this.Request.RawUrl);
        }

        public void Authenticate(string userName, bool isPersistant)
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

                    break;
                default:
                    break;
            }

            /*if (verifyState != Account.VerifyStateEnum.Valid)
            {
                this.CheckLoginAttemptsInfo();
            }*/
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


        #endregion

        public string GetPageName()
        {
            return "CheckHistoryResultCommon.aspx";
        }
    }
}