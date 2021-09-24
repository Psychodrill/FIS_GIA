using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GVUZ.ServiceModel.Import.WebService.Dto
{
    public class RecListDto : BaseDto
    {
        [XmlArrayItem(ElementName = "FinSourceEduForm")]
        public FinSourceAndEduFormDto[] FinSourceAndEduForms;

        [XmlElement(ElementName = "Application")]
        public RecListApplicationDto Application;
    }
}
