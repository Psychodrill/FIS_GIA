using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class RatingList
    {
        public int Id { get; set; }
        public int PositionOnList { get; set; }
        public bool WithoutEntranceExam { get; set; }
        public int PointsEe { get; set; }
        public bool RecomendetForEnroll { get; set; }
        public bool IsAgreed { get; set; }
        public bool CheckedExam { get; set; }
        public string ReasonForExclusion { get; set; }
        public int RatingId { get; set; }
        public int ApplicationId { get; set; }
        public bool InOrder { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? DateOfEdit { get; set; }

        public virtual Application Application { get; set; }
        public virtual Rating Rating { get; set; }
        public virtual UserPolicy User { get; set; }
    }
}
