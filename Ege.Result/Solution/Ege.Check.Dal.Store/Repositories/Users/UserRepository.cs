namespace Ege.Check.Dal.Store.Repositories.Users
{
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Dal.Common.Mappers;
    using Ege.Dal.Common.Repositories;
    using JetBrains.Annotations;

    internal class UserRepository : Repository, IUserRepository
    {
        private const string GetUserByIdProcedureName = "GetUser";
        private const string GetUserByLoginProcedureName = "GetUser";
        private const string ResetPasswordProcedureName = "ResetPassword";
        private const string ActivateUserProcedureName = "SetUserAndRegionActivation";
        private const string CreateUserProcedureName = "CreateUser";
        private const string UpdateUserProcedureName = "UpdateUser";
        private const string DeleteUserProcedureName = "DeleteUser";
        private const string GetDtoByIdProcedureName = "GetUsers";
        private const string GetUsersProcedureName = "GetUsers";
        private const string IdParameterName = "@Id";
        private const string LoginParameterName = "@Login";
        private const string PasswordParameterName = "@Password";
        private const string StateParameterName = "@State";
        private const string RoleParameterName = "@Role";
        private const string RegionParameterName = "@RegionId";
        private const string IsEnabledParameterName = "@IsEnabled";
        private const string TakeParameterName = "@Take";
        private const string SkipParameterName = "@Skip";

        [NotNull] private readonly IDataReaderMapper<UserDtoPage> _dtoMapper;
        [NotNull] private readonly IDataReaderMapper<UserModel> _mapper;
        [NotNull] private readonly IDataReaderSyncMapper<UserModel> _syncMapper;

        public UserRepository(
            [NotNull] IDataReaderMapper<UserModel> mapper,
            [NotNull] IDataReaderSyncMapper<UserModel> syncMapper,
            [NotNull] IDataReaderMapper<UserDtoPage> dtoMapper)
        {
            _mapper = mapper;
            _syncMapper = syncMapper;
            _dtoMapper = dtoMapper;
        }

        public UserModel GetByIdSync(DbConnection connection, int id)
        {
            var command = StoredProcedureCommand(connection, GetUserByIdProcedureName);
            AddParameter(command, IdParameterName, id);
            using (var reader = command.ExecuteReader())
            {
                var result = _syncMapper.Map(reader);
                return result;
            }
        }

        public async Task<UserModel> GetById(DbConnection connection, int id)
        {
            var command = StoredProcedureCommand(connection, GetUserByIdProcedureName);
            AddParameter(command, IdParameterName, id);
            using (var reader = await command.ExecuteReaderAsync())
            {
                var result = await _mapper.Map(reader);
                return result;
            }
        }

        public async Task<UserModel> GetByLogin(DbConnection connection, string login)
        {
            var command = StoredProcedureCommand(connection, GetUserByLoginProcedureName);
            AddParameter(command, LoginParameterName, login);
            using (var reader = command.ExecuteReader())
            {
                var result = await _mapper.Map(reader);
                return result;
            }
        }

        public async Task ResetPassword(DbConnection connection, int id, string newPasswordHash)
        {
            var command = StoredProcedureCommand(connection, ResetPasswordProcedureName);
            AddParameter(command, IdParameterName, id);
            AddParameter(command, PasswordParameterName, newPasswordHash);
            await command.ExecuteNonQueryAsync();
        }

        public Task Activate(DbConnection connection, int id)
        {
            return SetActivation(connection, id, true);
        }

        public Task Deactivate(DbConnection connection, int id)
        {
            return SetActivation(connection, id, false);
        }

        public async Task Create(DbConnection connection, string login, string passwordHash, Role role, int? regionId,
                                 bool isEnabled)
        {
            var command = StoredProcedureCommand(connection, CreateUserProcedureName);
            AddParameter(command, LoginParameterName, login);
            AddParameter(command, PasswordParameterName, passwordHash);
            AddParameter(command, RoleParameterName, role);
            AddParameter(command, RegionParameterName, regionId);
            AddParameter(command, IsEnabledParameterName, isEnabled);
            await command.ExecuteNonQueryAsync();
        }

        public async Task Update(DbConnection connection, int id, string passwordHash, Role role, int? regionId,
                                 bool isEnabled)
        {
            var command = StoredProcedureCommand(connection, UpdateUserProcedureName);
            AddParameter(command, IdParameterName, id);
            AddParameter(command, PasswordParameterName, passwordHash);
            AddParameter(command, RoleParameterName, role);
            AddParameter(command, RegionParameterName, regionId);
            AddParameter(command, IsEnabledParameterName, isEnabled);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<UserDtoPage> Get(DbConnection connection, string login, int? regionId, Role? role, int take,
                                           int skip)
        {
            var command = StoredProcedureCommand(connection, GetUsersProcedureName);
            AddParameter(command, LoginParameterName, login);
            AddParameter(command, RoleParameterName, role);
            AddParameter(command, RegionParameterName, regionId);
            AddParameter(command, SkipParameterName, skip);
            AddParameter(command, TakeParameterName, take);
            using (var reader = await command.ExecuteReaderAsync())
            {
                return await _dtoMapper.Map(reader);
            }
        }

        public async Task<UserDto> GetDtoById(DbConnection connection, int id)
        {
            var command = StoredProcedureCommand(connection, GetDtoByIdProcedureName);
            AddParameter(command, IdParameterName, id);
            using (var reader = await command.ExecuteReaderAsync())
            {
                return (await _dtoMapper.Map(reader)).Users.FirstOrDefault();
            }
        }

        public async Task<bool> Delete(DbConnection connection, int id)
        {
            var command = StoredProcedureCommand(connection, DeleteUserProcedureName);
            AddParameter(command, IdParameterName, id);
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        private async Task SetActivation([NotNull] DbConnection connection, int id, bool state)
        {
            var command = StoredProcedureCommand(connection, ActivateUserProcedureName);
            AddParameter(command, IdParameterName, id);
            AddParameter(command, StateParameterName, state);
            await command.ExecuteNonQueryAsync();
        }
    }
}