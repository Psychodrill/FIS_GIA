using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class TmpOlympicTypeN
    {
        public int OlympicId { get; set; }
        public string Name { get; set; }
        public int? OlympicNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int OlympicYear { get; set; }
    }
}
