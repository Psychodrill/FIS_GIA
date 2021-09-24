namespace Ege.Check.Dal.Store.Factory
{
    using System.Data;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public interface IConnectionFactory<T> where T : IDbConnection
    {
        [NotNull]
        Task<T> CreateAsync();

        [NotNull]
        Task<T> CreateHscAsync();
    }
}