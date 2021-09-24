using System.Collections.Generic;
using System.ServiceModel;
using Rdms.Communication.Entities;
using Rdms.Communication.Exceptions;

namespace Rdms.Communication.Interface
{
	[ServiceContract]
	public interface IUserService
	{
		[OperationContract]
		UserDescription GetUserDescription(string login);

		[OperationContract]
		List<DirectoryAccess> GetUserPermission(string login);

		[OperationContract]
		List<UserDescription> GetAllUserDescriptions();

		[OperationContract]
		[FaultContract(typeof (LoginAlreadyInUseException))]
		[FaultContract(typeof (MandatoryValueMissedException))]
		void CreateUser(UserDescription description, List<DirectoryAccess> credentials);

		[OperationContract]
		[FaultContract(typeof (MandatoryValueMissedException))]
		void UpdateUser(UserDescription description, List<DirectoryAccess> credentials);

		[OperationContract]
		void DeleteUser(string login);
	}
}