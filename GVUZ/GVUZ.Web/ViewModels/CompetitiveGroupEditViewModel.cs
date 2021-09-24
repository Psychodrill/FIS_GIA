using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GVUZ.Web.ViewModels
{
	public class CompetitiveGroupEditViewModel
	{
		public int GroupID { get; set; }

		[DisplayName("Наименование конкурса")]
		[StringLength(250)]
		public string Name { get; set; }

        public int FilterCampaign { get; set; }

		[DisplayName("Курс")]
		public int CourseID { get; set; }

		//[DisplayName("Уровень образования")]
		//public int EducationalLevelID { get; set; }

		[DisplayName("Идентификатор в БД ОО (UID)")]
		[StringLength(200)]
		public string Uid { get; set; }

		//public string EducationLevelName { get; set; }

		[DisplayName("Приемная кампания")]
		public int CampaignID { get; set; }
		public string CampaignName { get; set; }

		public IEnumerable AllowedDirections { get; set; }
		public IEnumerable AllowedEdLevels { get; set; }
		public int AddEducationLevelID { get; set; }

		public int DirectionFilterType { get; set; }
		public bool AllowAnyDirectionsFilterType { get; set; }

		public class DisplayDataNames
		{
			[DisplayName("Специальности")]
			public string Directions { get { return null; } }

			[DisplayName("Уровень образования")]
			public string EducationLevel { get { return null; } }

			[DisplayName("Бюджетные места (общий конкурс)")]
			public string BudgetName { get { return null; } }

            [DisplayName("Квота приёма лиц, имеющих особое право")]
            public string QuotaName { get { return null; } }
            
            [DisplayName("Места с оплатой стоимости обучения")]
			public string PaidName { get { return null; } }

			[DisplayName("Целевой прием")]
			public string TargetName { get { return null; } }

			[DisplayName("Очное обучение")]
			public int NumberBudgetO { get { return 0; } }

			[DisplayName("Очно-заочное обучение")]
			public int NumberBudgetOZ { get { return 0; } }

			[DisplayName("Заочное обучение")]
			public int NumberBudgetZ { get { return 0; } }

            [DisplayName("Очное обучение")]
            public int NumberQuotaO { get { return 0; } }

            [DisplayName("Очно-заочное обучение")]
            public int NumberQuotaOZ { get { return 0; } }

            [DisplayName("Заочное обучение")]
            public int NumberQuotaZ { get { return 0; } }
            
            [DisplayName("Очное обучение")]
			public int NumberPaidO { get { return 0; } }

			[DisplayName("Очно-заочное обучение")]
			public int NumberPaidOZ { get { return 0; } }

			[DisplayName("Заочное обучение")]
			public int NumberPaidZ { get { return 0; } }

			[DisplayName("UID")]
			public int UID { get { return 0; } }

		}

		public DisplayDataNames DisplayData { get { return null; } }

		public class OrganizationData
		{
			public string Name { get; set; }
			public string UID { get; set; }
			public int ID { get; set; }
			public bool CanDelete { get; set; }
		}

		public OrganizationData[] Organizations { get; set; }
		public class DirectionInfo
		{
			public int DirectionID { get; set; }
			public int EducationLevelID { get; set; }
			public string DirectionName { get; set; }
			public int[] Data { get; set; }
			public string[] DataTargetUIDs { get; set; }

			[StringLength(200)]
			public string UID { get; set; }
		}

		public DirectionInfo[] Rows { get; set; }

		public int EntranceTestCount { get; set; }

		public class OOZZInfo
		{
			public bool HasO { get; set; }
			public bool HasOZ { get; set; }
			public bool HasZ { get; set; }

			public int CountOOZZ { get { return (HasO ? 1 : 0) + (HasOZ ? 1 : 0) + (HasZ ? 1 : 0); } }
		}

		//filter by edlevels
		public Dictionary<int, OOZZInfo> HasBudget;
		public Dictionary<int, OOZZInfo> HasPaid;
		public Dictionary<int, OOZZInfo> HasTarget;

		public bool CanEdit { get; set; }

		public Dictionary<string, AdmissionVolumeViewModel.DirectionInfo> CachedDirections { get; set; }
	}
}