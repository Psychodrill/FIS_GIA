namespace Ege.Hsc.Dal.Store.Repositories
{
    using System;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Hsc.Dal.Entities;
    using JetBrains.Annotations;

    public interface IUserRepository
    {
        [NotNull]
        Task<User> GetByLoginAsync([NotNull]DbConnection connection, string login);

        User GetByLoginSync([NotNull]DbConnection connection, string login);

        [NotNull]
        Task<User> MergeAsync([NotNull]DbConnection connection, string login, Guid ticket);
    }
}