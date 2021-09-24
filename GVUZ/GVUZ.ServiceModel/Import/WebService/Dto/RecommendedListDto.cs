using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GVUZ.ServiceModel.Import.WebService.Dto
{
    public class RecommendedListDto : BaseDto
    {
        public int Stage;

        [XmlArrayItem(ElementName = "RecList")]
        public RecListDto[] RecLists;
    }

    public class RecommendedListShort
    {
        public int Stage { get; set; }

        [XmlArrayItem(ElementName="RecList")]
        public RecListApplicationDto[] RecLists;
    }
}
