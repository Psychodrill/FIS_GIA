namespace Fbs.Web
{
    using System;
    using System.Configuration;

    using Fbs.Core;
    using System.Web;

    /// <summary>
    /// The redirect manager.
    /// </summary>
    public class RedirectManager
    {
        #region Constants and Fields

        private const string AdministratorAccountRedirectUrl = "/Administration/Accounts/Users/List.aspx";

        // private const string CertificateUserAccountRedirectUrl = "/Profile/View.aspx";
        private const string DefaultRedirectUrl = "/Profile/View.aspx";

        private const string EditUserAccountRedirectUrl = "/Profile/View.aspx";

        private const string HomeRedirectUrl = "/Profile/View.aspx"; // ""

        private const string SupportAccountRedirectUrl = "/Administration/Accounts/Users/List.aspx";

        #endregion

        // Скрытый конструктор
        #region Constructors and Destructors

        private RedirectManager()
        {
        }

        #endregion

        #region Properties

        private static string AuditorAccountRedirectUrl
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableOpenedFbs"])
                           ? "/Certificates/CommonNationalCertificates/CheckForOpenedFbs.aspx"
                           : "/Certificates/CommonNationalCertificates/Check.aspx";
            }
        }

        private static string CertificateUserAccountRedirectUrl
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableOpenedFbs"])
                           ? "/Certificates/CommonNationalCertificates/CheckForOpenedFbs.aspx"
                           : "/Certificates/CommonNationalCertificates/Check.aspx";
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Получение странцы по умолчанию в зависимости от типа пользователя
        /// </summary>
        /// <param name="accountLogin">
        /// The account Login.
        /// </param>
        /// <returns>
        /// url страницы
        /// </returns>
        public static string GetCurrentAuthUrl(string accountLogin)
        {
            string urlEditUserEsrp = string.Format(@"{0}/Profile/View.aspx", Config.UrlEsrp);

            // Получить тип пользователя
            Type accountType = Account.GetType(accountLogin);

            // Определить страницу перехода
            if (accountType == typeof(AdministratorAccount))
            {
                return AdministratorAccountRedirectUrl;
            }
            else if (accountType == typeof(AuditorAccount))
            {
                return AuditorAccountRedirectUrl;
            }
            else if (accountType == typeof(SupportAccount))
            {
                return SupportAccountRedirectUrl;
            }
            else if (accountType == typeof(UserAccount)
                     && UserAccount.GetUserAccount(accountLogin).Status == UserAccount.UserAccountStatusEnum.Revision)
            {
                return urlEditUserEsrp; // EditUserAccountRedirectUrl;
            }
            else if (accountType == typeof(UserAccount)
                     && Account.CheckRole(accountLogin, "CheckCommonNationalCertificate"))
            {
                return CertificateUserAccountRedirectUrl;
            }
            else
            {
                return DefaultRedirectUrl; // urlEditUserEsrp;
            }
        }
        /// <summary>
        /// The get redirect url.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// redirect url
        /// </returns>
        public static string GetRedirectUrl(HttpRequest request)
        {
            Uri uri = request.UrlReferrer ?? request.Url;
            return string.Format("{0}://{1}{2}", uri.Scheme, uri.Authority, uri.AbsolutePath);
        }

        #endregion
    }
}