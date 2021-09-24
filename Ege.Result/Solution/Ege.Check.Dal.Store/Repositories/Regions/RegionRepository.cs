namespace Ege.Check.Dal.Store.Repositories.Regions
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Dal.Common.Mappers;
    using JetBrains.Annotations;

    internal class RegionRepository : IRegionRepository
    {
        private const string GetAvailableRegionsSql = @"select REGION from rbdc_Regions where Enable = 1";
        [NotNull] private readonly IDataReaderCollectionMapper<AvailableRegion> _mapper;

        public RegionRepository([NotNull] IDataReaderCollectionMapper<AvailableRegion> mapper)
        {
            _mapper = mapper;
        }

        public async Task<ICollection<AvailableRegion>> GetAvailableRegions(DbConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandTimeout = 300;
            command.CommandText = GetAvailableRegionsSql;
            using (var reader = await command.ExecuteReaderAsync())
            {
                var result = await _mapper.Map(reader);
                return result;
            }
        }
    }
}