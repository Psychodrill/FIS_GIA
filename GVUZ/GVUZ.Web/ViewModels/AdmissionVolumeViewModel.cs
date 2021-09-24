using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FogSoft.Web.Mvc;
using GVUZ.Web.Controllers.Admission;

namespace GVUZ.Web.ViewModels
{
	public class AdmissionVolumeViewModel
	{
		public class RowData
		{
			[DisplayName("Уровень образования")]
			public short AdmissionItemTypeID { get; set; }
			public string AdmissionItemTypeName { get; set; }

			[DisplayName("Специальность")]
			public int? DirectionID { get; set; }
			public string DirectionName { get; set; }

            [DisplayName("Код")]
            public string DirectionCode { get; set; }
            public string DirectionNewCode { get; set; }

			public string ParentDirectionName { get; set; }
            public string ParentDirectionCode { get; set; }
            public string QualificationCode { get; set; }

			[DisplayName("Контрольные цифры приема (общий конкурс)")]
			public string BudgetName {get { return null; }}

			//[LocalRequired]
			[LocalRange(0, 99999)]
			[DisplayName("Очное обучение")]
			public int NumberBudgetO { get; set; }

			//[LocalRequired]
			[LocalRange(0, 99999)]
			[DisplayName("Очно-заочное обучение")]
			public int NumberBudgetOZ { get; set; }

			//[LocalRequired]
			[LocalRange(0, 99999)]
			[DisplayName("Заочное обучение")]
			public int NumberBudgetZ { get; set; }

			[DisplayName("Планируемый прием на места с оплатой стоимости обучения")]
			public string PaidName { get { return null; } }

			//[LocalRequired]
			[LocalRange(0, 99999)]
			[DisplayName("Очное обучение")]
			public int NumberPaidO { get; set; }

			//[LocalRequired]
			[LocalRange(0, 99999)]
			[DisplayName("Очно-заочное обучение")]
			public int NumberPaidOZ { get; set; }

			//[LocalRequired]
			[LocalRange(0, 99999)]
			[DisplayName("Заочное обучение")]
			public int NumberPaidZ { get; set; }


			[DisplayName("Целевой прием")]
			public string TargetName { get { return null; } }

			//[LocalRequired]
			[LocalRange(0, 99999)]
			[DisplayName("Очное обучение")]
			public int NumberTargetO { get; set; }

			//[LocalRequired]
			[LocalRange(0, 99999)]
			[DisplayName("Очно-заочное обучение")]
			public int NumberTargetOZ { get; set; }

			//[LocalRequired]
			[LocalRange(0, 99999)]
			[DisplayName("Заочное обучение")]
			public int NumberTargetZ { get; set; }

            [DisplayName("Квота приёма лиц, имеющих особое право")]
            public string QuotaName { get { return null; } }

            [LocalRange(0, 99999)]
            [DisplayName("Очное обучение")]
            public int? NumberQuotaO { get; set; }

            [LocalRange(0, 99999)]
            [DisplayName("Очно-заочное обучение")]
            public int? NumberQuotaOZ { get; set; }
            
            [LocalRange(0, 99999)]
            [DisplayName("Заочное обучение")]
            public int? NumberQuotaZ { get; set; }
            public bool GroupLast { get; set; }

			public bool DisableForEditing { get; set; }

			[DisplayName("UID")]
			[StringLength(200)]
			public string UID { get; set; }

		    public int AdmissionVolumeId { get; set; }

            [DisplayName("Доступно для распределения")]
            public int AvailableForDistribution { get; set; }

            [DisplayName("Из них распределено")]
            public int TotalDistributed { get; set; }

            public bool IsForUGS { get; set; }
            public int? ParentDirectionID { get; set; }
            public int ParentID { get; set; }
        }

		public RowData DisplayData { get { return null; } }

		public RowData[] Items { get; set; }

		public List<List<List<RowData>>> TreeItems { get; set; }

		public class CampaignData
		{
			public int ID { get; set; }
			public string Name { get; set; }
		}

		public CampaignData[] AllowedCampaigns { get; set; }
		public CampaignData[] AllowedCoursesByCampaign { get; set; }
		[DisplayName("Приемная кампания")]
		public int SelectedCampaignID { get; set; }
		[DisplayName("Курс")]
		public int SelectedCourse { get; set; }

		public class AvailFormsInfo
		{
			public int FormID { get; set; }
			public int LevelID { get; set; }
			public int SourceID { get; set; }
		}

		public AvailFormsInfo[] AvailForms { get; set; }
		
		public bool IsFormAvail(int levelID, int sourceID, int formID)
		{

			return AvailForms != null && AvailForms.Any(x => x.FormID == formID && x.LevelID == levelID && x.SourceID == sourceID);
		}

        public bool IsQuotaEnabled(int levelId)
        {
            return (levelId == 2 || levelId == 3 || levelId == 5 || levelId == 19);
        }

		public class DirectionInfo
		{
            [DisplayName("Id направления (DirectionID)")]
			public int DirectionID { get; set; }
		    public int ParentID { get; set; }

		    [DisplayName("Код")]
			public string DirectionCode { get; set; }
			[DisplayName("Наименование")]
			public string DirectionName { get; set; }
			[DisplayName("Код УГС")]
			public string ParentCode { get; set; }
			[DisplayName("Наименование УГС")]
			public string ParentName { get; set; }
			[DisplayName("Уровень образования")]
			public string EducationLevelName { get; set; }

			public string QualificationCode { get; set; }

            [DisplayName("Новый код")]
            public string NewCode { get; set; }
		}

		public DirectionInfo DirectionDisp { get { return null; } }

		public bool CanEdit { get; set; }

		public Dictionary<string, DirectionInfo> CachedDirections { get; set; }

        public string[] BudgetLevels { get; set; }
	}
}