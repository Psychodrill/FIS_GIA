using System.ComponentModel;
using System.Xml.Serialization;

namespace GVUZ.ServiceModel.Import.WebService.Dto
{
    public class AdmissionVolumeCollectionDto
    {
        [XmlArray("AdmissionVolumeItems")] [XmlArrayItem(ElementName = "Item")] public AdmissionVolumeDto[] Collection;
    }

    [Description("Объем приема")]
    public class AdmissionVolumeDto : BaseDto
    {
        public string CampaignUID;
        public string Course;
        public string DirectionID;
        public string EducationLevelID;

        public string NumberBudgetO;
        public string NumberBudgetOZ;
        public string NumberBudgetZ;
        public string NumberPaidO;
        public string NumberPaidOZ;
        public string NumberPaidZ;
        public string NumberTargetO;
        public string NumberTargetOZ;
        public string NumberTargetZ;
        public string NumberQuotaO;
        public string NumberQuotaOZ;
        public string NumberQuotaZ;

        public bool IsPlan;

        public string ParentDirectionID;
    }
}