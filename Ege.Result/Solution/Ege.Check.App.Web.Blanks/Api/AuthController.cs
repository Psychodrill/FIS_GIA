namespace Ege.Check.App.Web.Blanks.Api
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Ege.Check.App.Web.Common.Auth;
    using Ege.Check.App.Web.Common.Filters;
    using Ege.Check.Logic.Models.Requests;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Check.Logic.Services.Staff.Users;
    using JetBrains.Annotations;

    [RoutePrefix("api/auth")]
    public class AuthController : AuthControllerBase
    {
        private readonly Role[] _allowedRoles = {Role.Fct, Role.HscOperator};

        public AuthController(
            [NotNull] IUserService userService, 
            [NotNull] IAuthCookieCreator authCookieCreator)
            : base(userService, authCookieCreator)
        {
        }

        [Route("login")]
        [HttpPost]
        public Task<HttpResponseMessage> Login([FromBody] StaffLoginRequest request)
        {
            return Login(request, _allowedRoles);
        }

        [Route("logout")]
        [HttpPost]
        [StaffAuthenticationFilter]
        [Authorize]
        public new HttpResponseMessage Logout()
        {
            return base.Logout();
        }
    }
}
