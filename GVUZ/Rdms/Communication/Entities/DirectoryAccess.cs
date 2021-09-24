namespace Rdms.Communication.Entities
{
	/// <summary>
	/// 	Право пользователя на работу со справочником.
	/// </summary>
	public class DirectoryAccess
	{
		/// <summary>
		/// 	Справочник
		/// </summary>
		public int DirectoryId { get; set; }

		/// <summary>
		/// 	Код разрешения
		/// </summary>
		public byte Permission { get; set; }
	}
}