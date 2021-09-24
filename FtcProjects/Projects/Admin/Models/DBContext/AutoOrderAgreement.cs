using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class AutoOrderAgreement
    {
        public int Id { get; set; }
        public bool IsAgreed { get; set; }
        public int InstitutionId { get; set; }
    }
}
