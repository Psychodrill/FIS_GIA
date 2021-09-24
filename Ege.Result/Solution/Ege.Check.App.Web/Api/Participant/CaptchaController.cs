namespace Ege.Check.App.Web.Api.Participant
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix("api/captcha")]
    public class CaptchaController : ApiController
    {
        [Route("")]
        public async Task<HttpResponseMessage> Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, await Services.CaptchaRetriever.Retrieve());
        }
    }
}
