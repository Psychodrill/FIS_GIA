using System.Security.Principal;
using Esrp;
using Esrp.CheckAuthReference;
using Fbs.Web.CheckAuthService;

namespace Fbs.Web
{
    using System;
    using System.Web;
    using System.Web.Security;

    using Fbs.Utility;
    using Fbs.Utility.IoC;

    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;
    using System.Collections.Specialized;

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
            if (this.Request.Url.LocalPath.StartsWith("/embed"))
            {
                NameValueCollection nvc = new NameValueCollection(this.Request.QueryString);
                
                nvc.Add("embed","true");
                this.Context.RewritePath(this.Request.Url.LocalPath.Remove(0, 6),this.Request.PathInfo,"embed=true");
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
            if (this.Response.IsRequestBeingRedirected && !String.IsNullOrEmpty(this.Request.QueryString["embed"]) && Convert.ToBoolean(this.Request.QueryString["embed"]) &&!Response.RedirectLocation.Contains("embed="))
            {
                this.Response.RedirectLocation += (this.Response.RedirectLocation.Contains("?") ? "&" : "?") + "embed=true";
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
                
                // если это доступ к удаленным сервисам
                if ((ex.InnerException != null && (ex.InnerException is System.Net.WebException || ex.InnerException is System.Net.Sockets.SocketException))
                    || (ex.InnerException != null && ex.InnerException.InnerException != null 
                            && (ex.InnerException.InnerException is System.Net.WebException || ex.InnerException.InnerException is System.Net.Sockets.SocketException)))
                {
                    LogManager.Warning(
                        string.Format(
                            "Удаленные проверки через закрытый контур не работают. Возможно сервер недоступен. {0}. {1}",
                            ex.InnerException.Message,
                            ex.InnerException.InnerException != null
                                ? ex.InnerException.InnerException.Message
                                : string.Empty));
                    HttpContext.Current.Response.TrySkipIisCustomErrors = true;
                    HttpContext.Current.Response.Redirect(@"/ChecksNotAvailable.aspx");
                }

                // Если ошибка попадает в список ошибок, предназначеных для "ручной" обработки, 
                // то обработаю эту ошибку. Иначе - публикую ошибку в логе.
                if (ManualErrorHandler.CanHandle(ex))
                {
                    ManualErrorHandler.Handle(ex);
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

            /* если мы ломимся с фрейма - надо пробить нового юзера в базу фбс */
            if (!Request.IsAuthenticated && Request.Params["ticket"] != null)
            {
                try
                {
                    var ticket = FormsAuthentication.Decrypt(Request.Params["ticket"]);
                    if (ticket != null)
                    {
                        var accountUpdater = new AccountDataUpdater(ticket.Name);
                        if (accountUpdater.CheckUserTicket(new Guid(ticket.UserData), 3)) /* Ломимся из ФИС */
                        {
                            accountUpdater.ActualizeRegData();
                            FormsAuthentication.SetAuthCookie(ticket.Name, false);
                            this.Response.Redirect(Request.Url.LocalPath, true);
                        }
                    }
                }
                catch
                {
                    Request.Cookies.Clear();
                }
            }

            // Проверка существования запрашиваемого ресурса 
            ResourceExistenceChecker.Check();
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