using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FogSoft.Web.Mvc;
using GVUZ.Web.Models;
using GVUZ.Web.ViewModels;

namespace GVUZ.Web.Portlets.Applications
{
	public class ApplicationSendingViewModel : ApplicationInfoViewModelBase
	{
		[DisplayName("ФИО")]
		public string FIO { get; set; }
		[DisplayName("Дата рождения")]
		public string DOB { get; set; }
		[DisplayName("Документ, удостоверяющий личность")]
		public string IdentityDocument { get; set; }
		[DisplayName("Пол")]
		public string Gender { get; set; }
		[DisplayName("Гражданство")]
		public string Citizen { get; set; }
		[DisplayName("Место рождения")]
		public string POB { get; set; }
		[DisplayName("Идентификатор в БД ОО (UID)")]
		[StringLength(200)]
		public string Uid { get; set; }
		[DisplayName("О себе дополнительно сообщаю")]
		public string CustomInformation { get; set; }

		[DisplayName("Подтверждаю подачу заявления не более чем в 5 ВУЗов")]
		public bool ApproveInstitutionCount { get; set; }
		
		[DisplayName("Нуждаюсь в общежитии")]
		public bool NeedHostel { get; set; }
		
		public bool FirstHigherEducation { get; set; }
		[DisplayName("Даю согласие на обработку моих персональных данных в порядке, установленном Федеральным законом Российской Федерации от 27.07.2006 № 152-Ф3")]
		public bool ApprovePersonalData { get; set; }
		
		public bool FamiliarWithLicenseAndRules { get; set; }

		[DisplayName("Дата регистрации")]
		[LocalRequired]
		[Date(">today-100y")]
		[Date("<=today")]
		public DateTime RegistrationDate { get; set; }

		[DisplayName("С условиями выбора специальности ознакомлен")]
		public bool FamiliarWithAdmissionType { get; set; }
		[DisplayName("Дата предоставления подлинника документа об образовании")]
		public string EducationDocumentDate { get; set; }
		[DisplayName("С датой предоставления подлинника документа об образовании ознакомлен")]
		public bool FamiliarWithOriginalDocumentDeliveryDate { get; set; }

		public ApplicationStepType ApplicationStep { get; set; }

		public bool ShowDenyMessage { get; set; }

		[DisplayName("Приемная кампания")]
		[LocalRequired]
		public int CampaignID { get; set; }

		public IEnumerable Campaigns { get; set; }


		[DisplayName("Конкурс")]
		[LocalRequired]
		public int DisplayCompetitiveGroupID { get; set; }

		public int[] SelectedCompetitiveGroupIDs { get; set; }

		[DisplayName("Направления подготовки")]
		[LocalRequired]
		public int DisplayDirectionID { get; set; }

		public string[] SelectedDirectionIDs { get; set; }


		public Dictionary<int, IEnumerable> CompetitiveGroupNamesByCampaign { get; set; }

		public int ApplicationID { get; set; }

        public class EducationForms
        {
            [DisplayName("Очная форма - Бюджетные места")]
            public bool BudgetO { get; set; }
            [DisplayName("Очно-заочная (вечерняя) - Бюджетные места")]
            public bool BudgetOZ { get; set; }
            [DisplayName("Заочная форма - Бюджетные места")]
            public bool BudgetZ { get; set; }
            [DisplayName("Очная форма - Платные места")]
            public bool PaidO { get; set; }
            [DisplayName("Очно-заочная (вечерняя) - Платные места")]
            public bool PaidOZ { get; set; }
            [DisplayName("Заочная форма - Платные места")]
            public bool PaidZ { get; set; }
            [DisplayName("Очная форма - Целевой прием")]
            public bool TargetO { get; set; }
            [DisplayName("Очно-заочная (вечерняя) - Целевой прием")]
            public bool TargetOZ { get; set; }
            [DisplayName("Заочная форма - Целевой прием")]
            public bool TargetZ { get; set; }

            public IEnumerable TargetOrganizationsO { get; set; }
            public IEnumerable TargetOrganizationsOZ { get; set; }
            public IEnumerable TargetOrganizationsZ { get; set; }

            public bool HasBudget
            {
                get { return BudgetO || BudgetOZ || BudgetZ; }
            }
            public bool HasPaid
            {
                get { return PaidO || PaidOZ || PaidZ; }
            }
            public bool HasTarget
            {
                get { return TargetO || TargetOZ || TargetZ; }
            }
            public bool HasAny
            {
                get { return HasBudget || HasPaid || HasTarget; }
            }
        }

        [DisplayName("Формы обучения и источники финансирования")]
        public EducationForms EducationFormsAvailable { get; set; }
        public EducationForms EducationFormsSelected { get; set; }

        public ApplicationPrioritiesViewModel Priorities { get; set; }

		public IEnumerable TargetOrganizations { get; set; }
		[DisplayName("Организация целевого приема")]
		public int SelectedTargetOrganizationIDO { get; set; }
		[DisplayName("Организация целевого приема")]
		public int SelectedTargetOrganizationIDOZ { get; set; }
		[DisplayName("Организация целевого приема")]
		public int SelectedTargetOrganizationIDZ { get; set; }
		 
		public bool IsDraft { get; set; }

		public string ActionCommand { get; set; }

        [DisplayName("Приоритет заявления (только в случае нескольких заявлений от одного абитуриента)")]
        public int? Priority { get; set; }
	}
}