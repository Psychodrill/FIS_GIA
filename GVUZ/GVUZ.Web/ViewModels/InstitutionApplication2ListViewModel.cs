using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;

namespace GVUZ.Web.ViewModels {

	public class InstitutionApplication2ListModel {
		public int InstitutionID { get; set; }

		public class ApplicationData {
			[DisplayName("Действие")]
			public int ApplicationID { get; set; }
			[DisplayName("№ заявления")]
			public string ApplicationNumber { get; set; }
			public int Status { get; set; }			
			[DisplayName("Статус")]
			public string StatusName { get; set; }
			[DisplayName("Дата последней проверки")]
			public DateTime? LastCheckDate { get; set; }
			[DisplayName("Конкурс")]
			public string CompetitiveGroups { get; set; }
			[DisplayName("ФИО")]
			public string EntrantFIO { get; set; }
			[DisplayName("Фамилия")]
			public string EntrantLastName { get; set; }
			[DisplayName("Имя")]
			public string EntrantFirstName { get; set; }
			[DisplayName("Отчество")]
			public string EntrantMiddleName { get; set; }

			[DisplayName("Вид документа удостов. личность")]
			public string DocumentTypeName { get; set; }
			[DisplayName("Документ, удостоверяющий личность")]
			public string DocumentNumber { get; set; }

			[DisplayName("Дата рождения")]
			public DateTime? DocumentBirthDate { get; set; }

			[DisplayName("Дата регистрации")]
			public DateTime? RegistrationDate { get; set; }

			[DisplayName("Рекомендован к зачислению")]
			public string Recomended { get; set; }

			public ApplicationData() { }
		}

		public class FilterFields {
			[DisplayName("№ заявления")]
			public string ApplicationNumber { get; set; }
			[DisplayName("Сдал документы")]
			public bool? OriginalDocumentsReceived { get; set; }

			[DisplayName("Фамилия")]
			public string EntrantLastName { get; set; }
			[DisplayName("Имя")]
			public string EntrantFirstName { get; set; }
			[DisplayName("Отчество")]
			public string EntrantMiddleName { get; set; }

			[DisplayName("Дата регистрации")]
			public DateTime? RegistrationDate { get; set; }

			[DisplayName("Конкурс")]
			public int? CompetitiveGroupItemId { get; set; }

			[DisplayName("Серия паспорта")]
			public string DocumentSeries { get; set; }
			[DisplayName("Номер паспорта")]
			public string DocumentNumber { get; set; }

			[DisplayName("Льгота")]
			public int? BenefitID { get; set; }
			[DisplayName("Рекомендован к зачислению")]
			public bool? Recomended { get; set; }
		}

		public List<ApplicationData> Applications { get; set; }
		public List<string> Errors { get; set; }
		public int PageSize { get; set; }
		public int CurrentPage { get; set; }
		public int ApplicationCount { get; set; }
		public int ApplicationTotal { get; set; }

		public ApplicationData FieldDescr {	get { return null; }	}
		public FilterFields FilterField {	get { return null; }		}

		public InstitutionApplication2ListModel() {
			Applications=new List<ApplicationData>();
			Errors=new List<string>();
		}
	}
}