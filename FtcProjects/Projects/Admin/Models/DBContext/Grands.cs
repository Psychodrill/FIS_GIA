using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class Grands
    {
        public int Number { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
