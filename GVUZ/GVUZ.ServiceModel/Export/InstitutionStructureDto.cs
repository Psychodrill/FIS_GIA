using System.Xml.Serialization;

namespace GVUZ.ServiceModel.Export
{
    public class InstitutionStructureDto
    {
        public string BriefName;
        [XmlArrayItem(ElementName = "Structure")] public InstitutionStructureDto[] ChildStructure;
        public string Name;
        public string Site;
        public string InstitutionID;

        [XmlIgnore]
        public int? ItemID { get; set; }

        public string DirectionID { get; set; }
    }
}