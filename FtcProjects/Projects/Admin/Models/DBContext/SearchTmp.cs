using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class SearchTmp
    {
        public int DocumentId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int CategoryId { get; set; }
        public bool IsMedical { get; set; }
    }
}
