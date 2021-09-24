using GVUZ.Helper;

namespace GVUZ.Web.Portlets
{
	public class PortletAjaxLinkAttribute : GeneratorPortletLinkAttribute
	{
		public string AjaxActionLinkName { get; set; }

		public PortletAjaxLinkAttribute(string ajaxActionLinkName)
		{
			AjaxActionLinkName = ajaxActionLinkName;
		}

		public override string ExecuteMethod(object[] args)
		{
			return PortletLinkHelper.AjaxActionLink(AjaxActionLinkName);
		}
	}
}