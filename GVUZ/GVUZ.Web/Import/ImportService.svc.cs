using System;
using System.ServiceModel.Activation;
using System.Xml.Linq;
using FogSoft.Helpers;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.SQL.Dictionaries;
using GVUZ.Web.Import.Infractrusture;

namespace GVUZ.Web.Import {
	/// <summary>
	/// Сервис импорта
	/// </summary>
	[AspNetCompatibilityRequirements(RequirementsMode=AspNetCompatibilityRequirementsMode.Allowed)]
	public class ImportService:IImportService {
		/// <summary>
		/// Получение списка справочников
		/// </summary>
		public XElement GetDictionariesList(XElement data) {
			using(var importer=new GetDictionariesListService(data)) {
				return importer.ProcessData();
			}
		}

		/// <summary>
		/// Получение справочника
		/// </summary>
		public XElement GetDictionaryDetails(XElement data) {
			using(var importer=new GetDictionaryDetailsService(data)) {
				return importer.ProcessData();
			}
		}

		/// <summary>
		/// Информация о вузе
		/// </summary>
		public XElement GetInstitutionInfo(XElement data) {
			using(var importer=new GetInstitutionInfoService(data)) {
				return importer.ProcessData();
			}
		}

		/// <summary>
		/// Получение части сведений о вузе
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public XElement GetInstitutionPartOfInfo(XElement data) {
			using(var importer=new GetInstitutionPartOfInfoService(data)) {
				return importer.ProcessData();
			}
		}

		/// <summary>
		/// Импорт пакета
		/// </summary>
		public XElement DoImport(XElement data) {
			using(var importer=new DoImportService(data)) {
				return importer.ProcessData();
			}
		}

		/// <summary>
		/// Импрорт одного заявления с возвращением результата проверки заявления в ФБС
		/// </summary>
		public XElement DoImportApplicationSingle(XElement data) {
			try {
				using(var importer=new DoImportApplicationSingleService(data)) {
					return importer.ProcessData();
				}
			} catch(Exception ex) {
				LogHelper.Log.ErrorFormat("Импорт 1 заявления: {0}",ex.Message);
				throw;
			}
		}

		/// <summary>
		/// Валидация пакета
		/// </summary>
		public XElement DoValidate(XElement data) {
			using(var importer=new DoValidateService(data)) {
				return importer.ProcessData();
			}
		}

		/// <summary>
		/// Удаление
		/// </summary>
		public XElement DoDelete(XElement data) {
			using(var importer=new DoDeleteService(data)) {
				return importer.ProcessData();
			}
		}

		/// <summary>
		/// Результат проверки заявлений
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public XElement DoCheckApplication(XElement data) {
			using(var importer=new GetResultCheckApplicationService(data)) {
				return importer.ProcessData();
			}
		}

		/// <summary>
		/// Проверка одного заявления в ФБС
		/// </summary>
		public XElement DoCheckApplicationSingle(XElement data) {
			using(var importer=new DoCheckApplicationSingleService(data)) {
				return importer.ProcessData();
			}
		}

		/// <summary>
		/// Получение результата импорта
		/// </summary>
		public XElement GetImportResult(XElement data) {
			using(var importer=new GetResultImportApplicationService(data)) {
				return importer.ProcessData();
			}
		}

		/// <summary>
		/// Получение результата удаления
		/// </summary>
		public XElement GetDeleteResult(XElement data) {
			using(var importer=new GetResultDeleteApplicationService(data)) {
				return importer.ProcessData();
			}
		}

