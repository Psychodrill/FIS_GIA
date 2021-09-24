using System.Collections.Generic;
using System.ServiceModel;
using Rdms.Communication.Entities;

namespace Rdms.Communication.Interface
{
	public enum DocumentRelationType
	{
		Directory = 1,
		Version = 2,
		Record = 3
	}

	[ServiceContract]
	public interface IDocumentService
	{
		[OperationContract]
		int CreateDocument(DocumentRelationType relationType, int relationId, int? versionId, Document document);

		[OperationContract]
		void DeleteDocument(int documentId);

		[OperationContract]
		Document GetDocument(int documentId);

		[OperationContract]
		List<Document> GetDocuments(DocumentRelationType relationType, int relationId, int? versionId);

		#region Operations with document body

		[OperationContract]
		void UploadDocumentBody(int documentId, byte[] body);

		[OperationContract]
		byte[] DownloadDocumentBody(int documentId);

		#endregion
	}
}