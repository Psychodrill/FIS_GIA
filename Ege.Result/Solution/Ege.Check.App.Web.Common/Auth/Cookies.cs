namespace Ege.Check.App.Web.Common.Auth
{
    using System.Collections.Generic;
    using System.Net.Http.Headers;
    using JetBrains.Annotations;

    public static class Cookies
    {
        public const string ParticipantCookieName = "Participant";
        public const string StaffCookieName = "Staff";

        [NotNull] public static ICollection<CookieHeaderValue> None = new CookieHeaderValue[0];
    }
}