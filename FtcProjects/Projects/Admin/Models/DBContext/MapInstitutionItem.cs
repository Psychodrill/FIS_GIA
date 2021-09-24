using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class MapInstitutionItem
    {
        public int NewInstitutionItemId { get; set; }
        public int? OldInstitutionItemId { get; set; }
        public string NewUid { get; set; }
        public string OldUid { get; set; }
    }
}
