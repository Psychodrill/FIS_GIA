using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models
{
    public class InstitutionDetail
    {
        public int InstitutionId { get; set; }
        public string FullName { get; set; }
        public string BriefName { get; set; }
    }

    public class InstitutionDocs
    {
        public int AttachmentId { get; set; }
        public string DocumentName { get; set; }
        public string DisplayName { get; set; }

    }

}
