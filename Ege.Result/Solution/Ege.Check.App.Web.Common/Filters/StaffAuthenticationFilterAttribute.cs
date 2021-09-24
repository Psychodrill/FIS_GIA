namespace Ege.Check.App.Web.Common.Filters
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Filters;
    using System.Web.Mvc.Filters;
    using Ege.Check.App.Web.Common.Auth;
    using Ege.Check.Logic.Services.Staff.Users;
    using JetBrains.Annotations;
    using ActionFilterAttribute = System.Web.Mvc.ActionFilterAttribute;
    using IAuthenticationFilter = System.Web.Http.Filters.IAuthenticationFilter;

    public class StaffAuthenticationFilterAttribute : 
        ActionFilterAttribute, 
        IAuthenticationFilter,
        System.Web.Mvc.Filters.IAuthenticationFilter
    {
        private static IUserService _service;

        [NotNull]
        private static IUserService Service
        {
            get 
            {
                return _service = _service ?? 
                    GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IUserService))
                    as IUserService;
            }
        }

        // web api
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (context.Request == null)
            {
                throw new InvalidOperationException("context.Request is null");
            }
            var cookieModel = AuthenticationFilterHelper.GetFromRequestCookie(
                context.Request, Cookies.StaffCookieName, StaffCookieModel.Deserialize);
            var principal = await AuthenticateFromCookie(cookieModel);
            if (principal != null)
            {
                context.Principal = principal;
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public new bool AllowMultiple
        {
            get { return false; }
        }

        // mvc
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (filterContext.HttpContext == null || filterContext.HttpContext.Request == null)
            {
                throw new InvalidOperationException(string.Format("filterContext.HttpContext or filterContext.HttpContext.Request is null ({0})", filterContext.HttpContext));
            }
            var cookieModel = AuthenticationFilterHelper.GetFromRequestCookie(
                filterContext.HttpContext.Request, Cookies.StaffCookieName, StaffCookieModel.Deserialize);
            var principal = AuthenticateFromCookieSync(cookieModel);
            if (principal != null)
            {
                filterContext.Principal = principal;
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }

        private async Task<IStaffPrincipal> AuthenticateFromCookie(StaffCookieModel cookieModel)
        {
            IStaffPrincipal result = null;
            if (cookieModel != null)
            {
                var principal = await Service.GetById(cookieModel.Id);
                if (principal != null)
                {
                    result = new StaffPrincipal(principal);
                }
            }
            return result;
        }

        private IStaffPrincipal AuthenticateFromCookieSync(StaffCookieModel cookieModel)
        {
            IStaffPrincipal result = null;
            if (cookieModel != null)
            {
                var principal = Service.GetByIdSync(cookieModel.Id);
                if (principal != null)
                {
                    result = new StaffPrincipal(principal);
                }
            }
            return result;
        }
    }
}
