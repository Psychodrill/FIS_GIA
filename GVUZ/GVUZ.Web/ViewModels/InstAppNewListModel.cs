using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace GVUZ.Web.ViewModels {

	public class InstAppNewListModel{
		public int InstitutionID { get; set; }

		public class DataFileds {
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

			public DataFileds() { }
		}

		public class FilterFields {	// Фильтры		
			public int InstitutionID { get; set; }
			public string OrderField { get; set; }
			public string OrderDirection { get; set; }
			public int PageSize { get; set; }
			public int CurrentPage { get; set; }
			public int ListType { get; set; } // 0 - Новые, 1 - Не прошедшие проверку, 2 - Отозванные, 3 - Принятые, 4 - Рекомендованные к зачислению

			[DisplayName("№ заявления")]
			public string ApplicationNumber { get; set; }
			[DisplayName("Сдал документы")]
			public int? OriginalDocumentsReceived { get; set; }

			[DisplayName("Фамилия")]
			public string EntrantLastName { get; set; }
			[DisplayName("Имя")]
			public string EntrantFirstName { get; set; }
			[DisplayName("Отчество")]
			public string EntrantMiddleName { get; set; }

			[DisplayName("Дата регистрации c")]
			public DateTime? RegistrationDateFrom { get; set; }
			[DisplayName("Дата регистрации по")]
			public DateTime? RegistrationDateTo { get; set; }

			[DisplayName("Конкурс")]
			public int? CompetitiveGroupId { get; set; }		

			[DisplayName("Серия паспорта")]
			public string DocumentSeries { get; set; }
			[DisplayName("Номер паспорта")]
			public string DocumentNumber { get; set; }

			[DisplayName("Льгота")]
			public int? Benefit { get; set; }
			[DisplayName("Рекомендован к зачислению")]
			public int? Recomended { get; set; }

			public FilterFields() { }

		}

		public List<string> Errors { get; set; }
		public int PageSize { get; set; }
		public int CurrentPage { get; set; }
		public int Count { get; set; }
		public int Total { get; set; }

		public List<DataFileds> Items { get; set; }
		public DataFileds			Data { get { return null; } }
		public FilterFields		Filter { get { return null; } }

		public IEnumerable Benefits { get; set; }
		public IEnumerable CompetitiveGroups { get; set; }

		//public List<> Model.Benefits
		public InstAppNewListModel(){
			Items=new List<DataFileds>();
			Errors=new List<string>();
		}
	}
}