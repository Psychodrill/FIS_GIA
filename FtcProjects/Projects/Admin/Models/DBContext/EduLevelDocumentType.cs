using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EduLevelDocumentType
    {
        public int Id { get; set; }
        public int LevelId { get; set; }
        public int DocumentTypeId { get; set; }

        public virtual DocumentType DocumentType { get; set; }
        public virtual EduLevels Level { get; set; }
    }
}
