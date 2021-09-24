using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class TmpEgeMarksStat
    {
        public int? InstId { get; set; }
        public int? AppId { get; set; }
        public int? EntrId { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string DocSer { get; set; }
        public string DocNum { get; set; }
        public string RegionName { get; set; }
        public int? RegionCode { get; set; }
        public int Id { get; set; }
        public int? MainId { get; set; }
        public string FullName { get; set; }
        public string FullNameParent { get; set; }
        public string IsPrivate { get; set; }
        public string TypeName { get; set; }
        public string UchrName { get; set; }
        public string DirName { get; set; }
        public string DirNewCode { get; set; }
        public string EduFormName { get; set; }
        public string EduLevelName { get; set; }
        public string EduFinansSourceName { get; set; }
        public int? Rus { get; set; }
        public int? Math { get; set; }
        public int? Obsh { get; set; }
        public int? Hist { get; set; }
        public int? Fis { get; set; }
        public int? Him { get; set; }
        public int? Bio { get; set; }
        public int? InYaz { get; set; }
        public int? Litr { get; set; }
        public int? Geo { get; set; }
        public int? Inform { get; set; }
        public int? OlimpId { get; set; }
        public string OlimpName { get; set; }
        public string OlimpOrganizerName { get; set; }
        public int? CompGroupId { get; set; }
        public string SubjectName { get; set; }
    }
}
