using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using FogSoft.Web.Mvc;
using GVUZ.Model.Institutions;

namespace GVUZ.Web.ViewModels {
	public class InstitutionApplicationListViewModel {

		public InstitutionApplicationListViewModel() { }

		public int InstitutionID { get; set; }

		public class ApplicationData {
			[DisplayName("")]
			public bool Checked { get; set; }
			[DisplayName("Действие")]
			public int ApplicationID { get; set; }
			[DisplayName("№ заявления")]
			public string ApplicationNumber { get; set; }
			[DisplayName("ФИО")]
			public string EntrantFIO { get; set; }
			[DisplayName("Документ, удостоверяющий личность")]
			public string EntrantDocData { get; set; }
			[DisplayName("Дата регистрации")]
			public string ApplicationDate { get; set; }
			[DisplayName("Источник")]
			public string Source { get; set; }
			[DisplayName("Тип нарушения")]
			public string ViolationName { get; set; }
			[DisplayName("Приемная кампания")]
			public string CampaignName { get; set; }
			// нужен для массовых операций
			[DisplayName("Причина решения")]
			public string StatusDecision { get; set; }
			[DisplayName("Статус")]
			public string StatusName { get; set; }
			public int StatusID { get; set; }
			[DisplayName("Дата последней проверки")]
			public string LastCheckDate { get; set; }
			[DisplayName("Конкурс")]
			public string CompetitiveGroupName { get; set; }
			public string Benefit { get; set; }
			public string BenefitShort { get; set; }
			public int BenefitID { get; set; }
			[DisplayName("Сдал документы")]
			public bool OriginalDocumentsRecieved { get; set; }
			public decimal NumberRating { get; set; }

			[DisplayName("Дата рождения")]
			public string BirthDate { get; set; }

			public string IdentityDocumentType { get; set; }

			[DisplayName("Рейтинг")]
			// cумма баллов
			public string Rating {
				get {
					if(NumberRating<0) return "";//невалидный рейтинг
					if(BenefitID==0)
						return NumberRating.ToString("0.####");
					return NumberRating.ToString("0.####")+" ("+BenefitShort+")";
				}
			}

			public string ApplicationUID { get; set; }
			public DateTime ApplicationDateDate { get; set; }

			public bool IsRequiresBudgetO { get; set; }
			public bool IsRequiresBudgetOZ { get; set; }
			public bool IsRequiresBudgetZ { get; set; }
			public bool IsRequiresPaidO { get; set; }
			public bool IsRequiresPaidOZ { get; set; }
			public bool IsRequiresPaidZ { get; set; }
			public bool IsRequiresTargetO { get; set; }
			public bool IsRequiresTargetOZ { get; set; }
			public bool IsRequiresTargetZ { get; set; }

			public ApplicationPrioritiesViewModel Priorities { get; set; }

			private bool IsRequiresFormSource(int form,int source) {
				if(source==EDSourceConst.Budget)
					return Priorities.ApplicationPriorities.Any(x => x.EducationFormId==form&&(x.EducationSourceId==source||x.EducationSourceId==EDSourceConst.Quota)&&x.Priority.HasValue);
				else return Priorities.ApplicationPriorities.Any(x => x.EducationFormId==form&&x.EducationSourceId==source&&x.Priority.HasValue);
			}

