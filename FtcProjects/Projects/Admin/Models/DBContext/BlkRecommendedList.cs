using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BlkRecommendedList
    {
        public int Stage { get; set; }
        public string ApplicationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int EduLevelId { get; set; }
        public int EduFormId { get; set; }
        public int DirectionId { get; set; }
        public string CompetitiveGroupUid { get; set; }
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Uid { get; set; }
        public int ImportPackageId { get; set; }
        public int InstitutionId { get; set; }
    }
}
