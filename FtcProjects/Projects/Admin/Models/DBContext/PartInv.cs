using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class PartInv
    {
        public int? Number { get; set; }
        public Guid? Participantid { get; set; }
        public string Gia { get; set; }
    }
}
