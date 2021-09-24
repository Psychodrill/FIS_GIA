namespace Ege.Check.Logic.BlankServers
{
    using System.Collections.Generic;
    using System.IO;
    using Ege.Check.Logic.Models.Servers;
    using JetBrains.Annotations;

    class PageCountFileParser : IPageCountFileParser
    {
        [NotNull]private readonly IPageCountDataParser _pageCountDataParser;

        public PageCountFileParser([NotNull]IPageCountDataParser pageCountDataParser)
        {
            _pageCountDataParser = pageCountDataParser;
        }

        public ICollection<PageCountData> GetPageCountData(Stream file)
        {
            var result = new List<PageCountData>();
            using (var reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        result.Add(_pageCountDataParser.Parse(line));
                    }
                }
            }
            return result;
        }
    }
}
