using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class Direction
    {
        public Direction()
        {
            AdmissionVolume = new HashSet<AdmissionVolume>();
            AllowedDirections = new HashSet<AllowedDirections>();
            ApplicationConsidered = new HashSet<ApplicationConsidered>();
            CompetitiveGroup = new HashSet<CompetitiveGroup>();
            InstitutionDirectionRequest = new HashSet<InstitutionDirectionRequest>();
            InstitutionItem = new HashSet<InstitutionItem>();
            LicensedDirection = new HashSet<LicensedDirection>();
            PlanAdmissionVolume = new HashSet<PlanAdmissionVolume>();
        }

        public int DirectionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string SysGuid { get; set; }
        public string Edulevel { get; set; }
        public string Eduprogramtype { get; set; }
        public string Ugscode { get; set; }
        public string Ugsname { get; set; }
        public string Qualificationcode { get; set; }
        public string Qualificationname { get; set; }
        public string Period { get; set; }
        public string EduDirectory { get; set; }
        public string EduprAdditional { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string NewCode { get; set; }
        public short? EducationLevelId { get; set; }
        public string EiisId { get; set; }
        public int? EsrpId { get; set; }
        public int? EducationProgramTypeId { get; set; }
        public bool? IsVisible { get; set; }
        public string EiisIdNew { get; set; }
        public string IsActual { get; set; }

        public virtual EduProgramTypes EducationProgramType { get; set; }
        public virtual ParentDirection Parent { get; set; }
        public virtual DirectionSubjectLinkDirection DirectionSubjectLinkDirection { get; set; }
        public virtual EntranceTestCreativeDirection EntranceTestCreativeDirection { get; set; }
        public virtual EntranceTestProfileDirection EntranceTestProfileDirection { get; set; }
        public virtual ICollection<AdmissionVolume> AdmissionVolume { get; set; }
        public virtual ICollection<AllowedDirections> AllowedDirections { get; set; }
        public virtual ICollection<ApplicationConsidered> ApplicationConsidered { get; set; }
        public virtual ICollection<CompetitiveGroup> CompetitiveGroup { get; set; }
        public virtual ICollection<InstitutionDirectionRequest> InstitutionDirectionRequest { get; set; }
        public virtual ICollection<InstitutionItem> InstitutionItem { get; set; }
        public virtual ICollection<LicensedDirection> LicensedDirection { get; set; }
        public virtual ICollection<PlanAdmissionVolume> PlanAdmissionVolume { get; set; }
    }
}
