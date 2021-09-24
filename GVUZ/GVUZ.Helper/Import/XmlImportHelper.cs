using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GVUZ.Helper.Import
{
    [Serializable]
    [XmlRoot(ElementName = "Error", IsNullable = false)]
    public class ErrorInfo
    {
        public string ErrorCode { get; set; }
        public string ErrorText { get; set; }
    }

    public class XmlImportHelper
    {
        /// <summary>
        ///     Единый формат ошибки WCF сервиса
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static XElement GenerateErrorElement(string error, string errorcode)
        {
            XElement result;
            try
            {
                result = new Serializer().SerializeToXElement(new ErrorInfo {ErrorCode = errorcode, ErrorText = error});
            }
            catch
            {
                result = new XElement("Error", new XElement("ErrorCode", errorcode), new XElement("ErrorText", error));
            }
            return result.EmbraceToRoot();
        }

        public static XElement GenerateErrorElement(string error)
        {
            return GenerateErrorElement(error, "0");
        }

        public static XElement GetXmlStringForElement(string packageXml, string elementName)
        {
            return GetXmlStringForElement(XElement.Parse(packageXml), elementName);
        }

        public static XElement GetXmlStringForElement(XElement serviceData, string elementName)
        {
            return serviceData.Descendants(elementName).FirstOrDefault(x => x.Name == elementName);
        }

        public static XElement GetXmlStringForElementWithCampaignUIDWithoutAddress(string packageXml, string elementName,
                                                                                   out string packageData)
        {
            return GetXmlStringForElementWithCampaignUIDWithoutAddress(XElement.Parse(packageXml), elementName,
                                                                       out packageData);
        }

        public static XElement GetXmlStringForElementWithCampaignUIDWithoutAddress(XElement serviceData,
                                                                                   string elementName,
                                                                                   out string packageData)
        {
            packageData = null;
            XElement el = serviceData.DescendantsAndSelf(elementName).FirstOrDefault(x => x.Name == elementName);
            if (el == null)
                return GenerateErrorElement(String.Format("No {0} element", elementName));
            IEnumerable<XElement> volumeItems = el.Descendants("AdmissionVolume").SelectMany(x => x.Descendants("Item"));
            foreach (XElement volumeItem in volumeItems)
            {
                if (volumeItem.Element("CampaignUID") == null)
                {
                    volumeItem.Element("UID").AddAfterSelf(XElement.Parse("<CampaignUID>Default</CampaignUID>"));
                }
                if (volumeItem.Element("Course") == null)
                {
                    volumeItem.Element("EducationLevelID").AddAfterSelf(XElement.Parse("<Course>1</Course>"));
                }
            }
            IEnumerable<XElement> compGroups = el.Descendants("CompetitiveGroup");
            foreach (XElement cg in compGroups)
            {
                if (cg.Element("CampaignUID") == null)
                {
                    cg.Element("UID").AddAfterSelf(XElement.Parse("<CampaignUID>Default</CampaignUID>"));
                }
                if (cg.Element("Course") == null)
                {
                    cg.Element("EducationLevelID").AddAfterSelf(XElement.Parse("<Course>1</Course>"));
                }
            }
            foreach (XElement regAddr in el.Descendants("RegistrationAddress").ToArray()) regAddr.Remove();
            foreach (XElement regAddr in el.Descendants("FactAddress").ToArray()) regAddr.Remove();
            foreach (XElement regAddr in el.Descendants("MobilePhone").ToArray()) regAddr.Remove();
            foreach (XElement regAddr in el.Descendants("Email").ToArray()) regAddr.Remove();
            foreach (XElement regAddr in el.Descendants("ForeignLanguages").ToArray()) regAddr.Remove();
            foreach (XElement regAddr in el.Descendants("FatherData").ToArray()) regAddr.Remove();
            foreach (XElement regAddr in el.Descendants("MotherData").ToArray()) regAddr.Remove();
            foreach (XElement regAddr in el.Descendants("OriginalDocumentsReceived").ToArray()) regAddr.Remove();

            XElement[] docNumbers = el.Descendants("DocumentNumber").ToArray();
            foreach (XElement docNumber in docNumbers)
            {
                if (docNumber.Parent.Name == "InstitutionDocument") continue;
                XElement targetNode = docNumber.ElementsBeforeSelf("DocumentSeries").FirstOrDefault() ?? docNumber;
                targetNode.AddBeforeSelf(XElement.Parse("<OriginalReceived>1</OriginalReceived>"));
            }

            IEnumerable<XElement> apps = el.Descendants("Application");
            foreach (XElement app in apps)
            {
                XElement appGroup = app.Descendants("CompetitiveGroupID").FirstOrDefault();
                if (appGroup == null) continue;
                XElement commonBenefit = app.Descendants("ApplicationCommonBenefit").FirstOrDefault();
                if (commonBenefit != null)
                {
                    commonBenefit.Descendants("UID")
                                 .FirstOrDefault()
                                 .AddAfterSelf(
                                     XElement.Parse("<CompetitiveGroupID>" + appGroup.Value + "</CompetitiveGroupID>"));
                }
                XElement cg =
                    el.Descendants("CompetitiveGroup")
                      .Where(x => x.Descendants("UID").First().Value == appGroup.Value)
                      .FirstOrDefault();
                if (cg != null)
                {
                    string[] cgiUIDs =
                        cg.Descendants("CompetitiveGroupItem")
                          .SelectMany(x => x.Elements("UID"))
                          .Select(x => "<CompetitiveGroupItemID>" + x.Value + "</CompetitiveGroupItemID>")
                          .ToArray();
                    appGroup.AddAfterSelf(
                        XElement.Parse("<SelectedCompetitiveGroupItems>" + String.Join(" ", cgiUIDs) +
                                       "</SelectedCompetitiveGroupItems>"));
                }
                //else
                //{
                //    appGroup.AddBeforeSelf(XElement.Parse("<SelectedCompetitiveGroupItems></SelectedCompetitiveGroupItems>"));
                //}
                app.Descendants("EntranceTestResult")
                   .SelectMany(x => x.Elements("EntranceTestTypeID"))
                   .ToList()
                   .ForEach(x => x.AddAfterSelf(appGroup));
                appGroup.AddBeforeSelf(
                    XElement.Parse("<SelectedCompetitiveGroups>" + appGroup + "</SelectedCompetitiveGroups>"));
                appGroup.Remove();
            }

            XElement[] orders = el.Descendants("OrderOfAdmission").ToArray();
            foreach (XElement order in orders)
            {
                if (!order.Elements("EducationLevelID").Any())
                {
                    //добавляем хоть что-то
                    order.Elements("FinanceSourceID")
                         .First()
                         .AddAfterSelf(XElement.Parse("<EducationLevelID>2</EducationLevelID>"));
                }
            }

            XElement[] cgs = el.Descendants("CompetitiveGroup").ToArray();
            foreach (XElement cg in cgs)
            {
                XElement edLevel = cg.Elements("EducationLevelID").FirstOrDefault();
                if (edLevel != null)
                {
                    cg.Descendants("CompetitiveGroupItem").SelectMany(x => x.Elements("UID")).ToList().ForEach(
                        x => x.AddAfterSelf(edLevel));
                    cg.Descendants("CompetitiveGroupTargetItem").SelectMany(x => x.Elements("UID")).ToList().ForEach(
                        x => x.AddAfterSelf(edLevel));
                    edLevel.Remove();
                }
            }

            packageData = el.ToString(SaveOptions.None);
            return null;
        }
    }

    public static class ExtensionsImport
    {
        public static XElement AddInstitutionId(this XElement data, int institutionId)
        {
            if (institutionId != 0)
                data.Add(XElement.Parse("<InstitutionID>" + institutionId + "</InstitutionID>"));

            return data;
        }

        public static XElement EmbraceToRoot(this XElement data)
        {
            return data;
            //return new XElement("Root", data);
        }
    }
}