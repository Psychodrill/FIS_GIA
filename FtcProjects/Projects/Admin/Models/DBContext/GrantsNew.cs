using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class GrantsNew
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? Date { get; set; }
    }
}
