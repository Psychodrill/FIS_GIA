using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ViewModels
{
	public class CompetitiveGroupSelectedDirectionsViewModel
	{
		public int CompetitiveGroupID { get; set; }
		public int EducationLevelID { get; set; }
		public int FilterType { get; set; }
		public int[] SelectedDirections { get; set; }
	}
}