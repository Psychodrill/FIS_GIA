namespace Ege.Check.App.Web.Common.Auth
{
    using System.Net.Http.Headers;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public interface IAuthCookieCreator
    {
        [NotNull]
        CookieHeaderValue CreateParticipantCookie([NotNull] ParticipantCacheModel participant);

        [NotNull]
        CookieHeaderValue CreateStaffCookie([NotNull] StaffCookieModel user);
    }
}
