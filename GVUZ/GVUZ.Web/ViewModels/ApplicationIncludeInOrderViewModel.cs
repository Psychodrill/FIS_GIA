using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using FogSoft.Web.Mvc;

namespace GVUZ.Web.ViewModels
{
	public class ApplicationIncludeInOrderViewModel
	{
		public int ApplicationID { get; set; }

		[DisplayName("Приказ")]
		public int OrderStageID { get; set; }

		[DisplayName("№ заявления")]
		[LocalRequired]
		public string ApplicationNumber { get; set; }
		[DisplayName("ФИО")]
		public string FIO { get; set; }
		[DisplayName("Документ, удостоверяющий личность")]
		public string DocumentData { get; set; }

		[DisplayName("Специальность")]
		public int DirectionID { get; set; }
		public IEnumerable Directions { get; set; }
        public int[] DirectionIDs { get; set;  }
        public string[] DirectionNames { get; set; }


		[DisplayName("Льготный приказ")]
		public bool IsForBeneficiary { get; set; }
		public int[] BeneficiaryDirections { get; set; }

		[DisplayName("Форма обучения")]
		public int EducationFormID { get; set; }
		public IEnumerable EducationForms { get; set; }

		[DisplayName("Источник финансирования")]
		public int EducationSourceID { get; set; }
		public IEnumerable EducationSources { get; set; }

		public bool NoData { get; set; }

		public int OrderTypeSelection { get; set; }
		public bool OrderForcePublished { get; set; }

		public string[] AvailableForms { get; set; }
		public string[] AvailableSources { get; set; }

		public bool NoOriginalDocuments { get; set; }
        public bool ForListener { get; set; }
        public bool IsVuz { get; set; }

		[DisplayName("Организация целевого приема")]
		public string TargetOrganizationNameO { get; set; }

		[DisplayName("Организация целевого приема")]
		public string TargetOrganizationNameOZ { get; set; }

		[DisplayName("Организация целевого приема")]
		public string TargetOrganizationNameZ { get; set; }

		public string OrderErrorMessage { get; set; }


        // IDs заявлений для массового включения заявлений в приказ
        public int[] applicationIds { get; set; }

        // абитуриент (для массовых операций - проверять, что все заявления от разных абитуриентов)
        public int EntrantID { get; set; }

        public bool CanIncludeInOrder { get; set; }

        // включить заявление в отдельный приказ для иностранцев если заявитель не гражданин РФ, 
        // выбрано бюджетное финансирование и отмечено данное поле
        [DisplayName("По межправительственным соглашениям")]
        public bool IsBudgetForeigner { get; set; }

        // Признак иностранного гражданина
	    public bool IsForeignCitizen { get; set; }
	}
}