using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class CampaignVoc : VocabularyBase<CampaignVocDto>
    {
        public CampaignVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class CampaignVocDto : VocabularyBaseDto
    {
        public int CampaignID { get; set; }
        public int CampaignTypeID { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public int EducationFormFlag { get; set; }
        public int StatusID { get; set; }

        public override int ID
        {
            get { return CampaignID; }
            set { CampaignID = value; }
        }
    }
}
