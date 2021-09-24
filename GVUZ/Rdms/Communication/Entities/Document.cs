using System;

namespace Rdms.Communication.Entities
{
	/// <summary>
	/// 	Документ.
	/// </summary>
	public class Document
	{
		/// <summary>
		/// 	Идентификатор документа
		/// </summary>
		public int? Id { get; set; }

		/// <summary>
		/// 	Название документа
		/// </summary>
		public String Name { get; set; }

		/// <summary>
		/// 	Название файла документа
		/// </summary>
		public String FileName { get; set; }

		/// <summary>
		/// 	Автор документа
		/// </summary>
		public String Author { get; set; }

		/// <summary>
		/// 	Дополнительное описание документа
		/// </summary>
		public String Description { get; set; }

		/// <summary>
		/// 	Дата и время загрузки документа в систему
		/// </summary>
		public DateTime UploadDate { get; set; }
	}
}