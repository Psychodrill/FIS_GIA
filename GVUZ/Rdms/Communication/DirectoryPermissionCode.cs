namespace Rdms.Communication
{
	public enum DirectoryPermissionCode : byte
	{
		/// <summary>
		/// 	Нет доступа
		/// </summary>
		NoAccess = 0,
		/// <summary>
		/// 	Просмотр
		/// </summary>
		View = 1,
		/// <summary>
		/// 	Редактирование
		/// </summary>
		Edit = 2,
		/// <summary>
		/// 	Управление версиями
		/// </summary>
		Control = 3
	}
}