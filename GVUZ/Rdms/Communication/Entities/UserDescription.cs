namespace Rdms.Communication.Entities
{
	/// <summary>
	/// 	Пользователь системы.
	/// </summary>
	public class UserDescription
	{
		/// <summary>
		/// 	Системное имя пользователя.
		/// 	Короткое имя, используемое для аутентификации.
		/// </summary>
		public string Login { get; set; }

		/// <summary>
		/// 	Пароль пользователя.
		/// 	Сервис не возвращает пароль, в результатах это поле всегда null.
		/// 	При обновлении следует указать null, чтобы пароль не был изменен.
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// 	ФИО сотрудника.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 	Адрес электронной почты.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// 	Код роли пользователя.
		/// 	1 - Пользователь,
		/// 	2 - Администратор.
		/// </summary>
		public int Role { get; set; }
	}
}