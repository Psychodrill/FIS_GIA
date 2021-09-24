namespace Ege.Check.Dal.Store.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Common;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Services.Dtos.Enums;
    using Ege.Dal.Common;
    using Ege.Dal.Common.Mappers;
    using JetBrains.Annotations;

    internal class AnswerCriteriaCollectionMapper : DataReaderMapper<ExamInfoCacheModel>,
                                                    IDataReaderMapper<IDictionary<int, ExamInfoCacheModel>>
    {
        private const string HasCriteria = "HasCriteria";
        private const string ParentId = "ParentId";
        private const string Type = "TaskTypeCode";
        private const string Number = "TaskNumber";
        private const string MaxValue = "MaxValue";
        private const string CriteriaName = "CriteriaName";
        private const string MinValue = "MinValue";
        private const string IsComposition = "IsComposition";
        private const string IsBasicMath = "IsBasicMath";
        private const string IsForeignLanguage = "IsForeignLanguage";
        private const string DisplayNumber = "DisplayNumber";
        private const string SubjectCode = "SubjectCode";

        async Task<IDictionary<int, ExamInfoCacheModel>>
            IMapper<DbDataReader, Task<IDictionary<int, ExamInfoCacheModel>>>.Map([NotNull] DbDataReader from)
        {
            var result = new Dictionary<int, ExamInfoCacheModel>();
            if (!await from.ReadAsync())
            {
                return result;
            }
            var ordinals = GetOrdinals(from);
            int? subjectCode = from.GetInt32(ordinals.SubjectCodeOrdinal);
            do
            {
                var criteria = await MapSingle(subjectCode.Value, from, ordinals);
                result.Add(subjectCode.Value, criteria.Value);
                subjectCode = criteria.Key;
            } while (subjectCode != null);
            return result;
        }

        [NotNull]
        private Ordinals GetOrdinals([NotNull] DbDataReader from)
        {
            return new Ordinals
                {
                    HasCriteriaOrdinal = GetOrdinal(from, HasCriteria),
                    ParentIdOrdinal = GetOrdinal(from, ParentId),
                    TypeOrdinal = GetOrdinal(from, Type),
                    NumberOrdinal = GetOrdinal(from, Number),
                    MaxValueOrdinal = GetOrdinal(from, MaxValue),
                    CriteriaNameOrdinal = GetOrdinal(from, CriteriaName),
                    MinValueOrdinal = GetOrdinal(from, MinValue),
                    IsBasicMathOrdinal = GetOrdinal(from, IsBasicMath),
                    IsCompositionOrdinal = GetOrdinal(from, IsComposition),
                    IsForeignLanguageOrdinal = GetOrdinal(from, IsForeignLanguage),
                    DisplayNumberOrdinal = GetOrdinal(from, DisplayNumber),
                    SubjectCodeOrdinal = GetOrdinal(from, SubjectCode),
                };
        }

        private async Task<KeyValuePair<int?, ExamInfoCacheModel>> MapSingle(
            int subjectCode,
            [NotNull] DbDataReader from,
            [NotNull] Ordinals ordinals)
        {
            var partBTasks = new List<TaskBInfoCacheModel>();
            var tasksWithCriteria = new List<TaskWithCriteriaInfoCacheModel>();
            TaskWithCriteriaInfoCacheModel currentTaskWithCriteria = null;

            var result = new ExamInfoCacheModel
                {
                    Threshold = from.GetInt32(ordinals.MinValueOrdinal),
                    IsBasicMath = from.GetBoolean(ordinals.IsBasicMathOrdinal),
                    IsComposition = from.GetBoolean(ordinals.IsCompositionOrdinal),
                    IsForeignLanguage = from.GetBoolean(ordinals.IsForeignLanguageOrdinal),
                    PartB = partBTasks,
                    WithCriteria = tasksWithCriteria,
                };
            int? nextSubjectCode = null;

            do
            {
                var rowType = await from.GetNullableBooleanAsync(ordinals.HasCriteriaOrdinal);
                if (!rowType.HasValue)
                {
                }
                else if (!rowType.Value)
                {
                    partBTasks.Add(new TaskBInfoCacheModel
                        {
                            Type = (TaskType) from.GetInt32(ordinals.TypeOrdinal),
                            Number = await from.GetNullableInt32Async(ordinals.NumberOrdinal),
                            MaxValue = await from.GetNullableInt32Async(ordinals.MaxValueOrdinal),
                            LegalSymbols = await from.GetNullableStringAsync(ordinals.CriteriaNameOrdinal),
                            DisplayNumber = await from.GetNullableStringAsync(ordinals.DisplayNumberOrdinal),
                        });
                }
                else
                {
                    var currentParentId = await from.GetNullableInt32Async(ordinals.ParentIdOrdinal);

                    if (currentParentId == null)
                    {
                        currentTaskWithCriteria = new TaskWithCriteriaInfoCacheModel
                            {
                                Type = (TaskType) from.GetInt32(ordinals.TypeOrdinal),
                                Number = await from.GetNullableInt32Async(ordinals.NumberOrdinal) ?? 0,
                                MaxValue = await from.GetNullableInt32Async(ordinals.MaxValueOrdinal) ?? 0,
                                DisplayNumber = await from.GetNullableStringAsync(ordinals.DisplayNumberOrdinal),
                            };

                        tasksWithCriteria.Add(currentTaskWithCriteria);
                    }
                    else
                    {
                        if (currentTaskWithCriteria == null)
                        {
                            throw new InvalidOperationException(
                                "Invalid datareader ordering (child mapped before parent)");
                        }
                        currentTaskWithCriteria.Criteria = currentTaskWithCriteria.Criteria ??
                                                           new List<TaskCriterionCacheModel>();
                        currentTaskWithCriteria.Criteria.Add(new TaskCriterionCacheModel
                            {
                                Type = (TaskType) from.GetInt32(ordinals.TypeOrdinal),
                                Number = await from.GetNullableInt32Async(ordinals.NumberOrdinal),
                                MaxValue = await from.GetNullableInt32Async(ordinals.MaxValueOrdinal),
                                Name = await from.GetNullableStringAsync(ordinals.CriteriaNameOrdinal),
                            });
                    }
                }
            } while (await from.ReadAsync() && (nextSubjectCode = from.GetInt32(ordinals.SubjectCodeOrdinal)) == subjectCode);
            return new KeyValuePair<int?, ExamInfoCacheModel>(nextSubjectCode == subjectCode ? null : nextSubjectCode, result);
        }

        public override async Task<ExamInfoCacheModel> Map(DbDataReader @from)
        {
            if (!await from.ReadAsync())
            {
                return new ExamInfoCacheModel();
            }
            var ordinals = GetOrdinals(from);
            return (await MapSingle(from.GetInt32(ordinals.SubjectCodeOrdinal), from, ordinals)).Value;
        }

        private class Ordinals
        {
            public int CriteriaNameOrdinal;
            public int DisplayNumberOrdinal;
            public int HasCriteriaOrdinal;
            public int IsBasicMathOrdinal;
            public int IsCompositionOrdinal;
            public int IsForeignLanguageOrdinal;
            public int MaxValueOrdinal;
            public int MinValueOrdinal;
            public int NumberOrdinal;
            public int ParentIdOrdinal;
            public int SubjectCodeOrdinal;
            public int TypeOrdinal;
        }
    }
}