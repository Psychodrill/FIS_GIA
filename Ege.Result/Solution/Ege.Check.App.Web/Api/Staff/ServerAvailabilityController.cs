namespace Ege.Check.App.Web.Api.Staff
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Ege.Check.App.Web.Common.Controllers;
    using Ege.Check.App.Web.Common.Filters;
    using Ege.Hsc.Logic.Servers;
    using JetBrains.Annotations;

    [RoutePrefix("api/servers")]
    [StaffAuthenticationFilter]
    [Authorize(Roles="fct,fctoperator")]
    public class ServerAvailabilityController : FileControllerBase
    {
        [NotNull] private readonly IServerService _serverService;
        [NotNull] private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1);

        public ServerAvailabilityController([NotNull]IServerService serverService)
        {
            _serverService = serverService;
        }

        [Route("")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAvailabilityStatus()
        {
            var result = await _serverService.GetStatuses();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("{regionId:int}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAvailabilityStatus(int regionId)
        {
            var result = await _serverService.GetStatuses(regionId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("{regionId:int}/details")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetStatusDetailsForRegion(int regionId)
        {
            var stream = await _serverService.GenerateErrorsFile(regionId);
            if (stream == null)
            {
                return Request.CreateResponse(
                    HttpStatusCode.InternalServerError, "Null stream generated instead of excel");
            }
            return ExcelFileResponse(stream, string.Format("errors{0}.xlsx", regionId));
        }

        private async Task<T> RunIfNotLocked<T>([NotNull]Func<Task<T>> func, [NotNull]Func<T> defaultValue)
        {
            bool lockTaken = false;
            try
            {
                lockTaken = await Semaphore.WaitAsync(0);
                if (lockTaken)
                {
                    var task = func();
                    return task != null ? await task : defaultValue();
                }
            }
            finally
            {
                if (lockTaken)
                {
                    Semaphore.Release();
                }
            }
            return defaultValue();
        }

        [Route("{regionId:int}/check")]
        [HttpPost]
        public async Task<HttpResponseMessage> CheckRegion(int regionId)
        {
            return await RunIfNotLocked(
                async () =>
                {
                    var result = await _serverService.CheckStatus(regionId);
                    if (result == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound); // нет данных о регионе
                    }
                    return Request.CreateResponse(
                        HttpStatusCode.OK,
                        result);
                }, () => Request.CreateResponse(HttpStatusCode.Conflict));
        }

        [Route("checkall")]
        [HttpGet]
        public async Task<HttpResponseMessage> CheckAllRegions()
        {
            var fireAndForgetTask = RunIfNotLocked(
                async () =>
                {
                    await _serverService.CheckAllStatuses();
                    return true;
                }, () => false);
            if (fireAndForgetTask.IsCompleted && !fireAndForgetTask.Result)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict);
            }
            return await GetAvailabilityStatus();
        }
    }
}
