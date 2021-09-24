using System.Collections.Generic;
using System.ComponentModel;

namespace GVUZ.Web.Portlets.Searches
{
	public class SearchInformerViewModel
	{
        [DisplayName("ВУЗы")]
        public bool VuzCheckDirection { get; set; }

        [DisplayName("ССУЗы")]
        public bool SsuzCheckDirection { get; set; }

        [DisplayName("ВУЗы")]
        public bool VuzCheckInstitution { get; set; }

        [DisplayName("ССУЗы")]
        public bool SsuzCheckInstitution { get; set; }

        [DisplayName("Направление подготовки")]
        public string DirectionName { get; set; }

        [DisplayName("Образовательное учреждение")]
        public string InstitutionName { get; set; }
    }
}