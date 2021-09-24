namespace Ege.Check.Dal.Store.Mappers
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Servers;
    using Ege.Dal.Common.Mappers;

    class UpdatedBlankCollectionMapper : DataReaderCollectionMapper<UpdatedBlankInfo>
    {
        public override async Task<ICollection<UpdatedBlankInfo>> Map(DbDataReader @from)
        {
            var blankTypeOrdinal = GetOrdinal(from, "BlankType");
            var compositionPageCountOrdinal = GetOrdinal(from, "CompositionPageCount");
            var regionIdOrdinal = GetOrdinal(from, "RegionId");
            var codeOrdinal = GetOrdinal(from, "ParticipantCode");
            var examIdOrdinal = GetOrdinal(from, "ExamGlobalId");

            var result = new List<UpdatedBlankInfo>();
            while (await @from.ReadAsync())
            {
                result.Add(new UpdatedBlankInfo
                {
                    Code = from.GetString(codeOrdinal),
                    RegionId = from.GetInt32(regionIdOrdinal),
                    ExamGlobalId = from.GetInt32(examIdOrdinal),
                    BlankType = from.GetInt32(blankTypeOrdinal),
                    PageCount = from.GetInt32(compositionPageCountOrdinal),
                });
            }
            return result;
        }
    }
}
