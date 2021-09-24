namespace Rdms.Communication.Entities
{
	/// <summary>
	/// 	Перечень возможных состояний справочника.
	/// </summary>
	public enum DirectoryStateEnum : byte
	{
		Development = 0,
		Active = 1,
		Deprecated = 2
	}
}