using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FogSoft.Web.Mvc;
using GVUZ.Model.Institutions;

namespace GVUZ.Web.Models.Account
{
	[PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessageResourceType = typeof(Messages),
		ErrorMessageResourceName = "PasswordAndConfirmDoesNotMatch")]
	public class RegisterModel
	{
		[LocalRequired]
		[DisplayName("Тип образовательного учреждения")]
		public InstitutionType InstitutionType { get; set; }

		[LocalRequired]
		[DisplayName("Полное наименование образовательного учреждения")]
		public string InstitutionName { get; set; }

		[DisplayName("ИНН"), LocalRequired,
			Inn(true)]
		public string Inn { get; set; }

		[DisplayName("ОГРН"), LocalRequired,
			Ogrn(true)]
		public string Ogrn { get; set; }

		[LocalRequired]
		[DisplayName("ФИО уполномоченного сотрудника ОО")]
		public string AdministratorName { get; set; }

		[DisplayName("E-mail уполномоченного сотрудника ОО"), LocalRequired, Email]
		public string Email { get; set; }

		[LocalRequired]
		[ValidatePasswordLength]
		[DataType(DataType.Password)]
		[DisplayName("Пароль уполномоченного сотрудника ОО")]
		public string Password { get; set; }

		[LocalRequired]
		[DataType(DataType.Password)]
		[DisplayName("Подтверждение пароля")]
		public string ConfirmPassword { get; set; }
	}
}