			public uint RequiresMask {
				get {
					uint mask=0;
					mask=IsRequiresFormSource(EDFormsConst.O,EDSourceConst.Budget)?mask|1:mask;
					mask=IsRequiresFormSource(EDFormsConst.OZ,EDSourceConst.Budget)?mask|2:mask;
					mask=IsRequiresFormSource(EDFormsConst.Z,EDSourceConst.Budget)?mask|4:mask;
					mask=IsRequiresFormSource(EDFormsConst.O,EDSourceConst.Paid)?mask|8:mask;
					mask=IsRequiresFormSource(EDFormsConst.OZ,EDSourceConst.Paid)?mask|16:mask;
					mask=IsRequiresFormSource(EDFormsConst.Z,EDSourceConst.Paid)?mask|32:mask;
					mask=IsRequiresFormSource(EDFormsConst.O,EDSourceConst.Target)?mask|64:mask;
					mask=IsRequiresFormSource(EDFormsConst.OZ,EDSourceConst.Target)?mask|128:mask;
					mask=IsRequiresFormSource(EDFormsConst.Z,EDSourceConst.Target)?mask|256:mask;

					return mask;

				}
			}

			public bool IncludeInRecListAvailiable { get; set; }

			[DisplayName("Рекомендован к зачислению")]
			public bool IsInRecList { get; set; }

			public string IsInRecListString {
				get { return IsInRecList?"Да":"Нет"; }
			}

			public bool IsForeignCitizen { get; set; }

			// Id документа, удостоверяющего личность
			public int EntrantDocumentIdentityId { get; set; }
		}

		[DisplayName("Дата отзыва заявления")]
		public string DenyDescrDate {
			get { return null; }
		}

		public ApplicationData AppDescr {
			get { return null; }
		}

		public ApplicationData[] Applications { get; set; }

		// IDs заявлений для передачи с клиента
		public int[] applicationIds { get; set; }

		public string[] decisionReasons { get; set; }

		public class FilterDetails {
			[DisplayName("Дата регистрации с")]
			[Date(">today-100y")]
			[Date("<=today")]
			public DateTime? DateBegin { get; set; }
			[DisplayName("по")]
			[Date(">today-100y")]
			[Date("<=today")]
			public DateTime? DateEnd { get; set; }
			[DisplayName("Льгота")]
			public int Benefit { get; set; }
			[DisplayName("Место подачи")]
			public int Place { get; set; }
			[DisplayName("Тип нарушения")]
			public int ViolationTypeID { get; set; }
			[DisplayName("Сдал документы")]
			public int DocumentsReceived { get; set; }
			[DisplayName("Конкурс")]
			public string CompetitiveGroupName { get; set; }
			[DisplayName("Форма обучения")]
			public int EducationForm { get; set; }
            [DisplayName("Источник финансирования")]
            public int EducationSource { get; set; }
			[DisplayName("Приемная кампания")]
			public string CampaignName { get; set; }
			[DisplayName("Номер заявления")]
			public string ApplicationNumber { get; set; }
			[DisplayName("Фамилия")]
			public string EntrantLastName { get; set; }
			[DisplayName("Имя")]
			public string EntrantFirstName { get; set; }
			[DisplayName("Отчество")]
			public string EntrantMiddleName { get; set; }
			[DisplayName("Серия паспорта")]
			public string EntrantDocSeries { get; set; }
			[DisplayName("№ паспорта")]
			public string EntrantDocNumber { get; set; }

			[DisplayName("Рекомендован к зачислению")]
			public short IsInRecList { get; set; }
		}

		public string[] CompetitiveGroups { get; set; }
		public string[] Campaigns { get; set; }
		public IEnumerable EducationForms { get; set; }
        public IEnumerable EducationSource { get; set; }

		public const string EmptyText="[Не важно]";

		public FilterDetails Filter { get; set; }

		public IEnumerable ViolationTypes { get; set; }
		public IEnumerable Benefits { get; set; }

		public int? SortID { get; set; }
		public int? TabID { get; set; }
		public int? PageNumber { get; set; }
		public int TotalPageCount { get; set; }
		public int TotalItemCount { get; set; }
		public int TotalItemFilteredCount { get; set; }

		[DisplayName("Причина решения")]
		public string StatusDecision { get; set; }

		public int ApplicationID { get; set; }

		public bool CanCreateNewApplication { get; set; }

		public string XmlListPath { get; set; }

		public bool Checked { get; set; }

	}
}