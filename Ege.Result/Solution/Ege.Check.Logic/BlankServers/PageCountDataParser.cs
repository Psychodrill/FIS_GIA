namespace Ege.Check.Logic.BlankServers
{
    using System;
    using System.Text.RegularExpressions;
    using Ege.Check.Logic.Models.Servers;
    using JetBrains.Annotations;

    class PageCountDataParser : IPageCountDataParser
    {
        [NotNull]
        private readonly Regex _pageCountDataRegex = new Regex(@"^(\d*)_(\d*)_([^:]*):(\d*)$", RegexOptions.Compiled);

        public PageCountData Parse(string line)
        {
            const int barcodeGroup = 1;
            const int projectIdGroup = 2;
            const int projectNameGroup = 3;
            const int pageCountGroup = 4;

            var match = _pageCountDataRegex.Match(line);
            if (!match.Success)
            {
                throw new InvalidOperationException(string.Format("Invalid page count data string {0}", line));
            }
            int projectId;
            if (!int.TryParse(match.Groups[projectIdGroup].Value, out projectId))
            {
                throw new InvalidOperationException(string.Format("Invalid project batch id {0} in page count data string {1}", match.Groups[projectIdGroup].Value, line));
            }
            int pageCount;
            if (!int.TryParse(match.Groups[pageCountGroup].Value, out pageCount))
            {
                throw new InvalidOperationException(string.Format("Invalid page count {0} in page count data string {1}", match.Groups[pageCountGroup].Value, line));
            }
            return new PageCountData
            {
                Barcode = match.Groups[barcodeGroup].Value,
                ProjectBatchId = projectId,
                ProjectName = match.Groups[projectNameGroup].Value,
                PageCount = pageCount,
            };
        }
    }
}
