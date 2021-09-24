using System;
using System.Xml.Linq;
using GVUZ.Helper.Import;
using GVUZ.ServiceModel.Import.Core.Packages;
using GVUZ.ServiceModel.SQL.Dictionaries;

namespace GVUZ.Web.Import.Infractrusture
{
    public class GetDictionariesListService : ImportBaseService
    {
        public GetDictionariesListService(XElement data) : base(data) {}

        public override XElement ProcessData()
        {
            CheckAuth(true);

            PackageManager.CreateInfoPackage(string.Empty, _institutionId, PackageType.DictionariesList, _login, null, null);

            try
            {
                return XElement.Parse(DictionaryExporterSql.GetDictionariesListDtoString()).EmbraceToRoot();
            }
            catch (Exception)
            {
                return XmlImportHelper.GenerateErrorElement("При получении списка справочников произошла внутренняя ошибка сервиса");
            }
        }
    }
}