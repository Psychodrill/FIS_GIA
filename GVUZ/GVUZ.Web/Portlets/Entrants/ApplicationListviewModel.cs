using System;
using System.ComponentModel;

namespace GVUZ.Web.Portlets.Entrants
{
	public class ApplicationListViewModel
	{
		public int EntrantID { get; set; }

		public class ApplicationData
		{
			public int ApplicationID { get; set; }
			[DisplayName("Образовательное учреждение")]
			public string InstitutionName { get; set; }

			[DisplayName("Дата регистрации")]
			public DateTime RegistrationDate { get; set; }
			[DisplayName("Статус")]
			public string StatusName { get; set; }

			public int StatusID { get; set; }
		}

		public ApplicationData AppDescr { get { return null; } }

		public ApplicationData[] Applications { get; set; }
	}
}