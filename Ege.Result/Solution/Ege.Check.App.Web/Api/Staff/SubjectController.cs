namespace Ege.Check.App.Web.Api.Staff
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Ege.Check.App.Web.Common.Filters;
    using Ege.Check.Logic.Services.Staff.Exams;
    using JetBrains.Annotations;

    [RoutePrefix("api/staff/subject")]
    [StaffAuthenticationFilter]
    [Authorize(Roles = "fct")]
    public class SubjectController : ApiController
    {
        [NotNull] private readonly ISubjectExamService _service;

        public SubjectController([NotNull] ISubjectExamService service)
        {
            _service = service;
        }

        [Route("")]
        [HttpGet]
        public HttpResponseMessage GetAll()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _service.GetAllSubjects());
        }
    }
}