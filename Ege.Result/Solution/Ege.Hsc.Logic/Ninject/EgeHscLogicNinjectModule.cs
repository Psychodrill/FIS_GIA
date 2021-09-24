namespace Ege.Hsc.Logic.Ninject
{
    using System.Collections.Generic;
    using Ege.Check.Common;
    using Ege.Check.Logic.Models.Requests;
    using Ege.Hsc.Dal.Entities;
    using Ege.Hsc.Logic.Blanks;
    using Ege.Hsc.Logic.Configuration;
    using Ege.Hsc.Logic.Csv;
    using Ege.Hsc.Logic.Mappers;
    using Ege.Hsc.Logic.Models.Blanks;
    using Ege.Hsc.Logic.Models.Requests;
    using Ege.Hsc.Logic.Models.Servers;
    using Ege.Hsc.Logic.Requests;
    using Ege.Hsc.Logic.Servers;
    using global::Ninject.Modules;

    public class EgeHscLogicNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBlankApplicationSettings>().To<ConfigBlankApplicationSettings>().InSingletonScope();
            Bind<IBlankDownloader>().To<BlankDownloader>().InSingletonScope();
            Bind<IBlankService>().To<BlankService>().InSingletonScope();
            Bind<IFilePathHelper>().To<FilePathHelper>().InSingletonScope();
            Bind<IFileWriter>().To<FileWriter>().InSingletonScope();
            Bind<IBlankZipper>().To<BlankZipper>().InSingletonScope();
            Bind<IRequestService>().To<RequestService>().InSingletonScope();
            Bind<IServerService>().To<ServerService>().InSingletonScope();
            Bind<IServerChecker>().To<ServerChecker>().InSingletonScope();
            Bind<IServerFileParser>().To<ServerFileParser>().InSingletonScope();
            Bind<IErrorFileCreator>().To<ErrorFileCreator>().InSingletonScope();

            Bind<IMapper<BlankToDownload, Blank>>().To<BlankToDownloadEntityToBlankMapper>().InSingletonScope();
            Bind<IMapper<RequestData, RequestStatus>>().To<RequestDataToRequestStatusMapper>().InSingletonScope();
            Bind<IMapper<ParticipantBlankRequest, string>>().To<ParticipantNameExtractor>().InSingletonScope();
            Bind<IMapper<ParticipantBlankRequest, RequestedParticipant>>()
                .To<RequestToParticipantMapper>()
                .InSingletonScope();
            Bind<IMapper<BlankServerError, BlankServerErrorExcelModel>>()
                .To<BlankServerErrorToExcelModelMapper>()
                .InSingletonScope();
            Bind<IMapper<BlankRequest, ParticipantErrorCollectionExcelModel>>()
                .To<RequestErrorMapper>()
                .InSingletonScope();

            Bind<IRequestCsvParser>().To<RequestCsvParser>().InSingletonScope();

            Bind<IEqualityComparer<ParticipantBlankRequest>>()
                .To<ParticipantBlankRequestEqualityComparer>()
                .InSingletonScope()
                .Named(ParticipantBlankRequestEqualityComparer.Characteristic);
            Bind<IHscSettings>().To<HscSettings>().InSingletonScope();
            Bind<IInvalidPngRemover>().To<InvalidPngRemover>().InSingletonScope();
        }
    }
}