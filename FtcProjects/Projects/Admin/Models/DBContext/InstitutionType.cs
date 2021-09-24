using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class InstitutionType
    {
        public InstitutionType()
        {
            Institution = new HashSet<Institution>();
            InstitutionHistory = new HashSet<InstitutionHistory>();
        }

        public short InstitutionTypeId { get; set; }
        public string BriefName { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<Institution> Institution { get; set; }
        public virtual ICollection<InstitutionHistory> InstitutionHistory { get; set; }
    }
}
