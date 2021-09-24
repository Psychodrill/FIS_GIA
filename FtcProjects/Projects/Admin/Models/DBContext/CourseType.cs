using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class CourseType
    {
        public CourseType()
        {
            PreparatoryCourse = new HashSet<PreparatoryCourse>();
        }

        public int CourseId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<PreparatoryCourse> PreparatoryCourse { get; set; }
    }
}
