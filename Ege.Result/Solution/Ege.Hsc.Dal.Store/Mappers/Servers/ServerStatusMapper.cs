namespace Ege.Hsc.Dal.Store.Mappers.Servers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading;
    using System.Threading.Tasks;
    using Ege.Dal.Common;
    using Ege.Dal.Common.Mappers;
    using Ege.Hsc.Logic.Models.Servers;

    class ServerStatusMapper : DataReaderCollectionMapper<BlankServerStatus>
    {
        public override async Task<ICollection<BlankServerStatus>> Map(DbDataReader @from)
        {
            var regionId = GetOrdinal(from, "RegionId");
            var name = GetOrdinal(from, "Name");
            var url = GetOrdinal(from, "Url");
            var isAvailable = GetOrdinal(from, "IsAvailable");
            var serverBlankCount = GetOrdinal(from, "ServerBlankCount");
            var dbCount = GetOrdinal(from, "DbCount");
            var lastAvailabilityCheck = GetOrdinal(from, "LastAvailabilityCheck");
            var lastFileCheck = GetOrdinal(from, "LastFileCheck");
            var hasErrors = GetOrdinal(from, "HasErrors");

            var result = new List<BlankServerStatus>();
            while (await from.ReadAsync())
            {
                result.Add(new BlankServerStatus
                {
                    Id = from.GetInt32(regionId),
                    Region = from.GetString(name),
                    Server = await from.GetNullableStringAsync(url),
                    IsAvailable = from.GetBoolean(isAvailable),
                    ServerCount = from.GetInt32(serverBlankCount),
                    DbCount = from.GetInt32(dbCount),
                    LastAvailabilityCheck = await from.GetNullableDateTimeAsync(lastAvailabilityCheck),
                    LastFileCheck = await from.GetNullableDateTimeAsync(lastFileCheck),
                    HasErrors = from.GetBoolean(hasErrors),
                });
            }
            return result;
        }
    }
}
