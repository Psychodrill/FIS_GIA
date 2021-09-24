using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using FogSoft.Web.Mvc;
using System.Globalization;

namespace GVUZ.Web.ViewModels {

	#region Wz3
	public class ApplicationWz3ViewModel {

		public ApplicationWz3ViewModel() {	}

		public int ApplicationID { get; set; }
		public int? WizardStepID { get; set; }
		public int EntrantID { get; set; }
        public int StatusID { get; set; }
        public int IsFromKrym { get; set; }
		public bool ShowDenyMessage { get; set; }
		public bool HasEgeDocuments { get; set; }
		public short Course { get; set; }
		// public InstitutionDocumentData DescrInstitutionDocument { get { return null; } }
		public List<AppCompetitiveGroup> AppComGroups=new List<AppCompetitiveGroup>();
		public List<EntrantTestItemDocument> Documents=new List<EntrantTestItemDocument>();
		public List<IDName> InstitutionDocumentTypes=new List<IDName>();
		public EntrantTestItemDocument InsDocDescr { get { return null; } }
		public List<AppComposition> CompositionResult { get; set; }
		private static readonly AppComposition _baseComposition=new AppComposition();
		public AppComposition BaseComposition { get { return _baseComposition; } }
        public int GetChekcEGE { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsDistant { get; set; }
        public int? IsDisabledDocumentID { get; set; }
        public string DistantPlace { get; set; }
    }

    public class AppCompetitiveGroup {

		[DisplayName("Конкурс")]
		public int GroupID { get; set; }

		[DisplayName("Конкурс")]
		public string GroupName { get; set; }

		public short Course { get; set; }

		public List<EntranceTestItem> TestItems=new List<EntranceTestItem>();

		public List<EntrantTestItemDocument> BenefitDocuments=new List<EntrantTestItemDocument>();

		// BenefitID	Name 1	Зачисление без вступительных испытаний
		[DisplayName("Зачисление без вступительных испытаний")]
		public EntrantTestItemDocument Benefit1 { get; set; }
		// BenefitID	Name 4	По квоте приёма лиц, имеющих особое право
		[DisplayName("По квоте приёма лиц, имеющих особое право")]
		public EntrantTestItemDocument Benefit4 { get; set; }
		// BenefitID	Name 5	Преимущественное право на поступление
		[DisplayName("Преимущественное право на поступление")]
		public EntrantTestItemDocument Benefit5 { get; set; }

		public EntranceTestItem EntranceTestItemDescr { get { return null; } }

		// Флаг, определяющий, доступна ли льгота "По квоте приёма лиц, имеющих особое право"
		public bool IsQuotaBenefitEnabled { get; set; }
		public bool HasBenefits { get; set; }
	}
	public class EntranceTestItem {
		public int? ID { get; set; }

		[DisplayName("Конкурс")]
		public int? GroupID { get; set; }

		[DisplayName("Конкурс")]
		public string GroupName { get; set; }

		public int? SubjectID { get; set; }
		public string SubjectName { get; set; }
		public bool? SubjectIsEge { get; set; }

		[DisplayName("Дисциплина")]
		public string SubjectNameView { get; set; }

		[DisplayName("Приоритет")]
		public int? Priority { get; set; }

		[DisplayName("Приоритет")]
		public int? TestPriority { get; set; }

		[DisplayName("Балл")]
		public decimal ResultValue { get; set; }

		[DisplayName("Балл ЕГЭ")]
		public decimal EgeResultValue { get; set; }

		public bool? HasEge { get; set; }

		public string ResultValueString {
			get { return ResultValue.ToString(); }
			set { ResultValue=Convert.ToDecimal(value.Replace(',','.'),CultureInfo.InvariantCulture); }
		}
		[DisplayName("Основание для оценки")]
		public int SourceID { get; set; }

		public int? AETD_ID { get; set; }

		public short? EntranceTestTypeID { get; set; }
		public int? EntranceTestItemID { get; set; }
		public bool? IsProfileSubject { get; set; }
		public short? BenefitID { get; set; }

		public EntrantTestItemDocument Doc { get; set; }

        public int? EducationLevelID { get; set; } 

        public bool? ApplicationIsForSPOandVO { get; set; }

        public bool? EntranceTestIsForSPOandVO { get; set; }

        public int? ReplacedEntranceTestItemID { get; set; }
    }
	public class EntrantTestItemDocument {
		public int? ID { get; set; }
		public int? ApplicationID { get; set; }
		public int? CompetitiveGroupID { get; set; }

