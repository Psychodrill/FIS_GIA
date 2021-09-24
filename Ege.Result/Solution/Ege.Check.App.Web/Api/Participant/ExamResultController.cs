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

    [RoutePrefix("api/exam")]
    [Authorize]
    [ParticipantAuthenticationFilter]
    public class ExamResultController : ApiController
    {
        [Route("")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAll()
        {
            var principal = User as ParticipantPrincipal;
            if (principal == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            var results = await Services.ExamListService.GetExamList(principal.Participant);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [Route("{examId:int}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetDetails(int examId)
        {
            var principal = User as ParticipantPrincipal;
            if (principal == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            var details = await Services.ExamDetailsService.GetExamDetails(new KeyValuePair<ParticipantCacheModel, int>(principal.Participant, examId));
            if (details == null)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            return Request.CreateResponse(HttpStatusCode.OK, details);
        }
    }
}
