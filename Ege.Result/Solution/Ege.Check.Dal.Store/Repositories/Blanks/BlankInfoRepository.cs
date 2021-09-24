namespace Ege.Check.Dal.Store.Repositories.Blanks
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Servers;
    using Ege.Dal.Common.Mappers;
    using Ege.Dal.Common.Repositories;
    using JetBrains.Annotations;

    class BlankInfoRepository : Repository, IBlankInfoRepository
    {
        private const string UpdatePageCountProc = "UpdateCompositionPageCount";
        private const string GetAllBlanksProc = "GetBlanksWithCompositionPageCount";
        private const string RegionIdParam = "RegionId";
        private const string ServerDataParam = "ServerData";

        [NotNull]private readonly IDataTableMapper<IEnumerable<PageCountData>> _pageCountTableMapper;
        [NotNull]private readonly IDataReaderCollectionMapper<UpdatedBlankInfo> _updatedBlankMapper;

        public BlankInfoRepository(
            [NotNull]IDataTableMapper<IEnumerable<PageCountData>> pageCountTableMapper, 
            [NotNull]IDataReaderCollectionMapper<UpdatedBlankInfo> updatedBlankMapper)
        {
            _pageCountTableMapper = pageCountTableMapper;
            _updatedBlankMapper = updatedBlankMapper;
        }

        public async Task<ICollection<UpdatedBlankInfo>> UpdatePageCount(DbConnection connection, int regionId, ICollection<PageCountData> pageCountData)
        {
            var cmd = StoredProcedureCommand(connection, UpdatePageCountProc);
            cmd.CommandTimeout = 600;
            AddParameter(cmd, RegionIdParam, regionId);
            AddParameter(cmd, ServerDataParam, _pageCountTableMapper.Map(pageCountData));
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                return await _updatedBlankMapper.Map(reader);
            }
        }

        public async Task<ICollection<UpdatedBlankInfo>> GetAllBlanksWithCompositionPageCount(DbConnection connection)
        {
            var cmd = StoredProcedureCommand(connection, GetAllBlanksProc);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                return await _updatedBlankMapper.Map(reader);
            }
        }
    }
}