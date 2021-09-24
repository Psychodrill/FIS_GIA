using System.Collections.Generic;
using System.ServiceModel;
using Rdms.Communication.Entities;

namespace Rdms.Communication.Interface
{
	[ServiceContract]
	public interface IFileService
	{
		[OperationContract]
		int CreateFile(Document file);

		[OperationContract]
		void UpdateFile(Document file);

		//[OperationContract]
		//void DeleteFile(int fileId);

		//[OperationContract]
		//Document GetFile(int fieldId);

		[OperationContract]
		List<Document> GetFileList();

		#region Operations with file body

		[OperationContract]
		void UploadFileBody(int fileId, byte[] body);

		[OperationContract]
		byte[] DownloadFileBody(int documentId);

		#endregion
	}
}