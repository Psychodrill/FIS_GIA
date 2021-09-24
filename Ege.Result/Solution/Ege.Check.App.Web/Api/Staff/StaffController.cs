namespace Ege.Check.App.Web.Api.Staff
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Ege.Check.App.Web.Common.Auth;
    using Ege.Check.App.Web.Common.Filters;
    using Ege.Check.App.Web.Models.Requests;
    using Ege.Check.Logic.Models.Requests;
    using Ege.Check.Logic.Services.Staff.Users;
    using JetBrains.Annotations;

    [RoutePrefix("api/staff")]
    public class StaffController : AuthControllerBase
    {
        public StaffController(
            [NotNull] IUserService userService,
            [NotNull] IAuthCookieCreator authCookieCreator): base(userService, authCookieCreator)
        {
        }

        [Route("login")]
        [HttpPost]
        public Task<HttpResponseMessage> Login([FromBody] StaffLoginRequest request)
        {
            return base.Login(request);
        }

        [Route("logout")]
        [HttpPost]
        [StaffAuthenticationFilter]
        [Authorize]
        public new HttpResponseMessage Logout()
        {
            return base.Logout();
        }

        [Route("activate")]
        [HttpPost]
        [StaffAuthenticationFilter]
        [Authorize]
        public async Task<HttpResponseMessage> Activate()
        {
            var user = User as IStaffPrincipal;
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            await UserService.Activate(user.User.Id);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [Route("deactivate")]
        [HttpPost]
        [StaffAuthenticationFilter]
        [Authorize(Roles = "rcoi")]
        public async Task<HttpResponseMessage> Deactivate()
        {
            var user = User as IStaffPrincipal;
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            await UserService.Deactivate(user.User.Id);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [Route("setpassword")]
        [HttpPost]
        [StaffAuthenticationFilter]
        [Authorize]
        public async Task<HttpResponseMessage> SetPassword([FromBody] ResetPasswordRequest request)
        {
            if (request == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var user = User as IStaffPrincipal;
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            await UserService.SetPassword(user.User.Id, request.Password);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
