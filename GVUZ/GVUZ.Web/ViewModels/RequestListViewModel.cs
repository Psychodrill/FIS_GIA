using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using GVUZ.Web.Helpers;

namespace GVUZ.Web.ViewModels
{
	public class RequestListViewModel : IOrderable, IPageable
	{
		
		public int? SortID { get; set; }
		public int? PageNumber { get; set; }
		public int TotalPageCount { get; set; }
		public int TotalItemFilteredCount { get; set; }
		public int TotalItemCount { get; set; }

		public class FilterData
		{
			[DisplayName("Полное наименование")]
			public string FullName { get; set; }
			[DisplayName("Число заявок")]
			public int RequestNumber { get; set; }
			[DisplayName("Дата последней заявки")]
			public DateTime Date { get; set; }
		}
		public FilterData Filter { get; set; }

		public class InstitutionData
		{
            [DisplayName("Полное наименование")]
            public string FullName { get; set; }
            [DisplayName("Число заявок")]
            public int RequestNumber { get; set; }
            [DisplayName("Дата последней заявки")]
            public string Date { get; set; }

			public int InstitutionID { get; set; }
		}

        /*public class RequestData
        {
            [DisplayName("Специальность")]
            public string Direction { get; set; }
            [DisplayName("Дата последней заявки")]
            public string Comment { get; set; }

            public int InstitutionID { get; set; }
        }*/

		public InstitutionData InstitutionDataNull { get { return null; } }

		public InstitutionData[] Institutions { get; set; }

		public int CurrentInstitutionID { get; set; }
	}
}