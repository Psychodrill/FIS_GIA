namespace Ege.Check.Dal.Store.Repositories.Users
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Staff;
    using JetBrains.Annotations;

    public interface IUserRepository
    {
        UserModel GetByIdSync([NotNull] DbConnection connection, int id);

        [NotNull]
        Task<UserModel> GetById([NotNull] DbConnection connection, int id);

        [NotNull]
        Task<UserModel> GetByLogin([NotNull] DbConnection connection, string login);

        [NotNull]
        Task ResetPassword([NotNull] DbConnection connection, int id, string newPasswordHash);

        [NotNull]
        Task Activate([NotNull] DbConnection connection, int id);

        [NotNull]
        Task Deactivate([NotNull] DbConnection connection, int id);

        [NotNull]
        Task Create([NotNull] DbConnection connection, [NotNull] string login, [NotNull] string passwordHash, Role role,
                    int? regionId, bool isEnabled);

        [NotNull]
        Task Update([NotNull] DbConnection connection, int id, string passwordHash, Role role, int? regionId,
                    bool isEnabled);

        [NotNull]
        Task<UserDtoPage> Get([NotNull] DbConnection connection, string login, int? regionId, Role? role, int take,
                              int skip);

        [NotNull]
        Task<UserDto> GetDtoById([NotNull] DbConnection connection, int id);

        [NotNull]
        Task<bool> Delete([NotNull] DbConnection connection, int id);
    }
}