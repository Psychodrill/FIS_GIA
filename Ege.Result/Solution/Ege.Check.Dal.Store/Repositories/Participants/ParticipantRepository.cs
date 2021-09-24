namespace Ege.Check.Dal.Store.Repositories.Participants
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Dal.Common.Mappers;
    using Ege.Dal.Common.Repositories;
    using JetBrains.Annotations;

    internal class ParticipantRepository : Repository, IParticipantRepository
    {
        private const string ProcedureName = "GetParticipants";
        private const string HashParameterName = "@ParticipantHash";

        [NotNull] private readonly IDataReaderMapper<ParticipantCollectionCacheModel> _mapper;

        public ParticipantRepository([NotNull] IDataReaderMapper<ParticipantCollectionCacheModel> mapper)
        {
            _mapper = mapper;
        }

        public async Task<ParticipantCollectionCacheModel> GetByHash(DbConnection connection, string hash)
        {
            var command = StoredProcedureCommand(connection, ProcedureName);
            AddParameter(command, HashParameterName, hash);
            using (var reader = await command.ExecuteReaderAsync())
            {
                var result = await _mapper.Map(reader);
                return result;
            }
        }

        public async Task<string> GetCodeByRbdId(DbConnection connection, System.Guid rbdId)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = "select top 1 ParticipantCode from ap_Participants where ParticipantRbdId = @rbdId";
            AddParameter(cmd, "rbdId", rbdId);
            return await cmd.ExecuteScalarAsync() as string ?? string.Empty;
        }
    }
}
