using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EduLevelsToUgsCode
    {
        public short EducationLevelId { get; set; }
        public string QualificationCode { get; set; }

        public virtual AdmissionItemType EducationLevel { get; set; }
    }
}
