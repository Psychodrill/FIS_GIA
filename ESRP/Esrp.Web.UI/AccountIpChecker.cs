using System;
using System.Text;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.Configuration;
using System.Web.SessionState;
using Esrp.Core;
using Esrp.Utility;

namespace Esrp.Web
{/*
    /// <summary>
    /// Проверка допустимости входа с текущего ip адреса при установленной авторизационной куке
    /// </summary>
    public static class AccountIpChecker
    {

        #region Private properties

        /// Имя объекта в сессии
        private const string CheckSessionKey = "AuthenticationCheck";
        // Значение объекта при пройденной проверке
        private const string CheckPassedValue = "Passed";

        private static HttpSessionState Session
        {
            get { return HttpContext.Current.Session; }
        }

        private static HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        private static HttpResponse Response
        {
            get { return HttpContext.Current.Response; }
        }

        private static System.Security.Principal.IPrincipal User
        {
            get { return HttpContext.Current.User; }
        }

        private static string SourcePageUrl
        {
            get
            {
                return Request.Url.ToString();
            }
        }

        private static string ReferrerPageUrl
        {
            get
            {
                if (Request.UrlReferrer == null)
                    return null;

                return Request.UrlReferrer.ToString();
            }
        }

        private static string CheckSessionValue
        {
            get
            {
                // Для текущего ресурса сессия может быть не инстанцирована. Например если это 
                // не .aspx страница.
                if (Session == null || Session[CheckSessionKey] == null)
                    return string.Empty;

                return Session[CheckSessionKey].ToString();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Установка флага успешной проверки
        /// </summary>
        public static void SetCheckPassed()
        {
            Session[CheckSessionKey] = CheckPassedValue;
        }

        /// <summary>
        /// Удаление флага проверки
        /// </summary>
        public static void ClearCheck()
        {
            Session.Remove(CheckSessionKey);
        }

        /// <summary>
        /// Проверка IP адреса пользователя
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        public static void Check(string userName)
        {
            /// Проверка может вызываться из Http хэндлера и текущим ресурсом может быть 
            /// не обязательно .aspx страница. В этом случае сессии не будет инстанцирована.
            /// Пропущу такие ресурсы
            if (HttpContext.Current.Session == null)
                return;

            // Если пользователь не авторизован, то пропустим его
            if (!User.Identity.IsAuthenticated)
                return;

            // Если пользователь авторизован, и установлен флаг проведенной проверки, то
            // пропустим пользователя
            if (User.Identity.IsAuthenticated && CheckSessionValue.Length > 0)
                return;

            // Если IP совпадает в адресом, с которого данный пользователь логинился прошлый раз, 
            // то установлю флаг проведенной проверки и пропущу пользователя дальше
            if (Account.CheckLastAccountIp(userName))
            {
                SetCheckPassed();
                return;
            }

            // IP не совпадает.
            // Т.к. напрямую удалить куку не удается, установлю авторизационной куке время жизни, 
            // меньшее текущей даты. Этим самым я по сути удаляю куку.
            HttpCookie cookie = Response.Cookies[Config.FormsAuthenticationCookieName];
            cookie.Expires = new DateTime();
            Response.Cookies.Set(cookie);
            // Запишу варнинг в лог
            PublishFailedIpCheckWarning(userName);
            // Выполню редирект на страницу логина
            Response.Redirect(Config.LoginPageUrl, true);
        }

        #endregion

        #region Private methods

        private static void PublishFailedIpCheckWarning(string userName)
        {
            // Сформирую информацию
            StringBuilder message = new StringBuilder("Попытка входа по авторизационной куке с отличного от предыдущего IP-адреса.");
            message.Append("\n");
            message.AppendFormat("Login: {0}", userName).Append("\n");
            message.AppendFormat("Password: {0}", "**********").Append("\n");
            message.AppendFormat("User ip: {0}", Request.UserHostAddress).Append("\n");
            message.AppendFormat("Source page: {0}", SourcePageUrl).Append("\n");
            if (ReferrerPageUrl != null)
                message.AppendFormat("Source referrer: {0}", ReferrerPageUrl).Append("\n");

            // Запишу варнинг в лог
            LogManager.Warning(new ApplicationException(message.ToString()));
        }

        #endregion

    }
  * */
}
