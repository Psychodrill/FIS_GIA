using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class TmpSledstvieVedetNikita
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
