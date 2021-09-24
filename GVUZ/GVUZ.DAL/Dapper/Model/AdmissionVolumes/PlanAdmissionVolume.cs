using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace GVUZ.DAL.Dapper.Model.AdmissionVolumes
{
    [Table("PlanAdmissionVolume")]
    public class PlanAdmissionVolume
    {
        public int PlanAdmissionVolumeID { get; set; }
        public int CampaignID { get; set; }
        public short AdmissionItemTypeID { get; set; }
        public int? DirectionID { get; set; }
        public short EducationSourceID { get; set; }
        public short EducationFormID { get; set; }
        public int Number { get; set; }
        public string UID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? AdmissionVolumeGUID { get; set; }
        public int? ParentDirectionID { get; set; }
    }
}
