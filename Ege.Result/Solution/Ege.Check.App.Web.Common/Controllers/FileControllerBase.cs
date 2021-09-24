namespace Ege.Check.App.Web.Common.Controllers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Http;
    using JetBrains.Annotations;

    public class FileControllerBase : ApiController
    {
        protected HttpResponseMessage FileResponse([NotNull] Stream stream, string fileName, string mimeType)
        {
            stream.Position = 0;
            var result = Request.CreateResponse(HttpStatusCode.OK);
            if (result == null)
            {
                throw new InvalidOperationException("Request.CreateResponse returned null");
            }
            result.Content = new StreamContent(stream);
            if (result.Content.Headers == null)
            {
                throw new InvalidOperationException("Request.Content.Headers is null");
            }
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(mimeType)
            {
                CharSet = "UTF-8"
            };
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileNameStar = fileName,
                Size = stream.Length,
            };
            result.Content.Headers.ContentLength = stream.Length;
            return result;
        }

        protected HttpResponseMessage ZipFileResponse([NotNull] Stream stream, string fileName)
        {
            return FileResponse(stream, fileName, "application/zip");
        }

        protected HttpResponseMessage ExcelFileResponse([NotNull] Stream stream, string fileName)
        {
            return FileResponse(stream, fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}