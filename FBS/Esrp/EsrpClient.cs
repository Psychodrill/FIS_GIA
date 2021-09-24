namespace Esrp
{
    using System;
    using System.Text;
    using System.Web;

    using Esrp.CheckAuthReference;
    using Esrp.GetDataReference;

    /// <summary>
    /// The esrp client.
    /// </summary>
    public class EsrpClient
    {
        #region Constants and Fields

        private const string AccountLoginKey = "l";

        private const string EsrpresIdKey = "esrpres";

        private const string StatusAccountKey = "st";

        private const string SystemIdKey = "sid";

        private const string TypeAuthKey = "ra";

        private const string UrlKey = "rp";

        private string esrpUrl;

        #endregion

        #region Delegates

        /// <summary>
        /// The auth delegate.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        public delegate void AuthDelegate(string login);

        #endregion

        #region Public Events

        /// <summary>
        /// Пользователь авторизован, но не имеет доступа к целевой системе
        /// </summary>
        public event AuthDelegate OnAuthenticated;

        /// <summary>
        /// Пользователь авторизован
        /// </summary>
        public event AuthDelegate OnAuthorized;

        /// <summary>
        /// Пользователь не авторизован, или ему не удалось авторизоваться
        /// </summary>
        public event AuthDelegate OnNotAuthenticated;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets CheckServiceUrl.
        /// </summary>
        public string CheckServiceUrl
        {
            get
            {
                return string.Format("{0}/auth/CheckAuth.asmx", this.EsrpUrl);
            }
        }

        /// <summary>
        /// Gets DataServiceUrl.
        /// </summary>
        public string DataServiceUrl
        {
            get
            {
                return string.Format("{0}/auth/GetData.asmx", this.EsrpUrl);
            }
        }

        /// <summary>
        /// Gets or sets EsrpUrl.
        /// </summary>
        public string EsrpUrl
        {
            get
            {
                return this.esrpUrl;
            }

            set
            {
                this.esrpUrl = value.Trim();
                if (this.esrpUrl.EndsWith("/"))
                {
                    this.esrpUrl = this.EsrpUrl.Remove(this.esrpUrl.Length - 1);
                }
            }
        }

        /// <summary>
        /// Gets or sets SystemId.
        /// </summary>
        public SystemId SystemId { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The account data.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <returns>
        /// </returns>
        public UserDetails AccountData(string login)
        {
            var getData = new GetData { Url = this.DataServiceUrl };
            return getData.GetUserDetails(login, (int)this.SystemId);
        }

        /// <summary>
        /// The actualize reg data.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="updateAccountDate">
        /// The update account date.
        /// </param>
        /// <param name="updateOrganizationDate">
        /// The update organization date.
        /// </param>
        /// <returns>
        /// </returns>
        public ActualizationData ActualizeRegData(
            string login, DateTime updateAccountDate, DateTime updateOrganizationDate)
        {
            var getData = new GetData { Url = this.DataServiceUrl };
            return getData.GetActualizationData(login, updateAccountDate, updateOrganizationDate);
        }

        /// <summary>
        /// Получаем статус авторизации пользователя
        /// </summary>
        public int CheckUserTicket(Guid userGuid, string userLogin, int systemId)
        {
            var checkAuth = new CheckAuth { Url = this.CheckServiceUrl };
            try
            {
                var guidCheckResult = checkAuth.CheckUserTicket(userLogin, userGuid, systemId);
                if (guidCheckResult.StatusID == 1)
                {
                    if (guidCheckResult.Login == userLogin)
                        return 1;
                }

                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// The check account by web service.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        public void CheckAccountByWebService(string login, string password)
        {
            var checkAuth = new CheckAuth { Url = this.CheckServiceUrl };
            UserCheckResult checkResult = checkAuth.CheckUserAccess(login, password, (int)this.SystemId);
            var accountStatus = (AccountStatus)checkResult.StatusID;
            switch (accountStatus)
            {
                case AccountStatus.Authorized:
                    if (login == checkResult.Login)
                    {
                        this.authorized(checkResult.Login);
                    }
                    else
                    {
                        this.authenticated(login);
                    }

                    break;
                case AccountStatus.AuthorizedNoAccess:
                    this.authenticated(login);
                    break;
                case AccountStatus.NotAuthorized:
                    this.notAuthenticated(login);
                    break;
            }
        }

        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        public void Login(string returnUrl, HttpResponse response)
        {
            var urlBuilder = new StringBuilder(this.EsrpUrl);
            urlBuilder.Append("/auth/check.aspx");
            urlBuilder.AppendFormat("?{0}={1}", TypeAuthKey, (int)AuthorizationType.Logon);
            urlBuilder.AppendFormat("&{0}={1}", SystemIdKey, (int)this.SystemId);
            urlBuilder.AppendFormat("&{0}={1}", UrlKey, returnUrl);
            response.Redirect(urlBuilder.ToString(), true);
        }

        /// <summary>
        /// The logout.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        public void Logout(string returnUrl, HttpResponse response)
        {
            var urlBuilder = new StringBuilder(this.EsrpUrl);
            urlBuilder.Append("/auth/check.aspx");
            urlBuilder.AppendFormat("?{0}={1}", TypeAuthKey, (int)AuthorizationType.Logout);
            urlBuilder.AppendFormat("&{0}={1}", SystemIdKey, (int)this.SystemId);
            urlBuilder.AppendFormat("&{0}={1}", UrlKey, returnUrl);
            response.Redirect(urlBuilder.ToString(), true);
        }

        /// <summary>
        /// The organization data by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        public OrganizationData[] OrganizationDataById(int id)
        {
            var getData = new GetData { Url = this.DataServiceUrl };
            return getData.GetOrganizationData(id, 1);
        }

        /// <summary>
        /// The organizations data.
        /// </summary>
        /// <param name="logins">
        /// The logins.
        /// </param>
        /// <returns>
        /// </returns>
        public OrganizationData[] OrganizationsData(string[] logins)
        {
            var getData = new GetData { Url = this.DataServiceUrl };
            return getData.GetOrganizationData(logins);
        }

        /// <summary>
        /// The parse.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        public void Parse(HttpRequest request)
        {
            if (request.QueryString.Count != 0 && !string.IsNullOrEmpty(request.QueryString[StatusAccountKey])
                && !string.IsNullOrEmpty(request.QueryString[EsrpresIdKey])
                && !string.IsNullOrEmpty(request.QueryString[AccountLoginKey]))
            {
                short intStatusAccount;
                AccountStatus accountStatus = !string.IsNullOrEmpty(request.QueryString[StatusAccountKey])
                                              &&
                                              short.TryParse(
                                                  request.QueryString[StatusAccountKey], out intStatusAccount)
                                                  ? (AccountStatus)intStatusAccount
                                                  : AccountStatus.NotAuthorized;

                Guid authKey;
                try
                {
                    authKey = new Guid(request.QueryString[EsrpresIdKey]);
                }
                catch
                {
                    authKey = Guid.Empty;
                }

                string login = request[AccountLoginKey];

                switch (accountStatus)
                {
                    case AccountStatus.Authorized:
                        if (authKey != null && authKey != Guid.Empty && !string.IsNullOrEmpty(login))
                        {
                            var checkAuth = new CheckAuth { Url = this.CheckServiceUrl };
                            UserCheckResult checkResult = checkAuth.CheckUserTicket(login, authKey, (int)this.SystemId);
                            if (login == checkResult.Login && checkResult.StatusID == (int)AccountStatus.Authorized)
                            {
                                this.authorized(checkResult.Login);
                            }
                            else
                            {
                                this.authenticated(checkResult.Login);
                            }
                        }
                        else
                        {
                            this.notAuthenticated(login);
                        }

                        break;
                    case AccountStatus.AuthorizedNoAccess:
                        this.authenticated(login);
                        break;
                    case AccountStatus.NotAuthorized:
                        this.notAuthenticated(login);
                        break;
                }
            }
        }

        /// <summary>
        /// The redirect default.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        public void RedirectDefault(HttpRequest request, HttpResponse response)
        {
            if (request.QueryString[EsrpresIdKey] == Guid.Empty.ToString()
                && string.IsNullOrEmpty(request.QueryString[StatusAccountKey])
                && request.QueryString[StatusAccountKey] == ((int)AccountStatus.NotAuthorized).ToString())
            {
                response.Redirect("/", true);
            }
        }

        #endregion

        #region Methods

        private void authenticated(string login)
        {
            if (this.OnAuthenticated != null)
            {
                this.OnAuthenticated(login);
            }
        }

        private void authorized(string login)
        {
            if (this.OnAuthorized != null)
            {
                this.OnAuthorized(login);
            }
        }

        private void notAuthenticated(string login)
        {
            if (this.OnNotAuthenticated != null)
            {
                this.OnNotAuthenticated(login);
            }
        }



        #endregion
    }
}