using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BlkApplicationIndividualAchievements
    {
        public string ApplicationUid { get; set; }
        public string Iauid { get; set; }
        public string Ianame { get; set; }
        public decimal? Iamark { get; set; }
        public string EntrantDocumentUid { get; set; }
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Uid { get; set; }
        public int ImportPackageId { get; set; }
        public int InstitutionId { get; set; }
    }
}
