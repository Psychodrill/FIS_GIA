using System.ComponentModel;
using GVUZ.Web.Helpers;

namespace GVUZ.Web.ViewModels.Administration.Catalogs
{
	public class OlympicsViewModel : IPageable, IOrderable
	{
		public class OlympicData
		{
			[DisplayName("Действие")]
			public int OlympicID { get; set; }

			[DisplayName("Наименование олимпиады")]
			public string Name { get; set; }

			[DisplayName("Уровень")]
			public short? OlympicLevelID { get; set; }

		    [DisplayName("Уровень")]
			public string OlympicLevelName { get; set; }

            [DisplayName("Год олимпиады")]
            public int OlympicYear { get; set; }

			[DisplayName("Организатор")]
			public string OrganizerName { get; set; }
			
			[DisplayName("№ олимпиады")]
			public int OlympicNumber { get; set; }
		}

		public OlympicData[] Olympics { get; set; }

		public OlympicData OlympDescr { get { return null; } }
		
		public int? PageNumber { get; set; }
		public int TotalPageCount { get; set; }
		public int? SortID { get; set; }
	}
}