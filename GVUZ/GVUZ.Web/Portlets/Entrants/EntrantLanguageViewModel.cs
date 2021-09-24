using System.Collections;
using GVUZ.Web.Models;

namespace GVUZ.Web.Portlets.Entrants
{
	public class EntrantLanguageViewModel
	{
		public bool IsView { get; set; }
		public int EntrantID { get; set; }

		public int[] LanguageData { get; set; }
		public string[] LanguageDataView { get; set; }

		public IEnumerable LanguageList;
		public ApplicationStepType ApplicationStep { get; set; }

		public bool ShowDenyMessage { get; set; }

		public int? ApplicationID { get; set; }
	}
}