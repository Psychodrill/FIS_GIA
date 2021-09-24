using System.Data;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class CompetitiveGroupVoc: VocabularyBase<CompetitiveGroupVocDto>
    {
        public CompetitiveGroupVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class CompetitiveGroupVocDto : VocabularyBaseDto 
    {
        public int CompetitiveGroupID { get; set; }
        public override int ID
        {
            get { return CompetitiveGroupID; }
            set { CompetitiveGroupID = value; }
        }

        public int StatusID { get; set; }
        public int CampaignID { get; set; }
        public string CampaignUID { get; set; }
        public int Course { get; set; }


        
		public bool IsFromKrym { get; set; }
		public bool IsAdditional { get; set; }
		public int EducationFormId { get; set; }
		public int EducationSourceId { get; set; }
		public int EducationLevelID { get; set; }
		public int DirectionID { get; set; }
        public int? IdLevelBudget { get; set; }
        public int ParentDirectionID { get; set; }
    }
}
