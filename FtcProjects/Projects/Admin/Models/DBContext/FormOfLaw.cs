using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class FormOfLaw
    {
        public FormOfLaw()
        {
            Institution = new HashSet<Institution>();
            InstitutionHistory = new HashSet<InstitutionHistory>();
        }

        public int FormOfLawId { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public int? Code { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<Institution> Institution { get; set; }
        public virtual ICollection<InstitutionHistory> InstitutionHistory { get; set; }
    }
}
