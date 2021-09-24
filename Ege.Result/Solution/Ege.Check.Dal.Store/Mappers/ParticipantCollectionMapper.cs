namespace Ege.Check.Dal.Store.Mappers
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Dal.Common;
    using Ege.Dal.Common.Mappers;
    using JetBrains.Annotations;

    internal class ParticipantCollectionMapper : DataReaderMapper<ParticipantCollectionCacheModel>
    {
        [NotNull] private const string Hash = "ParticipantHash";
        [NotNull] private const string Code = "ParticipantCode";
        [NotNull] private const string Document = "DocumentNumber";
        [NotNull] private const string Region = "RegionId";

        public override async Task<ParticipantCollectionCacheModel> Map(DbDataReader @from)
        {
            var hash = GetOrdinal(from, Hash);
            var code = GetOrdinal(from, Code);
            var doc = GetOrdinal(from, Document);
            var region = GetOrdinal(from, Region);
            var result = new List<ParticipantCacheModel>();
            while (await from.ReadAsync())
            {
                result.Add(new ParticipantCacheModel
                    {
                        Hash = from.GetString(hash),
                        Code = await from.GetNullableStringAsync(code),
                        Document = await from.GetNullableStringAsync(doc),
                        RegionId = from.GetInt32(region),
                    });
            }
            return new ParticipantCollectionCacheModel
                {
                    Participants = result,
                };
        }
    }
}