using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using FogSoft.Web.Mvc;
using GVUZ.Helper.MVC;

namespace GVUZ.Web.ViewModels
{
	public class EntranceTestViewModelC : BaseEditViewModel
	{
		public IEnumerable EntranceSubjects { get; set; }

		public int CompetitiveGroupID { get; set; }

		[DisplayName("Вступительное испытание")]
		public int? EntranceTestID { get; set; }

		[StringLength(100)]
		public string EntranceTestName { get; set; }

		[DisplayName("Форма проведения")]
		[LocalRequired]
		[StringLength(100)]
		public string Form { get; set; }

		[DisplayName("UID")]
		[StringLength(200)]
		public string UID { get; set; }

		//для показа
		[DisplayName("Аттестационное испытание")]
		public int AttestationType { get; set; }

		[DisplayName("Мин. балл")]
		[LocalRange(0, 100)]
		public decimal? MinScore { get; set; }

        [DisplayName("Приоритет (От 1 до 10, 1 – максимальный приоритет)")]
        [LocalRange(1, 10, ErrorMessage = "Приоритет должен быть в диапазоне от 1 до 10")]
        public int? EntranceTestPriority { get; set; }

		public string MinScoreString
		{
			get { return MinScore.ToString(); }
			set { MinScore = String.IsNullOrEmpty(value) ? (decimal?)null : Convert.ToDecimal(value.Replace(',', '.'), CultureInfo.InvariantCulture); }
		}

		public int? TestItemID { get; set; }

		public int TestTypeID { get; set; }

		public bool HideMainType { get; set; }

		public bool CanCopyFromParent { get; set; }
		public string ParentLevelName { get; set; }

		public EntranceTestItemData[] TestItems { get; set; }
		public EntranceTestItemData[] CreativeTestItems { get; set; }
		public EntranceTestItemData[] CustomTestItems { get; set; }
		public EntranceTestItemData[] ProfileTestItems { get; set; }

		public string CreativeTestItemName { get; set; }
		public int CreativeTestTypeID { get; set; }

		public string CustomTestItemName { get; set; }
		public int CustomTestTypeID { get; set; }

		public string ProfileTestItemName { get; set; }		

		public class EntranceTestItemData
		{
			public int ItemID { get; set; }
			public int TestType { get; set; }
			public string TestName { get; set; }
			public string Form { get; set; }
			public decimal? Value { get; set; }
            public int? EntranceTestPriority { get; set; }

			public string ValueString { get { return Value.HasValue ? Value.Value.ToString("0.####") : ""; } }

			/// <summary>
			/// Для возврата, чтобы правильно количество пересчитывать в дереве
			/// </summary>
			public bool CanRemove { get; set; }

			public int BenefitCount { get; set; }

			[StringLength(200)]
			public string UID { get; set; }
		}

		public EntranceTestViewModelC()
		{
		}

		public EntranceTestViewModelC(int competitiveGroupID)
		{
			CompetitiveGroupID = competitiveGroupID;
		}

		public virtual bool IsAdd
		{
			get { return true; }
		}

		public bool EditDisabled { get; set; }

		public int? ProfileTestSubjectID { get; set; }

		public int GeneralBenefitCount { get; set; }

		public bool CanEdit { get; set; }

		public int MainTestAllowedCount { get; set; }

		public bool IsCustomEntranceTestsForMainIsAllowed { get; set; }

		public bool IsAnyCountOfProfileItemsAllowed { get; set; }
	}
}