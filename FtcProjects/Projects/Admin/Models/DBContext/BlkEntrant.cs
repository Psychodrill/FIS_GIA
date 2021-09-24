using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BlkEntrant
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public int GenderId { get; set; }
        public string CustomInformation { get; set; }
        public string Snils { get; set; }
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Uid { get; set; }
        public int ImportPackageId { get; set; }
        public int InstitutionId { get; set; }
    }
}
