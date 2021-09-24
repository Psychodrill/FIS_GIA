namespace Ege.Check.App.Web.Common.Auth
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using System.Web.Security;
    using JetBrains.Annotations;

    public class AuthenticationFilterHelper
    {
        public static TUserModel GetFromRequestCookie<TUserModel>(
            [NotNull] HttpRequestMessage request,
            [NotNull] string cookieName,
            [NotNull] Func<string, TUserModel> deserialize)
            where TUserModel : class
        {
            var cookiesHeader = (request.Headers.GetCookies() ?? Cookies.None).FirstOrDefault();
            if (cookiesHeader == null || cookiesHeader.Cookies == null)
            {
                return null;
            }

            var participantCookie = cookiesHeader.Cookies.FirstOrDefault(c => c != null && cookieName.Equals(c.Name));
            if (participantCookie == null || participantCookie.Value == null)
            {
                return null;
            }

            return GetFromCookie(cookieName, participantCookie.Value, deserialize);
        }

        public static TUserModel GetFromRequestCookie<TUserModel>(
            [NotNull] HttpRequestBase request,
            [NotNull] string cookieName,
            [NotNull] Func<string, TUserModel> deserialize)
            where TUserModel : class
        {
            var cookie = request.Cookies != null ? request.Cookies[cookieName] : null;
            if (cookie == null || cookie.Value == null)
            {
                return null;
            }
            return GetFromCookie(cookieName, cookie.Value, deserialize);
        }

        public static TUserModel GetFromCookie<TUserModel>(
            [NotNull] string cookieName, 
            [NotNull] string cookieValue,
            [NotNull] Func<string, TUserModel> deserialize)
            where TUserModel : class
        {
            try
            {
                var ticket = FormsAuthentication.Decrypt(cookieValue);
                var model = ticket != null
                                ? deserialize(ticket.Name)
                                : null;
                if (model == null)
                {
                    CookieHelper.Remove(HttpContext.Current, cookieName);
                }
                return model;
            }
            catch (Exception)
            {
                CookieHelper.Remove(HttpContext.Current, cookieName);
                return null;
            }
        }
    }
}