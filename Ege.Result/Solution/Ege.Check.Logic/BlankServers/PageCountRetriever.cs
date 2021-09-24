namespace Ege.Check.Logic.BlankServers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ege.Check.Common.Http;
    using Ege.Check.Dal.Blanks;
    using Ege.Check.Logic.Models.Servers;
    using JetBrains.Annotations;

    class PageCountRetriever : IPageCountRetriever
    {
        [NotNull]private readonly IBlankModelCreator _blankModelCreator;
        [NotNull]private readonly IHttpFileLoader _httpFileLoader;
        [NotNull] private readonly IPageCountFileParser _serverFileParser;

        public PageCountRetriever(
            [NotNull]IBlankModelCreator blankModelCreator, 
            [NotNull]IHttpFileLoader httpFileLoader, 
            [NotNull]IPageCountFileParser serverFileParser)
        {
            _blankModelCreator = blankModelCreator;
            _httpFileLoader = httpFileLoader;
            _serverFileParser = serverFileParser;
        }

        public async Task<ICollection<PageCountData>> GetPageCountData(string serverUrl, DateTime examDate, int subjectCode)
        {
            if (string.IsNullOrWhiteSpace(serverUrl))
            {
                throw new ArgumentException("server url is invalid");
            }
            var url = _blankModelCreator.GetPageCountFileUrl(serverUrl, examDate, subjectCode);
            var result = await _httpFileLoader.Load(url, _serverFileParser.GetPageCountData,
                "Error while checking page count file {0} : HTTP status code {1}");
            return result;
        }
    }
}