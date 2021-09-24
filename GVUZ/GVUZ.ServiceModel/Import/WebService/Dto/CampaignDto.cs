using System.ComponentModel;
using System.Xml.Serialization;

namespace GVUZ.ServiceModel.Import.WebService.Dto
{
    public class CampaignInfoDto
    {
        [XmlArrayItem(ElementName = "Campaign")] public CampaignDto[] Campaigns;
    }

    [Description("Приемная кампания")]
    public class CampaignDto : BaseDto
    {
        public bool? AdditionalSet;
        public int CampaignID;
        [XmlArrayItem(ElementName = "EducationFormID")] public string[] EducationForms;
        public string Name;
        public int StatusID;
        public string YearEnd;
        public string YearStart;

        [XmlArrayItem(ElementName = "EducationLevel")]
        public CampaignEducationLevelDto[] EducationLevels { get; set; }

        [XmlArrayItem(ElementName = "CampaignDate")]
        public CampaignDateDto[] CampaignDates { get; set; }

        // Признак приема граждан Крыма для конкретной Campaign
        // если не указан (null) то используется значение CampaignInfoDto.IsFromKrym
        // если указан, то переопределяет глобальное значение для данной Campaign
        [XmlElement(ElementName = "IsForKrym")]
        public bool? IsFromKrym { get; set; }
    }

    public class CampaignEducationLevelDto
    {
        public string Course;
        public string EducationLevelID;
    }

    [Description("Дата приемной кампании")]
    public class CampaignDateDto : BaseDto
    {
        public string Course;
        public string DateEnd;
        public string DateOrder;
        public string DateStart;
        public string EducationFormID;
        public string EducationLevelID;
        public string EducationSourceID;
        public string Stage;
    }
}