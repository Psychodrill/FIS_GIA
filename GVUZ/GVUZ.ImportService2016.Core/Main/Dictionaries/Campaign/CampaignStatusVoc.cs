using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries.Campaign
{
    public class CampaignStatusVoc : VocabularyBase<CampaignStatusVocDto>
    {
        public CampaignStatusVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class CampaignStatusVocDto : VocabularyBaseDto
    {
        public int StatusID { get; set; }
        public override int ID
        {
            get { return StatusID; }
            set { StatusID = value; }
        }
    }
}
