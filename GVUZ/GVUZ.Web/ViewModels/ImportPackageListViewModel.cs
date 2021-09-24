using System;
using System.Collections;
using System.ComponentModel;
using FogSoft.Web.Mvc;

namespace GVUZ.Web.ViewModels
{
	public class ImportListViewModel
	{
		public int? SortID { get; set; }
		public int? PageNumber { get; set; }
		public int TotalPageCount { get; set; }
		public int TotalItemCount { get; set; }
		public int TotalItemFilteredCount { get; set; }

		public class ImportPackageData
		{
			public int ID { get; set; }

			[DisplayName("Идентификатор")]
			public string PackageID { get; set; }
			[DisplayName("Логин")]
			public string Login { get; set; }
			[DisplayName("Наименование ОО")]
			public string InstitutionName { get; set; }

			public int InstitutionID { get; set; }

			[DisplayName("Дата отправки")]
			public string DateSent { get; set; }
			[DisplayName("Дата обработки")]
			public string DateProcessing { get; set; }
			[DisplayName("Тип запроса")]
			public string Type { get; set; }
			[DisplayName("Содержание")]
			public string Content { get; set; }
			[DisplayName("Статус")]
			public string Status { get; set; }
		}

		public ImportPackageData ImportPackageDescr
		{
			get { return null; }
		}

		public ImportPackageData[] ImportPackages { get; set; }
		
		[DisplayName("Наименование ОО")]
		public string SelectedInstitution { get; set; }
		public IEnumerable Institutions { get; set; }

        public IEnumerable Logins { get; set; }

		[DisplayName("Тип")]
		public int SelectedType { get; set; }
		public IEnumerable Types { get; set; }

		[DisplayName("Дата отправки с")]
		[Date("<=today")]
		public DateTime? DateBegin { get; set; }
		[DisplayName("по")]
		[Date("<=today")]
		public DateTime? DateEnd { get; set; }

		[DisplayName("Логин")]
		public string SelectedLogin { get; set; }
	}
}