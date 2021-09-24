namespace Ege.Check.Logic.Services.Staff.Users
{
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.CacheFactory;
    using Ege.Check.Dal.Cache.StaffUsers;
    using Ege.Check.Dal.Store.Repositories.Users;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Dal.Common.Factory;
    using JetBrains.Annotations;

    internal class UserService : IUserService
    {
        [NotNull] private readonly ICacheFactory _cacheFactory;
        [NotNull] private readonly IDbConnectionFactory _connectionFactory;
        [NotNull] private readonly IPasswordHasher _passwordHasher;
        [NotNull] private readonly IUserRepository _repository;
        [NotNull] private readonly IStaffUserCache _staffUserCache;

        public UserService(
            [NotNull] IDbConnectionFactory connectionFactory,
            [NotNull] IUserRepository repository,
            [NotNull] IPasswordHasher passwordHasher,
            [NotNull] ICacheFactory cacheFactory,
            [NotNull] IStaffUserCache staffUserCache)
        {
            _connectionFactory = connectionFactory;
            _repository = repository;
            _passwordHasher = passwordHasher;
            _cacheFactory = cacheFactory;
            _staffUserCache = staffUserCache;
        }

        public async Task<UserModel> GetByLoginAndPassword(string login, string password)
        {
            UserModel user;
            using (var connection = await _connectionFactory.CreateAsync())
            {
                user = await _repository.GetByLogin(connection, login);
            }
            if (user == null || user.PasswordHash == null ||
                !_passwordHasher.ValidatePassword(password, user.PasswordHash))
            {
                return null;
            }
            return user;
        }

        public async Task<UserModel> GetById(int id)
        {
            var cacheConnection = _cacheFactory.GetCache();
            var user = _staffUserCache.Get(cacheConnection, id);
            if (user == null)
            {
                using (var connection = await _connectionFactory.CreateAsync())
                {
                    user = await _repository.GetById(connection, id);
                }
                _staffUserCache.Put(cacheConnection, id, user);
            }
            return user;
        }

        public UserModel GetByIdSync(int id)
        {
            var cacheConnection = _cacheFactory.GetCache();
            var user = _staffUserCache.Get(cacheConnection, id);
            if (user == null)
            {
                using (var connection = _connectionFactory.CreateSync())
                {
                    user = _repository.GetByIdSync(connection, id);
                }
                _staffUserCache.Put(cacheConnection, id, user);
            }
            return user;
        }

        public async Task SetPassword(int id, string password)
        {
            using (var connection = await _connectionFactory.CreateAsync())
            {
                await _repository.ResetPassword(connection, id, _passwordHasher.CreateHash(password));
            }
        }

        public async Task Activate(int id)
        {
            using (var connection = await _connectionFactory.CreateAsync())
            {
                await _repository.Activate(connection, id);
            }
            var cacheConnection = _cacheFactory.GetCache();
            _staffUserCache.Put(cacheConnection, id, null);
        }

        public async Task Deactivate(int id)
        {
            using (var connection = await _connectionFactory.CreateAsync())
            {
                await _repository.Deactivate(connection, id);
            }
            var cacheConnection = _cacheFactory.GetCache();
            _staffUserCache.Put(cacheConnection, id, null);
        }

        public async Task Create(UserDto user)
        {
            if (user.Login == null || user.Password == null || (user.Role == Role.Rcoi && !user.RegionId.HasValue))
            {
                return;
            }
            if (user.Role != Role.Rcoi)
            {
                user.IsEnabled = true;
            }
            using (var connection = await _connectionFactory.CreateAsync())
            {
                await _repository.Create(connection, user.Login, _passwordHasher.CreateHash(user.Password), user.Role,
                                       user.RegionId, user.IsEnabled);
            }
        }

        public async Task Update(UserDto user)
        {
            if (user.Role == Role.Rcoi && !user.RegionId.HasValue)
            {
                return;
            }
            if (user.Role == Role.Fct)
            {
                user.IsEnabled = true;
            }
            using (var connection = await _connectionFactory.CreateAsync())
            {
                await _repository.Update(connection, user.Id,
                                         user.Password != null ? _passwordHasher.CreateHash(user.Password) : null,
                                         user.Role, user.RegionId, user.IsEnabled);
            }
            var cacheConnection = _cacheFactory.GetCache();
            _staffUserCache.Put(cacheConnection, user.Id, null);
        }

        public async Task<UserDtoPage> Get(string login, int? regionId, Role? role, int take, int skip)
        {
            using (var connection = await _connectionFactory.CreateAsync())
            {
                return await _repository.Get(connection, login, regionId, role, take, skip);
            }
        }

        public async Task<UserDto> GetDtoById(int id)
        {
            using (var connection = await _connectionFactory.CreateAsync())
            {
                return await _repository.GetDtoById(connection, id);
            }
        }

        public async Task<bool> Delete(int id)
        {
            using (var connection = await _connectionFactory.CreateAsync())
            {
                return await _repository.Delete(connection, id);
            }
        }
    }
}