	    public int EgeMinValue { get; set; }
	    public int IsFromKrym { get; set; }

	    public int? SourceID { get; set; }
		public int? SubjectID { get; set; }
		public short? BenefitID { get; set; }
		public string BenefitName { get; set; }
		public int? EntranceTestItemID { get; set; }

		public int?	EntrantDocumentID { get; set; }
		public int?	DocumentTypeID { get; set; }
		public string DocumentTypeName { get; set; }
		public string DocumentSeries { get; set; }
		public string DocumentNumber { get; set; }

		[DisplayFormat(DataFormatString="{0:dd.MM.yyyy}")]
		public DateTime? DocumentDate { get; set; }
		public string DocumentDateString { get { return ((DocumentDate==null)?"":DocumentDate.Value.ToString("dd.MM.yyyy")); } }

		[DisplayName("Тип документа")]
		public int? InstitutionDocumentTypeID { get; set; }
		public string InstitutionDocumentTypeName { get; set; }

		[DisplayName("Дата выдачи")]
		[DisplayFormat(DataFormatString="{0:dd.MM.yyyy}")]
		public DateTime? InstitutionDocumentDate { get; set; }
		public string InstitutionDocumentDateString { get { return ((InstitutionDocumentDate==null)?"":InstitutionDocumentDate.Value.ToString("dd.MM.yyyy")); } }

	    public int? OlympApproved { get; set; }
	    public int? AppealStatusID { get; set; }
	    public string AppealStatusName { get; set; }

	    [DisplayName("Номер документа")]
		public string InstitutionDocumentNumber { get; set; }
		[DisplayName("Балл")]
		public decimal? ResultValue { get; set; }
		public string ResultValueString {
			get { return ResultValue.ToString(); }
			set { ResultValue=Convert.ToDecimal(value.Replace(',','.'),CultureInfo.InvariantCulture); }
		}
		public bool? HasEge { get; set; }
		public decimal? EgeResultValue { get; set; }
		public bool isCommon { get { return (BenefitID!=null && SubjectID==null && EntranceTestItemID==null); } }

		public int? EGE_SubjectID { get; set; }
		public string Description {
			#region DocList
			/*
1	Документ, удостоверяющий личность
2	Свидетельство о результатах ЕГЭ
3	Аттестат о среднем (полном) общем образовании
4	Диплом о высшем профессиональном образовании
5	Диплом о среднем профессиональном образовании
6	Диплом о начальном профессиональном образовании
7	Диплом о неполном высшем профессиональном образовании
8	Академическая справка
9	Диплом победителя/призера олимпиады школьников
10	Диплом победителя/призера всероссийской олимпиады школьников
11	Справка об установлении инвалидности
12	Заключение психолого-медико-педагогической комиссии
13	Заключение об отсутствии противопоказаний для обучения
14	Военный билет
15	Иной документ
16	Аттестат об основном общем образовании
17	Справка ГИА
18	Справка об обучении в другом ВУЗе
19	Иной документ об образовании
20	Диплом чемпиона/призера Олимпийских игр
21	Диплом чемпиона/призера Паралимпийских игр
22	Диплом чемпиона/призера Сурдлимпийских игр
23	Диплом чемпиона мира
24	Диплом чемпиона Европы
25	Диплом об окончании аспирантуры (адъюнкатуры)
26	Диплом кандидата наук
*/ 
			#endregion
			get {
				//if(ResultValue==0 && DocumentTypeID==2) {	// Случай ошибочного назначения докмента 
				//    return "Для данного предмета нет результата в указанном сертификате ЕГЭ";
				//}
				if(EntrantDocumentID!=null) {
					if(DocumentTypeID==2 && EGE_SubjectID==null ){	// Если в справке ЕГЭ не найден необходимый предмет, то
						return "Для данного предмета нет результата в указанном свидетельстве ЕГЭ";
					}
					if(DocumentTypeID==17 && EGE_SubjectID==null ){	// Если в справке ГИА не найден необходимый предмет, то
						return "Для данного предмета нет результата в указанной справке ГИА";
					}
					if(DocumentTypeID==2) {
						if(AppealStatusID!=null) {
							if(AppealStatusID>0) {
								return (DocumentTypeName+" "+(DocumentSeries??"")+" № "+DocumentNumber+" "+ (DocumentDate!=null?"от "+DocumentDate.Value.ToString("yyyy")+" года":"")+ " (Апелляция - "+AppealStatusName+")");
							}
						}
						return (DocumentTypeName+" "+(DocumentSeries??"")+" № "+DocumentNumber+" "+(DocumentDate!=null?"от "+DocumentDate.Value.ToString("yyyy")+" года":""));
					}
					if((DocumentTypeID==9)||(DocumentTypeID==10)) {
						if(OlympApproved==1) {
							return (DocumentTypeName+" "+(DocumentSeries??"")+" № "+DocumentNumber+" "+ (DocumentDate!=null?"от "+DocumentDate.Value.ToString("yyyy")+" года":"")+ " (результаты подтверждены)");
						}
						return (DocumentTypeName+" "+(DocumentSeries??"")+" № "+DocumentNumber+" "+ (DocumentDate!=null?"от "+DocumentDate.Value.ToString("yyyy")+" года":"")+  " (результаты не подтверждены)");
					}
					return (DocumentTypeName+" "+(DocumentSeries??"")+" № "+DocumentNumber+" "+(DocumentDate!=null?"от "+DocumentDate.Value.ToString("dd.MM.yyyy"):""));
				}
				if(InstitutionDocumentTypeID!=null) {
					return InstitutionDocumentTypeName+" "+InstitutionDocumentNumber+" "+(InstitutionDocumentDate!=null?"от "+InstitutionDocumentDate.Value.ToString("dd.MM.yyyy"):"");
				}
				if(SubjectID!=null&&EntrantDocumentID==null) { return "Результат ЕГЭ (балл не проверен)"; }
				if(BenefitName!=null) {
					return BenefitName+" "+DocumentSeries??""+" № "+DocumentNumber+" "+(DocumentDate!=null?"от "+DocumentDate.Value.ToString("dd.MM.yyyy"):"");
				}
				return "Описания документа нет";
			}
		}

