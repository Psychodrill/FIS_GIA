namespace Ege.Check.Dal.Store.Mappers.Staff
{
    using System;
    using System.Data;
    using System.Linq;
    using Ege.Check.Logic.Models;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Services.Dtos.Enums;
    using Ege.Dal.Common.Mappers;

    internal class TaskSettingsDataTableMapper : IDataTableMapper<ExamInfoCacheModel>
    {
        private const string ClientIdColumn = "ClientId";
        private const string ClientParentIdColumn = "ClientParentId";
        private const string TaskNumberColumn = "TaskNumber";
        private const string DisplayNumberColumn = "DisplayNumber";
        private const string MaxValueColumn = "MaxValue";
        private const string TaskTypeCodeColumn = "TaskTypeCode";
        private const string SettingValueColumn = "SettingValue";
        private const string IsTaskSettingColumn = "IsTaskSetting";

        public DataTable Map(ExamInfoCacheModel from)
        {
            var result = new DataTable();
            result.Columns.Add(ClientIdColumn, typeof (int));
            result.Columns.Add(ClientParentIdColumn, typeof (int));
            result.Columns.Add(TaskNumberColumn, typeof (int));
            result.Columns.Add(DisplayNumberColumn, typeof (string));
            result.Columns.Add(MaxValueColumn, typeof (int));
            result.Columns.Add(TaskTypeCodeColumn, typeof (int));
            result.Columns.Add(SettingValueColumn, typeof (string));
            result.Columns.Add(IsTaskSettingColumn, typeof (bool));
            if (from == null)
            {
                return result;
            }
            var currentRow = 0;
            var taskNumbers = new int[(int) Enum.GetValues(typeof (TaskType)).Cast<TaskType>().Max() + 1];
            foreach (var setting in from.PartB ?? Enumerable.Empty<TaskBInfoCacheModel>())
            {
                if (setting == null)
                {
                    continue;
                }
                ++currentRow;
                result.Rows.Add(currentRow, null, ++taskNumbers[(int) TaskType.B], setting.DisplayNumber,
                                setting.MaxValue ?? 0, setting.Type, setting.LegalSymbols, false);
            }
            foreach (var setting in from.WithCriteria ?? Enumerable.Empty<TaskWithCriteriaInfoCacheModel>())
            {
                if (setting == null || !Enum.IsDefined(typeof (TaskType), setting.Type))
                {
                    continue;
                }
                int parentRow;
                result.Rows.Add(parentRow = ++currentRow, null,
                                setting.Criteria != null && setting.Criteria.Any()
                                    ? (int?) null
                                    : ++taskNumbers[(int) setting.Type],
                                setting.DisplayNumber, setting.MaxValue, setting.Type, null, true);
                foreach (var criterion in setting.Criteria ?? Enumerable.Empty<TaskCriterionCacheModel>())
                {
                    if (criterion == null)
                    {
                        continue;
                    }
                    result.Rows.Add(++currentRow, parentRow, ++taskNumbers[(int) setting.Type], null, criterion.MaxValue,
                                    criterion.Type, criterion.Name, true);
                }
            }
            return result;
        }
    }
}