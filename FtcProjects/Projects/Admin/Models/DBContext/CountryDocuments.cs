using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class CountryDocuments
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public int DocumentId { get; set; }

        public virtual CountryType Country { get; set; }
        public virtual DocumentType Document { get; set; }
    }
}
