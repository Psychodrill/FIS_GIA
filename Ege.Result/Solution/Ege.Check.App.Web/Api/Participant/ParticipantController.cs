using Common.Logging;
using Ege.Check.Captcha;
using Ege.Check.Logic.Models.Cache;
using Ege.Check.Logic.Services.Participant.Participants;
using JetBrains.Annotations;

namespace Ege.Check.App.Web.Api.Participant
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Ege.Check.App.Web.Common.Auth;
    using Ege.Check.App.Web.Filters;
    using Ege.Check.App.Web.Models.Requests;
    using Ege.Check.Common.Extensions;

    [RoutePrefix("api/participant")]
    public class ParticipantController : ApiController
    {
        [NotNull] private static readonly ILog Logger = LogManager.GetLogger<ParticipantController>();

        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Login([FromBody] ParticipantLoginRequest request)
        {
            if (request == null || request.Hash == null || request.Hash.Length != 32 ||
                !(request.Code == null ^ request.Document == null))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Пожалуйста, проверьте правильность введённых данных");
            }

            if (Services.CaptchaTokenHelper.CheckCaptcha)
            {
                var t = await Services.RecaptchaService.Validate(request.reCaptureToken);

                if (!t.Success)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, string.Join(",", t.ErrorCodes));
                }
            }
            //if (!Services.CaptchaTokenHelper.IsCorrect(request.Captcha, request.Token))
            //{
            //    return Request.CreateResponse(HttpStatusCode.BadRequest, "Пожалуйста, проверьте правильность введённого кода с картинки");
            //}

            if (request.Document != null)
            {
                int len = request.Document.Length;
                for (int i = 0; i < (12 - len); i++)
                {
                    request.Document = "0" + request.Document;
                }
            }

           // var participant = new ParticipantServiceResult();
           // participant.Participant = new ParticipantCacheModel();

            var participant = await Services.ParticipantService.Get(
                request.Hash, request.Code, request.Document, request.Region);
            if (participant == null || participant.Participant == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Участник не найден");
            }
            if (participant.Collision)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict,
                                              "Найдено несколько участников " +
                                              "с совпадающими данными. " +
                                              "Пожалуйста, обратитесь " +
                                              "в региональный центр обработки информации " +
                                              "вашего региона или на официальный сайт " +
                                              "РЦОИ вашего региона");
            }
            var cookie = Services.AuthCookieCreator.CreateParticipantCookie(participant.Participant);
            var response = Request.CreateResponse(HttpStatusCode.NoContent);
            //;//.NoContent);
            response.Headers.AddCookies(cookie.ToEnumerable());
            return response;
        }

        [Route("logout")]
        [HttpPost]
        [Authorize]
        [ParticipantAuthenticationFilter]
        public HttpResponseMessage Logout()
        {
            var response = Request.CreateResponse(HttpStatusCode.NoContent);
            CookieHelper.Remove(response, Cookies.ParticipantCookieName);
            return response;
        }
    }
}