namespace Ege.Check.App.Web.Blanks.Api
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Ege.Check.App.Web.Blanks.Esrp;
    using Ege.Check.App.Web.Common.Filters;
    using Ege.Check.Logic.Models.Requests;
    using Ege.Hsc.Logic.Configuration;
    using Ege.Hsc.Logic.Requests;
    using JetBrains.Annotations;

    [RoutePrefix("api/blanks")]
    public class BlankController : ApiController
    {
        [NotNull] private readonly IRequestService _requestService;
        [NotNull] private readonly IUserReferenceCreator _userReferenceCreator;
        [NotNull] private readonly IHscSettings _settings;

        public BlankController([NotNull]IRequestService requestService, [NotNull]IUserReferenceCreator userReferenceCreator, [NotNull]IHscSettings settings)
        {
            _requestService = requestService;
            _userReferenceCreator = userReferenceCreator;
            _settings = settings;
        }

        [Route("")]
        [HttpPost]
        [StaffAuthenticationFilter]
        [EsrpAuthorize]
        public async Task<HttpResponseMessage> RequestSingleParticipant(ParticipantBlankRequest request)
        {
            var user = _userReferenceCreator.Create(User, true);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            if (request == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var result = await _requestService.ProcessSingleParticipant(request, user);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("multi")]
        [HttpPost]
        [StaffAuthenticationFilter]
        [EsrpAuthorize]
        public async Task<HttpResponseMessage> UploadCsv()
        {
            var user = _userReferenceCreator.Create(User, _settings.CsvUploadAllowedForEsrp);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            if (Request == null || Request.Content == null || !Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            if (provider.Contents == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var formDataPart = provider.Contents.FirstOrDefault(c => !IsFilePart(c));
            var note = formDataPart != null ? await formDataPart.ReadAsStringAsync() : null;
            var requestId = await _requestService.CreateRequest(
                user, 
                note, 
                provider.Contents.Where(IsFilePart).Select(async file => await file.ReadAsStreamAsync()));
            return Request.CreateResponse(HttpStatusCode.OK, requestId);
        }

        private bool IsFilePart(HttpContent content)
        {
            return content != null && content.Headers != null && content.Headers.ContentDisposition != null
                   && !string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName);
        }
    }
}
