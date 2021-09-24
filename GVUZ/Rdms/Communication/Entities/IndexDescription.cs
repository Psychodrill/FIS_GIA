using System.Collections.Generic;

namespace Rdms.Communication.Entities
{
	/// <summary>
	/// 	Индекс.
	/// 	Группа полей, определяющая уникальность записи
	/// 	в пределах версии справочника.
	/// </summary>
	public class IndexDescription
	{
		/// <summary>
		/// 	Идентификатор.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 	Поля справочника, определяющие уникальность.
		/// </summary>
		public IList<int> Fields { get; set; }
	}
}