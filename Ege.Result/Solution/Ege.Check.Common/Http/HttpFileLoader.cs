namespace Ege.Check.Common.Http
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using global::Common.Logging;
    using JetBrains.Annotations;

    class HttpFileLoader : IHttpFileLoader
    {
        [NotNull]
        private static readonly ILog Logger = LogManager.GetLogger<HttpFileLoader>();

        public async Task<T> Load<T>(string url, Func<Stream, T> parser, string errorMessage)
        {
            var result = default(T);
            try
            {
                using (var httpClient = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                using (var response = httpClient.SendAsync(request).Result)
                {
                    if (response != null && response.Content != null && response.IsSuccessStatusCode)
                    {
                        using (var responseStream = await response.Content.ReadAsStreamAsync())
                        {
                            result = parser(responseStream);
                        }
                    }
                    else
                    {
                        Logger.InfoFormat(errorMessage,
                            url, response != null ? response.StatusCode : (HttpStatusCode?)null);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat(errorMessage, ex, new object[] { url, "(exception)" });
            }
            return result;
        }
    }
}
