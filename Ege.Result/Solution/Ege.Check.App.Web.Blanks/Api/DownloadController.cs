namespace Ege.Check.App.Web.Blanks.Api
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Ege.Check.App.Web.Blanks.Esrp;
    using Ege.Check.App.Web.Common.Controllers;
    using Ege.Check.App.Web.Common.Filters;
    using Ege.Hsc.Logic.Blanks;
    using Ege.Hsc.Logic.Requests;
    using JetBrains.Annotations;

    [RoutePrefix("api/downloads")]
    public class DownloadController : FileControllerBase
    {
        [NotNull]private readonly IFilePathHelper _filePathHelper;
        [NotNull] private readonly IRequestService _requestService;
        [NotNull] private readonly IBlankZipper _zipper;
        [NotNull] private readonly IUserReferenceCreator _userReferenceCreator;

        public DownloadController(
            [NotNull]IFilePathHelper filePathHelper, 
            [NotNull]IRequestService requestService, 
            [NotNull]IBlankZipper zipper, 
            [NotNull]IUserReferenceCreator userReferenceCreator)
        {
            _filePathHelper = filePathHelper;
            _requestService = requestService;
            _zipper = zipper;
            _userReferenceCreator = userReferenceCreator;
        }

        [Route("")]
        [HttpGet]
        [StaffAuthenticationFilter]
        [EsrpAuthorize]
        public async Task<HttpResponseMessage> GetDownloads(int take = 10, int skip = 0)
        {
            var user = _userReferenceCreator.Create(User, true);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            if (skip < 0 || take <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var result = await _requestService.GetRequests(user, skip, take);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("{id:guid}")]
        [HttpGet]
        [StaffAuthenticationFilter]
        [EsrpAuthorize]
        public async Task<HttpResponseMessage> Download(Guid id)
        {
            var user = _userReferenceCreator.Create(User, true);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            var permissions = await _requestService.IsRequestOwner(id, user);
            if (!permissions.MultiRequestPermission && !permissions.SingleRequestPermission)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            var path = _filePathHelper.GetZipPath(id);
            if (!File.Exists(path))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            var file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return ZipFileResponse(file, permissions.MultiRequestPermission ? "blanks.zip" : _zipper.GetFioFilename(file));
        }
    }
}
