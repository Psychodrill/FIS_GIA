using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FogSoft.Web.Mvc;

namespace GVUZ.Web.Models.Account
{
	public class LogOnModel
	{
		[LocalRequired]
		[DisplayName("Имя пользователя")]
		public string UserName { get; set; }

		[LocalRequired]
		[DataType(DataType.Password)]
		[DisplayName("Пароль")]
		public string Password { get; set; }

		[DisplayName("Запомнить?")]
		public bool RememberMe { get; set; }
	}
}