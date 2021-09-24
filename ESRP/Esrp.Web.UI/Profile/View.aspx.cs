namespace Esrp.Web.Personal.Profile
{
    using System;

    using Esrp.Core;
    using Esrp.Utility;
    using Esrp.Core.Loggers;
    using System.Configuration;

    /// <summary>
    /// The view.
    /// </summary>
    public partial class View : BasePage
    {
        // Пути до .ascx файлов контролов просмотра профиля
        #region Constants and Fields

        private const string IntrantProfileViewControlPath = "/Controls/IntrantProfileView.ascx";

        private const string UserProfileViewControlPath = "/Controls/UserProfileView.ascx";

        private Type mAccountType;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets AccountType.
        /// </summary>
        public Type AccountType
        {
            get
            {
                if (this.mAccountType == null)
                {
                    this.mAccountType = Account.GetType(Account.ClientLogin);
                }

                if (this.mAccountType == null)
                {
                    LogManager.Error(string.Format("невозможно определить тип пользователя {0}. Возможно у него нет групп в бд", Account.ClientLogin));
                    Response.Redirect(@"~\Error.aspx");
                }

                return this.mAccountType;
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
            // Добавление контролов происходит в коде, т.к. при загрузке страницы необходмо 
            // знать статус пользователя ВУЗ/СУЗ, а для этого его (пользователя) нужно получить.
            // В этом случае я получу ошибку, т.к. отработают оба метода получения пользователя - 
            // GetUserAccount и GetIntrantAccount и один из них вернет null. Кроме того, 
            // исключается повторный возов метода Account.GetType().
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["LogUserView"]))
                AccountEventLogger.LogAccountViewEvent(Account.ClientLogin);
            if (this.AccountType == typeof(UserAccount))
            {
                this.Form.Controls.Add(this.Page.LoadControl(UserProfileViewControlPath));
            }
            else
            {
                this.Form.Controls.Add(this.Page.LoadControl(IntrantProfileViewControlPath));
            }
        }

        #endregion
    }
}