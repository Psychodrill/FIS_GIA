using System.Xml.Linq;
using GVUZ.Helper.Import;
using GVUZ.ServiceModel.Import.Core.Packages;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.Schemas;
using GVUZ.Web.Import.Infractrusture.Exceptions;

namespace GVUZ.Web.Import.Infractrusture
{
    /// <summary>
    /// Удаление
    /// </summary>
    public class DoDeleteService : ImportBaseService
    {
        public DoDeleteService(XElement data) : base(data) { }

        public override XElement ProcessData()
        {
            CheckAuth(false);
            ValidatePackage(XsdManager.XsdName.DoDeleteServiceRequest);

            var dataForDelete = GetElementByNameWithThrowError("DataForDelete");
            if (!dataForDelete.HasElements) throw new ImportXmlValidationException("Тег DataForDelete не может быть пустым");

            dataForDelete = TrimPackageStrings(dataForDelete);

            int packageId = PackageManager.CreatePackage(dataForDelete.ToString(), _institutionId, PackageType.Delete, _login);
            return XElement.Parse(PackageHelper.GenerateXmlPackageIntoString(
                new DeletePackageInfo { PackageID = packageId.ToString() })).AddInstitutionId(_institutionId).EmbraceToRoot();
        }
    }
}