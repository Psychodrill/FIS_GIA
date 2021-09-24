using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class CampaignDate
    {
        public int CampaignDateId { get; set; }
        public int CampaignId { get; set; }
        public int Course { get; set; }
        public short EducationLevelId { get; set; }
        public short EducationFormId { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? DateOrder { get; set; }
        public string Uid { get; set; }
        public bool IsActive { get; set; }
        public int Stage { get; set; }
        public short EducationSourceId { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual AdmissionItemType EducationForm { get; set; }
        public virtual AdmissionItemType EducationLevel { get; set; }
        public virtual AdmissionItemType EducationSource { get; set; }
    }
}
