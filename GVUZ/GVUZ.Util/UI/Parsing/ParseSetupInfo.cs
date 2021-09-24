using System;
using System.Globalization;
using System.Linq;
using GVUZ.Util.Services.Parser;

namespace GVUZ.Util.UI.Parsing
{
    public class ParseSetupInfo
    {
        public ParseSetupInfo()
        {
            Properties.Settings.Default.Reload();

            Filter = new ParseImportPackageFilter();
            if (Properties.Settings.Default.parseMinDateChecked)
            {
                Filter.MinDate = Properties.Settings.Default.parseMinDate;
            }
            if (Properties.Settings.Default.parseFilterInstitution)
            {
                string instIdList = Properties.Settings.Default.parseInstitutionId;
                
                if (!string.IsNullOrEmpty(instIdList))
                {
                    Filter.InstitutionId = instIdList.Replace(" ", string.Empty)
                              .Replace(Environment.NewLine, string.Empty)
                              .Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                              .Select(ParseId).Where(id => id > 0).ToArray();
                }
            }

            TruncateTable = Properties.Settings.Default.parseTruncateTable;
            ConnectionString = Properties.Settings.Default.parseConnectionString;
        }

        private static int ParseId(string idString)
        {
            int id;
            if (int.TryParse(idString, NumberStyles.Integer, CultureInfo.InvariantCulture, out id))
            {
                return id;
            }

            return 0;
        }

        public ParseImportPackageFilter Filter { get; set; }
        public string ConnectionString { get; set; }
        public bool TruncateTable { get; set; }
    }
}