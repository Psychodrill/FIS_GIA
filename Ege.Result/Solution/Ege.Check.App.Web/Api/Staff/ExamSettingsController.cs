namespace Ege.Check.App.Web.Api.Staff
{
    using System.Data.SqlTypes;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Ege.Check.App.Web.Common.Auth;
    using Ege.Check.App.Web.Common.Filters;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Check.Logic.Services.Dtos.Enums;
    using Ege.Check.Logic.Services.Staff.Settings;
    using JetBrains.Annotations;

    [RoutePrefix("api/staff/examsettings")]
    [StaffAuthenticationFilter]
    [Authorize(Roles = "fct,rcoi")]
    public class ExamSettingsController : ApiController
    {
        [NotNull] private readonly IExamSettingService _service;

        public ExamSettingsController([NotNull] IExamSettingService service)
        {
            _service = service;
        }

        [Route("")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetSettingsForMyRegion([FromUri] ExamWave wave)
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
                                          await _service.GetSettingsByRegion(user.User.RegionId.Value, wave));
        }

        [Route("")]
        [HttpPost]
        public async Task<HttpResponseMessage> SetSettingsForMyRegion([FromBody] ExamSettings settings)
        {
            if (settings == null || settings.Settings == null)
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
            await _service.SetSettings(user.User.RegionId.Value, settings);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [Route("{regionId:int}")]
        [HttpGet]
        [Authorize(Roles = "fct")]
        public async Task<HttpResponseMessage> GetSettings(int regionId, [FromUri] ExamWave wave)
        {
            return Request.CreateResponse(HttpStatusCode.OK, await _service.GetSettingsByRegion(regionId, wave));
        }

        [Route("{regionId:int}")]
        [HttpPost]
        [Authorize(Roles = "fct")]
        public async Task<HttpResponseMessage> SetSettings(int regionId, [FromBody] ExamSettings settings)
        {
            if (settings == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            await _service.SetSettings(regionId, settings);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [Route("gek/{examId:int}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetGekDocument(int examId)
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
                                          await _service.GetGekDocument(user.User.RegionId.Value, examId));
        }

        [Route("gek/{examId:int}/{regionId:int}")]
        [HttpGet]
        [Authorize(Roles = "fct")]
        public async Task<HttpResponseMessage> GetGekDocument(int examId, int regionId)
        {
            return Request.CreateResponse(HttpStatusCode.OK,
                                          await _service.GetGekDocument(regionId, examId));
        }

        [Route("gek/{examId:int}")]
        [HttpPost]
        public async Task<HttpResponseMessage> EditGekDocument(int examId, [FromBody] GekDocument document)
        {
            if (document == null || document.CreateDate < SqlDateTime.MinValue ||
                document.CreateDate > SqlDateTime.MaxValue)
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
            await _service.UpdateGekDocument(user.User.RegionId.Value, examId, document);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [Route("gek/{examId:int}/{regionId:int}")]
        [HttpPost]
        [Authorize(Roles = "fct")]
        public async Task<HttpResponseMessage> EditGekDocument(int examId, int regionId, [FromBody] GekDocument document)
        {
            if (document == null || document.CreateDate < SqlDateTime.MinValue ||
                document.CreateDate > SqlDateTime.MaxValue)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            await _service.UpdateGekDocument(regionId, examId, document);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [Route("gek/{examId:int}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteGekDocument(int examId)
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
            await _service.DeleteGekDocument(user.User.RegionId.Value, examId);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [Route("gek/{examId:int}/{regionId:int}")]
        [HttpDelete]
        [Authorize(Roles = "fct")]
        public async Task<HttpResponseMessage> DeleteGekDocument(int examId, int regionId)
        {
            await _service.DeleteGekDocument(regionId, examId);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}