		/// <summary>
		/// Тестовый ответ импорта
		/// </summary>
		/// <returns></returns>
		public XElement GetTestImport() {
			return XElement.Parse(
				@"<ImportResultPackage xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><PackageID>181</PackageID><Log><Successful><AdmissionVolumes>0</AdmissionVolumes><CompetitiveGroupItems>0</CompetitiveGroupItems><Applications>0</Applications><OrdersOfAdmissions>0</OrdersOfAdmissions></Successful><Failed /></Log><Conflicts><Applications /><CompetitiveGroupItems><CompetitiveGroupItemID>3</CompetitiveGroupItemID></CompetitiveGroupItems></Conflicts></ImportResultPackage>
			");
		}

		/// <summary>
		/// Тестовый ответ удаления
		/// </summary>
		public XElement GetTestRemove() {
			return XElement.Parse(PackageHelper.GenerateXmlPackageIntoString(ServiceTestResults.CreateDeleteResultPackage()));
		}

		/// <summary>
		/// Тестовый ответ справочников
		/// </summary>
		public XElement GetTestDictionariesList() {
			return XElement.Parse(DictionaryExporterSql.GetDictionariesListDtoString());
		}

		/// <summary>
		/// Тестовый ответ справочника
		/// </summary>
		public XElement GetTestDictionaryDetails() {
            var exporter = new DictionaryExporterSql(1);
			return XElement.Parse(exporter.GetTestDictionaryDataString(1));
		}

		/// <summary>
		/// Тестовый ответ результат проверки заявлений
		/// </summary>
		public XElement GetTestCheckApplication() {
			return XElement.Parse(
				"<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ImportedAppCheckResultPackage xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n  <PackageID>43</PackageID>\r\n  <StatusCheckCode>3</StatusCheckCode>\r\n  <StatusCheckMessage>Processed</StatusCheckMessage>\r\n  <EgeDocumentCheckResults>\r\n    <EgeDocumentCheckResult>\r\n      <Application>\r\n        <ApplicationNumber>45245245</ApplicationNumber>\r\n        <RegistrationDate>2010-08-12</RegistrationDate>\r\n      </Application>\r\n      <EgeDocuments>\r\n        <EgeDocument>\r\n          <StatusCode>2</StatusCode>\r\n          <StatusMessage>Verified</StatusMessage>\r\n          <DocumentNumber>545245234</DocumentNumber>\r\n          <DocumentDate>2010-06-01</DocumentDate>\r\n          <CorrectResults>\r\n            <CorrectResultItemDto>\r\n              <SubjectName>Математика</SubjectName>\r\n              <Score>4</Score>\r\n            </CorrectResultItemDto>\r\n          </CorrectResults>\r\n          <IncorrectResults>\r\n            <IncorrectResultItemDto>\r\n              <SubjectName>Русский язык</SubjectName>\r\n              <Score>5</Score>\r\n              <Comment>В заявлении указана оценка 4.</Comment>\r\n            </IncorrectResultItemDto>\r\n          </IncorrectResults>\r\n        </EgeDocument>\r\n      </EgeDocuments>\r\n    </EgeDocumentCheckResult>\r\n  </EgeDocumentCheckResults>\r\n  <GetEgeDocuments>\r\n    <GetEgeDocument>\r\n      <Application>\r\n        <ApplicationNumber>3421</ApplicationNumber>\r\n        <RegistrationDate>2010-07-11</RegistrationDate>\r\n      </Application>\r\n      <EgeDocuments>\r\n        <EgeDocument>\r\n          <CertificateNumber>АВ 3434431</CertificateNumber>\r\n          <TypographicNumber>А 34134510-КТ</TypographicNumber>\r\n          <Year>2010</Year>\r\n          <Status>Valid</Status>\r\n          <Marks>\r\n            <Mark>\r\n              <SubjectName>Алгебра</SubjectName>\r\n              <SubjectMark>4</SubjectMark>\r\n            </Mark>\r\n            <Mark>\r\n              <SubjectName>Геометрия</SubjectName>\r\n              <SubjectMark>5</SubjectMark>\r\n            </Mark>\r\n          </Marks>\r\n        </EgeDocument>\r\n      </EgeDocuments>\r\n    </GetEgeDocument>\r\n  </GetEgeDocuments>\r\n</ImportedAppCheckResultPackage>");
		}
	}
}
