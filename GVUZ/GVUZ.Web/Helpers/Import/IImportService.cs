using System.ServiceModel;
using System.ServiceModel.Web;
using System.Xml.Linq;

namespace GVUZ.Web.Import {
	/// <summary>
	/// Интерфейс сервиса импорта
	/// </summary>
	[ServiceContract]
	[ServiceErrorPolicyBehaviorAttribute]
	public interface IImportService {
		[OperationContract]
		[WebInvoke(UriTemplate="/dictionary")]
		XElement GetDictionariesList(XElement data);

		[OperationContract]
		[WebInvoke(UriTemplate="/dictionarydetails")]
		XElement GetDictionaryDetails(XElement data);

		[OperationContract]
		[WebInvoke(UriTemplate="/institutioninfo")]
		XElement GetInstitutionInfo(XElement data);

		[OperationContract]
		[WebInvoke(UriTemplate="/institutioninfo/partof")]
		XElement GetInstitutionPartOfInfo(XElement data);

		[OperationContract]
		[WebInvoke(UriTemplate="/import",RequestFormat=WebMessageFormat.Xml,ResponseFormat=WebMessageFormat.Xml)]
		XElement DoImport(XElement data);

		[OperationContract]
		[WebInvoke(UriTemplate="/import/application/single",RequestFormat=WebMessageFormat.Xml,ResponseFormat=WebMessageFormat.Xml)]
		XElement DoImportApplicationSingle(XElement data);

		[OperationContract]
		[WebInvoke(UriTemplate="/import/result",RequestFormat=WebMessageFormat.Xml,ResponseFormat=WebMessageFormat.Xml)]
		XElement GetImportResult(XElement data);

		[OperationContract]
		[WebInvoke(UriTemplate="/validate",RequestFormat=WebMessageFormat.Xml,ResponseFormat=WebMessageFormat.Xml)]
		XElement DoValidate(XElement data);

		[OperationContract]
		[WebInvoke(UriTemplate="/delete",RequestFormat=WebMessageFormat.Xml,ResponseFormat=WebMessageFormat.Xml)]
		XElement DoDelete(XElement data);

		[OperationContract]
		[WebInvoke(UriTemplate="/delete/result",RequestFormat=WebMessageFormat.Xml,ResponseFormat=WebMessageFormat.Xml)]
		XElement GetDeleteResult(XElement data);

		[OperationContract]
		[WebInvoke(UriTemplate="/checkapplication",RequestFormat=WebMessageFormat.Xml,ResponseFormat=WebMessageFormat.Xml)]
		XElement DoCheckApplication(XElement data);

		[OperationContract]
		[WebInvoke(UriTemplate="/checkapplication/single",RequestFormat=WebMessageFormat.Xml,ResponseFormat=WebMessageFormat.Xml)]
		XElement DoCheckApplicationSingle(XElement data);

		[OperationContract]
		[WebGet(UriTemplate="/test/import")]
		XElement GetTestImport();

		[OperationContract]
		[WebGet(UriTemplate="/test/delete")]
		XElement GetTestRemove();

		[OperationContract]
		[WebGet(UriTemplate="/test/dictionary")]
		XElement GetTestDictionariesList();

		[OperationContract]
		[WebGet(UriTemplate="/test/dictionarydetails",RequestFormat=WebMessageFormat.Xml,ResponseFormat=WebMessageFormat.Xml)]
		XElement GetTestDictionaryDetails();

		[OperationContract]
		[WebGet(UriTemplate="/test/checkapplication",RequestFormat=WebMessageFormat.Xml,ResponseFormat=WebMessageFormat.Xml)]
		XElement GetTestCheckApplication();
	}
}
