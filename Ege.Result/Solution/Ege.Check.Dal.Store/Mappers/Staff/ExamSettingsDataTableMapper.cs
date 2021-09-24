namespace Ege.Check.Dal.Store.Mappers.Staff
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Dal.Common.Mappers;

    internal class ExamSettingsDataTableMapper : IDataTableMapper<KeyValuePair<int, IEnumerable<ExamSetting>>>
    {
        private const string RegionIdColumn = "RegionId";
        private const string ExamGlobalIdColumn = "ExamGlobalId";
        private const string ShowResultColumn = "ShowResult";
        private const string ShowBlanksColumn = "ShowBlanks";

        public DataTable Map(KeyValuePair<int, IEnumerable<ExamSetting>> @from)
        {
            var result = new DataTable();
            result.Columns.Add(RegionIdColumn, typeof (int));
            result.Columns.Add(ExamGlobalIdColumn, typeof (int));
            result.Columns.Add(ShowResultColumn, typeof (bool));
            result.Columns.Add(ShowBlanksColumn, typeof (bool));
            if (from.Value == null)
            {
                return result;
            }
            object regionId = from.Key;
            foreach (var setting in from.Value ?? Enumerable.Empty<ExamSetting>())
            {
                if (setting == null)
                {
                    continue;
                }
                result.Rows.Add(regionId, setting.ExamGlobalId, setting.ShowResult, setting.ShowBlank);
            }
            return result;
        }
    }
}