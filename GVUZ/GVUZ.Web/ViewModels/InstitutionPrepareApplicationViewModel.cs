using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FogSoft.Web.Mvc;
using GVUZ.Web.Portlets.Applications;

namespace GVUZ.Web.ViewModels
{
	public class InstitutionPrepareApplicationViewModel
	{
		public int InstitutionID { get; set; }
		public string INN { get; set; }

		[DisplayName("Конкурс")]
		[LocalRequired]
		public int DisplayCompetitiveGroupID { get; set; }

		public int[] SelectedCompetitiveGroupIDs { get; set; }

		[DisplayName("Направления подготовки")]
		[LocalRequired]
		public int DisplayDirectionID { get; set; }

		public string[] SelectedDirectionIDs { get; set; }

		public Dictionary<int, IEnumerable> CompetitiveGroupNamesByCampaign { get; set; }

		public bool ExistsGroupsWithoutEntranceTests { get; set; }

		public Dictionary<string, int> CompetitiveGroupEducationForms { get; set; }

		[DisplayName("Номер заявления ОО")]
		[LocalRequired]
		public string ApplicationNumber { get; set; }

		[DisplayName("Дата регистрации")]
		[LocalRequired]
		[Date(">today-100y")]
		[Date("<=today")]
		public DateTime RegistrationDate { get; set; }

		//[LocalRequired]
		[DisplayName("Серия документа")]
		[StringLength(20)]
		public string DocumentSeries { get; set; }

		[DisplayName("Серия / № документа, удостоверяющего личность")]
		[LocalRequired]
		[StringLength(50)]
		public string DocumentNumber { get; set; }

		[DisplayName("Вид документа удостов. личность")]
		[LocalRequired]
		public int IdentityDocumentTypeID { get; set; }

		public IEnumerable IdentityDocumentList { get; set; }

		public bool CheckForExistingBeforeCreate { get; set; }

		[DisplayName("Приемная кампания")]
		[LocalRequired]
		public int CampaignID { get; set; }

		public IEnumerable Campaigns { get; set; }

		[DisplayName("Формы обучения")]
		public string EducationFormsDisp { get; set; }

        /// <summary>
        /// Устаревший вариант выбора источников финансирования и форм обучения в заявлениию
        /// Новый вариант - работа с приоритетами, свойство Priorities
        /// </summary>
        [Obsolete("Старый вариант выбора источников винансирования и форм обучения. Использовать Priorities - новый вариант с периоритетами")]
		public ApplicationSendingViewModel.EducationForms EducationForms { get; set; }

        /// <summary>
        /// Новый вариант выбора источников финансирования и форм обучения в заявлении.
        /// Для новых заявлений заполняется только он.
        /// Старый вариант будет переведён в новый.
        /// </summary>
        public ApplicationPrioritiesViewModel Priorities { get; set; }

		[DisplayName("Организация целевого приема")]
		public int SelectedTargetOrganizationIDO { get; set; }
		[DisplayName("Организация целевого приема")]
		public int SelectedTargetOrganizationIDOZ { get; set; }
		[DisplayName("Организация целевого приема")]
		public int SelectedTargetOrganizationIDZ { get; set; }

        public bool CheckZerozBeforeCreate { get; set; }
        public bool CheckUniqueBeforeCreate { get; set; }

        [DisplayName("Приоритет заявления (только в случае нескольких заявлений от одного абитуриента)")]
        public int? Priority { get; set; }

        public int? EntrantId { get; set; }
	}
}