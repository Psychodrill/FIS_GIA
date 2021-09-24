namespace Ege.Hsc.Dal.Store.Mappers.Blanks
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Blanks;
    using Ege.Dal.Common;
    using Ege.Dal.Common.Mappers;
    using Ege.Hsc.Logic.Models.Blanks;

    class LoadedBlankMapper : DataReaderMapper<LoadedBlanks>
    {
        private const string Id = "Id";
        private const string SubjectCode = "SubjectCode";
        private const string ExamDate = "ExamDate";
        private const string Barcode = "Barcode";
        private const string BlankType = "BlankType";
        private const string PageCount = "PageCount";
        private const string RegionId = "RegionId";
        private const string ProjectBatchId = "ProjectBatchId";
        private const string ProjectName = "ProjectName";

        public async override Task<LoadedBlanks> Map(DbDataReader @from)
        {
            var result = new List<LoadedBlank>();
            var idOrdinal = GetOrdinal(from, Id);
            var subjectCodeOrdinal = GetOrdinal(from, SubjectCode);
            var examDateOrdinal = GetOrdinal(from, ExamDate);
            var barcodeOrdinal = GetOrdinal(from, Barcode);
            var blankTypeOrdinal = GetOrdinal(from, BlankType);
            var pageCountOrdinal = GetOrdinal(from, PageCount);
            var regionOrdinal = GetOrdinal(from, RegionId);
            var projectBatchId = GetOrdinal(from, ProjectBatchId);
            var projectName = GetOrdinal(from, ProjectName);
            while (await from.ReadAsync())
            {
                result.Add(new LoadedBlank
                    {
                        ParticipantId = from.GetInt32(idOrdinal),
                        BlankData = new BlankIntermediateModel
                        {
                            Barcode = from.GetString(barcodeOrdinal),
                            BlankType = from.GetInt32(blankTypeOrdinal),
                            ExamDate = from.GetDateTime(examDateOrdinal),
                            PageCount = from.GetInt32(pageCountOrdinal),
                            SubjectCode = from.GetInt32(subjectCodeOrdinal),
                            ProjectName = await from.GetNullableStringAsync(projectName),
                            ProjectBatchId = from.GetInt32(projectBatchId),
                        },
                        RegionId = from.GetInt32(regionOrdinal),
                    });
            }
            return new LoadedBlanks
                {
                    Blanks = result
                };
        }
    }
}
