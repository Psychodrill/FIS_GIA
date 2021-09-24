using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using FogSoft.Web.Mvc;
using GVUZ.DAL.Dapper.ViewModel.CompetitiveGroups;
using System.Collections.Generic;

namespace GVUZ.Web.Portlets.Applications
{
	public class ApplicationEntranceTestViewModelC
	{
		public class EntranceTestData
		{
			public int EntranceTestItemID { get; set; }
			[DisplayName("Дисциплина")]
			public string SubjectName { get; set; }

			public int CompetitiveGroupID;
			[DisplayName("Конкурс")]
			public string CompetitiveGroupName { get; set; }
			public int EntranceTestType { get; set; }
			public bool HasBenefits { get; set; }
			public int SubjectID { get; set; }
			public bool IsProfileSubject { get; set; }
			public bool IsEgeSubject { get; set; }

            public string CompetitiveGroupEduLevelCode { get; set; }

            [DisplayName("Приоритет")]
            public int? Priority { get; set; }

            //public string StudyBeginningDate { get; set; }
            //public string StudyEndingDate { get; set; }
            //public string StudyPeriod 
            //{

            //	set
            //             {
            //		//this.CompetitiveGroupProperties.
            //             }
            //}
            public List<CompetitiveGroupProperty> CompetitiveGroupProperties { get; set; }
        }

		public EntranceTestData DescrTestData {	get { return null; }	}
		public EntranceTestData[] EntranceTests { get; set; }

        public List<CompetitiveGroupProperty> CompetitiveGroupProperties { get; set; }
        public class AttachedDocumentData
		{
			public int ID { get; set; }
			public int EntranceTestItemID { get; set; }
			public int CompetitiveGroupID { get; set; }
			[DisplayName("Основание для оценки")]
			public int SourceID { get; set; }
            public Int16 BenefitID { get; set; }
			[DisplayName("Балл")]
			public decimal ResultValue { get; set; }

            [DisplayName("Балл ЕГЭ")]
            public decimal EgeResultValue { get; set; }
			public string ResultValueString
			{
				get { return ResultValue.ToString(); }
				set { ResultValue = Convert.ToDecimal(value.Replace(',', '.'), CultureInfo.InvariantCulture); }
			}
			public int EntrantDocumentID { get; set; }
			public int ApplicationID { get; set; }
			public int DocumentTypeID { get; set; }

			public string DocumentDescription { get; set; }

			public int SubjectID { get; set; }

			public int? InstitutionDocumentTypeID { get; set; }
			public string InstitutionDocumentTypeName { get; set; }
			public string InstitutionDocumentNumber { get; set; }
			public DateTime? InstitutionDocumentDate { get; set; }

			public string BenefitErrorMessage { get; set; }
		}

		public AttachedDocumentData DescrAttachedData
		{
			get { return null; }
		}


		public AttachedDocumentData[] AttachedDocs { get; set; }

		public AttachedDocumentData[] GlobalDocs { get; set; }

		public bool ShowDenyMessage { get; set; }

		public int ApplicationID { get; set; }

		public int EntrantID { get; set; }

	    public bool HasEgeDocuments { get; set; }

		public class SelectionDocumentData
		{
			public int DocumentID { get; set; }
			public int TypeID { get; set; }
			public string Description { get; set; }
			public string DenyMessage { get; set; }
		}

		public class SelectionDocumentsForEntranceTest
		{
			public int EntranceTestItemID { get; set; }
			public SelectionDocumentData[] DocAdd { get; set; }
			public SelectionDocumentData[] DocExisting { get; set; }
			public SelectionDocumentData[] DocCurrent { get; set; }
		}

		public class SelectionDocumentsForEntranceTestManual
		{
			public int EntranceTestItemID { get; set; }
			public SelectionDocumentData[] DocExisting { get; set; }
			public bool CanAddManualValue { get; set; }
			public bool ShouldCheckEgeBefore { get; set; }
			public string SubjectName { get; set; }
		}

		public class InstitutionDocumentData{
			[DisplayName("Тип документа")]
			public int DocumentTypeID { get; set; }
			[StringLength(50)]
			[DisplayName("Номер документа")]
			public string DocumentNumber { get; set; }
			[DisplayName("Дата выдачи")]
			public DateTime DocumentDate { get; set; }
			[DisplayName("Балл")]
			//[LocalRange(0, 100)]
			public decimal Value { get; set; }
		}

		public InstitutionDocumentData DescrInstitutionDocument {get { return null; }}

		public IEnumerable InstitutionDocumentTypes { get; set; }

		public int Course { get; set; }

      /// <summary>
      /// Флаг, определяющий, доступна ли льгота "По квоте приёма лиц, имеющих особое право"
      /// </summary>
      public bool IsQuotaBenefitEnabled { get; set; }

		//public int BenefitCompetitiveGroupID { get; set; }
		//public IEnumerable CompetitiveGroupNames { get; set; }
	}
}