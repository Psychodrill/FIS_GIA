using System.Collections.Generic;
using GVUZ.Web.ViewModels;

namespace GVUZ.Web.Portlets.Searches
{
	public class JsonInstitutionSearchResult
	{
		public Dictionary<string, object> Objects = new Dictionary<string, object>();
		public Dictionary<string, object> Children = new Dictionary<string, object>();
	}
}