using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using GVUZ.Web.Helpers;

namespace GVUZ.Web.ViewModels.Administration.Catalogs
{
	public class EntranceTestsViewModel : IPageable, IOrderable
	{

		public class TestData
		{
			[DisplayName(@"Действие")]
			public int ID { get; set; }

			[DisplayName(@"Общеобразовательные предметы")]
			public string[] Subjects { get; set; }

			[DisplayName(@"Направления подготовки (специальности)")]
			public string[] Directions { get; set; }
		}

		public TestData TestDecsr
		{
			get { return null; }
		}

		public TestData[] Tests { get; set; }

		public int? PageNumber { get; set; }

		public int TotalPageCount { get; set; }

		public int? SortID { get; set; }
	}
}