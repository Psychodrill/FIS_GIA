namespace Fbs.Utility.Interfaces
{
    /// <summary>
    /// интерфейс для кэша хранения разрешений пользователя
    /// </summary>
    public interface IUserRolesCacheStorage : ICacheStorage
    {
        // сам интерфейс нужен для IoC
    }
}
