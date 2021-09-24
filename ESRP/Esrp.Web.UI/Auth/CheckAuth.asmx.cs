namespace Esrp.Web.Auth
{
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Web;
    using System.Web.Services;

    using Esrp.Core;
    using Esrp.Core.Systems;
    using Esrp.Web.Administration.IPCheck;

    /// <summary>
    /// Summary description for CheckAuth
    /// </summary>
    [WebService(Namespace = "urn:ersp:v1")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
        // [System.Web.Script.Services.ScriptService]
    public class CheckAuth : WebService
    {
        #region Constants and Fields

        private static readonly IPChecker _ipChecker =
            new IPChecker(ConfigurationManager.AppSettings["ESRPAuth.AllowedIPs"]);

        #endregion

        #region Public Methods

        /// <summary>
        /// The check user access.
        /// </summary>
        /// <param name="userLogin">
        /// The user login.
        /// </param>
        /// <param name="userPassword">
        /// The user password.
        /// </param>
        /// <param name="systemID">
        /// The system id.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        [WebMethod]
        public UserCheckResult CheckUserAccess(string userLogin, string userPassword, int systemID)
        {
            CheckHttps();
            if (systemID < (int)SystemKind.Esrp || systemID > (int)SystemKind.Fbd)
            {
                throw new ArgumentOutOfRangeException("Invalid System ID");
            }

            Account.VerifyStateEnum verifyState = Account.Verify(userLogin, userPassword);
            if (verifyState == Account.VerifyStateEnum.Valid)
            {
                if (GeneralSystemManager.HasUserAccess(userLogin, (SystemKind)systemID))
                {
                    return new UserCheckResult { Login = userLogin, StatusID = 1 };
                }
                else
                {
                    return new UserCheckResult { StatusID = 0 };
                }
            }
            else
            {
                return new UserCheckResult { StatusID = 0 };
            }
        }

        /// <summary>
        /// The check user status by uid.
        /// </summary>
        /// <param name="receivedUID">
        /// The received uid.
        /// </param>
        /// <param name="systemID">
        /// The system id.
        /// </param>
        /// <returns>
        /// </returns>
        [WebMethod]
        public UserCheckResult CheckUserTicket(string login, Guid receivedUID, int systemID)
        {
            CheckHttps();

            if (UserAuthRequestCache.Cache.CheckUserTicket(login, receivedUID, systemID))
                return new UserCheckResult { Login = login, StatusID = 1 };

            return new UserCheckResult { StatusID = 0 };
        }

        #endregion

        #region Methods

        internal static void CheckHttps()
        {
            string isHttps = ConfigurationManager.AppSettings["ESRPAuth.RequireHTTPS"];
            if (HttpContext.Current != null && (string.IsNullOrEmpty(isHttps) || Convert.ToBoolean(isHttps)))
            {
                if (!HttpContext.Current.Request.Url.OriginalString.StartsWith("https://"))
                {
                    throw new ArgumentException("Service requires https protocol");
                }
            }

            string allowedIPs = ConfigurationManager.AppSettings["ESRPAuth.AllowedIPs"];
            if (!string.IsNullOrEmpty(allowedIPs) && HttpContext.Current != null)
            {
                IPAddress_My addr;
                IPAddress_My.TryParse(HttpContext.Current.Request.UserHostAddress, out addr);
                if (!_ipChecker.IsInRage(addr))
                {
                    throw new ArgumentException(string.Format("Disallowed request from {0}", addr));
                }
            }
        }

        #endregion
    }
}