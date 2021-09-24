namespace Ege.Check.App.Web.Common.Auth
{
    using System.Net.Http.Headers;
    using System.Web.Security;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    internal class AuthCookieCreator : IAuthCookieCreator
    {
        public CookieHeaderValue CreateParticipantCookie(ParticipantCacheModel participant)
        {
            return CreateAuthCookie(Cookies.ParticipantCookieName, participant.Serialize(), false);
        }

        public CookieHeaderValue CreateStaffCookie(StaffCookieModel user)
        {
            return CreateAuthCookie(Cookies.StaffCookieName, user.Serialize(), false);
        }

        [NotNull]
        private static CookieHeaderValue CreateAuthCookie(string cookieName, string user, bool isPersistent)
        {
            var ticket = new FormsAuthenticationTicket(user, isPersistent, 1);
            var encTicket = FormsAuthentication.Encrypt(ticket);
            var result = new CookieHeaderValue(cookieName, encTicket) {Path = "/"};
            return result;
        }
    }
}
