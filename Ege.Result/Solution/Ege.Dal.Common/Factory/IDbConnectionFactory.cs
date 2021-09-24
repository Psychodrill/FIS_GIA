namespace Ege.Dal.Common.Factory
{
    using System.Data.Common;
    using Ege.Check.Dal.Store.Factory;
    using JetBrains.Annotations;

    public interface IDbConnectionFactory : IConnectionFactory<DbConnection>
    {
        [NotNull]
        DbConnection CreateSync();

        [NotNull]
        DbConnection CreateHscSync();
    }
}