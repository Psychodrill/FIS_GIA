namespace GVUZ.DAL.Dapper.ViewModel.CompetitiveGroups
{
    public class CampaignWithTypeViewModel
    {
        public int CampaignID { get; set; }
        public int ID { get { return CampaignID; } }
        public int YearStart { get; set; }
        public string Name { get; set; }
        public int CampaignTypeID { get; set; }
        public int EducationLevelID { get; set; }
        public string EducationLevelName { get; set; }

        public int EducationFormFlag { get; set; }
    }
}
