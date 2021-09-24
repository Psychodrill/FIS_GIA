using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BansFromRon
    {
        public int? Id { get; set; }
        public int EsrpId { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public DateTime? DateBegine { get; set; }
        public DateTime? DateEnd { get; set; }
        public string Comment { get; set; }
    }
}
