namespace Esrp.Web
{
    using System;
    using System.Configuration;
    using System.Security.Principal;
    using System.Web;
    using System.Web.UI;

    /// <summary>
    /// The base master page.
    /// </summary>
    public class BaseMasterPage : MasterPage
    {
        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether AllowCheckCert.
        /// </summary>
        public bool AllowCheckCert
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["AllowCheckCert"]);
            }
        }

        /// <summary>
        /// Gets CurrentUserIp.
        /// </summary>
        public string CurrentUserIp
        {
            get
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
        }

        /// <summary>
        /// Gets CurrentUserName.
        /// </summary>
        public string CurrentUserName
        {
            get
            {
                return this.User.Identity.Name;
            }
        }

        /// <summary>
        /// Показывать ли п.меню "Обезличенные проверки"
        /// </summary>
        public bool EnableHashedChecks
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableHashedChecks"]);
            }
        }

        /// <summary>
        /// Gets User.
        /// </summary>
        public IPrincipal User
        {
            get
            {
                return HttpContext.Current.User;
            }
        }

        #endregion
    }
}