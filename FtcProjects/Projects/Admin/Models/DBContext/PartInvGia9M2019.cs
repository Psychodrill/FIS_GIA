using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class PartInvGia9M2019
    {
        public int? Numb { get; set; }
        public string Lastname { get; set; }
        public string Name { get; set; }
        public string Middlename { get; set; }
        public string DocumentSeries { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? BDay { get; set; }
        public Guid? ParticipantId { get; set; }
    }
}
