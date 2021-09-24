namespace Ege.Dal.Common.Ninject
{
    using System.Data.Common;
    using System.Data.SqlClient;
    using Ege.Check.Dal.Store.Bulk.Load;
    using Ege.Check.Dal.Store.Factory;
    using Ege.Dal.Common.Bulk;
    using Ege.Dal.Common.Factory;
    using Ege.Dal.Common.Helpers;
    using global::Ninject.Modules;

    public class EgeDalCommonNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDbConnectionFactory>().To<SqlConnectionFactory>().InSingletonScope();
            Bind<IConnectionFactory<SqlConnection>>().To<SqlConnectionFactory>().InSingletonScope();
            Bind<IConnectionFactory<DbConnection>>().To<SqlConnectionFactory>().InSingletonScope();
            Bind<IConnectionStringProvider>().To<ConfigConnectionStringProvider>().InSingletonScope();
            Bind<IDbTypeFactory>().To<DbTypeFactory>().InSingletonScope();
            Bind<IBulkLoader>().To<BulkLoader>().InSingletonScope();
            Bind<IDataMerger>().To<DataMerger>().InSingletonScope();
        }
    }
}