using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using FogSoft.Helpers;
using GVUZ.Helper.Import;
using GVUZ.ServiceModel.Import.Bulk.Attributes;

namespace GVUZ.ServiceModel.Import.Schemas
{
    public class XsdManager
    {
        public enum XsdName
        {
            [RootTagName("AuthData")] [Description("запрос-авторизация.xsd")] AuthData,

            [Description("запрос-импорт.xsd")] [RootTagName("PackageData")] DoValidateServiceRequest,

            [Description("ответ-валидация.xsd")] [RootTagName("Root")] DoValidateServiceResponse,

            [Description("запрос-детали по справочнику.xsd")] [RootTagName("GetDictionaryContent")] GetDictionaryDetailsServiceRequest,

            [Description("ответ-детали по справочнику.xsd")] [RootTagName("Root")] GetDictionaryDetailsServiceResponse,

            [Description("запрос-импорт 1 заявления.xsd")] [RootTagName("PackageData")] DoImportApplicationSingleServiceRequest,

            [Description("ответ-импорт 1 заявления.xsd")] [RootTagName("Root")] DoImportApplicationSingleServiceResponse,

            [Description("запрос-импорт.xsd")] [RootTagName("PackageData")] DoImportServiceRequest,

            [Description("ответ-импорт.xsd")] [RootTagName("Root")] DoImportServiceResponse,

            [Description("запрос-получение результата импорта.xsd")] [RootTagName("GetResultImportApplication")] GetResultImportApplicationServiceRequest,

            [Description("ответ-получение результата импорта.xsd")] [RootTagName("Root")] GetResultImportApplicationServiceResponse,

            [Description("запрос-получение результата удаления.xsd")] [RootTagName("GetResultDeleteApplication")] GetResultDeleteApplicationServiceRequest,

            [Description("ответ-получение результата удаления.xsd")] [RootTagName("Root")] GetResultDeleteApplicationServiceResponse,

            [Description("запрос-часть сведений по ОУ.xsd")] [RootTagName("PackageData")] GetInstitutionPartOfInfoServiceRequest,

            [Description("ответ-получение части сведений об ОУ.xsd")] [RootTagName("Root")] GetInstitutionPartOfInfoServiceResponse,

            [Description("запрос-проверка 1 заявления.xsd")] [RootTagName("CheckApp")] DoCheckApplicationSingleServiceRequest,

            [Description("???")] [RootTagName("Root")] DoCheckApplicationSingleServiceResponse,

            [Description("запрос-получение сведений об ОУ.xsd")] [RootTagName("Root")] GetInstitutionInfoServiceRequest,

            [Description("ответ-получение сведений об ОУ.xsd")] [RootTagName("Root")] GetInstitutionInfoServiceResponse,

            [Description("запрос-проверка заявлений в пакете.xsd")] [RootTagName("GetResultCheckApplication")] GetResultCheckApplicationServiceRequest,

            [Description("???")] [RootTagName("Root")] GetResultCheckApplicationServiceResponse,

            [Description("запрос-список справочников.xsd")] [RootTagName("Root")] GetDictionariesListServiceRequest,

            [Description("ответ-список справочников.xsd")] [RootTagName("Root")] GetDictionariesListServiceResponse,

            [Description("запрос-удаление.xsd")] [RootTagName("DataForDelete")] DoDeleteServiceRequest,

            [Description("ответ-удаление.xsd")] [RootTagName("Root")] DoDeleteServiceResponse
        }

        private readonly string _xsdBaseDirectory;

        public XsdManager()
        {
            _xsdBaseDirectory = XsdDirectoryDefault;
        }

        public XsdManager(string xsdDirectory)
        {
            _xsdBaseDirectory = xsdDirectory;
        }

        /// <summary>
        ///     Папка со схемами
        /// </summary>
        public static string XsdDirectoryDefault
        {
            get { return AssemblyDirectory + @"\Import\Schemas\"; }
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public string GetXsd(XsdName xsdName)
        {
            return Path.Combine(_xsdBaseDirectory, xsdName.GetEnumDescription());
        }

        public string ValidateXmlBySheme(XElement xml, XsdName xsd)
        {
            return ValidateXmlBySheme(xml.ToString(), GetXsd(xsd));
        }

        public string ValidateXmlBySheme(string xml, XsdName xsd)
        {
            return ValidateXmlBySheme(xml, GetXsd(xsd));
        }

        public string ValidateXmlBySheme(string data, string xsd)
        {
            return XmlValidator.ValidateWithXsd(new StreamReader(xsd), new XmlTextReader(new StringReader(data)));
        }
    }
}