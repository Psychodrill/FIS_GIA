using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class InstitutionItemType
    {
        public InstitutionItemType()
        {
            InstitutionItem = new HashSet<InstitutionItem>();
        }

        public short ItemTypeId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<InstitutionItem> InstitutionItem { get; set; }
    }
}
