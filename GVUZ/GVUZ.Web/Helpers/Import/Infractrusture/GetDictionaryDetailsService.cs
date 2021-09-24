using System;
using System.Xml.Linq;
using FogSoft.Helpers;
using GVUZ.Helper.Import;
using GVUZ.ServiceModel.Import.Core.Packages;
using GVUZ.ServiceModel.Import.Schemas;
using GVUZ.ServiceModel.SQL.Dictionaries;

namespace GVUZ.Web.Import.Infractrusture
{
    public class GetDictionaryDetailsService : ImportBaseService
    {
        public GetDictionaryDetailsService(XElement data) : base(data) {}

        public override XElement ProcessData()
        {
            CheckAuth(true);
            ValidatePackage(XsdManager.XsdName.GetDictionaryDetailsServiceRequest);

            var content = GetElementByName(_data, "GetDictionaryContent", true);
            var code = GetElementByName(_data, "DictionaryCode", true);
            
            PackageManager.CreateInfoPackage(content.ToString(SaveOptions.None), _institutionId, PackageType.DictionaryElement, _login, null, null);

            try
            {
                var exporter = new DictionaryExporterSql(_institutionId);
                var res = exporter.GetDictionaryDataString(code.Value.To(0));
                return XElement.Parse(res).EmbraceToRoot();
            }
            catch (NotSupportedException e)
            {
                return XmlImportHelper.GenerateErrorElement(e.Message);
            }
            catch (Exception)
            {
                return XmlImportHelper.GenerateErrorElement("При получении содержания справочника с указанным кодом произошла внутренняя ошибка сервиса");
            }
        }
    }
}