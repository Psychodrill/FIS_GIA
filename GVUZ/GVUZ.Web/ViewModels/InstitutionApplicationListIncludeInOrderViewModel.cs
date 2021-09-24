using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FogSoft.Web.Mvc;

namespace GVUZ.Web.ViewModels
{
	public class InstitutionApplicationListIncludeInOrderViewModel
	{
		public int InstitutionID { get; set; }

		public class ApplicationData
		{
			[DisplayName("Действие")]
			public int ApplicationID { get; set; }
			[DisplayName("№ заявления")]
			public string ApplicationNumber { get; set; }
			[DisplayName("ФИО")]
			public string EntrantFIO { get; set; }
			[DisplayName("Док-т, удостоверяющий личность")]
			public string EntrantDocData { get; set; }

			[DisplayName("Уровень образования")]
			public string EducationLevel { get; set; }
			[DisplayName("Форма обучения")]
			public string EducationForm { get; set; }
			[DisplayName("Специальность")]
			public string DirectionName { get; set; }
			[DisplayName("Льгота")]
			public string BenefitName { get; set; }
			[DisplayName("Количество баллов")]
			public decimal BallCount { get; set; }
            [DisplayName("Организация целевого приёма")]
            public string TargetOrganisationName { get; set; }
			public string BallCountString
			{
				get
				{
					return BallCount.ToString("0.####");
				}
			}

			public bool CanBeDeleted { get; set; }

			public string EducationalSource { get; set; }
			public bool OriginalDocumentsRecieved { get; set; }
			public DateTime DenyDate { get; set; }// Дата отзыва заявления
			public string StatusName { get; set; }
			public string ViolationName { get; set; }
			public string CompetitiveGroupName { get; set; }

			public DateTime ApplicationDate { get; set; }
			public string ApplicationUID { get; set; }
		}

		public ApplicationData AppDescr
		{
			get { return null; }
		}

		public ApplicationData[] Applications { get; set; }

		public class FilterDetails
		{
			[DisplayName("Дата регистрации с")]
			[Date(">today-100y")]
			[Date("<=today")]
			public DateTime? DateBegin { get; set; }
			[DisplayName("по")]
			[Date(">today-100y")]
			[Date("<=today")]
			public DateTime? DateEnd { get; set; }

			[DisplayName("Номер заявления ОО")]
			public string ApplicationNumber { get; set; }
			[DisplayName("Фамилия")]
			public string LastName { get; set; }
			[DisplayName("Имя")]
			public string FirstName { get; set; }
			[DisplayName("Отчество")]
			public string MiddleName { get; set; }

			[DisplayName("Док-т, удост. личность")]
			public string DocumentLabel { get; set; }
			[DisplayName("серия")]
			public string DocumentSeries { get; set; }
			[DisplayName("№")]
			public string DocumentNumber { get; set; }

			[DisplayName("Специальность")]
			public string DirectionName { get; set; }

			[DisplayName("Конкурс")]
			public string CompetitiveGroupName { get; set; }
		}

		public string[] Directions { get; set; }
		public string[] CompetitiveGroups { get; set; }

		public const string EmptyText = "[Не важно]";

		public FilterDetails Filter { get; set; }

		public int? SortID { get; set; }
		public int? TabID { get; set; }
		public int? PageNumber { get; set; }
		public int TotalPageCount { get; set; }
		public int TotalItemFilteredCount { get; set; }
		public int TotalItemCount { get; set; }

		public int OrderID { get; set; }

		public IncludeInOrderListViewModel Order { get; set; }

		[DisplayName("Идентификатор в БД ОО (UID)")]
		public string OrderUID { get; set; }
	}
}