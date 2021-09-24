using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class AllowedDirections
    {
        public int InstitutionId { get; set; }
        public int DirectionId { get; set; }
        public short? AdmissionItemTypeId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Year { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int AllowedDirectionsId { get; set; }
        public string EiisId { get; set; }
        public int? EsrpId { get; set; }
        public int? AllowedDirectionStatusId { get; set; }

        public virtual AdmissionItemType AdmissionItemType { get; set; }
        public virtual AllowedDirectionStatus AllowedDirectionStatus { get; set; }
        public virtual Direction Direction { get; set; }
        public virtual Institution Institution { get; set; }
    }
}
