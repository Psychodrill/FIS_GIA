using System;
using System.ServiceModel;

namespace Rdms.Communication.Interface
{
	[ServiceContract]
	public interface IExportService
	{
		[OperationContract]
		byte[] BuildPackage(int versionId);

		[OperationContract]
		byte[] GetByDate(int directoryId, DateTime date);
	}
}