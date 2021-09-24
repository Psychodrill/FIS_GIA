namespace Rdms.Communication.Entities
{
	/// <summary>
	/// 	Описание справочника
	/// </summary>
	public class DirectoryDescription
	{
		/// <summary>
		/// 	Идентификатор справочника.
		/// </summary>
		public int? Id { get; set; }

		/// <summary>
		/// 	Название справочника.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 	Дополнительное описание справочника.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 	Состояние справочника.
		/// 	1 - В разработке,
		/// 	2 - Действует,
		/// 	3 - Не действует.
		/// </summary>
		public DirectoryStateEnum State { get; set; }

		/// <summary>
		/// 	Тема справочника
		/// </summary>
		public int? ThemeId { get; set; }

		/// <summary>
		/// 	Маска наименования версий
		/// </summary>
		public MaskTypeEnum VersionMask { get; set; }
	}
}