namespace Rdms.Communication.Entities
{
	/// <summary>
	/// 	Перечень возможных состояний версии справочника.
	/// </summary>
	public enum VersionStateEnum : byte
	{
		Development = 0,
		Active = 1,
		Deprecated = 2,
		Refused = 3
	}
}