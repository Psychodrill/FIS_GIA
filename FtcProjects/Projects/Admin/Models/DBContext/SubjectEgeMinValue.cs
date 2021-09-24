using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class SubjectEgeMinValue
    {
        public int ScoreId { get; set; }
        public int SubjectId { get; set; }
        public int MinValue { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
