using System;
using System.ComponentModel;

namespace GVUZ.Web.ViewModels {
	public class ApplicationData {
		[DisplayName(@"Действие")]
		public int ApplicationID { get; set; }

		[DisplayName(@"Номер заявления")]
		public string ApplicationNumber { get; set; }

		public DateTime ApplicationRegistrationDateTime { get; set; }

		[DisplayName(@"Дата регистрации")]
		public string ApplicationRegistrationDate {
			get { return ApplicationRegistrationDateTime.ToString("dd.MM.yyyy"); }
		}

		[DisplayName(@"Приёмная кампания")]
		public string ApplicationCampaign { get; set; }

		[DisplayName(@"Конкурс")]
		public string ApplicationCompetitiveGroup { get; set; }

		[DisplayName(@"Статус заявления")]
		public string ApplicationStatus { get; set; }

		[DisplayName(@"Льгота")]
		public string ApplicationBenefit { get; set; }

		//[DisplayName(@"Сумма баллов")]
		//public decimal ApplicationRating { get; set; }

		public int ApplicationStatusID { get; set; }
	}
}