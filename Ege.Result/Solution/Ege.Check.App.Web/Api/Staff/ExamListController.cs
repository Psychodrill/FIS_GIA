namespace Ege.Check.App.Web.Api.Staff
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Ege.Check.Logic.Services.Staff.Exams;
    using JetBrains.Annotations;

    [RoutePrefix("api/staff/examlist")]
    public class ExamListController : ApiController
    {
        [NotNull] private readonly ISubjectExamService _service;

        public ExamListController([NotNull] ISubjectExamService service)
        {
            _service = service;
        }

        [Route("")]
        [HttpGet]
        public HttpResponseMessage GetExamList()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _service.GetAllExams());
        }
    }
}