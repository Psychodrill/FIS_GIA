using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class TmpEntrantsGak
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Middlename { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Documentseries { get; set; }
        public string Documentnumber { get; set; }
    }
}
