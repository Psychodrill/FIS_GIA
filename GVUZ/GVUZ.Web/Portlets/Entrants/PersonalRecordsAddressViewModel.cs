using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FogSoft.Web.Mvc;
using GVUZ.Web.Models;

namespace GVUZ.Web.Portlets.Entrants
{
	public class PersonalRecordsAddressViewModel
	{
		public class Address
		{
			[DisplayName("Индекс")]
			[StringLength(6)]
			[Symbol(SymbolType.Numeric)]
			//[LocalRequired]
			public string PostalCode { get; set; }
			
			[DisplayName("Страна")]
			//[LocalRequired]
			public int CountryID { get; set; }
			
			public string CountryName { get; set; }

			public int CountryHasRegions { get; set; }

			[DisplayName("Регион")]
			//[LocalRequired]
			public int? RegionID { get; set; }

			[DisplayName("Регион")]
			//[LocalRequired]
			public string RegionName { get; set; }
			
			[DisplayName("Населенный пункт")]
			[StringLength(255)]
			[Symbol(SymbolType.AlphaNumericDash)]
			public string CityName { get; set; }
			
			[DisplayName("Улица")]
			//[LocalRequired]
			[StringLength(255)]
			[Symbol(SymbolType.AlphaNumericDash)]
			public string Street { get; set; }
			
			[DisplayName("Дом")]
			//[LocalRequired]
			[StringLength(10)]
			[Symbol(SymbolType.NumberLetter)]
			public string Building { get; set; }

			[DisplayName("Корпус (строение)")]			
			[StringLength(10)]
			[Symbol(SymbolType.NumberLetter)]
			public string BuildingPart { get; set; }
			
			[DisplayName("Квартира")]
			//[LocalRequired]
			[StringLength(10)]
			[Symbol(SymbolType.Numeric)]
			public string Room { get; set; }
			
			[DisplayName("Телефон")]			
			[StringLength(20)]
			[PhoneNumber]
			public string Phone { get; set; }

			public int? AddressID { get; set; }
		}

		private Address _registrationAddress = new Address();
		private Address _factAddress = new Address();
		public ApplicationStepType ApplicationStep { get; set; }

		public Address RegistrationAddress
		{
			get { return _registrationAddress; }
			set { _registrationAddress = value; }
		}

		public Address FactAddress
		{
			get { return _factAddress; }
			set { _factAddress = value; }
		}

		[DisplayName("Совпадает с пропиской")]
		public bool IsOnlyRegistration { get; set; }

		[DisplayName("Требуется общежитие")]
		public bool HostelRequired { get; set; }

		public IEnumerable CountryList { get; set; }
		public IEnumerable RegistrationRegionsList { get; set; }
		public IEnumerable FactRegionsList { get; set; }

		[DisplayName("Мобильный")]		
		[StringLength(20)]
		[PhoneNumber]
		public string Mobile { get; set; }

		[DisplayName("E-Mail")]
		[StringLength(150)]
		[Email]
		public string Email { get; set; }

		public int EntrantID { get; set; }

		public int ApplicationID { get; set; }

		public bool ShowDenyMessage { get; set; }

		public string ActionCommand { get; set; }
	}
}