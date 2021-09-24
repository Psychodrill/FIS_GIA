using GVUZ.DAL.Dapper.Model.Dictionary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dapper.Model.Campaigns
{
    [Table("CampaignEducationLevel")]
    public class CampaignEducationLevel
    {
        public int CampaignEducationLevelID { get; set; }

        public int CampaignID { get; set; }

        public int Course { get; set; }

        public short EducationLevelID { get; set; }

        public bool? PresentInLicense { get; set; }

        public virtual AdmissionItemType AdmissionItemType { get; set; }

        public virtual Campaign Campaign { get; set; }

        public bool CanRemove { get; set; }
    }
}
