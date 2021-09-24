namespace Ege.Hsc.Dal.Store.Ninject
{
    using System.Collections.Generic;
    using Ege.Dal.Common.Mappers;
    using Ege.Hsc.Dal.Entities;
    using Ege.Hsc.Dal.Store.Mappers;
    using Ege.Hsc.Dal.Store.Mappers.Blanks;
    using Ege.Hsc.Dal.Store.Mappers.Requests;
    using Ege.Hsc.Dal.Store.Mappers.Servers;
    using Ege.Hsc.Dal.Store.Repositories;
    using Ege.Hsc.Logic.Models.Blanks;
    using Ege.Hsc.Logic.Models.Servers;
    using global::Ninject.Modules;

    public class EgeHscDalNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserRepository>().To<UserRepository>().InSingletonScope();
            Bind<IDataReaderMapper<User>>().To<UserMapper>().InSingletonScope();
            Bind<IDataReaderSyncMapper<User>>().To<UserMapper>().InSingletonScope();

            Bind<IBlankDownloadRepository>().To<BlankDownloadRepository>().InSingletonScope();
            Bind<IDataReaderListMapper<BlankToDownload>>().To<BlankToDownloadDataReaderCollectionMapper>().InSingletonScope();
            Bind<IDataReaderMapper<LoadedBlanks>>()
                .To<LoadedBlankMapper>()
                .InSingletonScope();
            Bind<IDataTableMapper<IEnumerable<BlankDownload>>>().To<BlankTableMapper>().InSingletonScope();

            Bind<IBlankRequestRepository>().To<BlankRequestRepository>().InSingletonScope();
            Bind<IDataReaderCollectionMapper<BlankRequest>>().To<BlankRequestMapper>().InSingletonScope();
            Bind<IDataReaderMapper<RequestedParticipant>>().To<RequestedParticipantMapper>().InSingletonScope();
            Bind<IDataReaderMapper<RequestDataPage>>().To<RequestDataMapper>().InSingletonScope();
            Bind<IDataTableMapper<IEnumerable<RequestedParticipant>>>()
                .To<RequestedParticipantTableMapper>()
                .InSingletonScope();
            Bind<IDataReaderCollectionMapper<BlankServerAvailabilityModel>>()
                .To<ServerAvailabilityModelMapper>()
                .InSingletonScope();
            Bind<IDataTableMapper<IDictionary<int, bool>>>().To<ServerAvailabilityTableMapper>().InSingletonScope();
            Bind<IDataReaderCollectionMapper<ServerBlanks>>().To<ServerBlanksMapper>().InSingletonScope();
            Bind<IDataTableMapper<IEnumerable<ServerErrors>>>().To<ServerErrorsTableMapper>().InSingletonScope();
            Bind<IDataReaderCollectionMapper<BlankServerStatus>>().To<ServerStatusMapper>().InSingletonScope();
            Bind<IDataReaderCollectionMapper<BlankServerError>>().To<ServerErrorMapper>().InSingletonScope();
            Bind<IDataReaderMapper<RequestPermission>>().To<RequestPermissionMapper>().InSingletonScope();

            Bind<IParticipantRepository>().To<ParticipantRepository>().InSingletonScope();

            Bind<IRegionServerRepository>().To<RegionServerRepository>().InSingletonScope();

            Bind<IStateRepository>().To<StateRepository>().InSingletonScope();

            Bind<IDataTableMapper<IEnumerable<DownloadedBlank>>>().To<DownloadedBlankMapper>().InSingletonScope();
        }
    }

    public abstract class ExtendedNinjectModule : NinjectModule
    {
        protected void BindSingleton<TInterface, TImplementation>()
            where TImplementation : TInterface
        {
            Bind<TInterface>().To<TImplementation>().InSingletonScope();
        }
    }
}
