using System.ComponentModel;
using GVUZ.Web.Helpers;

namespace GVUZ.Web.ViewModels.Administration.Catalogs
{
	public class QualificationsViewModel : IPageable, IOrderable
	{
		public class QualificationData
		{
			[DisplayName("Действие")]
			public int QualificationID { get; set; }

			[DisplayName("Наименование")]
			public string Name { get; set; }

			[DisplayName("Код")]
			public string Code { get; set; }
		}

		public QualificationData[] Qualifications { get; set; }

		public QualificationData QuaDescr { get { return null; } }
		
		public int? PageNumber { get; set; }
		public int TotalPageCount { get; set; }
		public int? SortID { get; set; }
	}
}