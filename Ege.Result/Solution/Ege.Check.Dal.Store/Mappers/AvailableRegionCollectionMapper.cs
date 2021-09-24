namespace Ege.Check.Dal.Store.Mappers
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Dal.Common.Mappers;
    using JetBrains.Annotations;

    internal class AvailableRegionCollectionMapper : IDataReaderCollectionMapper<AvailableRegion>
    {
        [NotNull] private const string Id = "REGION";

        public async Task<ICollection<AvailableRegion>> Map(DbDataReader @from)
        {
            if (from == null)
            {
                return new AvailableRegion[0];
            }
            var result = new List<AvailableRegion>();
            var id = from.GetOrdinal(Id);
            while (await from.ReadAsync())
            {
                result.Add(new AvailableRegion
                    {
                        Id = from.GetInt32(id),
                    });
            }
            return result;
        }
    }
}