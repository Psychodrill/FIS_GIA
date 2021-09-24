using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class PreparatoryCourse
    {
        public PreparatoryCourse()
        {
            CourseSubject = new HashSet<CourseSubject>();
        }

        public int PreparatoryCourseId { get; set; }
        public int InstitutionId { get; set; }
        public string Information { get; set; }
        public int? MoreInformation { get; set; }
        public int? CourseTypeId { get; set; }
        public string CourseName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual CourseType CourseType { get; set; }
        public virtual Institution Institution { get; set; }
        public virtual Attachment MoreInformationNavigation { get; set; }
        public virtual ICollection<CourseSubject> CourseSubject { get; set; }
    }
}
