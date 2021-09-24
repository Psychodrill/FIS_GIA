using System;

namespace Rdms.Communication.Entities
{
	/// <summary>
	/// 	Описание версии справочника
	/// </summary>
	public class VersionDescription
	{
		/// <summary>
		/// 	Идентификатор версии справочника
		/// </summary>
		public int? Id { get; set; }

		/// <summary>
		/// 	Справочник
		/// </summary>
		public int DirectoryId { get; set; }

		/// <summary>
		/// 	Номер версии справочника
		/// </summary>
		public int Number { get; set; }

		/// <summary>
		/// 	Краткое название версии
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 	Дополнительное описание версии
		/// </summary>
		public string Note { get; set; }

		/// <summary>
		/// 	Состояние версии справочника.
		/// 	0 - В разработке,
		/// 	1 - Действует,
		/// 	2 - Не действует,
		/// 	3 - Отклонена.
		/// </summary>
		public VersionStateEnum State { get; set; }

		/// <summary>
		/// 	Дата введения в действие.
		/// </summary>
		public DateTime? ActivationDate { get; set; }
	}
}