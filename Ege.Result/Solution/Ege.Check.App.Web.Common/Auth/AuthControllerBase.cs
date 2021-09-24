namespace Ege.Check.App.Web.Common.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Ege.Check.Common.Extensions;
    using Ege.Check.Logic.Models.Requests;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Check.Logic.Services.Staff.Users;
    using JetBrains.Annotations;

    public abstract class AuthControllerBase : ApiController
    {
        [NotNull]protected readonly IUserService UserService;
        [NotNull] protected readonly IAuthCookieCreator AuthCookieCreator;

        protected AuthControllerBase([NotNull]IUserService userService, [NotNull]IAuthCookieCreator authCookieCreator)
        {
            UserService = userService;
            AuthCookieCreator = authCookieCreator;
        }

        protected async Task<HttpResponseMessage> Login(StaffLoginRequest request, IEnumerable<Role> allowedRoles = null)
        {
            if (request == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var user = await UserService.GetByLoginAndPassword(request.Login, request.Password);
            if (user == null || allowedRoles != null && !allowedRoles.Contains(user.Role))
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            var response = Request.CreateResponse(HttpStatusCode.NoContent);
            if (response == null)
            {
                throw new InvalidOperationException("Request.CreateResponse returned null");
            }
            response.Headers.AddCookies(
                AuthCookieCreator.CreateStaffCookie(new StaffCookieModel { Id = user.Id }).ToEnumerable());
            return response;
        }

        protected HttpResponseMessage Logout()
        {
            var response = Request.CreateResponse(HttpStatusCode.NoContent);
            if (response == null)
            {
                throw new InvalidOperationException("Request.CreateResponse returned null");
            }
            CookieHelper.Remove(response, Cookies.StaffCookieName);
            return response;
        }
    }
}
