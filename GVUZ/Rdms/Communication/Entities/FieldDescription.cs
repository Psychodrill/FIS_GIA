using System.Runtime.Serialization;
using Rdms.Communication.Entities.Constraint;

namespace Rdms.Communication.Entities
{
	/// <summary>
	/// 	Поле справочника.
	/// </summary>
	[DataContract]
	public class FieldDescription
	{
		/// <summary>
		/// 	Идентификатор поля справочника.
		/// </summary>
		[DataMember]
		public int Id { get; set; }

		/// <summary>
		/// 	Название поля.
		/// </summary>
		[DataMember]
		public string Name { get; set; }

		/// <summary>
		/// 	Кодовое обозначение поля.
		/// 	Код поля используется при формировании таблицы
		/// 	с содержимым справочника в БД системы.
		/// </summary>
		[DataMember]
		public string Code { get; set; }

		/// <summary>
		/// 	Описание поля.
		/// </summary>
		[DataMember]
		public string Description { get; set; }

		/// <summary>
		/// 	Тип поля.
		/// 	1 - Целое число,
		/// 	2 - Вещественное число,
		/// 	3 - Текст,
		/// 	4 - Дата,
		/// 	5 - Булево значение (правда или ложь),
		/// 	6 - Файл,
		/// 	7 - Ссылка на другой справочник.
		/// </summary>
		[DataMember]
		public ValueTypeEnum Type { get; set; }

		/// <summary>
		/// 	Размер поля.
		/// 	Указывается только для полей типа Текст.
		/// </summary>
		[DataMember]
		public int Size { get; set; }

		/// <summary>
		/// 	Обязательность заполнения.
		/// </summary>
		[DataMember]
		public bool Nullable { get; set; }

		/// <summary>
		/// 	Идентификатор версии справочника, на которую указывает ссылка.
		/// 	Заполняется только для поля типа Ссылка.
		/// </summary>
		[DataMember]
		public int? Reference { get; set; }

		/// <summary>
		/// 	Положение поля в таблице.
		/// </summary>
		[DataMember]
		public int Order { get; set; }

		/// <summary>
		/// 	Идентификатор поля для отображения в ссылке на запись
		/// </summary>
		[DataMember]
		public int? ReferencedFieldId { get; set; }


		/// <summary>
		/// 	Ограничение на значение поля.
		/// </summary>
		[DataMember]
		public IConstraint Constraint { get; set; }
	}
}