using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class AbitInstitutionGeneral
    {
        public int InstitutionId { get; set; }
        public string FullName { get; set; }
        public string BriefName { get; set; }
        public int InstTypeId { get; set; }
        public string InstType { get; set; }
        public string Inn { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Site { get; set; }
        public bool HasHostel { get; set; }
        public int? HostelCapacity { get; set; }
        public bool HasHostelForEntrants { get; set; }
        public bool HasMilitaryDepartment { get; set; }
        public int? HeadId { get; set; }
        public int HasBranches { get; set; }
        public string FactAddress { get; set; }
        public string Email { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsFilial { get; set; }
        public string IslodGuid { get; set; }
    }
}
