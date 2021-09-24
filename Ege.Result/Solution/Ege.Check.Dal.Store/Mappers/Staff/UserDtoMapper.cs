namespace Ege.Check.Dal.Store.Mappers.Staff
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Dal.Common;
    using Ege.Dal.Common.Mappers;
    using JetBrains.Annotations;

    internal class UserDtoMapper : DataReaderMapper<UserDtoPage>
    {
        private const string Id = "UserId";
        private const string Login = "LoginName";
        private const string RegionId = "RegionId";
        private const string RegionName = "RegionName";
        private const string Role = "Role";
        private const string IsEnabled = "IsEnabled";
        private const string Count = "UserCount";

        [NotNull] private readonly IDictionary<Role, string> _roleNames = new Dictionary<Role, string>
            {
                {Logic.Models.Staff.Role.Fct, "Администратор ФЦТ"},
                {Logic.Models.Staff.Role.Rcoi, "Администратор РЦОИ"},
                {Logic.Models.Staff.Role.FctOperator, "Оператор ФЦТ"},
                {Logic.Models.Staff.Role.HscOperator, "Оператор выгрузки бланков"},
            };

        public override async Task<UserDtoPage> Map(DbDataReader from)
        {
            if (!await from.ReadAsync())
            {
                return new UserDtoPage
                    {
                        Count = 0,
                        Users = new List<UserDto>(),
                    };
            }

            var id = GetOrdinal(from, Id);
            var login = GetOrdinal(from, Login);
            var regionId = GetOrdinal(from, RegionId);
            var regionName = GetOrdinal(from, RegionName);
            var role = GetOrdinal(from, Role);
            var isEnabled = GetOrdinal(from, IsEnabled);
            var count = GetOrdinal(from, Count);

            var users = new List<UserDto>();
            var result = new UserDtoPage
                {
                    Count = from.GetInt32(count),
                    Users = users,
                };
            int? lastId = null;
            do
            {
                var currentId = from.GetInt32(id);
                if (currentId == lastId)
                {
                    continue;
                }

                lastId = currentId;
                var currentRole = (Role) from.GetInt32(role);
                string currentRoleName;
                _roleNames.TryGetValue(currentRole, out currentRoleName);
                users.Add(new UserDto
                    {
                        Id = currentId,
                        IsEnabled = from.GetBoolean(isEnabled),
                        Login = from.GetString(login),
                        RegionId = await from.GetNullableInt32Async(regionId),
                        RegionName = await from.GetNullableStringAsync(regionName),
                        Role = currentRole,
                        RoleName = currentRoleName,
                    });
            } while (await from.ReadAsync());

            return result;
        }
    }
}