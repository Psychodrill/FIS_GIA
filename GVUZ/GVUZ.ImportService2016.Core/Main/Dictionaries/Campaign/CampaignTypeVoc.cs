using System.Data;
namespace GVUZ.ImportService2016.Core.Main.Dictionaries.Campaign
{
    public class CampaignTypeVoc : VocabularyBase<CampaignTypeVocDto>
    {
        public CampaignTypeVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class CampaignTypeVocDto : VocabularyBaseDto
    {
        public int CampaignTypeID { get; set; }
        public override int ID
        {
            get { return CampaignTypeID; }
            set { CampaignTypeID = value; }
        }
    }
}
