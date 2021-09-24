using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class RecomendedListsVoc : VocabularyBase<RecomendedListsVocDto>
    {
        public RecomendedListsVoc(DataTable dataTable) : base(dataTable) 
        { 
        
        }
    }

    public class RecomendedListsVocDto : VocabularyBaseDto
    {
        public int RecListID { get; set; }
        public int ApplicationID { get; set; }
        public int CampaignID { get; set; }
        public int CompetitiveGroupID { get; set; }
        public int Stage { get; set; }
        public DateTime? DateDelete { get; set; }

        public override int ID
        {
            get { return RecListID; }
            set { RecListID = value; }
        }
    }
}
