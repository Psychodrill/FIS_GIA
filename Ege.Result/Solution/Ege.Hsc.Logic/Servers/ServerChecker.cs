namespace Ege.Hsc.Logic.Servers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Common.Logging;
    using Ege.Check.Common.Http;
    using Ege.Check.Dal.Blanks;
    using Ege.Hsc.Logic.Models.Servers;
    using JetBrains.Annotations;

    class ServerChecker : IServerChecker
    {
        [NotNull]private readonly IBlankModelCreator _blankModelCreator;
        [NotNull] private readonly IServerFileParser _serverFileParser;
        [NotNull] private readonly IHttpFileLoader _httpFileLoader;

        [NotNull] private static readonly ILog Logger = LogManager.GetLogger<ServerChecker>();

        public ServerChecker(
            [NotNull]IBlankModelCreator blankModelCreator, 
            [NotNull]IServerFileParser serverFileParser, 
            [NotNull]IHttpFileLoader httpFileLoader)
        {
            _blankModelCreator = blankModelCreator;
            _serverFileParser = serverFileParser;
            _httpFileLoader = httpFileLoader;
        }

        public async Task<KeyValuePair<int, bool>> CheckAvailability(BlankServerAvailabilityModel server)
        {
            if (string.IsNullOrWhiteSpace(server.Url) || server.Url == null)
            {
                return new KeyValuePair<int, bool>(server.RegionId, false);
            }
            var url = _blankModelCreator.GetListFileUrl(server.Url, server.ExamDate, server.SubjectCode);
            try
            {
                using (var httpClient = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Head, url))
                using (var response = await httpClient.SendAsync(request))
                {
                    return new KeyValuePair<int, bool>(server.RegionId, response != null && response.IsSuccessStatusCode);
                }
            }
            catch (Exception ex)
            {
                Logger.InfoFormat("Error while checking availability of {0} : {1}", url, ex);
                return new KeyValuePair<int, bool>(server.RegionId, false);
            }
        }

        public async Task<ServerErrors> CheckFile(string serverUrl, KeyValuePair<ExamFolder, ISet<string>> serverBlanks)
        {
            Logger.InfoFormat("Checking {0}", serverUrl);
            if (string.IsNullOrWhiteSpace(serverUrl) || serverUrl == null)
            {
                throw new ArgumentException("serverUrl is invalid");
            }
            var dbBlanks = serverBlanks.Value ?? new HashSet<string>();
            var url = _blankModelCreator.GetListFileUrl(
                serverUrl, serverBlanks.Key.ExamDate, serverBlanks.Key.SubjectCode);
            var filesOnServer = await _httpFileLoader.Load(url, _serverFileParser.GetCodes, 
                "Error while checking contents file {0} : HTTP status code {1}. Assuming there are no files in the folder");
            bool success = filesOnServer != null;
            filesOnServer = filesOnServer ?? new HashSet<string>();
            var count = filesOnServer.Count;
            filesOnServer.SymmetricExceptWith(dbBlanks);    // filesOnServer = all errors
            dbBlanks.IntersectWith(filesOnServer);          // select errors from dbBlanks
            filesOnServer.ExceptWith(dbBlanks);             // delete dbBlanks errors from filesOnServer
            Logger.InfoFormat("Checked {0}", serverUrl);
            return new ServerErrors
            {
                FileRead = success,
                Count = count,
                ExamDate = serverBlanks.Key.ExamDate,
                Extra = filesOnServer,
                Missing = dbBlanks,
            };
        }
    }
}
