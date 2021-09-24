using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FogSoft.Web.Mvc;
using GVUZ.Web.Models;

namespace GVUZ.Web.Portlets.Entrants
{
	public class PersonalRecordsDataViewModel
	{
		public class Person
		{
			[DisplayName("Фамилия")]
			[LocalRequired]
			[StringLength(255)]
			public string LastName { get; set; }
			[DisplayName("Имя")]
			[LocalRequired]
			[StringLength(255)]
			public string FirstName { get; set; }
			[DisplayName("Отчество")]			
			[StringLength(255)]
			public string MiddleName { get; set; }

			[DisplayName("Пол")]
			[LocalRequired]
			public int GenderID { get; set; }
		}

		public class Parent
		{
			private readonly Person _person = new Person();
			public Person PersonData
			{
				get { return _person; }
			}

			[DisplayName("Место работы")]
			[StringLength(255)]
			public string WorkPlace { get; set; }
			[DisplayName("Должность")]
			[StringLength(150)]
			public string Position { get; set; }
			[DisplayName("Служ. телефон")]
			[StringLength(20)]
			[PhoneNumber]
			public string WorkPhone { get; set; }

			public bool IsAllFieldsEmpty()
			{
				return String.IsNullOrWhiteSpace(WorkPhone)
				       && String.IsNullOrWhiteSpace(WorkPlace)
				       && String.IsNullOrWhiteSpace(Position)
				       && String.IsNullOrWhiteSpace(PersonData.LastName)
				       && String.IsNullOrWhiteSpace(PersonData.FirstName)
				       && String.IsNullOrWhiteSpace(PersonData.MiddleName);
			}
		}

		private Person _entrant = new Person();
		private Parent _father = new Parent();
		private Parent _mother = new Parent();

		public Person Entrant
		{
			get { return _entrant; }
			set { _entrant = value; }
		}

		public Parent Father
		{
			get { return _father; }
			set { _father = value; }
		}

		public Parent Mother
		{
			get { return _mother; }
			set { _mother = value; }
		}

		[DisplayName("Дата рождения")]
		[LocalRequired]
		[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
		[Date("<today-7y")]
		[Date(">today-100y")]
		public DateTime BirthDate { get; set; }

		public int IdentityDocumentID { get; set; }

		[DisplayName("Вид документа удостов. личность")]
		[LocalRequired]
		public int DocumentTypeID { get; set; }

		//[LocalRequired]
		[StringLength(20)]
		public string DocumentSeries { get; set; }

		[DisplayName("Серия / № документа")]
		[LocalRequired]
		[StringLength(50)]
		public string DocumentNumber { get; set; }

		[DisplayName("Кем выдан")]
		//[LocalRequired]
		[StringLength(200)]
		public string DocumentOrganization { get; set; }

		[DisplayName("Дата выдачи")]
		[LocalRequired]
		[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
		[Date(">today-100y")]
		[Date("<=today")]
		public DateTime? DocumentDate { get; set; }

		//[LocalRequired]
		[StringLength(7)]
		[DisplayName("Код подразделения")]
		public string SubdivisionCode { get; set; }
		
		[DisplayName("Ссылка на документ удостов. личность")]
		//[LocalRequired]
		public Guid DocumentAttachmentID { get; set; }
		public string DocumentAttachmentName { get; set; }

		[DisplayName("Гражданство")]
		[LocalRequired]
		public int NationalityID { get; set; }

		[DisplayName("Место рождения")]
		//[LocalRequired]
		[StringLength(200)]
		public string BirthPlace { get; set; }

		[DisplayName("О себе могу сообщить следующее")]
		public string CustomInformation { get; set; }

		public IEnumerable GenderList { get; set; }
		public IEnumerable IdentityDocumentList { get; set; }
		public IEnumerable NationalityList { get; set; }
        public IEnumerable Citizenships { get; set; }
        public int[] RussianDocs { get; set; }

		public string GenderName { get; set; }
		public string IdentityDocumentName { get; set; }
		public string NationalityName { get; set; }


        public int EntrantID { get; set; }

		public int MaxFileSize { get; set; }

		public bool ForceAddData { get; set; }
		public bool DisableDocumentDataEditing { get; set; }
		public bool IsEdit
		{
			get { return EntrantID > 0 && !ForceAddData; }
		}

		public ApplicationStepType ApplicationStep { get; set; }
		public int ApplicationStepInt
		{
			get { return (int) ApplicationStep; }
			set { ApplicationStep = (ApplicationStepType) value; }
		}

		[DisplayName("Требуется общежитие")]
		public bool NeedHostel { get; set; }

		public bool ShowDenyMessage { get; set; }

/*		переехали на первый шаг, тут не нужны
		[DisplayName("Номер заявления ОО")]
		[LocalRequired]
		public string ApplicationNumber { get; set; }

		[DisplayName("Дата регистрации")]
		[LocalRequired]
		[Date(">today-100y")]
		[Date("<=today")]
		public DateTime? RegistrationDate { get; set; }
*/

		public int ApplicationID { get; set; }

		public string ActionCommand { get; set; }
	}
}