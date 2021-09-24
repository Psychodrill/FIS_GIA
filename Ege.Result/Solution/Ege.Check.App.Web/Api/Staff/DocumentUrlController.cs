namespace Ege.Check.App.Web.Api.Staff
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Ege.Check.App.Web.Common.Filters;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Check.Logic.Services.Staff.Documents;
    using JetBrains.Annotations;

    [RoutePrefix("api/staff/documentUrls")]
    [StaffAuthenticationFilter]
    [Authorize(Roles = "fct,rcoi")]
    public class DocumentUrlController : ApiController
    {
        [NotNull] private readonly IDocumentUrlService _service;

        public DocumentUrlController([NotNull] IDocumentUrlService service)
        {
            _service = service;
        }

        [Route("")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetDocuments()
        {
            return Request.CreateResponse(HttpStatusCode.OK, await _service.GetAllDocuments());
        }

        [Route("")]
        [HttpPost]
        [Authorize(Roles = "fct")]
        public async Task<HttpResponseMessage> SetDocuments([FromBody] DocumentUrlsCollection documents)
        {
            if (documents == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            await _service.UpdateDocuments(documents);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}