using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GVUZ.Web.Helpers
{
    public static class ExportHelper
    {
        private const string QuotPattern = "(?<quot>\"+)";
        private static readonly Regex QuotExpr = new Regex(QuotPattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);

        public static string EscapeCsvField(this string raw, bool useQuotesIfEmpty = false)
        {
            const string quotedFormat = "\"{0}\"";
            const string emptyQuoted = "\"\"";

            if (string.IsNullOrEmpty(raw))
            {
                return useQuotesIfEmpty ? emptyQuoted : string.Empty;
            }

            raw = raw.Replace(Environment.NewLine, " ").Replace("\n", " ").Replace("\r", " ").Trim();

            var m = QuotExpr.Matches(raw);
            List<char> line = new List<char>(raw.ToCharArray());
            List<int> insertPos = new List<int>();

            foreach (Match match in m)
            {
                if (match.Success)
                {
                    Group group = match.Groups["quot"];
                    if (@group != null && group.Value.Length % 2 != 0)
                    {
                        if (group.Captures.Count == 1)
                        {
                            insertPos.Add(group.Captures[0].Index);
                        }
                    }
                }
            }

            int offset = 0;
            foreach (int pos in insertPos)
            {
                line.Insert(pos + (++offset), '"');
            }

            raw = new string(line.ToArray());

            return string.Format(quotedFormat, raw);
        }
    }
}