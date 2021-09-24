using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FogSoft.Web.Mvc;
using GVUZ.Web.Models;

namespace GVUZ.Web.ViewModels.Administration
{
	public class ChangeUserPasswordViewModel
	{
		[LocalRequired]
		[ValidatePasswordLength]
		[DataType(DataType.Password)]
		[DisplayName("Новый пароль")]
		public string NewPassword { get; set; }

		[LocalRequired]
		[DataType(DataType.Password)]
		[DisplayName("Подтверждение пароля")]
		public string ConfirmPassword { get; set; }

		[DisplayName("Имя пользователя")]
		public string UserName { get; set; }

		public Guid UserID { get; set; }
	}
}