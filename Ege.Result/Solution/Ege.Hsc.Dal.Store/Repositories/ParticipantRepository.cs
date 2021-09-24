namespace Ege.Hsc.Dal.Store.Repositories
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Dal.Common.Repositories;

    class ParticipantRepository : Repository, IParticipantRepository
    {
        private const string LoadFromCheckEgeProcedureName = "LoadParticipants";

        public async Task<int> LoadFromCheckEgeDb(DbConnection connection, DbTransaction transaction)
        {
            var command = StoredProcedureCommand(connection, LoadFromCheckEgeProcedureName);
            command.CommandTimeout = 300;
            command.Transaction = transaction;
            return await command.ExecuteNonQueryAsync();
        }
    }
}
