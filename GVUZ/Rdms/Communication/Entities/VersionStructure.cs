using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rdms.Communication.Entities
{
	/// <summary>
	/// 	Структура версии справочника
	/// </summary>
	[DataContract]
	public class VersionStructure
	{
		/// <summary>
		/// 	Идентификатор версии справочника
		/// </summary>
		[DataMember]
		public int? Id { get; set; }

		/// <summary>
		/// 	Поля справочника
		/// </summary>
		[DataMember]
		public List<FieldDescription> Fields { get; set; }

		/// <summary>
		/// 	Индексы, определяющие уникальность записей
		/// </summary>
		[DataMember]
		public List<IndexDescription> Indices { get; set; }
	}
}