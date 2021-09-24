using System.Xml.Linq;
using FogSoft.Helpers;
using GVUZ.Helper.Import;
using GVUZ.ServiceModel.Export;
using GVUZ.ServiceModel.Import.Core.Packages;

namespace GVUZ.Web.Import.Infractrusture
{
    public class GetInstitutionInfoService : ImportBaseService
    {
        public GetInstitutionInfoService(XElement data) : base(data) {}

        public override XElement ProcessData()
        {
            CheckAuth(false);

            LogHelper.Log.WarnFormat(">>> Запрос информации по вузу (GetInstitutionInfoService) institutionId={0}", _institutionId);

            PackageManager.CreateInfoPackage(string.Empty, _institutionId, PackageType.InstitutitionInformation, _login, null, null);
            //return InstitutionExporter.GetInsitututionsData(_institutionId, IsIncludeFilials).EmbraceToRoot();

				GVUZ.ServiceModel.SQL.InstitutionExporter exporter=new ServiceModel.SQL.InstitutionExporter(_institutionId, IsIncludeFilials);
				return exporter.GetInsitututionsData().EmbraceToRoot();
        }
    }
}