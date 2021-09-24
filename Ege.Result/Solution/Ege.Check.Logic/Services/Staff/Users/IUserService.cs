namespace Ege.Check.Logic.Services.Staff.Users
{
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Staff;
    using JetBrains.Annotations;

    public interface IUserService
    {
        UserModel GetByIdSync(int id);

        [NotNull]
        Task<UserModel> GetById(int id);

        [NotNull]
        Task<UserModel> GetByLoginAndPassword(string login, string password);

        [NotNull]
        Task SetPassword(int id, string password);

        [NotNull]
        Task Activate(int id);

        [NotNull]
        Task Deactivate(int id);

        [NotNull]
        Task Create([NotNull] UserDto user);

        [NotNull]
        Task Update([NotNull] UserDto user);

        [NotNull]
        Task<UserDtoPage> Get(string login, int? regionId, Role? role, int take, int skip);

        [NotNull]
        Task<UserDto> GetDtoById(int id);

        [NotNull]
        Task<bool> Delete(int id);
    }
}