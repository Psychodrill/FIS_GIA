using System;
using System.Runtime.Serialization;

namespace Rdms.Communication.Entities
{
	/// <summary>
	/// 	Событие.
	/// </summary>
	[DataContract]
	public class EventDescription
	{
		/// <summary>
		/// 	Идентификатор события.
		/// </summary>
		[DataMember]
		public int Id { get; set; }

		/// <summary>
		/// 	Логин пользователя.
		/// </summary>
		[DataMember]
		public string UserLogin { get; set; }

		/// <summary>
		/// 	Момент фиксации события.
		/// </summary>
		[DataMember]
		public DateTime Moment { get; set; }

		/// <summary>
		/// 	Сообщение, описывающее событие.
		/// </summary>
		[DataMember]
		public string Message { get; set; }

		/// <summary>
		/// 	Тип события.
		/// </summary>
		[DataMember]
		public int? Type { get; set; }
	}
}