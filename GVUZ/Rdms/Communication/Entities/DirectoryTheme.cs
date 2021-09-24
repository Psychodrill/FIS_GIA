namespace Rdms.Communication.Entities
{
	/// <summary>
	/// 	Тема справочника.
	/// </summary>
	public class DirectoryTheme
	{
		/// <summary>
		/// 	Идентификатор темы справочника.
		/// </summary>
		public virtual int? Id { get; set; }

		/// <summary>
		/// 	Название темы справочника.
		/// </summary>
		public virtual string Name { get; set; }
	}
}