namespace Ege.Check.App.Web.Api.Staff
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Ege.Check.App.Web.Common.Auth;
    using Ege.Check.App.Web.Common.Filters;
    using Ege.Check.App.Web.Models.Requests;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Check.Logic.Services.Staff.ExamCancellation;
    using JetBrains.Annotations;

    [RoutePrefix("api/staff/cancel")]
    [StaffAuthenticationFilter]
    [Authorize(Roles = "fct,rcoi")]
    public class ExamCancellationController : ApiController
    {
        [NotNull] private readonly IExamCancellationService _service;

        public ExamCancellationController([NotNull] IExamCancellationService service)
        {
            _service = service;
        }

        [Route("")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetCancelled(int take = 10, int skip = 0)
        {
            const int maxTake = 20;
            if (take > maxTake || take <= 0 || skip < 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var principal = User as IStaffPrincipal;
            if (principal == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            return Request.CreateResponse(HttpStatusCode.OK, await _service.Get(principal.User.RegionId, take, skip));
        }

        [Route("")]
        [HttpPost]
        public async Task<HttpResponseMessage> Cancel([FromBody] ExamCancellationRequest request)
        {
            if (request == null || request.Code == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            if (!User.IsInRole(Role.Fct.ToString()))
            {
                request.RegionId = ((IStaffPrincipal) User).User.RegionId.Value;
            }
            return Request.CreateResponse(HttpStatusCode.OK,
                                          await _service.Cancel(request.Code, request.RegionId, request.ExamGlobalId));
        }

        [Route("undo")]
        [HttpPost]
        public async Task<HttpResponseMessage> Uncancel([FromBody] ExamCancellationRequest request)
        {
            if (request == null || request.Code == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            if (!User.IsInRole(Role.Fct.ToString()))
            {
                request.RegionId = ((IStaffPrincipal) User).User.RegionId.Value;
            }
            return Request.CreateResponse(HttpStatusCode.OK,
                                          await _service.Uncancel(request.Code, request.RegionId, request.ExamGlobalId));
        }
    }
}