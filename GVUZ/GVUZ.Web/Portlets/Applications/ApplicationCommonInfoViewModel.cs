using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Serialization;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Web.Models;
using GVUZ.Web.Controllers.Admission;

namespace GVUZ.Web.Portlets.Applications
{
	public class ApplicationCommonInfoViewModel : ApplicationInfoViewModelBase
	{
		[DisplayName("Особые права")]
		public string Olympics { get; set; }

		public class CompetitiveGroupInfo
		{
			public int CompetitiveGroupID { get; set; }

			[DisplayName("Наименование конкурса")]
			public string CompetitiveGroupName { get; set; }

			[DisplayName("Конкурс")]
			public decimal Competition { get; set; }

			[DisplayName("Количество мест")]
			public int Places { get; set; }

			[DisplayName("Количество заявлений")]
			public int Requests { get; set; }

			[DisplayName("Рейтинг")]
			public string Rate { get; set; }

			[DisplayName("Приказ о зачислении")]
			public string EnrollmentOrder { get; set; }

			[DisplayName("Количество баллов")]
			public string Points { get; set; }
		}

		public CompetitiveGroupInfo[] CompetitiveGroups { get; set; }
	}
}