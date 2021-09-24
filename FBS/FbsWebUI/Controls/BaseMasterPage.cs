namespace Fbs.Web
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
        /// Показывать ли п.меню "Свидетельства"
        /// </summary>
        public bool AllowCheckCert
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["AllowCheckCert"]);
            }
        }

        /// <summary>
        /// В каком режиме работает приложение (открытое / закрытое)
        /// </summary>
        public bool EnableOpenedFbs
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableOpenedFbs"]);
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

        protected bool IsAdmin
        {
            get
            {
                return this.User.IsInRole("EditAdministratorAccount");
            }
        }

        /// <summary>
        /// получить get параметр типа инт
        /// </summary>
        /// <param name="name">
        /// название параметра
        /// </param>
        /// <returns>
        /// значение параметра
        /// </returns>
        public int GetParamInt(string name)
        {
            if (this.Page.Request.QueryString[name] != null)
            {
                int returnVal;
                if (int.TryParse(this.Page.Request.QueryString[name], out returnVal))
                {
                    return returnVal;
                }
            }

            return 0;
        }

        public long GetParamLong(string name)
        {
            if (this.Page.Request.QueryString[name] != null)
            {
                long returnVal;
                if (long.TryParse(this.Page.Request.QueryString[name], out returnVal))
                {
                    return returnVal;
                }
            }

            return 0;
        }
        #endregion
    }
}