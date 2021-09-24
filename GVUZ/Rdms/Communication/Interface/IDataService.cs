using System.Collections.Generic;
using System.ServiceModel;
using Rdms.Communication.Entities;
using Rdms.Communication.Exceptions;

namespace Rdms.Communication.Interface
{
	[ServiceContract]
	public interface IDataService
	{
		[OperationContract]
		[FaultContract(typeof (MandatoryValueMissedException))]
		[FaultContract(typeof (UniqueConstraintViolationException))]
		[FaultContract(typeof (ModifyNonDevelopmentVersionException))]
		int CreateRecord(int versionId, RecordContent record);

		[OperationContract]
		[FaultContract(typeof (MandatoryValueMissedException))]
		[FaultContract(typeof (UniqueConstraintViolationException))]
		[FaultContract(typeof (ModifyNonDevelopmentVersionException))]
		int[] ImportRecords(int versionId, RecordContent[] records);

		[OperationContract]
		[FaultContract(typeof (ModifyNonDevelopmentVersionException))]
		void DeleteRecord(int versionId, int recordId);

		[OperationContract]
		List<RecordContent> GetRecords(int versionId);

		[OperationContract]
		[FaultContract(typeof (MandatoryValueMissedException))]
		[FaultContract(typeof (UniqueConstraintViolationException))]
		[FaultContract(typeof (ModifyNonDevelopmentVersionException))]
		void UpdateRecord(int versionId, RecordContent record);

		[OperationContract]
		[FaultContract(typeof (ModifyNonDevelopmentVersionException))]
		void ClearRecords(int versionId);
	}
}