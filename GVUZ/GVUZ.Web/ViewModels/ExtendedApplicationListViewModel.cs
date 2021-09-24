using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using FogSoft.Web.Mvc;

namespace GVUZ.Web.ViewModels
{
	public class ExtendedApplicationListViewModel
	{
		public int InstitutionID { get; set; }

		public class ApplicationData
		{
			[DisplayName("Действие")]
			public int ApplicationID { get; set; }
			[DisplayName("№ заявления")]
			public string ApplicationNumber { get; set; }

			public int EntrantID { get; set; }

			[DisplayName("ФИО")]
			public string EntrantFIO { get; set; }
			[DisplayName("Док-т, удостоверяющий личность")]
			public string EntrantDocData { get; set; }
			[DisplayName("Дата регистрации")]
			public string ApplicationDate { get; set; }

			[DisplayName("Статус")]
			public string StatusName { get; set; }

			public int StatusID { get; set; }
			[DisplayName("Конкурс")]
			public string CompetitiveGroupName { get; set; }

			public string ViolationName { get; set; }
			public string Benefit { get; set; }
			public string EducationalForm { get; set; }
			public string EducationalSource { get; set; }
			public bool OriginalDocumentsRecieved { get; set; }
			
			public string DirectionName { get; set; } // специальность
			public string Rating { get; set; } // cумма баллов
			public DateTime DenyDate { get; set; } // Дата отзыва заявления

			public DateTime ApplicationDateDate { get; set; }
			public string ApplicationUID { get; set; }
		}

		public ApplicationData AppDescr
		{
			get { return null; }
		}

		[DisplayName("Сведения о заявлении")]
		public string AppPartDescr
		{
			get { return null; }
		}

		[DisplayName("Сведения об абитуриенте")]
		public string EntrantPartDescr
		{
			get { return null; }
		}

		public ApplicationData[] Applications { get; set; }

		public class FilterDetails
		{
			[DisplayName("Номер заявления")]
			public string ApplicationNumber { get; set; }

			[DisplayName("Дата регистрации с")]
			[Date(">today-100y")]
			[Date("<=today")]
			public DateTime? DateBegin { get; set; }
			[DisplayName("по")]
			[Date(">today-100y")]
			[Date("<=today")]
			public DateTime? DateEnd { get; set; }

			[DisplayName("Конкурс")]
			public int? CompetitiveGroupID { get; set; }

			[DisplayName("Статус заявления")]
			public int[] ApplicationStatusID { get; set; }

			[DisplayName("Фамилия")]
			public string EntrantLastName { get; set; }
			[DisplayName("Имя")]
			public string EntrantFirstName { get; set; }
			[DisplayName("Отчество")]
			public string EntrantMiddleName { get; set; }
			[DisplayName("Серия паспорта")]
			public string EntrantDocSeries { get; set; }
			[DisplayName("№ паспорта")]
			public string EntrantDocNumber { get; set; }

            [DisplayName("Приёмная кампания")]
            public int? CampaignID { get; set; }
		}

		public const string EmptyText = "[Не важно]";

		public FilterDetails Filter { get; set; }

		public IEnumerable CompetitiveGroups { get; set; }
		public IEnumerable ApplicationStatuses { get; set; }

		public int? SortID { get; set; }
		public int? PageNumber { get; set; }
		public int TotalPageCount { get; set; }
		public int TotalItemCount { get; set; }
		public int TotalItemFilteredCount { get; set; }

		[DisplayName("Причина решения")]
		public string StatusDecision { get; set; }

		public int ApplicationID { get; set; }

	}
}