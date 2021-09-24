using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class InstitutionStructure
    {
        public InstitutionStructure()
        {
            InverseParent = new HashSet<InstitutionStructure>();
        }

        public int InstitutionStructureId { get; set; }
        public int InstitutionItemId { get; set; }
        public int? ParentId { get; set; }
        public short Depth { get; set; }
        public string Lineage { get; set; }
        public string Uid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual InstitutionItem InstitutionItem { get; set; }
        public virtual InstitutionStructure Parent { get; set; }
        public virtual ICollection<InstitutionStructure> InverseParent { get; set; }
    }
}
