using GVUZ.Model.Institutions;

namespace GVUZ.Web.Portlets.Searches
{
	public class SearchService
	{
		public static string[] SearchInstitutionByName(string searchString)
		{
			using (InstitutionsEntities entities = new InstitutionsEntities())
			{
				return entities.GetInstitutionNames(searchString).ToArray();
			}
		}

		public static string[] SearchDirections(string searchString)
		{
			using (InstitutionsEntities entities = new InstitutionsEntities())
			{
				return entities.GetDirectionNames(searchString).ToArray();
			}
		}

		public static string[] SearchDirectionCodes(string searchString)
		{
			using (InstitutionsEntities entities = new InstitutionsEntities())
			{
				return entities.GetDirectionCodes(searchString).ToArray();
			}
		}

		public static string[] SearchRegions(string searchString)
		{
			using (InstitutionsEntities entities = new InstitutionsEntities())
			{
				return entities.GetRegionNames(searchString).ToArray();
			}
		}
	}
}