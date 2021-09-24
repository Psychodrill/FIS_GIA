using System;

namespace GVUZ.ServiceModel.Export
{
    public class InstitutionDetailsDto
    {
        public int InstitutionID { get; set; }
        public bool IsFilial { get; set; }
        public string FullName { get; set; }
        public string BriefName { get; set; }
        public int FormOfLawID { get; set; }
        public int RegionID { get; set; }
        public string Site { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public bool HasMilitaryDepartment { get; set; }
        public string INN { get; set; }
        public string OGRN { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime? LicenseDate { get; set; }
        public string Accreditation { get; set; }
    }
}