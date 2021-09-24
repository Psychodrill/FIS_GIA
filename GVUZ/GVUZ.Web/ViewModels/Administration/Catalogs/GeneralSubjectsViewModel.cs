using System.ComponentModel;
using GVUZ.Web.Helpers;

namespace GVUZ.Web.ViewModels.Administration.Catalogs
{
	public class GeneralSubjectsViewModel : IPageable, IOrderable
	{
		public class GeneralSubjectData
		{
			[DisplayName(@"Действие")]
			public int SubjectID { get; set; }

			[DisplayName(@"Наименование предмета")]
			public string Name { get; set; }
		}

		public GeneralSubjectData[] GeneralSubjects { get; set; }

		public GeneralSubjectData SubjDescr { get { return null; } }
		
		public int? PageNumber { get; set; }
		public int TotalPageCount { get; set; }
		public int? SortID { get; set; }
	}
}