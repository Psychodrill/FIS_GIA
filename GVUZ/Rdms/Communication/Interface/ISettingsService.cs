using System.ServiceModel;
using Rdms.Communication.Entities;

namespace Rdms.Communication.Interface
{
	[ServiceContract]
	public interface ISettingsService
	{
		[OperationContract]
		string GetDocumentStorageFolder();

		[OperationContract]
		void SetDocumentStorageFolder(string path);

		[OperationContract]
		SmtpSettings GetSmtpSettings();

		[OperationContract]
		void SetSmtpSettings(SmtpSettings settings);
	}
}