using System;
using System.Collections;
using System.ComponentModel;
using FogSoft.Web.Mvc;

namespace GVUZ.Web.ViewModels
{
	public class AccessListViewModel
	{
		public int? SortID { get; set; }
		public int? PageNumber { get; set; }
		public int TotalPageCount { get; set; }
		public int TotalItemCount { get; set; }
		public int TotalItemFilteredCount { get; set; }

		public class LogData
		{
			public int ID { get; set; }

			[DisplayName("Логин")]
			public string Login { get; set; }
			[DisplayName("Наименование ОО")]
			public string InstitutionName { get; set; }

			public int InstitutionID { get; set; }

			[DisplayName("Дата")]
			public string DateCreated { get; set; }
			[DisplayName("Тип запроса")]
			public string Type { get; set; }
			
			[DisplayName("Содержание")]
			public string Content { get; set; }


			[DisplayName("Объект")]
			public string ObjectName { get; set; }

			[DisplayName("Метод доступа")]
			public string ObjectMethod { get; set; }
		}

		public LogData AccessLogDescr
		{
			get { return null; }
		}

		public LogData[] AccessLogs { get; set; }
		
		[DisplayName("Наименование ОО")]
		public string SelectedInstitution { get; set; }
		public IEnumerable Institutions { get; set; }

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