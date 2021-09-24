using System.Xml.Linq;
using GVUZ.Helper.Import;
using GVUZ.ServiceModel.Import.Core.Packages;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.Schemas;
using GVUZ.Web.Import.Infractrusture.Exceptions;

namespace GVUZ.Web.Import.Infractrusture
{
    public class DoImportService : ImportBaseService
    {
        public DoImportService(XElement data) : base(data) {}

        public override XElement ProcessData()
        {
            CheckAuth(false);
            ValidatePackage(XsdManager.XsdName.DoImportServiceRequest);

            var packageData = GetElementByNameWithThrowError("PackageData");
            if (!packageData.HasElements) throw new ImportXmlValidationException("Тег PackageData не может быть пустым");

            packageData = TrimPackageStrings(packageData);
            //packageData = ReplaceEntranceTestNameWithSubjectIfPossible(packageData);

            int packageId = PackageManager.CreatePackage(packageData.ToString(), _institutionId, PackageType.Import, _login);
            return XElement.Parse(
						PackageHelper.GenerateXmlPackageIntoString( new ImportPackageInfo { PackageID = packageId.ToString() })
					).AddInstitutionId(_institutionId).EmbraceToRoot();
        }
    }
}