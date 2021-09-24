using System.Xml.Linq;
using FogSoft.Helpers;
using GVUZ.Helper.Import;
using GVUZ.ServiceModel.Import.Core.Packages;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.Schemas;

namespace GVUZ.Web.Import.Infractrusture
{
    /// <summary>
    /// Результат проверки заявлений в пакете
    /// </summary>
    public class GetResultCheckApplicationService : ImportBaseService
    {
        public GetResultCheckApplicationService(XElement data) : base(data) { }

        public override XElement ProcessData()
        {
            CheckAuth(false);
            ValidatePackage(XsdManager.XsdName.GetResultCheckApplicationServiceRequest);

            var packageData = GetElementByNameWithThrowError("GetResultCheckApplication");
            packageData = TrimPackageStrings(packageData);
            var checkPackage = new Serializer().Deserialize<GetResultCheckApplication>(packageData);
            var importPackage = PackageManager.PackageRepository.GetPackage(checkPackage.GetCorrectPackageID().To(0));

            var error = ValidateProcessPackageResults(importPackage, _institutionId, PackageType.Import);
            if (error != null) return error;

            if (string.IsNullOrEmpty(importPackage.CheckResultInfo))
                return XmlImportHelper.GenerateErrorElement("Пакет ждет отправки на проверку");

            var res = XElement.Parse(importPackage.CheckResultInfo);

            PackageManager.CreateInfoPackage(string.Empty, _institutionId, PackageType.ApplicationCheckResult, _login, null, res.ToString(SaveOptions.None));
            return res.AddInstitutionId(importPackage.InstitutionID).EmbraceToRoot();
        }
    }
}