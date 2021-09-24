using System.Collections.Generic;

namespace Rdms.Communication.Entities
{
	/// <summary>
	/// 	Запись справочника
	/// </summary>
	public class RecordContent
	{
		/// <summary>
		/// 	Идентификатор записи справочника
		/// </summary>
		public int? Id { get; set; }

		/// <summary>
		/// 	Идентификатор родительской записи справочника
		/// </summary>
		public int? ParentId { get; set; }

		/// <summary>
		/// 	Упорядоченный список значений полей справочника
		/// </summary>
		public IList<object> Values { get; set; }
	}
}