namespace Ege.Check.App.Web.Api.Staff
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Ege.Check.App.Web.Common.Filters;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Services.Staff.TaskSettings;
    using JetBrains.Annotations;

    [RoutePrefix("api/staff/tasksettings")]
    [StaffAuthenticationFilter]
    [Authorize(Roles = "fct")]
    public class TaskSettingsController : ApiController
    {
        [NotNull] private readonly IAnswerCriteriaService _service;

        public TaskSettingsController([NotNull] IAnswerCriteriaService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{subjectCode:int}")]
        public async Task<HttpResponseMessage> Get(int subjectCode)
        {
            return Request.CreateResponse(HttpStatusCode.OK, await _service.GetTaskSettings(subjectCode));
        }

        [HttpPost]
        [Route("{subjectCode:int}")]
        public async Task<HttpResponseMessage> Set(int subjectCode, [FromBody] ExamInfoCacheModel settings)
        {
            if (settings == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            await _service.SetTaskSettings(subjectCode, settings);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}