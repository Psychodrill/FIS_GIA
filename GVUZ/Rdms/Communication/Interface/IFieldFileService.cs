using System.ServiceModel;

namespace Rdms.Communication.Interface
{
	[ServiceContract]
	public interface IFieldFileService
	{
		[OperationContract]
		void UploadFieldFileBody(int versionId, int fieldId, int recordId, byte[] body);

		[OperationContract]
		byte[] DownloadFieldFileBody(int versionId, int fieldId, int recordId);
	}
}