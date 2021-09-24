namespace Ege.Check.App.Web.Api.Participant
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    [RoutePrefix("api/region")]
    public class RegionController : ApiController
    {
        [Route("")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetAvailableRegions()
        {
            return Request.CreateResponse(HttpStatusCode.OK, Services.RegionService.GetAvailableRegions());
        }
    }
}