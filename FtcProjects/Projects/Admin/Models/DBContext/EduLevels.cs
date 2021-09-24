using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EduLevels
    {
        public EduLevels()
        {
            EduLevelDocumentType = new HashSet<EduLevelDocumentType>();
            ForeignInstitutions = new HashSet<ForeignInstitutions>();
        }

        public int LevelId { get; set; }
        public short AdmissionItemTypeId { get; set; }
        public string Name { get; set; }

        public virtual AdmissionItemType AdmissionItemType { get; set; }
        public virtual ICollection<EduLevelDocumentType> EduLevelDocumentType { get; set; }
        public virtual ICollection<ForeignInstitutions> ForeignInstitutions { get; set; }
    }
}
