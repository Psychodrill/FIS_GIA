using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class CampaignEducationLevelVoc : VocabularyBase<CampaignEducationLevelVocDto>
    {
    public CampaignEducationLevelVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class CampaignEducationLevelVocDto : VocabularyBaseDto
    {
        public int CampaignEducationLevelID { get; set; }
        public int CampaignID { get; set; }

        public int Course { get; set; }
        public int EducationLevelID { get; set; }

        public override int ID
        {
            get
            {
                return CampaignEducationLevelID;
            }
            set
            {
                CampaignEducationLevelID = value;
            }
        }
    }
}
