namespace Ege.Check.App.Web.Api.Staff
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Ege.Check.App.Web.Common.Filters;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Logic.Services.Participant;
    using JetBrains.Annotations;

    [RoutePrefix("api/staff")]
    [StaffAuthenticationFilter]
    [Authorize(Roles = "fct")]
    public class AdminController : ApiController
    {
        [NotNull] private readonly ICacheFailureHelper _failureHelper;
        [NotNull] private readonly IMemoryCacheService _memoryCacheService;

        public AdminController(
            [NotNull] ICacheFailureHelper failureHelper,
            [NotNull] IMemoryCacheService memoryCacheService)
        {
            _failureHelper = failureHelper;
            _memoryCacheService = memoryCacheService;
        }

        [Route("cache/up")]
        [HttpGet]
        public HttpResponseMessage SetCacheIsNotFailed()
        {
            _failureHelper.Up();
            return GetCacheState();
        }

        [Route("cache/down")]
        [HttpGet]
        public HttpResponseMessage SetCacheIsFailed()
        {
            _failureHelper.Down();
            return GetCacheState();
        }

        [Route("cache/noget")]
        [HttpGet]
        public HttpResponseMessage SetCacheGetProhibited()
        {
            _failureHelper.ProhibitGet();
            return GetCacheState();
        }

        [Route("cache")]
        [HttpGet]
        public HttpResponseMessage GetCacheState()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    IsFailed = _failureHelper.IsCacheFailed(),
                    LastFailure = _failureHelper.GetLastFailureTime(),
                    IsGetProhibited = _failureHelper.IsGetProhibited(),
                    LastMemoryRefresh = _memoryCacheService.GetLastRefreshTime().Key,
                    IsLastMemoryRefreshSuccessful = _memoryCacheService.GetLastRefreshTime().Value,
                });
        }

        [Route("refreshmemory")]
        [HttpGet]
        public async Task<HttpResponseMessage> RefreshMemoryCache()
        {
            await _memoryCacheService.RefreshMemoryCache();
            return GetCacheState();
        }
    }
}