        public int? EducationLevelID { get; set; }
    }


	public class AppComposition {

        [DisplayName("Год")]
		[DataType(dataType :DataType.Date)]
		[DisplayFormat(DataFormatString="{0:dd.MM.yyyy}")]
		public DateTime? Year { get; set; }

		public string strYear { get { return string.Format("{0:yyyy}",Year); } }

		[DisplayName("Тема")]
		public string acrName { get; set; }

		[DisplayName("Результат")]
		public bool acrResult { get; set; }

        [DisplayName("Дата проведения экзамена")]
        [DataType(dataType: DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? ExamDate { get; set; }
        public string strExamDate { get { return string.Format("{0:dd.MM.yyyy}", ExamDate); } }

        [DisplayName("Наличие апелляции")]
        public bool? HasAppeal { get; set; }

        [DisplayName("Статус перепроверки")]
        public string AppealStatus { get; set; }

        [DisplayName("Дата загрузки")]
        [DataType(dataType: DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? DownloadDate { get; set; }
        public string strDownloadDate { get { return string.Format("{0:dd.MM.yyyy hh:mm:ss}", DownloadDate); } }

        [DisplayName("Путь к файлам")]
        public string CompositionPaths { get; set; }
    }


    #endregion

    public class SelectDocumentType {
		public int? ID { get; set; }
		public string Name { get; set; }
	}

	public class SelectEntDoc {
		public int? EntrantDocumentID { get; set; }
        public int? EntrantDocumentTypeID { get; set; }
		public string DocumentTypeName { get; set; }
		public string DocumentSeries { get; set; }
		public string DocumentNumber { get; set; }
        public bool? OlympApproved { get; set; }

        public string DocumentOlympApproved
        {
            get { return ((OlympApproved.GetValueOrDefault()) ? " (результаты подтверждены)" : " (результаты не подтверждены)"); }
        }

		[DisplayFormat(DataFormatString="{0:dd.MM.yyyy}")]
		public DateTime?	DocumentDate { get; set; }

	    public string DocumentDateString
	    {
	        get { return ((DocumentDate == null) ? "" : DocumentDate.Value.ToString("dd.MM.yyyy")); }
	    }

	    public int? EntrantID { get; set; }

		public string Description {
			get {
                return DocumentTypeName + " " + DocumentSeries + " № " + DocumentNumber + " " + (DocumentDate != null ? "от " + DocumentDate.Value.ToString("dd.MM.yyyy") : "") + ((EntrantDocumentTypeID == 9 || EntrantDocumentTypeID == 10) ? DocumentOlympApproved : "");
			}
		}
	}
	public class SelectEntDocsList {
		public int? SubjectId { get; set; }
		public List<SelectDocumentType> DocumentTypes=new List<SelectDocumentType>();
		public List<SelectEntDoc> EntrantDocuments=new List<SelectEntDoc>();
	}
}