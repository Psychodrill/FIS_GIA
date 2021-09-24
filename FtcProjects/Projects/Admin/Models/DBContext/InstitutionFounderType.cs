using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class InstitutionFounderType
    {
        public InstitutionFounderType()
        {
            InstitutionFounder = new HashSet<InstitutionFounder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string EiisId { get; set; }

        public virtual ICollection<InstitutionFounder> InstitutionFounder { get; set; }
    }
}
