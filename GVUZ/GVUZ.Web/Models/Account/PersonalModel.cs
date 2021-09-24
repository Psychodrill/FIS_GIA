using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FogSoft.Web.Mvc;

namespace GVUZ.Web.Models.Account
{
	[PropertiesMustMatch("NewPassword", "ConfirmPassword",
		ErrorMessage = "Новый пароль и подтверждение не совпадают.")]
	public class PersonalModel
	{
		[LocalRequired]
		[DataType(DataType.Password)]
		[DisplayName("Текущий пароль")]
		public string OldPassword { get; set; }

		[LocalRequired]
		[ValidatePasswordLength]
		[DataType(DataType.Password)]
		[DisplayName("Новый пароль")]
		public string NewPassword { get; set; }

		[LocalRequired]
		[DataType(DataType.Password)]
		[DisplayName("Подтверждение пароля")]
		public string ConfirmPassword { get; set; }

		[DataType(DataType.Text)]
		[DisplayName("Проверочный код")]
		public string PinCode { get; set; }

		[DataType(DataType.Text)]
		[DisplayName("Количество оставшихся просмотров")]
		public int? AvailableEgeCheckCount { get; set; }
	}
}