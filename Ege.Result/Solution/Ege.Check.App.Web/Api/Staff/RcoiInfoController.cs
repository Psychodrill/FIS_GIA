namespace Ege.Check.App.Web.Api.Staff
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Ege.Check.App.Web.Common.Auth;
    using Ege.Check.App.Web.Common.Filters;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Check.Logic.Services.Staff.Rcoi;
    using JetBrains.Annotations;

    [RoutePrefix("api/staff/regionInfo")]
    [StaffAuthenticationFilter]
    [Authorize(Roles = "fct,rcoi")]
    public class RcoiInfoController : ApiController
    {
        [NotNull] private readonly IRcoiInfoService _service;

        public RcoiInfoController([NotNull] IRcoiInfoService service)
        {
            _service = service;
        }


        [Route("")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetInfoForMyRegion()
        {
            var user = User as IStaffPrincipal;
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            if (!user.User.RegionId.HasValue)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            return Request.CreateResponse(HttpStatusCode.OK,
                                          await _service.GetRcoiInfoByRegion(user.User.RegionId.Value));
        }

        [Route("")]
        [HttpPost]
        public async Task<HttpResponseMessage> SetSettingsForMyRegion([FromBody] RcoiInfo info)
        {
            if (info == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var user = User as IStaffPrincipal;
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            if (!user.User.RegionId.HasValue)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            await _service.UpdateRcoiInfo(user.User.RegionId.Value, info);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [Route("{regionId:int}")]
        [HttpGet]
        [Authorize(Roles = "fct")]
        public async Task<HttpResponseMessage> GetRegionInfo(int regionId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, await _service.GetRcoiInfoByRegion(regionId));
        }

        [Route("{regionId:int}")]
        [HttpPost]
        [Authorize(Roles = "fct")]
        public async Task<HttpResponseMessage> SetRegion(int regionId, [FromBody] RcoiInfo info)
        {
            if (info == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            await _service.UpdateRcoiInfo(regionId, info);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}