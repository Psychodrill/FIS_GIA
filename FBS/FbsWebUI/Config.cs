namespace Fbs.Web
{
    using System;
    using System.Configuration;
    using System.Web.Configuration;

    /// <summary>
    /// Параметры конфигурации приложения
    /// </summary>
    public static class Config
    {
        #region Public Properties

        /// <summary>
        /// директория где храниться временный документ справок для распечатки
        /// </summary>
        public static string CNEPrintFolder
        {
            get
            {
                string configPath = ConfigurationManager.AppSettings["CNEPrintFolder"];
                if (string.IsNullOrEmpty(configPath))
                {
                    return "CNEPrintFolder";
                }

                return configPath;
            }
        }

        public static int MaxBatchCheckLines
        {
            get
            {
                string configValue = ConfigurationManager.AppSettings["maxBatchCheckLines"];
                int tryParse;
                if (string.IsNullOrEmpty(configValue) || !int.TryParse(configValue, out tryParse))
                {
                    return 10000;
                }

                return tryParse;
            }
        }

        public static bool IsOpenFbs
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableOpenedFbs"]);
            }
        }

        /// <summary>
        /// Mail администратора, на который отправляются уведомления о новых (не связанных с эталонными) организациях  (fbs@fbsege.ru)
        /// </summary>
        public static string AdminEMail
        {
            get
            {
                return ConfigurationManager.AppSettings["AdminEMail"];
            }
        }
        /// <summary>
        /// Телефон горячей линии ФИС ГИА и Приёма
        /// </summary>
        public static string SupportPhone
        {
            get { return ConfigurationManager.AppSettings["SupportPhone"]; }
        }
        /// <summary>
        /// Адрес электронной почты поддержки ФИС ГИА и Приёма
        /// </summary>
        public static string SupportMail
        {
            get { return ConfigurationManager.AppSettings["SupportMail"]; }
        }

        /// <summary>
        /// Gets AuthenticationSection.
        /// </summary>
        public static AuthenticationSection AuthenticationSection
        {
            get
            {
                return (AuthenticationSection)Configuration.GetSection("system.web/authentication");
            }
        }

        /// <summary>
        /// Конфигурационный файл приложения
        /// </summary>
        public static Configuration Configuration
        {
            get
            {
                return WebConfigurationManager.OpenWebConfiguration("/Web.Config");
            }
        }

        /// <summary>
        /// Секция описания ошибок
        /// </summary>
        public static CustomErrorsSection CustomErrorsSection
        {
            get
            {
                return (CustomErrorsSection)Configuration.GetSection("system.web/customErrors");
            }
        }

        /// <summary>
        /// Отключить галку "Запомнить меня"
        /// </summary>
        public static bool DisableRememberMe
        {
            get
            {
                return Convert.ToBoolean(ConfigurationSettings.AppSettings["DisableRememberMe"]);
            }
        }

        /// <summary>
        /// Url страницы перехода в случае возникновения 404 ошибки
        /// </summary>
        public static string Erorr404RedirectUrl
        {
            get
            {
                return CustomErrorsSection.Errors["404"].Redirect;
            }
        }

        /// <summary>
        /// Url страницы "Запрещенный IP адрес"
        /// </summary>
        public static string ForbiddenIpPageUrl
        {
            get
            {
                return ConfigurationSettings.AppSettings["ForbiddenIpPageUrl"];
            }
        }

        /// <summary>
        /// Gets FormsAuthenticationConfiguration.
        /// </summary>
        public static FormsAuthenticationConfiguration FormsAuthenticationConfiguration
        {
            get
            {
                return AuthenticationSection.Forms;
            }
        }

        /// <summary>
        /// Авторизационной куки 
        /// </summary>
        public static string FormsAuthenticationCookieName
        {
            get
            {
                return FormsAuthenticationConfiguration.Name;
            }
        }

        /// <summary>
        /// Секция описания http хэндлеров
        /// </summary>
        public static HttpHandlersSection HandlerErrorsSection
        {
            get
            {
                return (HttpHandlersSection)Configuration.GetSection("system.web/httpHandlers");
            }
        }

        /// <summary>
        /// Секция описания общих параметров соединения 
        /// </summary>
        public static HttpRuntimeSection HttpRuntimeSection
        {
            get
            {
                return (HttpRuntimeSection)Configuration.GetSection("system.web/httpRuntime");
            }
        }

        /// <summary>
        /// диапазоны IP-адресов, с которых можно работать в интерфейсе администратора
        /// </summary>
        public static string IPRangesForAdmins
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["IPRangesForAdmins"]))
                {
                    return string.Empty;
                }

                return ConfigurationManager.AppSettings["IPRangesForAdmins"];
            }
        }

        /// <summary>
        /// диапазоны IP-адресов, с которых можно работать в интерфейсе администратора
        /// </summary>
        public static string IPRangesForOuterSite
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["IPRangesForOuterSite"]))
                {
                    return string.Empty;
                }

                return ConfigurationManager.AppSettings["IPRangesForOuterSite"];
            }
        }

        /// <summary>
        /// Url страницы логина
        /// </summary>
        public static string LoginPageUrl
        {
            get
            {
                return FormsAuthenticationConfiguration.LoginUrl;
            }
        }

        /// <summary>
        /// Gets LoginTrace_MaxAttemptCount.
        /// </summary>
        public static int LoginTrace_MaxAttemptCount
        {
            get
            {
                return GetIntConfigValue("LoginTrace_MaxAttemptCount", 1, 3);
            }
        }

        /// <summary>
        /// Gets LoginTrace_TimeInterval.
        /// </summary>
        public static int LoginTrace_TimeInterval
        {
            get
            {
                return GetIntConfigValue("LoginTrace_TimeInterval", 1, 30);
            }
        }

        /// <summary>
        /// Gets LoginTrace_WaitTimout.
        /// </summary>
        public static int LoginTrace_WaitTimout
        {
            get
            {
                return GetIntConfigValue("LoginTrace_WaitTimout", 1, 30);
            }
        }

        /// <summary>
        /// Максимальный размер запроса (максимальный разхмер файла, который можно передать серверу)
        /// </summary>
        public static int MaxRequestLength
        {
            get
            {
                return HttpRuntimeSection.MaxRequestLength;
            }
        }

        /// <summary>
        /// Имя кукиса, хранящего закэшированный список ролей
        /// </summary>
        public static string RoleManagerCookeiName
        {
            get
            {
                return RoleManagerSection.CookieName;
            }
        }

        /// <summary>
        /// Секция описания менеджера ролей
        /// </summary>
        public static RoleManagerSection RoleManagerSection
        {
            get
            {
                return (RoleManagerSection)Configuration.GetSection("system.web/roleManager");
            }
        }

        /// <summary>
        /// Путь до папки с общедоступными документами
        /// </summary>
        public static string SharedDocumetsFolder
        {
            get
            {
                return "/" + ConfigurationSettings.AppSettings["SharedDocumentsFolder"].Trim('/') + "/";
            }
        }

        /// <summary>
        /// Url сайта ЕСРП 
        /// </summary>
        public static string UrlEsrp
        {
            get
            {
                return string.Format("{0}", ConfigurationSettings.AppSettings["UrlEsrp"]);
            }
        }

        /// <summary>
        /// Gets WildcardCommandTimeout.
        /// </summary>
        public static int WildcardCommandTimeout
        {
            get
            {
                return GetIntConfigValue("WildcardCommandTimeout", 1, 300);
            }
        }

        #endregion

        #region Methods

        private static int GetIntConfigValue(string paramName, int minValue, int defaultValue)
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[paramName]))
            {
                int value;
                if (int.TryParse(ConfigurationManager.AppSettings[paramName], out value))
                {
                    if (value >= minValue)
                    {
                        return value;
                    }
                }
            }

            return defaultValue;
        }

        #endregion
    }
}