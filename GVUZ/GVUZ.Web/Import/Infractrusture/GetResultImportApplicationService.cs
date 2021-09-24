using System.Xml.Linq;
using FogSoft.Helpers;
using GVUZ.Helper.Import;
using GVUZ.ServiceModel.Import.Core.Packages;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.Schemas;

namespace GVUZ.Web.Import.Infractrusture
{
    /// <summary>
    /// Получение результата импорта
    /// </summary>
    public class GetResultImportApplicationService : ImportBaseService
    {
        public GetResultImportApplicationService(XElement data) : base(data) { }

        public override XElement ProcessData()
        {
            CheckAuth(false);
            ValidatePackage(XsdManager.XsdName.GetResultImportApplicationServiceRequest);

            var packageData = GetElementByNameWithThrowError("GetResultImportApplication");
            var checkPackage = new Serializer().Deserialize<GetResultImportApplication>(packageData);
            var importPackage = PackageManager.PackageRepository.GetPackage(checkPackage.GetCorrectPackageID().To(0));

            var error = ValidateProcessPackageResults(importPackage, _institutionId, PackageType.Import);
            if (error != null) return error;

            var res = XElement.Parse(importPackage.ProcessResultInfo);
            if (res.Element("ErrorText") == null)
                res = res.AddInstitutionId(importPackage.InstitutionID);
            return res.EmbraceToRoot();
        }
    }
}