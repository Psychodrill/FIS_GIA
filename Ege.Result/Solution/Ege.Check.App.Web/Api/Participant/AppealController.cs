namespace Ege.Check.App.Web.Api.Participant
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Ege.Check.App.Web.Auth;
    using Ege.Check.App.Web.Filters;
    using Ege.Check.Logic.Models.Cache;

    [RoutePrefix("api/appeal")]
    [Authorize]
    [ParticipantAuthenticationFilter]
    public class AppealController : ApiController
    {
        [Route("{examId:int}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAppeals(int examId)
        {
            var principal = User as ParticipantPrincipal;
            if (principal == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            var appeals = await Services.AppealService.GetAppeals(
                new KeyValuePair<ParticipantCacheModel, int>(principal.Participant, examId));
            return Request.CreateResponse(HttpStatusCode.OK, appeals);
        }
    }
}