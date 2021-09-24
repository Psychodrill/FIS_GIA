using System.ServiceModel;
using Rdms.Communication.Entities;
using Rdms.Communication.Entities.Constraint;
using Rdms.Communication.Exceptions;

namespace Rdms.Communication.Interface
{
	[ServiceContract]
	[ServiceKnownType(typeof (IntConstraint))]
	[ServiceKnownType(typeof (DecimalConstraint))]
	[ServiceKnownType(typeof (DateTimeConstraint))]
	[ServiceKnownType(typeof (TextConstraint))]
	public interface IVersionService
	{
		[OperationContract]
		VersionDescription GetVersionDescription(int id);

		[OperationContract]
		VersionStructure GetVersionStructure(int id);

		[OperationContract]
		[FaultContract(typeof (UniqueConstraintViolationException))]
		[FaultContract(typeof (MandatoryValueMissedException))]
		[FaultContract(typeof (NameAlreadyInUseException))]
		int CreateVersion(VersionDescription description, VersionStructure structure);

		/// <returns>True, if table structure changed.</returns>
		[OperationContract]
		[FaultContract(typeof (UniqueConstraintViolationException))]
		[FaultContract(typeof (MandatoryValueMissedException))]
		[FaultContract(typeof (NameAlreadyInUseException))]
		[FaultContract(typeof (ModifyNonDevelopmentVersionException))]
		bool UpdateVersion(VersionDescription description, VersionStructure structure);

		[OperationContract]
		[FaultContract(typeof (IllegalDeletingException))]
		void DeleteVersion(int versionId);
	}
}