using System;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using FogSoft.Helpers;
using GVUZ.Model.Helpers;
using GVUZ.Model.Cache;
using GVUZ.Web.Infrastructure;
using GVUZ.Web.Security;
using System.Diagnostics;
using log4net;
using System.Reflection;

namespace GVUZ.Web.Auth
{
    /// <summary>
    /// Авторизационный номер 
    /// </summary>
    public class EsrpAuthModule : IHttpModule
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Инициализация
        /// </summary>
        /// 
        private string[] _paths;

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += OnAuthenticateRequest;
            context.EndRequest += OnEndRequest;
            _paths = (ConfigurationManager.AppSettings["ESRPAuth.SkipAuthPath"] ?? "").Split(';');
        }

        /// <summary>
        /// Конвертация вилдкардов в регесп
        /// </summary>
        private static string WildcardToRegex(string pattern)
        {
            return Regex.Escape(pattern)
                .Replace("\\*", ".*")
                .Replace("\\?", ".");
        }

        private static Regex[] _matchPathRegEx = null;

        /// <summary>
        /// Необходима ли автризация для доступа к данному url из настроек
        /// </summary>
        private bool CheckIsAuthRequired(string url)
        {
            if (_matchPathRegEx == null)
            {

                _matchPathRegEx = _paths.Select(x => new Regex(WildcardToRegex(x.Trim()), RegexOptions.Compiled | RegexOptions.IgnoreCase)).ToArray();
            }

            return _matchPathRegEx.All(regex => !regex.IsMatch(url));
        }


        private void OnEndRequest(object sender, EventArgs e)
        {
            if (HttpRuntime.Cache.Get("CurrentUser") != null)
            {
                HttpRuntime.Cache.Remove("CurrentUser");
            }
        }


        /// <summary>
        /// Событие на аутетнификацию
        /// </summary>
        private void OnAuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;
            FormsAuthenticationTicket ticket = GetCookie(context);


            //не проверяем особые пути
            if (context.Request.Url.AbsoluteUri.Contains("/LogOff"))
                return;
            if (context.Request.Url.AbsoluteUri.Contains("/AuthRedirect"))
                return;
            if (context.Request.Url.AbsoluteUri.Contains("/AuthError"))
                return;
            if (context.Request.Url.AbsoluteUri.Contains("/AjaxError"))
                return;
            if (!CheckIsAuthRequired(context.Request.Url.AbsoluteUri))
                return;
            //if (context.Request.Url.AbsoluteUri.Contains("/HttpError"))
            //{
            //    context.Response.Redirect("~/Error/HttpError");
            //    return;
            //}

            // если пользователь уже авторизован обновляем куку и ставим пользователя в контекст
            if (ticket != null)
            {
                RenewAuthCookie(context, ticket);
                context.User = new GenericPrincipal(new FormsIdentity(ticket), new string[0]);
            }
            else
            {
                string login = context.Request["l"];
                //string login = "roman@test.ru";
                //login = "test@test.ru";

                  // Вот здесь можно хардкодить для Галактики

                EsrpAuthHelperV2 helper = new EsrpAuthHelperV2();
                if (EsrpAuthHelperV2.ESRPAuthDisabled(login))
                {
                    Log.Debug("Аутентификация ЕСРП отключена".ToUpper());
                    if (String.IsNullOrEmpty(login))
                    {
                        context.Response.Redirect("~/Account/AuthError");
                        return;
                    }
                    //Проверять пароль нет смысла - все актуальные пароли есть только в ЕСРП
                    //Существование пользователя потом проверяется в рамках проверки его прав\ролей
                    ProcessAuth(context, helper, login, Guid.Empty);
                }
                else
                {
                    Log.Debug(String.Format("esrpres={0}, st={1}", context.Request["esrpres"], context.Request["st"]));
                    //если есть параметр, что мы пришли с авторизации
                    if (!String.IsNullOrWhiteSpace(context.Request["esrpres"]))
                    {
                        string status = context.Request["st"];
                        //если неуспешный статус, показываем соответствующую ошибку
                        if (status != "1")
                        {
                            context.Response.Redirect("~/Account/AuthError?statusID=" + status.ToString());
                        }
                        else
                        {
                            Guid guid = Guid.Parse(context.Request["esrpres"]);
                            //идём сами в ЕСРП и уточняем, всё ли хорошо

                            Log.Debug("CheckUserAuth - Begin");
                            int result = CheckEsrpAuth.CheckUserAuth(guid, login); // call esrp.(CheckAuth.asmx).CheckUserStatusByUID(id, 3)
                            Log.Debug(String.Format("CheckUserAuth - End (result={0})", result));
                            if (result == 1) //ok
                            {
                                if (!ProcessAuth(context, helper, login, guid))
                                    return;
                            }
                            else
                            {
                                //                            UserCheckResult checkResult = new CheckEsrpAuth().CheckUserStatusByUID(guid, 
                                context.Response.Redirect("~/Account/AuthError");
                            }
                        }

                        return;
                    }
                }

                //если на странице с ошибкой, выходим
                if (context.Request.Url.AbsolutePath.EndsWith("Error"))
                    return;
                int authState = 1;
                //если пользователь пошёл на логин, ставим статус что его обязательно нужно авторизовать
                if (context.Request.Url.AbsoluteUri.Contains("/LogOn"))
                    authState = 2;

                //redirect to esrp

                //путь для пользователя для авторизации
                string esrpPath = ConfigurationManager.AppSettings["ESRPAuth.Path"];
                if (!esrpPath.EndsWith("/")) esrpPath += "/";
                string fullEsrpPath = esrpPath + "auth/check.aspx"
                                      + "?&sid=3&ra=" + authState.ToString() + "&rp=" +
                                      HttpUtility.UrlEncode(context.Request.Url.AbsoluteUri);

                //check for ajax before redirection
                if (context.Request.HttpMethod == "POST")
                {
                    if (context.Request.ContentType.StartsWith("application/json"))
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.Write("{\"IsError\":true, \"Message\":\"Ошибка авторизации\"}");
                        context.Response.StatusCode = 200;
                        context.Response.End();
                        return;
                    }
                    else
                    {
                        context.Response.Redirect("~/Account/AuthError?statusID=" + 3);
                        return;
                    }
                }

                //если включен показ страницы заглушки, отправляем пользователя туда
                if (ConfigurationManager.AppSettings["ESRPAuth.SlowRedirectMode"].To(true))
                {
                    HttpContext.Current.Items["ESRPRedirectPath"] = fullEsrpPath;
                    context.RewritePath("~/Account/AuthRedirect");
                }
                else //иначе сразу в есрп
                    context.Response.Redirect(fullEsrpPath);
            }
        }

        private bool ProcessAuth(HttpContext context, EsrpAuthHelperV2 helper, string login, Guid guid)
        {
            if (!ProcessUpdateRights(context, helper, login))
                return false;
            ProcessAuthSuccess(context, login, guid);
            return true;
        }

        private bool ProcessUpdateRights(HttpContext context, EsrpAuthHelperV2 helper, string login)
        {
            // обновляем права
            Log.Debug(String.Format("UpdateUserRights - Begin(login={0})", login));                           
            UpdateUserDetailsResult updateUserStatus = helper.UpdateUserRights(login);
            Log.Debug(String.Format("UpdateUserRights - End (updateUserStatus={0})", updateUserStatus));
                           
            if (updateUserStatus != UpdateUserDetailsResult.Updated)
            {
                if (updateUserStatus == UpdateUserDetailsResult.Failed)
                {
                    context.Response.Redirect("~/Account/AuthError?statusID=" + 10);
                    return false;
                }

                //у пользователя нет иниститута и он не в одной из предопределённых ролей - ошибка
                if (updateUserStatus == UpdateUserDetailsResult.NoInstution
                    && !Roles.IsUserInRole(login, UserRole.FBDAdmin) && !Roles.IsUserInRole(login, UserRole.RonAdmin)
                    && !Roles.IsUserInRole(login, UserRole.FbdRonUser))
                {
                    context.Response.Redirect("~/Account/AuthError?statusID=" + 11);
                    return false;
                }
            }
            return true;
        }

        [DebuggerNonUserCode, DebuggerStepThrough]
        private void ProcessAuthSuccess(HttpContext context, string login, Guid guid)
        {
            //ставим куку и отправляем откуда пришёл авторизованным
            SetCookie(context, login, guid);

            //context.User = new GenericPrincipal(new FormsIdentity(resTicket), new string[0]);
            string absoluteUri = context.Request.Url.AbsoluteUri;
            absoluteUri = Regex.Replace(absoluteUri, @"(l|st|esrpres)\=([^&]+)\&?", "");
            while (absoluteUri.EndsWith("?") || absoluteUri.EndsWith("&"))
                absoluteUri = absoluteUri.Substring(0, absoluteUri.Length - 1);

            if (absoluteUri.EndsWith("Account/LogOn"))
            {
                absoluteUri = absoluteUri.Substring(0, absoluteUri.Length - "Account/LogOn".Length);
            }
            
            context.Response.Redirect(absoluteUri);
        }

        private static string AuthCookieName = "esrpAuth";

        /// <summary>
        /// Получаем авторизационную куку
        /// </summary>
        private FormsAuthenticationTicket GetCookie(HttpContext context)
        {
            HttpCookie cookie = context.Request.Cookies[AuthCookieName];
            string cookieVal = null;
            if (cookie != null)
                cookieVal = cookie.Value;
            if (cookieVal == null || cookieVal.Length < 2)
                return null;
            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookieVal);
                if (ticket.Expired) return null;
                return ticket;
            }
            catch (HttpException) //ошибочный тикет
            {
                return null;
            }
        }

        /// <summary>
        /// Обновляем авторизационную куку
        /// </summary>
        private void RenewAuthCookie(HttpContext context, FormsAuthenticationTicket ticket)
        {
            FormsAuthenticationTicket renewTicketIfOld = FormsAuthentication.RenewTicketIfOld(ticket);
            if (!renewTicketIfOld.Expired)
            {
                if(renewTicketIfOld.Expiration != ticket.Expiration)
                {
                    // TODO Подумать
                }
                string encryptedCookie = FormsAuthentication.Encrypt(renewTicketIfOld);
                var cookie = new HttpCookie(AuthCookieName, encryptedCookie);
                cookie.HttpOnly = true;
                //cookie.Expires = DateTime.Now.AddYears(1);
                context.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// Устанавливаем авторизацонную куку
        /// </summary>
        private FormsAuthenticationTicket SetCookie(HttpContext context, string userLogin, Guid guid)
        {
            var ticket = new FormsAuthenticationTicket(1, userLogin, DateTime.Now,
                DateTime.Now.AddMinutes(ConfigurationManager.AppSettings["ESRPAuth.CookieLifeTime"].To(20)),
                false, guid.ToString(), AuthCookieName);

            string encryptedCookie = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(AuthCookieName, encryptedCookie);
            //cookie.Expires = DateTime.Now.AddYears(1);
            context.Response.Cookies.Add(cookie);
            return ticket;
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// Разлогиниваем пользователя
        /// </summary>
        public static void SignOut()
        {
            HttpResponse response = HttpContext.Current.Response;
            //убиваем куку
            response.Cookies.Add(new HttpCookie(AuthCookieName) { Expires = DateTime.Now.AddYears(-1) });
            WebCacheManager.ClearUserRights(UserHelper.GetCurrentUserID());
            FilterStateManager.Current.RemoveAll();
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }
    }
}