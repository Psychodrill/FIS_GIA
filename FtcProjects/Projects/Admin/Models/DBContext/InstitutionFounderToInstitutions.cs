using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class InstitutionFounderToInstitutions
    {
        public int Id { get; set; }
        public int InstitutionFounderId { get; set; }
        public int InstitutionId { get; set; }
        public string EiisId { get; set; }

        public virtual Institution Institution { get; set; }
        public virtual InstitutionFounder InstitutionFounder { get; set; }
    }
}
