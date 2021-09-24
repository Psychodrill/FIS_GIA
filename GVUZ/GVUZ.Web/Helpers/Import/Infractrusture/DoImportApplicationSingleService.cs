using System.Linq;
using System.Threading;
using System.Xml.Linq;
using FogSoft.Helpers;
using GVUZ.Helper.Import;
using GVUZ.ServiceModel.Import.Core.Packages;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.Schemas;
using GVUZ.ServiceModel.Import.WebService.Dto.Result;
using GVUZ.ServiceModel.Import.WebService.Dto.Result.Import;

namespace GVUZ.Web.Import.Infractrusture {
	public class DoImportApplicationSingleService:ImportBaseService {
		public DoImportApplicationSingleService(XElement data) : base(data) { }

		public override XElement ProcessData() {
			CheckAuth(false);
			ValidatePackage(XsdManager.XsdName.DoImportApplicationSingleServiceRequest);



			/* 1. Загружаем пакет в БД */
			var packageData=GetElementByNameWithThrowError("PackageData");
			/* Статус заявления, нужен для корректного выключения проверки для отозванных */
			var statusValue=packageData.Descendants().Where(element => element.Name.LocalName=="StatusID").Single().Value;

			packageData=TrimPackageStrings(packageData);

			var packageId=PackageManager.CreatePackage(packageData.ToString(),_institutionId,
				 PackageType.ImportApplicationSingle,_login,string.Empty,PackageStatus.PlacedInQueue);

			LogHelper.Log.WarnFormat(">>> Загрузка одного заявления (DoImportApplicationSingleService) packageId={0}, institutionId={1}",	 packageId,_institutionId);

//#warning Поменять обратно на прямую загрузку в БД а не через сервис!!!
			/* Висим пока пакет не проверится - запрашиваем через каждые 10 сек */
			while(!PackageManager.IsPackageProcessed(packageId)) {
				Thread.Sleep(10000);
			}

			/* 3. Извлекаем пакет  */
			var importPackage=PackageManager.PackageRepository.GetPackage(packageId);
			if(importPackage==null||importPackage.InstitutionID!=_institutionId)
				return CreateErrorResultXml("Ошибка загрузки пакета");
			if(importPackage.ProcessResultInfo==null)
				return CreateErrorResultXml("Ошибка проверки пакета");

            XElement results = XElement.Parse(importPackage.ProcessResultInfo);
            return results; //.AddInstitutionId(_institutionId).EmbraceToRoot();

            ///*  Смотрим - есть ли ошибки при загрузке заявления, если есть возвращаем */
            //var resultPackage=new Serializer().Deserialize<ImportResultPackage>(importPackage.ProcessResultInfo);
            //if(resultPackage.Conflicts.Exists||resultPackage.Log.Failed.Exists)
            //    return CreateErrorResultXml(resultPackage);

            ///* 4. Проверяем только принятые */
            //if(statusValue=="4") {
            //    var resultCheckPackage=PackageManager.CheckPackageApplications(importPackage);
            //    if(resultCheckPackage==null||resultCheckPackage.EgeDocumentCheckResults==null)
            //        return CreateErrorResultXml("Ошибка вызова сервиса ФБС");

            //    var validationResult=resultCheckPackage.EgeDocumentCheckResults.SingleOrDefault();
            //    if(validationResult==null)
            //        return CreateErrorResultXml("Ошибка проверки заявления в ФБС");

            //    /* Возвращаем результат проверки */
            //    return ReturnImportApplicationSingleResult(importPackage,validationResult);
            //}

            ///* Возвращаем результат проверки */
            //return ReturnImportApplicationSingleResult(importPackage,null);
		}

		public XElement ReturnImportApplicationSingleResult(ServiceModel.Import.ImportPackage importPackage,EgeDocumentCheckResultDto validationResult) {
			return XElement.Parse(PackageHelper.GenerateXmlPackageIntoString(new AppSingleImportResult {
				EgeDocuments=validationResult.EgeDocuments,
				Conflicts=new ConflictsResultDto(),
				Log=new SingleAppImportError() { Successful=new SuccessfulImportStatisticsDto { Applications="1" } }
			})).AddInstitutionId(importPackage.InstitutionID).EmbraceToRoot();
		}

		public XElement CreateErrorResultXml(ImportResultPackage result) {
			return CreateErrorResultXml(result,null);
		}

		public XElement CreateErrorResultXml(string error) {
			return CreateErrorResultXml(null,error);
		}

		public XElement CreateErrorResultXml(ImportResultPackage result,string error) {
			return XElement.Parse(PackageHelper.GenerateXmlPackageIntoString(new AppSingleImportResult {
				Conflicts=result!=null?result.Conflicts:null,
				Log=new SingleAppImportError {
					Failed=result!=null&&result.Log!=null?result.Log.Failed:null,
					Error=error
				}
			}));
		}
	}
}