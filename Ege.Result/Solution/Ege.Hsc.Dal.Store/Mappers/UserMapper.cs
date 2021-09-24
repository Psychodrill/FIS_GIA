namespace Ege.Hsc.Dal.Store.Mappers
{
    using System;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Common;
    using Ege.Dal.Common;
    using Ege.Dal.Common.Mappers;
    using Ege.Hsc.Dal.Entities;
    using JetBrains.Annotations;

    internal class UserMapper : DataReaderMapper<User>, IDataReaderSyncMapper<User>
    {
        private const string Id = "Id";
        private const string Login = "Login";
        private const string Ticket = "Ticket";

        User IMapper<DbDataReader, User>.Map([NotNull] DbDataReader from)
        {
            if (!from.Read())
            {
                return null;
            }
            var id = GetOrdinal(from, Id);
            var login = GetOrdinal(from, Login);
            var ticket = GetOrdinal(from, Ticket);

            return new User
                {
                    Id = from.GetInt32(id),
                    Login = from.GetString(login),
                    Ticket = from.IsDBNull(ticket) ? (Guid?) null : from.GetGuid(ticket)
                };
        }

        public override async Task<User> Map(DbDataReader @from)
        {
            if (!await @from.ReadAsync())
            {
                return null;
            }

            var id = GetOrdinal(from, Id);
            var login = GetOrdinal(from, Login);
            var ticket = GetOrdinal(from, Ticket);

            return new User
                {
                    Id = from.GetInt32(id),
                    Login = from.GetString(login),
                    Ticket = await from.GetNullableGuidAsync(ticket)
                };
        }
    }
}