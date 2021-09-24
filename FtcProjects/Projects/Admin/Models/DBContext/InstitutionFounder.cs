using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class InstitutionFounder
    {
        public InstitutionFounder()
        {
            InstitutionFounderToInstitutions = new HashSet<InstitutionFounderToInstitutions>();
        }

        public int Id { get; set; }
        public int? InstitutionFounderTypeId { get; set; }
        public string OrganizationFullName { get; set; }
        public string OrganizationShortName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string Phones { get; set; }
        public string Faxes { get; set; }
        public string Emails { get; set; }
        public string Ogrn { get; set; }
        public string Inn { get; set; }
        public string Kpp { get; set; }
        public string LawAddress { get; set; }
        public string PhysicalAddress { get; set; }
        public string EiisId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsDeactivated { get; set; }
        public string InitPassword { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }

        public virtual InstitutionFounderType InstitutionFounderType { get; set; }
        public virtual ICollection<InstitutionFounderToInstitutions> InstitutionFounderToInstitutions { get; set; }
    }
}
