using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GVUZ.DAL.Dapper.Model.Campaigns
{
    [Table("Campaign")]
    public partial class Campaign
    {
        public int CampaignID { get; set; }
        public int InstitutionID { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public int EducationFormFlag { get; set; }
        public int StatusID { get; set; }
        [StringLength(200)]
        public string UID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid CampaignGUID { get; set; }
        public int CampaignTypeID { get; set; }

    }
}
