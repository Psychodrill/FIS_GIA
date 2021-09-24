using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class Entrant
    {
        public int? EntrantId { get; set; }
        public int? PersonId { get; set; }
        public int? InstitutionId { get; set; }
    }
}
