using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BulkEntrant
    {
        public int? Id { get; set; }
        public Guid Guid { get; set; }
        public int InstitutionId { get; set; }
        public int ImportPackageId { get; set; }
        public string Uid { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public int GenderId { get; set; }
        public string CustomInformation { get; set; }
        public string Email { get; set; }
        public int? RegionId { get; set; }
        public int? TownTypeId { get; set; }
        public string Address { get; set; }
        public string IsFromKrymEntrantDocumentUid { get; set; }
    }
}
