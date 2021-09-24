namespace Esrp.Web
{
    using System;
    using System.Web;
    using System.Web.Security;

    using Esrp.Utility;
    using Esrp.Utility.IoC;

    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using Esrp.Web.Extentions;
    using System.Collections.Specialized;
    using System.Text.RegularExpressions;
    using System.Configuration;

    /// <summary>
    /// The global.
    /// </summary>
    public class Global : HttpApplication
    {
        #region Constants and Fields

        private string FatalErrorMessage =
            "Произошла критическая ошибка. Нажмите кнопку \"Назад\" в браузере и попробуйте ещё раз.";

        #endregion

        #region Public Properties

        /// <summary>
        /// Имя текущего пользователя
        /// </summary>
        public string CurrentUserName
        {
            get
            {
                return this.User.Identity.Name;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The application_ authenticate request.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The application_ begin request.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            
            string path = this.Context.Request.Path.ToLower();

            int lastExtension = path.LastIndexOf(".aspx");
            if (lastExtension == -1)
            {
                return;
            }
            if (this.Request.QueryString.Keys.Count>0)
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["EnhacedSecurity"]))
                {
                    if (this.Request.QueryString.Keys.Count == 1 && String.IsNullOrEmpty(this.Request.QueryString.Keys[0]))
                    {

                        string qs = UrlShortener.ShortUrlUtils.RetrieveUrlFromDatabase(this.Request.QueryString[0]).RealUrl.Replace("+","%2B");
                        NameValueCollection nvc = HttpUtility.ParseQueryString(qs);
                        qs = "";
                        foreach (string str in nvc.Keys)
                        {
                            qs += String.IsNullOrEmpty(qs) ? "" : "&";
                            if(str!=null)
                            qs += String.Format("{0}={1}",str, HttpUtility.UrlEncode(nvc[str]));         
                        }
                        string url = string.Format("{0}?{1}", this.Request.RawUrl.Substring(0,this.Request.RawUrl.IndexOf("?")), qs);
                        this.Context.RewritePath(url,false);
                    }
                    else
                        this.Response.Redirect(HttpUtility.UrlDecode(this.Request.RawUrl), true);
                }
        }

        /// <summary>
        /// The application_ end.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Application_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The application_ end request.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            
            if (this.Response.IsRequestBeingRedirected && Convert.ToBoolean(ConfigurationManager.AppSettings["EnhacedSecurity"]))
            {

                Uri location = new Uri(this.Response.RedirectLocation,UriKind.RelativeOrAbsolute);
                if (location.IsAbsoluteUri && (location.Host != HttpContext.Current.Request.Url.Host || location.Port != HttpContext.Current.Request.Url.Port))
                    return;
                
                string referrer = location.ToString();
                 string url = referrer.IndexOf("?")>0?referrer.Substring(0, referrer.IndexOf("?")):referrer;
                
                string oldqs =referrer.IndexOf("?")>0?referrer.Substring(referrer.IndexOf("?") + 1):"";
                if (!string.IsNullOrEmpty(oldqs))
                {
                    string queryParams = oldqs;
                    NameValueCollection col = HttpUtility.ParseQueryString(oldqs);
                    if (col.Keys[0] == null)
                    {
                        oldqs = oldqs.Replace(col[0], UrlShortener.ShortUrlUtils.RetrieveUrlFromDatabase(col[0]).RealUrl);
                    }
                    
                    string eqs = UrlShortener.ShortUrlUtils.GenerateShortUrl(HttpUtility.UrlEncode(oldqs));
                    this.Response.RedirectLocation = String.Format("{0}?{1}", url, eqs);

                }
            }
        }

        /// <summary>
        /// The application_ error.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                // получу последнюю ошибку
                Exception ex = this.Server.GetLastError();

                // Если ошибка попадает в список ошибок, предназначеных для "ручной" обработки, 
                // то обработаю эту ошибку. Иначе - публикую ошибку в логе.
                if (ManualErrorHandler.CanHandle(ex))
                {
                    ManualErrorHandler.Handle(ex);
                }
                else if (ex.InnerException != null)
                {
                    LogManager.Error(ex.InnerException);
                }
                else
                {
                    LogManager.Error(ex);
                }
            }
            catch
            {
                this.Response.Write(this.FatalErrorMessage);
            }
        }

        /// <summary>
        /// The application_ post acquire request state.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Application_PostAcquireRequestState(object sender, EventArgs e)
        {
            // Проверка допустимости входа с текущего ip адреса 
            // при установленной авторизационной куке
            // AccountIpChecker.Check(CurrentUserName);
        }

        /// <summary>
        /// The application_ post authenticate request.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            // Если в web.config в секции authentication/forms задан параметр requireSSL и 
            // текущая страница отдается по http, то перекину пользователя на https 
            if (FormsAuthentication.RequireSSL && !this.Request.IsSecureConnection)
            {
                this.Response.Redirect(
                    this.Context.Request.Url.ToString().Replace(Uri.UriSchemeHttp, Uri.UriSchemeHttps), true);
            }

            // Проверка существования запрашиваемого ресурса 
            //ResourceExistenceChecker.Check();
        }

        /// <summary>
        /// The application_ start.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Application_Start(object sender, EventArgs e)
        {
            this.InitializeDependencyInjectionContainer();
        }

        private void InitializeDependencyInjectionContainer()
        {
            IUnityContainer container = new UnityContainerFactory().CreateConfiguredContainer();
            var serviceLocator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }

        /// <summary>
        /// The session_ end.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Session_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The session_ start.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Session_Start(object sender, EventArgs e)
        {
        }

        #endregion
    }
}