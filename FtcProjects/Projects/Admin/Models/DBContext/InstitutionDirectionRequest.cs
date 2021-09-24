using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class InstitutionDirectionRequest
    {
        public int RequestId { get; set; }
        public int InstitutionId { get; set; }
        public int DirectionId { get; set; }
        public int RequestType { get; set; }
        public DateTime RequestDate { get; set; }
        public string RequestComment { get; set; }
        public DateTime? DenialDate { get; set; }
        public string DenialComment { get; set; }
        public bool IsDenied { get; set; }

        public virtual Direction Direction { get; set; }
        public virtual Institution Institution { get; set; }
    }
}
