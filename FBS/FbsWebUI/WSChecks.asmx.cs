using System.Security.Authentication;
using Fbs.Web.WebServices;

namespace Fbs.Web
{
    using System.ComponentModel;
    using System.Web;
    using System.Web.Security;
    using System.Web.Services;
    using System.Web.Services.Protocols;

    using Esrp;

    using Fbs.Core;
    using Fbs.Core.WebServiceCheck;
    using Fbs.Web.CheckAuthService;

    /// <summary>
    /// Имя пользователя и пароль будут передаваться в SOAP-заголовках
    /// </summary>
    public class UserCredentials : SoapHeader
    {
        #region Fields

        public string Client;

        public string Login;

        public string Password;

        #endregion
    }

    /// <summary>
    /// Summary description for WSChecks
    /// </summary>
    [WebService(Namespace = "urn:fbs:v2")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WSChecks : WebService
    {
        #region Fields

        public UserCredentials CredentialsHeader;

        #endregion

        #region Properties

        private bool isPossible { get; set; }

        #endregion

        private const string AuthenticationErrorMessage =
            "Ошибка авторизации! Проверьте правильность формата входящего запроса, а так же правильность указанного логина и пароля в заголовке UserCredentials";

        #region Public Methods and Operators

        /// <summary>
        /// The auhenticate.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The auhenticate.
        /// </returns>
        public bool Auhenticate()
        {
            if (this.CredentialsHeader == null)
                return false;

            var esrpClient = new EsrpClient { SystemId = SystemId.Fbs, EsrpUrl = Config.UrlEsrp };
            esrpClient.OnAuthenticated += this.esrpClient_OnAuthenticated;
            esrpClient.OnAuthorized += this.esrpClient_OnAuthorized;
            esrpClient.OnNotAuthenticated += this.esrpClient_OnNotAuthenticated;
            esrpClient.CheckAccountByWebService(this.CredentialsHeader.Login, this.CredentialsHeader.Password);

            return this.isPossible;
        }

        /// <summary>
        /// The batch check.
        /// </summary>
        /// <param name="queryXML">
        /// The query xml.
        /// </param>
        /// <returns>
        /// The batch check.
        /// </returns>
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public string BatchCheck(string queryXML)
        {
            if (!this.Auhenticate())
                return AuthenticationErrorMessage;

            var result = (new WSBatchCheck(this.CredentialsHeader.Client)).BeginBatchCheck(queryXML);
            this.LogOut();
            return result;
        }

        /// <summary>
        /// The batch check by CertificateNumber.
        /// </summary>
        /// <param name="queryXML">
        /// The query xml.
        /// </param>
        /// <returns>
        /// The batch check.
        /// </returns>
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public string BatchCheckNN(string queryXML)
        {
            if (!this.Auhenticate())
                return AuthenticationErrorMessage;

            var result = (new WSBatchCheck(this.CredentialsHeader.Login)).BeginBatchCheckNN(queryXML);
            this.LogOut();
            return result;
        }

        /// <summary>
        /// The get batch check query sample.
        /// </summary>
        /// <returns>
        /// The get batch check query sample.
        /// </returns>
        [WebMethod]
        public string GetBatchCheckQuerySample()
        {
            string result = (new WSBatchCheck(null)).GetQuerySample();
            this.LogOut();
            return result;
        }

        /// <summary>
        /// The get batch check query sample by CertificateNumber.
        /// </summary>
        /// <returns>
        /// result
        /// </returns>
        [WebMethod]
        public string GetBatchCheckQuerySampleNN()
        {
            string result = (new WSBatchCheck(null)).GetBatchCheckQuerySampleNN();
            this.LogOut();
            return result;
        }

        /// <summary>
        /// The get batch check result.
        /// </summary>
        /// <param name="queryXML">
        /// The query xml.
        /// </param>
        /// <returns>
        /// The get batch check result.
        /// </returns>
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public string GetBatchCheckResult(string queryXML)
        {
            if (!this.Auhenticate())
                return AuthenticationErrorMessage;

            var result = (new WSBatchCheck(this.CredentialsHeader.Client)).GetResult(queryXML);
            this.LogOut();
            return result;
        }

        /// <summary>
        /// The get batch check result.
        /// </summary>
        /// <param name="queryXML">
        /// The query xml.
        /// </param>
        /// <returns>
        /// The get batch check result.
        /// </returns>
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public string GetBatchCheckResultNN(string queryXML)
        {
            if (!this.Auhenticate())
                return AuthenticationErrorMessage;

            string result = (new WSBatchCheck(this.CredentialsHeader.Login)).GetResultNN(this.CredentialsHeader.Password, queryXML);
            this.LogOut();
            return result;
        }

        /// <summary>
        /// The get single check query sample.
        /// </summary>
        /// <returns>
        /// The get single check query sample.
        /// </returns>
        [WebMethod]
        public string GetSingleCheckQuerySample()
        {
            string result = (new WSSingleCheck(null)).GetQuerySample();
            this.LogOut();
            return result;
        }

        /// <summary>
        /// The get single check query sample by PassportNumber.
        /// </summary>
        /// <returns>
        /// result
        /// </returns>
        [WebMethod]
        public string GetSingleCheckQuerySampleNN()
        {
            string result = (new WSSingleCheck(null)).GetQuerySampleNN();
            this.LogOut();
            return result;
        }

        /// <summary>
        /// The single check.
        /// </summary>
        /// <param name="queryXML">
        /// The query xml.
        /// </param>
        /// <returns>
        /// The single check.
        /// </returns>
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public string SingleCheck(string queryXML)
        {
            if (!this.Auhenticate())
                return AuthenticationErrorMessage;

            var result = (new WSSingleCheck(this.CredentialsHeader.Client)).Check(this.CredentialsHeader.Client, queryXML);
            this.LogOut();
            return result;
        }

        /// <summary>
        /// The single check by PassportNumber.
        /// </summary>
        /// <param name="queryXML">
        /// The query xml.
        /// </param>
        /// <returns>
        /// The single check.
        /// </returns>
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public string SingleCheckNN(string queryXML)
        {
            if (!this.Auhenticate())
                return AuthenticationErrorMessage;

            string result = (new WSSingleCheck(this.CredentialsHeader.Login))
                .SingleCheckNN(this.CredentialsHeader.Login, HttpContext.Current.Request.UserHostAddress, queryXML);
            this.LogOut();
            return result;
        }

        #endregion

        #region Methods

        private void LogOut()
        {
            FormsAuthentication.SignOut();
        }

        private void LogOutNotAuthenticate()
        {
            this.isPossible = false;
            this.LogOut();
        }

        private void esrpClient_OnAuthenticated(string login)
        {
            this.LogOutNotAuthenticate();
        }

        private void esrpClient_OnAuthorized(string login)
        {
            var updatesData = new AccountDataUpdater(login);
            if (updatesData.FbsSystemsAccount())
            {
                updatesData.LoginUser = this.CredentialsHeader.Client;
                login = this.CredentialsHeader.Login = this.CredentialsHeader.Client;
            }

            updatesData.ActualizeRegData();

            Account.VerifyStateEnum verifyState = Account.Verify(string.IsNullOrEmpty(login) ? string.Empty : login);

            if (verifyState == Account.VerifyStateEnum.Valid && Account.CheckRole(login, "ViewCertificateSection"))
            {
                FormsAuthentication.SetAuthCookie(login, true);
                this.isPossible = true;
            }
            else
            {
                this.LogOutNotAuthenticate();
            }
        }

        private void esrpClient_OnNotAuthenticated(string login)
        {
            this.LogOutNotAuthenticate();
        }

        #endregion
    }
}