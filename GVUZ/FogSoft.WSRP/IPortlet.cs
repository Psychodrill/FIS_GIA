using System.Collections.Generic;
using System.Collections.Specialized;

namespace FogSoft.WSRP
{
	/// <summary>
	/// Interface that WSRP portlet should implement.</summary>
	public interface IPortlet
	{
		// TODO: add more contexts parameters support

		/// <summary>
		/// Returns markup string for the specified parameters.</summary>
		/// <param name="mimeTypes"></param>
		/// <param name="mode"><see cref="PortletMode"/>, <see cref="PortletMode.View"/> by default.</param>
		/// <param name="windowState"><see cref="PortletWindowState"/>, <see cref="PortletWindowState.Normal"/> by default.</param>
		/// <param name="navigationalState">Navigational state encoded on the URLs</param>
		/// <param name="userContextKey"></param>
		MarkupResponse GetMarkup(MimeType[] mimeTypes, PortletMode mode = PortletMode.View, PortletWindowState windowState = PortletWindowState.Normal, string navigationalState = "", string userContextKey = "");


		/// <summary>
		/// Performs blocking interaction with the current portlet.</summary>
		/// <param name="formParameters">Form parameters from the consumer.</param>
		/// <param name="interactionState">Interaction state encoded on the URLs in the markup only.</param>
		/// <param name="mimeTypes"></param>
		/// <param name="mode"><see cref="PortletMode"/>, <see cref="PortletMode.View"/> by default.</param>
		/// <param name="windowState"><see cref="PortletWindowState"/>, <see cref="PortletWindowState.Normal"/> by default.</param>
		/// <param name="uploadContexts"></param>
		// TODO: add result support when needed
		BlockingInteractionResponse PerformBlockingInteraction(Dictionary<string, string> formParameters, string interactionState, MimeType[] mimeTypes, PortletMode mode = PortletMode.View, PortletWindowState windowState = PortletWindowState.Normal, UploadContext[] uploadContexts = null);
	}
}