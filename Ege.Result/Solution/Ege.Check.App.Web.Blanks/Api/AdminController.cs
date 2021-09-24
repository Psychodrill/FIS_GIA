namespace Ege.Check.App.Web.Blanks.Api
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Ege.Check.App.Web.Common.Filters;
    using Ege.Check.Logic.BlankServers;
    using Ege.Check.Logic.Services.Participant;
    using Ege.Hsc.Logic.Blanks;
    using Ege.Hsc.Logic.Servers;
    using JetBrains.Annotations;

    [RoutePrefix("api/admin")]
    [StaffAuthenticationFilter]
    [Authorize(Roles="fct")]
    public class AdminController : ApiController
    {
        [NotNull] private readonly IBlankService _blankService;
        [NotNull] private readonly IServerService _serverService;
        [NotNull] private readonly IServerPageCountService _serverPageCountService;
        [NotNull] private readonly IMemoryCacheService _memoryCacheService;
        [NotNull] private readonly IInvalidPngRemover _invalidPngRemover;

        public AdminController(
            [NotNull]IBlankService blankService, 
            [NotNull]IServerService serverService, 
            [NotNull]IServerPageCountService serverPageCountService, 
            [NotNull]IMemoryCacheService memoryCacheService, 
            [NotNull]IInvalidPngRemover invalidPngRemover)
        {
            _blankService = blankService;
            _serverService = serverService;
            _serverPageCountService = serverPageCountService;
            _memoryCacheService = memoryCacheService;
            _invalidPngRemover = invalidPngRemover;
        }

        [Route("loadblanks")]
        [HttpGet]
        public async Task<HttpResponseMessage> LoadBlanks()
        {
            await _serverService.LoadServersFromCheckEgeDb();
            await _blankService.LoadBlanksFromCheckEgeDb();
            return Request.CreateResponse(HttpStatusCode.OK, "OK");
        }

        [Route("checkavailability")]
        [HttpGet]
        public async Task<HttpResponseMessage> CheckServerAvailability()
        {
            await _serverService.CheckServersAvailability();
            return Request.CreateResponse(HttpStatusCode.OK, "OK");
        }

        [Route("loadcompositionpagecount")]
        [HttpGet]
        public async Task<HttpResponseMessage> LoadCompositionPageCount()
        {
            await _memoryCacheService.RefreshMemoryCache(
                refreshSubjectsAndExams: true,
                refreshRegionSettings: true,
                refreshAvailableRegions: false,
                refreshAnswerCriteria: false,
                refreshCancelledExams: false);
            await _serverPageCountService.LoadPageCount();
            return Request.CreateResponse(HttpStatusCode.OK, "OK");
        }

        [Route("loadpagescount")]
        [HttpGet]
        public async Task<HttpResponseMessage> LoadPagesCount()
        {
            await _memoryCacheService.RefreshMemoryCache(
                refreshSubjectsAndExams: true,
                refreshRegionSettings: true,
                refreshAvailableRegions: false,
                refreshAnswerCriteria: false,
                refreshCancelledExams: false);
            await _serverPageCountService.LoadPageCountIntoSpecialTable();
            return Request.CreateResponse(HttpStatusCode.OK, "OK");
        }

        [Route("fixpagecountinconsistencies")]
        [HttpGet]
        public async Task<HttpResponseMessage> FixPageCountInconsistencies()
        {
            await _blankService.FixInconsistenciesWithCheckEgeDb();
            return Request.CreateResponse(HttpStatusCode.OK, "OK");
        }

        [Route("checkinvalidpngs")]
        [HttpGet]
        public async Task<HttpResponseMessage> CheckInvalidPngs()
        {
            await _invalidPngRemover.RemoveInvalidPngsFromStorage(false);
            return Request.CreateResponse(HttpStatusCode.OK, "OK");
        }

        [Route("removeinvalidpngs")]
        [HttpGet]
        public async Task<HttpResponseMessage> RemoveInvalidPngs()
        {
            await _invalidPngRemover.RemoveInvalidPngsFromStorage();
            return Request.CreateResponse(HttpStatusCode.OK, "OK");
        }
    }
}
