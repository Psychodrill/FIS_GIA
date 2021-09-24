using System.Collections.Generic;

namespace Rdms.Communication.Entities
{
	/// <summary>
	/// 	Документы справочника
	/// </summary>
	public class DirectoryDocuments
	{
		/// <summary>
		/// 	Идентификатор справочника.
		/// </summary>
		public int? Id { get; set; }

		/// <summary>
		/// 	Список документов, прикрепленных к справочнику.
		/// </summary>
		public IList<Document> Documents { get; set; }
	}
}