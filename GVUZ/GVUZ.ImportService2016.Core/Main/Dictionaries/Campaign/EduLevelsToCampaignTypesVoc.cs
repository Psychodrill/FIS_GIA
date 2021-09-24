using System.Data;
namespace GVUZ.ImportService2016.Core.Main.Dictionaries.Campaign
{
    public class EduLevelsToCampaignTypesVoc : VocabularyBase<EduLevelsToCampaignTypesVocDto>
    {
        public EduLevelsToCampaignTypesVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class EduLevelsToCampaignTypesVocDto : VocabularyBaseDto
    {
        public int CampaignTypeID { get; set; }
        public int AdmissionItemTypeID { get; set; }
    }
}