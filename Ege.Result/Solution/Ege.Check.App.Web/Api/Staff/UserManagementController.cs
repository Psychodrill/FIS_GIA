namespace Ege.Check.App.Web.Api.Staff
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Ege.Check.App.Web.Common.Filters;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Check.Logic.Services.Staff.Users;
    using JetBrains.Annotations;

    [StaffAuthenticationFilter]
    [Authorize(Roles = "fct")]
    [RoutePrefix("api/staff/user")]
    public class UserManagementController : ApiController
    {
        [NotNull] private readonly IUserService _userService;

        public UserManagementController([NotNull] IUserService userService)
        {
            _userService = userService;
        }

        [Route("")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateUser([FromBody] UserDto user)
        {
            if (user == null || user.Login == null || user.Password == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            await _userService.Create(user);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [Route("{id:int}")]
        [HttpPost]
        public async Task<HttpResponseMessage> EditUser(int id, [FromBody] UserDto user)
        {
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            user.Id = id;
            await _userService.Update(user);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [Route("")]
        [HttpGet]
        public async Task<HttpResponseMessage> Get(int take = 10, int skip = 0, string login = null, int? regionId = null,
                                                   Role? role = null)
        {
            if (take <= 0 || skip < 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return Request.CreateResponse(HttpStatusCode.OK, await _userService.Get(login, regionId, role, take, skip));
        }

        [Route("{id:int}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetById(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, await _userService.GetDtoById(id));
        }

        [Route("{id:int}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, await _userService.Delete(id));
        }
    }
}