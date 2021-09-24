using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EduProgramTypes
    {
        public EduProgramTypes()
        {
            Direction = new HashSet<Direction>();
        }

        public string Code { get; set; }
        public string EiisId { get; set; }
        public string Name { get; set; }
        public string Shortname { get; set; }
        public int? EsrpId { get; set; }
        public int Id { get; set; }

        public virtual ICollection<Direction> Direction { get; set; }
    }
}
