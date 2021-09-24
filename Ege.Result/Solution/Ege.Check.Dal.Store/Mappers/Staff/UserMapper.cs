namespace Ege.Check.Dal.Store.Mappers.Staff
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Common;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Dal.Common;
    using Ege.Dal.Common.Mappers;
    using JetBrains.Annotations;

    internal class UserMapper : DataReaderMapper<UserModel>, IDataReaderSyncMapper<UserModel>
    {
        private const string Id = "UserId";
        private const string Region = "RegionId";
        private const string Enabled = "IsEnabled";
        private const string Role = "Role";
        private const string Password = "Password";
        private const string Login = "LoginName";

        UserModel IMapper<DbDataReader, UserModel>.Map([NotNull] DbDataReader from)
        {
            if (!from.Read())
            {
                return null;
            }
            var id = GetOrdinal(from, Id);
            var region = GetOrdinal(from, Region);
            var isEnabled = GetOrdinal(from, Enabled);
            var role = GetOrdinal(from, Role);
            var password = GetOrdinal(from, Password);
            var login = GetOrdinal(from, Login);

            var result = new UserModel
                {
                    Id = from.GetInt32(id),
                    IsEnabled = from.GetBoolean(isEnabled),
                    RegionId = from.IsDBNull(region) ? (int?) null : from.GetInt32(region),
                    PasswordHash = from.GetString(password),
                    Login = from.GetString(login),
                    Role = from.IsDBNull(role)
                               ? Logic.Models.Staff.Role.None
                               : (Role) from.GetInt32(role),
                };
            return result;
        }

        public override async Task<UserModel> Map(DbDataReader @from)
        {
            if (!await @from.ReadAsync())
            {
                return null;
            }

            var id = GetOrdinal(from, Id);
            var region = GetOrdinal(from, Region);
            var isEnabled = GetOrdinal(from, Enabled);
            var role = GetOrdinal(from, Role);
            var password = GetOrdinal(from, Password);
            var login = GetOrdinal(from, Login);

            var result = new UserModel
                {
                    Id = from.GetInt32(id),
                    IsEnabled = from.GetBoolean(isEnabled),
                    RegionId = await from.GetNullableInt32Async(region),
                    PasswordHash = from.GetString(password),
                    Login = from.GetString(login),
                    Role = await from.IsDBNullAsync(role)
                               ? Logic.Models.Staff.Role.None
                               : (Role) from.GetInt32(role),
                };
            return result;
        }
    }
}