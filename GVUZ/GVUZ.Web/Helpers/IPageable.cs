namespace GVUZ.Web.Helpers
{
	/// <summary>
	/// Объект может показываться постранично
	/// </summary>
	public interface IPageable
	{
		int? PageNumber { get; set; }
		int TotalPageCount { get; set; }
	}
}