namespace Ege.Check.Logic.Services
{
    using System.Threading.Tasks;
    using Common.Logging;
    using Ege.Check.Logic.LoadServices.Preprocessing;
    using Ege.Check.Logic.Models.Services;
    using Ege.Check.Logic.Services.Attributes;
    using Ege.Check.Logic.Services.Dtos.Models;
    using Ege.Check.Logic.Services.Load;
    using Ege.Check.Logic.Services.Participant.Participants;
    using JetBrains.Annotations;
    using LogManager = Common.Logging.LogManager;

    [LoggingServiceBehavior]
    public class EgeDataService : IEgeDataService
    {
        [NotNull] private readonly IDataLoader<AnswerDto> _answersLoader;
        [NotNull] private readonly IDataLoader<AppealDto> _appealsLoader;
        [NotNull] private readonly IBatchSizeSettingsReader _batchSizeSettingsReader;
        [NotNull] private readonly IDataLoader<BlankInfoDto> _blanksLoader;
        [NotNull] private readonly IDataLoader<ExamDto> _examsLoader;
        [NotNull] private readonly IDataLoader<ParticipantExamDto> _participantExamsLoader;
        [NotNull] private readonly IDataLoader<ParticipantDto> _participantsLoader;
        [NotNull] private readonly IDataLoader<RegionDto> _regionsLoader;
        [NotNull] private readonly IDataLoader<SubjectDto> _subjectsLoader;
        [NotNull] private readonly IDataLoader<ParticipantExamLinkDto> _participantExamLinksLoader;
        [NotNull] private readonly IParticipantService _participantService;
        [NotNull] private static readonly ILog Log = LogManager.GetLogger<EgeDataService>();
        
        public EgeDataService(
            [NotNull] IDataLoader<RegionDto> regionsLoader,
            [NotNull] IDataLoader<SubjectDto> subjectsLoader,
            [NotNull] IDataLoader<ExamDto> examsLoader,
            [NotNull] IDataLoader<ParticipantDto> participantsLoader,
            [NotNull] IDataLoader<ParticipantExamDto> participantExamsLoader,
            [NotNull] IDataLoader<BlankInfoDto> blanksLoader,
            [NotNull] IDataLoader<AppealDto> appealsLoader,
            [NotNull] IDataLoader<AnswerDto> answersLoader,
            [NotNull] IBatchSizeSettingsReader batchSizeSettingsReader, 
            [NotNull] IDataLoader<ParticipantExamLinkDto> participantExamLinksLoader,
            [NotNull] IParticipantService participantService)
        {
            _regionsLoader = regionsLoader;
            _subjectsLoader = subjectsLoader;
            _examsLoader = examsLoader;
            _participantsLoader = participantsLoader;
            _participantExamsLoader = participantExamsLoader;
            _blanksLoader = blanksLoader;
            _appealsLoader = appealsLoader;
            _answersLoader = answersLoader;
            _batchSizeSettingsReader = batchSizeSettingsReader;
            _participantExamLinksLoader = participantExamLinksLoader;
            _participantService = participantService;
        }
        public async Task<EgeServiceResponse> StartMergeRegions()
        {
            return await _regionsLoader.StartLoadData();
        }

        public async Task<EgeServiceResponse> StartMergeSubjects()
        {
            return await _subjectsLoader.StartLoadData();
        }

        public async Task<EgeServiceResponse> StartMergeExams()
        {
            return await _examsLoader.StartLoadData();
        }

        public async Task<EgeServiceResponse> StartMergeParticipants()
        {
            return await _participantsLoader.StartLoadData();
        }

        public async Task<EgeServiceResponse> StartMergeParticipantExams()
        {
            return await _participantExamsLoader.StartLoadData();
        }

        public async Task<EgeServiceResponse> StartMergeBlanks()
        {
            await _answersLoader.StartLoadData();
            return await _blanksLoader.StartLoadData();
        }

        public async Task<EgeServiceResponse> StartMergeAppeals()
        {
            return await _appealsLoader.StartLoadData();
        }

        public async Task<EgeServiceResponse> StartMergeAnswers()
        {
          //  return new EgeServiceResponse();
            return await _answersLoader.StartLoadData();
        }

