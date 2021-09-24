using System;
using System.Xml.Serialization;

namespace GVUZ.ServiceModel.Import.Bulk.Model.Results
{
    public class ApplicationShortRefResult
    {
        [XmlAttribute("ApplicationNumber")]
        public string ApplicationNumber { get; set; }

        [XmlAttribute("RegistrationDate")]
        public DateTime RegistrationDate { get; set; }

        [XmlAttribute("DirectionID")]
        public int DirectionID { get; set; }

        [XmlAttribute("EducationFormID")]
        public int EducationFormID { get; set; }

        [XmlAttribute("FinanceSourceID")]
        public int FinanceSourceID { get; set; }

        [XmlAttribute("EducationLevelID")]
        public int EducationLevelID { get; set; }
    }
}