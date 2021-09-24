using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GVUZ.DAL.Dapper.Model.TargetOrganization
{
    [Table("CompetitiveGroupTarget")]
    public partial class CompetitiveGroupTarget
    {
        public string Name { get; set; }
        public int CompetitiveGroupTargetID { get; set; }

        [Required]
        [StringLength(250)]
        public string ContractOrganizationName { get; set; }
        //{
        //    get { return Name == null ? ContractOrganizationName : Name; }
        //    set { }
        //}

        [StringLength(200)]
        public string UID { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int InstitutionID { get; set; }

        public bool CanRemove { get; set; }
        public bool HaveContract { get; set; }
        public string ContractNumber { get; set; }
        public string ContractDate { get; set; }
        public string ContractOrganizationOGRN { get; set; }
        public string ContractOrganizationKPP { get; set; }
        public string EmployerOrganizationName { get; set; }
        public string EmployerOrganizationOGRN { get; set; }
        public string EmployerOrganizationKPP { get; set; }
        public string LocationEmployerOrganizations { get; set; }

    }
}
