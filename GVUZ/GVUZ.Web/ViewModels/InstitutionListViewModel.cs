using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using GVUZ.Web.Helpers;

namespace GVUZ.Web.ViewModels
{
	public class InstitutionListViewModel : IOrderable, IPageable
	{
		
		public int? SortID { get; set; }
		public int? PageNumber { get; set; }
		public int TotalPageCount { get; set; }
		public int TotalItemFilteredCount { get; set; }
		public int TotalItemCount { get; set; }

		public class FilterData
		{
			[DisplayName("Краткое наименование")]
			public string ShortName { get; set; }
			[DisplayName("Тип ОО")]
			public int InstitutionTypeID { get; set; }
			public int InstitutionTypeIDF { get; private set; }

			[DisplayName("Регион")]
			public int RegionID { get; set; }
			public int RegionIDF { get; set; }
			[DisplayName("Учредитель")]
			public string Owner { get; set; }

			[DisplayName("Полное наименование")]
			public string FullName { get; set; }
			[DisplayName("ИНН")]
			public string INN { get; set; }
			[DisplayName("ОГРН")]
			public string OGRN { get; set; }

			[DisplayName("Вид ОО")]
			public int FormOfLawID { get; set; }
		}

		public IEnumerable InstitututionTypes { get; set; }
		public IEnumerable FormOfLaws { get; set; }
		public IEnumerable Regions { get; set; }
		public string[] OwnerDepartments { get; set; }

		public FilterData Filter { get; set; }

		public class InstitutionData
		{
			[DisplayName("Краткое наименование")]
			public string ShortName { get; set; }
			[DisplayName("Полное наименование")]
			public string FullName { get; set; }
			[DisplayName("Тип ОО")]
			public string InstitutionTypeName { get; set; }
			[DisplayName("Вид ОО")]
			public string FormOfLawName { get; set; }
			[DisplayName("Регион")]
			public string RegionName { get; set; }
			[DisplayName("Учредитель")]
			public string Owner { get; set; }
			[DisplayName("Количество пользователей")]
			public int UserCount { get; set; }

			public int InstitutionID { get; set; }
            public int StatusId { get; set; }
		}

		public InstitutionData InstitutionDataNull { get { return null; } }

		public InstitutionData[] Institutions { get; set; }

		public int CurrentInstitutionID { get; set; }


	}
}