using System.ComponentModel;

namespace GVUZ.Web.ViewModels
{
	public class CompetitiveGroupViewModel1234
	{
		[DisplayName("Уровень образования")]
		public short EducationalLevelID { get; set; }
		[DisplayName("Уровень образования")]
		public string EducationalLevelName { get; set; }

		[DisplayName("Специальность")]
		public int? DirectionID { get; set; }
		public string DirectionName { get; set; }

		public int GroupID { get; set; }

		[DisplayName("Наименование конкурса")]
		public string GroupName { get; set; }

		[DisplayName("Курс")]
		public string CourseName { get; set; }

		[DisplayName("Приемная кампания")]
		public string CampaignName { get; set; }

		[DisplayName("Бюджетные места (общий конкурс)")]
		public string BudgetName { get { return null; } }

		[DisplayName("Очное обучение")]
		public int? NumberBudgetO { get; set; }

		[DisplayName("Очно-заочное обучение")]
		public int? NumberBudgetOZ { get; set; }

		[DisplayName("Заочное обучение")]
		public int? NumberBudgetZ { get; set; }

        [DisplayName("Квота приёма лиц, имеющих особое право")]
        public string QuotaName { get { return null; } }

        [DisplayName("Очное обучение")]
        public int? NumberQuotaO { get; set; }

        [DisplayName("Очно-заочное обучение")]
        public int? NumberQuotaOZ { get; set; }

        [DisplayName("Заочное обучение")]
        public int? NumberQuotaZ { get; set; }
        
        [DisplayName("Места с оплатой стоимости обучения")]
		public string PaidName { get { return null; } }

		[DisplayName("Очное обучение")]
		public int? NumberPaidO { get; set; }

		[DisplayName("Очно-заочное обучение")]
		public int? NumberPaidOZ { get; set; }

		[DisplayName("Заочное обучение")]
		public int? NumberPaidZ { get; set; }


		[DisplayName("Целевой прием")]
		public string TargetName { get { return null; } }

		[DisplayName("Очное обучение")]
		public int? NumberTargetO { get; set; }

		[DisplayName("Очно-заочное обучение")]
		public int? NumberTargetOZ { get; set; }

		[DisplayName("Заочное обучение")]
		public int? NumberTargetZ { get; set; }


		[DisplayName("Вступительные испытания")]
		public int EntranceTestCount { get; set; }

		public bool CanEdit { get; set; }
	}
}