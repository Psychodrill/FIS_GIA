using System.Xml.Linq;
using GVUZ.Helper.Import;
using GVUZ.ServiceModel.Import.Core.Packages;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.Schemas;

namespace GVUZ.Web.Import.Infractrusture
{
    public class DoValidateService : ImportBaseService
    {
        public DoValidateService(XElement data) : base(data) { }

        public override XElement ProcessData()
        {
            CheckAuth(false);
            ValidatePackage(XsdManager.XsdName.DoValidateServiceRequest);

            var packageData = GetElementByNameWithThrowError("PackageData").ToString(SaveOptions.None);
            var validationPackage = PackageManager.CreateValidationPackage(packageData);

			if (validationPackage == null)
				return XmlImportHelper.GenerateErrorElement("Ошибка в формате XML");

			using (var packageHandler = PackageManager.GetPackageProcessor(validationPackage))
			{
				var error = packageHandler.ValidatePackage(validationPackage.PackageData, PackageType.Import);

				ValidationResultPackage resultPackage;
			    if (error == null) resultPackage = new ValidationResultPackage {StatusCode = "1", Message = "Пакет валидацию прошел успешно"};
			    else return XmlImportHelper.GenerateErrorElement(error);
				
                var resultPackageString = PackageHelper.GenerateXmlPackageIntoString(resultPackage);
				if (_institutionId > 0)
					PackageManager.CreateInfoPackage(packageData, _institutionId, PackageType.Validation, _login, null, resultPackageString);
				
				return XElement.Parse(resultPackageString).EmbraceToRoot();
			}
        }
    }
}