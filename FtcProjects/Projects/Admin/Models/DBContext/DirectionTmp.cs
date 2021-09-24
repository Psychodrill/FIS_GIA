using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class DirectionTmp
    {
        public int DirectionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string SysGuid { get; set; }
        public string Edulevel { get; set; }
        public string Eduprogramtype { get; set; }
        public string Ugscode { get; set; }
        public string Ugsname { get; set; }
        public string Qualificationcode { get; set; }
        public string Qualificationname { get; set; }
        public string Period { get; set; }
        public string EduDirectory { get; set; }
        public string EduprAdditional { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string NewCode { get; set; }
    }
}
