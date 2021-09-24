namespace Ege.Check.Dal.Store.Mappers
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Services.Dtos.Enums;
    using Ege.Dal.Common;
    using Ege.Dal.Common.Mappers;

    internal class AnswerCollectionMapper : DataReaderMapper<AnswerCollectionCacheModel>
    {
        private const string PrimaryMark = "PrimaryMark";
        private const string TestMark = "TestMark";
        private const string Mark5 = "Mark5";
        private const string Type = "TaskTypeCode";
        private const string Number = "TaskNumber";
        private const string Answer = "AnswerValue";
        private const string Mark = "Mark";
        private const string IsHidden = "IsHidden";
        private const string Barcode = "Barcode";
        private const string BlankType = "BlankType";
        private const string PageCount = "PageCount";
        private const string RowType = "RowType";
        private const string ProjectBatchId = "ProjectBatchId";
        private const string ProjectName = "ProjectName";

        public override async Task<AnswerCollectionCacheModel> Map(DbDataReader @from)
        {
            if (!await from.ReadAsync())
            {
                return null;
            }

            var primaryMark = GetOrdinal(from, PrimaryMark);
            var testMark = GetOrdinal(from, TestMark);
            var mark5 = GetOrdinal(from, Mark5);
            var type = GetOrdinal(from, Type);
            var number = GetOrdinal(from, Number);
            var answer = GetOrdinal(from, Answer);
            var mark = GetOrdinal(from, Mark);
            var isHidden = GetOrdinal(from, IsHidden);
            var barcode = GetOrdinal(from, Barcode);
            var blankType = GetOrdinal(from, BlankType);
            var pageCount = GetOrdinal(from, PageCount);
            var rowType = GetOrdinal(from, RowType);
            var projectBatchId = GetOrdinal(from, ProjectBatchId);
            var projectName = GetOrdinal(from, ProjectName);

            var answers = new List<AnswerCacheModel>();
            var blanks = new List<BlankCacheModel>();
            var result = new AnswerCollectionCacheModel
                {
                    PrimaryMark = from.GetInt32(primaryMark),
                    TestMark = from.GetInt32(testMark),
                    Mark5 = from.GetInt32(mark5),
                    Answers = answers,
                    Blanks = blanks,
                    IsHidden = from.GetBoolean(isHidden),
                };

            do
            {
                if (!from.GetBoolean(rowType))
                {
                    answers.Add(new AnswerCacheModel
                        {
                            Type = (TaskType) from.GetInt32(type),
                            Number = from.GetInt32(number),
                            Answer = from.GetString(answer),
                            Mark = from.GetInt32(mark),
                        });
                }
                else
                {
                    blanks.Add(new BlankCacheModel
                    {
                        Barcode = from.GetString(barcode),
                        PageCount = await from.GetNullableInt32Async(pageCount),
                        BlankType = from.GetInt32(blankType),
                        ProjectName = await from.GetNullableStringAsync(projectName),
                        ProjectBatchId = from.GetInt32(projectBatchId),
                    });
                }
            } while (await from.ReadAsync());
            return result;
        }
    }
}
