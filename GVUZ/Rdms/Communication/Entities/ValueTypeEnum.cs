namespace Rdms.Communication.Entities
{
	/// <summary>
	/// 	Перечень возможных типов поля.
	/// </summary>
	public enum ValueTypeEnum : byte
	{
		Integer = 1,
		Decimal = 2,
		Text = 3,
		Datetime = 4,
		Boolean = 5,
		File = 6,
		Reference = 7
	}
}