        public async Task<EgeServiceResponse> StartMergeParticipantExamLinks()
        {
            return await _participantExamLinksLoader.StartLoadData();
        }

        public async Task<EgeServiceResponse> MergeRegions(EgeServiceRequest request)
        {
            return await _regionsLoader.LoadData(request);
        }

        public async Task<EgeServiceResponse> MergeSubjects(EgeServiceRequest request)
        {
            return await _subjectsLoader.LoadData(request);
        }
        
        public async Task<EgeServiceResponse> MergeExams(EgeServiceRequest request)
        {
            return await _examsLoader.LoadData(request);
        }

        public async Task<EgeServiceResponse> MergeParticipants(EgeServiceRequest request)
        {
            return await _participantsLoader.LoadData(request);
        }

        public async Task<EgeServiceResponse> MergeParticipantExams(EgeServiceRequest request)
        {
            return await _participantExamsLoader.LoadData(request);
        }

        public async Task<EgeServiceResponse> MergeBlanks(EgeServiceRequest request)
        {
            return await _blanksLoader.LoadData(request);
        }

        public async Task<EgeServiceResponse> MergeAppeals(EgeServiceRequest request)
        {
            return await _appealsLoader.LoadData(request);
        }

        public async Task<EgeServiceResponse> MergeAnswers(EgeServiceRequest request)
        {
            return await _answersLoader.LoadData(request);
        }

        public async Task<EgeServiceResponse> MergeParticipantExamLinks(EgeServiceRequest request)
        {
            return await _participantExamLinksLoader.LoadData(request);
        }

        public async Task<EgeServiceResponse> FinalizeMergeRegions()
        {
   //         return new EgeServiceResponse();

            return await _regionsLoader.FinalizeLoadData();
        }

        public async Task<EgeServiceResponse> FinalizeMergeSubjects()
        {
  //          return new EgeServiceResponse();
            return await _subjectsLoader.FinalizeLoadData();
        }

        public async Task<EgeServiceResponse> FinalizeMergeExams()
        {
    //        return new EgeServiceResponse();
            return await _examsLoader.FinalizeLoadData();
        }

        public async Task<EgeServiceResponse> FinalizeMergeParticipants()
        {
//            return new EgeServiceResponse();
            return await _participantsLoader.FinalizeLoadData();
        }

        public async Task<EgeServiceResponse> FinalizeMergeParticipantExams()
        {
     //       return new EgeServiceResponse();

            return await _participantExamsLoader.FinalizeLoadData();
        }

        public async Task<EgeServiceResponse> FinalizeMergeBlanks()
        {
    //        return new EgeServiceResponse();
            return await _blanksLoader.FinalizeLoadData();
        }

        public async Task<EgeServiceResponse> FinalizeMergeAppeals()
        {
         //   return new EgeServiceResponse();
            return await _appealsLoader.FinalizeLoadData();
        }

        public async Task<EgeServiceResponse> FinalizeMergeAnswers()
        {
     //       return new EgeServiceResponse();
            return await _answersLoader.FinalizeLoadData();
        }

        public async Task<EgeServiceResponse> FinalizeMergeParticipantExamLinks()
        {
            //return new EgeServiceResponse();
            return await _participantExamLinksLoader.FinalizeLoadData();
        }

        public EgeBatchSizeServiceResponse GetBatchSize(EgeBatchSizeServiceRequest request)
        {
            return new EgeBatchSizeServiceResponse
                {
                    BatchSize = _batchSizeSettingsReader.Read(request.Dto)
                };
        }

        public void ErrorOnExecute(string taskName, string errorCode, string errorDescription, string sourceName)
        {
            Log.ErrorFormat("Error occurred in task: {0}. Error code: {1}. Source: {2}\r\n Error description: {3}", taskName, errorCode, sourceName,
                            errorDescription);
        }

        public async Task<string> GetParticipantCode(System.Guid rbdId)
        {
            Log.TraceFormat("Requested code of '{0}'", rbdId);
            var result = await _participantService.GetCodeByRbdId(rbdId);
            Log.TraceFormat("Result for '{0}' : '{1}'", rbdId, result);
            return result;
        }
    }
}
