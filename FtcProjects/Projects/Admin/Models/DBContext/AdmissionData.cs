using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class AdmissionData
    {
        public int AdmissionStructureId { get; set; }
        public int AdmissionItemId { get; set; }
        public int InstitutionId { get; set; }
        public int? ParentId { get; set; }
        public short Depth { get; set; }
        public string Lineage { get; set; }
        public short ItemTypeId { get; set; }
        public short ItemLevel { get; set; }
        public int? PlaceCount { get; set; }
        public int? TotalDirectionPlaceCount { get; set; }
        public string Name { get; set; }
        public bool? HasMilitaryDepartment { get; set; }
        public bool? HasPreparatoryCourses { get; set; }
        public bool? HasOlympics { get; set; }
        public short? EducationLevelId { get; set; }
        public int? DirectionId { get; set; }
        public short? StudyId { get; set; }
        public short? AdmissionTypeId { get; set; }
        public bool IsLeaf { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
