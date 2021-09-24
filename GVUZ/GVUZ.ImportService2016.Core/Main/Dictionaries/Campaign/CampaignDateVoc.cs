using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{

    public class CampaignDateVoc : VocabularyBase<CampaignDateVocDto>
    {
        public CampaignDateVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class CampaignDateVocDto : VocabularyBaseDto
    {
        public int CampaignDateID { get; set; }
        public int CampaignID { get; set; }

        public int Course { get; set; }
        public int Stage { get; set; }
        public int EducationLevelID { get; set; }
        public int EducationFormID { get; set; }
        public int EducationSourceID { get; set; }
        public bool IsActive { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public DateTime DateOrder { get; set; }

        public override int ID
        {
            get
            {
                return CampaignDateID;
            }
            set
            {
                CampaignDateID = value;
            }
        }
    }
}
