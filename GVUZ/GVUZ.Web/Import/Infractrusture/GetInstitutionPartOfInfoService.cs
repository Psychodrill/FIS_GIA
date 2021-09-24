using System.Xml.Linq;
using GVUZ.Helper.Import;
using GVUZ.ServiceModel.Export;
using GVUZ.ServiceModel.Import.Core.Packages;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.Schemas;
using GVUZ.Web.Import.Infractrusture.Exceptions;

namespace GVUZ.Web.Import.Infractrusture
{
    public class GetInstitutionPartOfInfoService : ImportBaseService
    {
        public GetInstitutionPartOfInfoService(XElement data) : base(data) {}

        public override XElement ProcessData()
        {
            CheckAuth(false);
            ValidatePackage(XsdManager.XsdName.GetInstitutionPartOfInfoServiceRequest);

            var packageData = GetElementByNameWithThrowError("PackageData");
            if (!packageData.HasElements) throw new ImportXmlValidationException("Тег PackageData не может быть пустым");

            var filter = new Serializer().Deserialize<InstitutionInformationFilter>(packageData);
            //var results = InstitutionExporter.GetInsitututionsData(_institutionId, filter, IsIncludeFilials);
				GVUZ.ServiceModel.SQL.InstitutionExporter exporter=new ServiceModel.SQL.InstitutionExporter(_institutionId, IsIncludeFilials, filter);
				var results=exporter.GetInsitututionsData();

            PackageManager.CreateInfoPackage(_data.ToString(SaveOptions.None), _institutionId, PackageType.InstitutitionPartOfInformation, _login, null, results.ToString(SaveOptions.None));

            return results.AddInstitutionId(_institutionId).EmbraceToRoot();
				//return exporter.GetInsitututionsData().EmbraceToRoot();
        }
    }
}