namespace Rdms.Communication.Entities
{
	/// <summary>
	/// 	Перечень возможных состояний учетной записи
	/// </summary>
	public enum UserStateEnum : byte
	{
		Inactive = 0,
		Active = 1,
		Deleted = 2
	}
}