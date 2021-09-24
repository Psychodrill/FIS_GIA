using System.ComponentModel;
using GVUZ.Web.Helpers;

namespace GVUZ.Web.ViewModels.Administration.Catalogs
{
	public class ForeignLanguagesViewModel : IPageable,IOrderable
	{

		public class ForeignLanguageData
		{
			[DisplayName("Действие")]
			public int LanguageID { get; set; }

			[DisplayName("Наименование")]
			public string Name { get; set; }
		}

		public ForeignLanguageData[] ForeignLanguages { get; set; }

		public ForeignLanguageData LangDescr { get { return null; } }				

		public int? PageNumber { get; set; }
		public int TotalPageCount { get; set; }
		public int? SortID { get; set; }
	}
}