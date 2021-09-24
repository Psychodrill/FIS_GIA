using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GVUZ.ServiceModel.Import.WebService.Dto
{
    public class FinSourceAndEduFormDto : BaseDto
    {
        [XmlElement(ElementName = "EducationalLevelID")]
        public int EducationLevelID;
        public int EducationFormID;
        public string CompetitiveGroupID;
        public int DirectionID;
    }
}
