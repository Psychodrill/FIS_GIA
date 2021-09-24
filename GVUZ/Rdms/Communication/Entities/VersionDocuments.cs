using System.Collections.Generic;

namespace Rdms.Communication.Entities
{
	/// <summary>
	/// 	Документы версии справочника
	/// </summary>
	public class VersionDocuments
	{
		/// <summary>
		/// 	Идентификатор версии справочника
		/// </summary>
		public int? Id { get; set; }

		/// <summary>
		/// 	Документы, соответствующие версии справочника
		/// </summary>
		public virtual IList<Document> Documents { get; set; }
	}
}