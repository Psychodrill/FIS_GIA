using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GVUZ.DAL.Dapper.ViewModel.Dictionary
{
    public partial class CampaignStatusView
    {
        public int CampaignStatusID { get; set; }
        public int ID { get { return CampaignStatusID; } }

        [StringLength(50)]
        public string Name { get; set; }
    }
}