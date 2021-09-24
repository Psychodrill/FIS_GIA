namespace GVUZ.Web.Portlets.Applications
{
	public class ApplicationViewModel
	{
		public int EntrantID { get; set; }
		public string Content { get; set; }

		public bool CanView { get; set; }
		public bool CanEdit { get; set; }
		public int ApplicationID { get; set; }
		public string DenyMessage { get; set; }
	}
}