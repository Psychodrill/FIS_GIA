using System;
using System.Collections.Generic;

namespace GVUZ.DAL.Dto
{
    public class TransferCheckDto
    {
        public int DirectionID { get; set; }
        public string DirectionCode { get; set; }
        public string DirectionName { get; set; }
        public short EducationLevelID { get; set; }
        public string EducationLevelName { get; set; }
        public short EducationSourceID { get; set; }
        public string EducationSourceName { get; set; }
        public short EducationFormID { get; set; }
        public string EducationFormName { get; set; }
        public int AVVolume { get; set; }
        public int CGVolume { get; set; }
    }
}
