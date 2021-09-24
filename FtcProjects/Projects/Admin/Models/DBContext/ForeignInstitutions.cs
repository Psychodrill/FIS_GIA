using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ForeignInstitutions
    {
        public int Id { get; set; }
        public int CountryTypeId { get; set; }
        public string Name { get; set; }
        public int? EduLevelId { get; set; }

        public virtual CountryType CountryType { get; set; }
        public virtual EduLevels EduLevel { get; set; }
    }
}
