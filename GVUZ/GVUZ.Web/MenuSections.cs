using System.Collections.Generic;

namespace GVUZ.Web
{
	public static class MenuSections
	{
		public const string ApplicationsDraft = "ApplicationsDraft";
		public const string Applications = "Applications";
		public const string Institution = "Institution";
		public const string Administration = "Administration";
		public const string Import = "Import";
		public const string Entrants = "Entrants";
		public const string EgeCheck = "EgeCheck";
	    public const string OlympiadCheck = "OlympiadCheck";
        public const string OlympiñDiplomant = "OlympiñDiplomant";
	    public const string OrdersOfAdmission = "OrdersOfAdmission";
        public const string InstitutionReports = "InstitutionReports";
        public const string AutoOrders = "AutoOrders";

		public static IEnumerable<string> GetAvailableSections()
		{
			yield return ApplicationsDraft;
			yield return Applications;
			yield return Institution;
			yield return Administration;
			yield return Import;
			yield return Entrants;
			yield return EgeCheck;
            yield return OlympiadCheck;
            yield return OlympiñDiplomant;
		    yield return OrdersOfAdmission;
            yield return InstitutionReports;
            yield return AutoOrders;
		}
	}
}