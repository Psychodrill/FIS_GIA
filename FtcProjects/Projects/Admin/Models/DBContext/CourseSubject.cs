using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class CourseSubject
    {
        public int CourseSubjectId { get; set; }
        public int PreparatoryCourseId { get; set; }
        public int? SubjectId { get; set; }
        public string SubjectName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual PreparatoryCourse PreparatoryCourse { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
