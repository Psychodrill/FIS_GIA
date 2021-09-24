namespace Ege.Hsc.Dal.Store.Repositories
{
    using System;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Dal.Common.Mappers;
    using Ege.Dal.Common.Repositories;
    using Ege.Hsc.Dal.Entities;
    using JetBrains.Annotations;
    using Ege.Check.Dal.Store.Repositories;

    internal class UserRepository : Repository, IUserRepository
    {
        [NotNull]private readonly IDataReaderMapper<User> _dataReaderMapper;
        [NotNull]private readonly IDataReaderSyncMapper<User> _syncMapper;

        private const string GetByLoginProcedureName = "GetUserByLogin";
        private const string MergeUserProcedureName = "MergeUser";

        private const string LoginParameterName = "Login";
        private const string TicketParameterName = "Ticket";


        public UserRepository([NotNull]IDataReaderMapper<User> dataReaderMapper, [NotNull]IDataReaderSyncMapper<User> syncMapper)
        {
            _dataReaderMapper = dataReaderMapper;
            _syncMapper = syncMapper;
        }

        public async Task<User> GetByLoginAsync(DbConnection connection, string login)
        {
            var cmd = StoredProcedureCommand(connection, GetByLoginProcedureName);
            AddParameter(cmd, LoginParameterName, login);
            using (var reader = await cmd.ExecuteReaderWithTimeElapsedLogAsync())
            {
                return await _dataReaderMapper.Map(reader);
            }
        }

        public User GetByLoginSync(DbConnection connection, string login)
        {
            var cmd = StoredProcedureCommand(connection, GetByLoginProcedureName);
            AddParameter(cmd, LoginParameterName, login);
            using (var reader = cmd.ExecuteReader())
            {
                return _syncMapper.Map(reader);
            }
        }

        public async Task<User> MergeAsync(DbConnection connection, string login, Guid ticket)
        {
            var cmd = StoredProcedureCommand(connection, MergeUserProcedureName);
            AddParameter(cmd, LoginParameterName, login);
            AddParameter(cmd, TicketParameterName, ticket);
            using (var reader = await cmd.ExecuteReaderWithTimeElapsedLogAsync())
            {
                return await _dataReaderMapper.Map(reader);
            }
        }
    }
}