using System.Collections.Generic;

namespace Rdms.Communication.Entities
{
	/// <summary>
	/// 	Структура справочника
	/// </summary>
	public class DirectoryStructure
	{
		/// <summary>
		/// 	Идентификатор справочника.
		/// </summary>
		public int? Id { get; set; }

		/// <summary>
		/// 	Номер действующей версии справочника.
		/// </summary>
		public int ActualNumber { get; set; }

		/// <summary>
		/// 	Список версий справочника.
		/// </summary>
		public IList<VersionDescription> Versions { get; set; }
	}
}