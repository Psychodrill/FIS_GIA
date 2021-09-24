namespace Ege.Check.App.Web.Filters
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Filters;
    using Ege.Check.App.Web.Auth;
    using Ege.Check.App.Web.Common.Auth;
    using Ege.Check.Logic.Models.Cache;

    public class ParticipantAuthenticationFilterAttribute : Attribute, IAuthenticationFilter
    {
        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (context.Request == null)
            {
                throw new InvalidOperationException("context.Request is null");
            }
            var participant = AuthenticationFilterHelper.GetFromRequestCookie(
                context.Request, Cookies.ParticipantCookieName, ParticipantCacheModel.Deserialize);
            if (participant != null)
            {
                context.Principal = new ParticipantPrincipal(participant);
            }
            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public bool AllowMultiple
        {
            get { return false; }
        }
    }
}