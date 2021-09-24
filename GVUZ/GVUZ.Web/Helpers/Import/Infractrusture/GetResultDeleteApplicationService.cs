using System.Xml.Linq;
using FogSoft.Helpers;
using GVUZ.Helper.Import;
using GVUZ.ServiceModel.Import.Core.Packages;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.Schemas;

namespace GVUZ.Web.Import.Infractrusture
{
    /// <summary>
    /// Получение результата удаления
    /// </summary>
    public class GetResultDeleteApplicationService : ImportBaseService
    {
        public GetResultDeleteApplicationService(XElement data) : base(data) { }

        public override XElement ProcessData()
        {
            CheckAuth(false);
            ValidatePackage(XsdManager.XsdName.GetResultDeleteApplicationServiceRequest);

            var packageData = GetElementByNameWithThrowError("GetResultDeleteApplication");
            var checkPackage = new Serializer().Deserialize<GetResultDeleteApplication>(packageData);
            var importPackage = PackageManager.PackageRepository.GetPackage(checkPackage.GetCorrectPackageID().To(0));

            var error = ValidateProcessPackageResults(importPackage, _institutionId, PackageType.Delete);
            if (error != null) return error;

            return XElement.Parse(importPackage.ProcessResultInfo).AddInstitutionId(importPackage.InstitutionID).EmbraceToRoot();
        }
    }
}