namespace Ege.Hsc.Dal.Store.Mappers.Servers
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Dal.Common.Mappers;
    using Ege.Hsc.Logic.Models.Servers;

    class ServerAvailabilityModelMapper : DataReaderCollectionMapper<BlankServerAvailabilityModel>
    {
        public override async Task<ICollection<BlankServerAvailabilityModel>> Map(DbDataReader @from)
        {
            var regionId = GetOrdinal(from, "RegionId");
            var url = GetOrdinal(from, "Url");
            var examDate = GetOrdinal(from, "ExamDate");
            var subjectCode = GetOrdinal(from, "SubjectCode");

            var result = new List<BlankServerAvailabilityModel>();
            while (await from.ReadAsync())
            {
                result.Add(new BlankServerAvailabilityModel
                {
                    ExamDate = from.GetDateTime(examDate),
                    RegionId = from.GetInt32(regionId),
                    SubjectCode = from.GetInt32(subjectCode),
                    Url = from.GetString(url),
                });
            }
            return result;
        }
    }
}
