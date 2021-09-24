namespace Ege.Check.Logic.Services
{
    using System;
    using System.ServiceModel;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Services;
    using JetBrains.Annotations;

    [ServiceContract]
    public interface IEgeDataService
    {
        [OperationContract]
        Task<EgeServiceResponse> StartMergeRegions();

        [OperationContract]
        Task<EgeServiceResponse> StartMergeSubjects();

        [OperationContract]
        Task<EgeServiceResponse> StartMergeExams();

        [OperationContract]
        Task<EgeServiceResponse> StartMergeParticipants();

        [OperationContract]
        Task<EgeServiceResponse> StartMergeParticipantExams();

        [OperationContract]
        Task<EgeServiceResponse> StartMergeBlanks();

        [OperationContract]
        Task<EgeServiceResponse> StartMergeAppeals();

        [OperationContract]
        Task<EgeServiceResponse> StartMergeAnswers();

        [OperationContract]
        Task<EgeServiceResponse> StartMergeParticipantExamLinks();

        [OperationContract]
        Task<EgeServiceResponse> MergeRegions([NotNull] EgeServiceRequest request);

        [OperationContract]
        Task<EgeServiceResponse> MergeSubjects([NotNull] EgeServiceRequest request);

        [OperationContract]
        Task<EgeServiceResponse> MergeExams([NotNull] EgeServiceRequest request);

        [OperationContract]
        Task<EgeServiceResponse> MergeParticipants([NotNull] EgeServiceRequest request);

        [OperationContract]
        Task<EgeServiceResponse> MergeParticipantExams([NotNull] EgeServiceRequest request);

        [OperationContract]
        Task<EgeServiceResponse> MergeBlanks([NotNull] EgeServiceRequest request);

        [OperationContract]
        Task<EgeServiceResponse> MergeAppeals([NotNull] EgeServiceRequest request);

        [OperationContract]
        Task<EgeServiceResponse> MergeAnswers([NotNull] EgeServiceRequest request);

        [OperationContract]
        Task<EgeServiceResponse> MergeParticipantExamLinks([NotNull] EgeServiceRequest request);

        [OperationContract]
        Task<EgeServiceResponse> FinalizeMergeRegions();

        [OperationContract]
        Task<EgeServiceResponse> FinalizeMergeSubjects();

        [OperationContract]
        Task<EgeServiceResponse> FinalizeMergeExams();

        [OperationContract]
        Task<EgeServiceResponse> FinalizeMergeParticipants();

        [OperationContract]
        Task<EgeServiceResponse> FinalizeMergeParticipantExams();

        [OperationContract]
        Task<EgeServiceResponse> FinalizeMergeBlanks();

        [OperationContract]
        Task<EgeServiceResponse> FinalizeMergeAppeals();

        [OperationContract]
        Task<EgeServiceResponse> FinalizeMergeAnswers();

        [OperationContract]
        Task<EgeServiceResponse> FinalizeMergeParticipantExamLinks();

        [OperationContract]
        EgeBatchSizeServiceResponse GetBatchSize([NotNull] EgeBatchSizeServiceRequest request);

        [OperationContract]
        Task<string> GetParticipantCode(Guid rbdId);

        [OperationContract]
        void ErrorOnExecute(string taskName, string errorCode, string errorDescription, [MessageParameter(Name = "SourceName")]string sourceName);
    }
}
