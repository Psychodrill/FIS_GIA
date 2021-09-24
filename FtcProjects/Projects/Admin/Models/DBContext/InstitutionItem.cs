using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class InstitutionItem
    {
        public InstitutionItem()
        {
            InstitutionStructure = new HashSet<InstitutionStructure>();
            InverseParent = new HashSet<InstitutionItem>();
        }

        public int InstitutionItemId { get; set; }
        public int InstitutionId { get; set; }
        public short ItemTypeId { get; set; }
        public string Name { get; set; }
        public string BriefName { get; set; }
        public int? DirectionId { get; set; }
        public int? ParentId { get; set; }
        public string Site { get; set; }
        public string Uid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Direction Direction { get; set; }
        public virtual Institution Institution { get; set; }
        public virtual InstitutionItemType ItemType { get; set; }
        public virtual InstitutionItem Parent { get; set; }
        public virtual ICollection<InstitutionStructure> InstitutionStructure { get; set; }
        public virtual ICollection<InstitutionItem> InverseParent { get; set; }
    }
}
