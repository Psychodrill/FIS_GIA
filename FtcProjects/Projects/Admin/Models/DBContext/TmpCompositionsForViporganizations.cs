using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class TmpCompositionsForViporganizations
    {
        public Guid ParticipantId { get; set; }
        public int InstitutionId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
