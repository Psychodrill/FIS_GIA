using System;
using System.Collections.Generic;
using System.Linq;

namespace FogSoft.WSRP.Portlets
{
	/// <summary>
	/// Base implementation for the <see cref="IPortlet"/> that checks parameters and throws <see cref="NotSupportedException"/>.</summary>
	/// <remarks>Inheritors should implement supported virtual members (like <see cref="GetViewMarkup"/> and so on).</remarks>
	public abstract class Portlet : IPortlet
	{
		protected PortletDescriptor Descriptor { get; private set; }

		protected Portlet(PortletDescriptor descriptor)
		{
			Descriptor = descriptor;
		}

		public MarkupResponse GetMarkup(MimeType[] mimeTypes, PortletMode mode = PortletMode.View, PortletWindowState windowState = PortletWindowState.Normal, string navigationalState = "", string userContextKey = "")
		{
			ValidateParameters(mode, windowState, mimeTypes);

			switch (mode)
			{
				case PortletMode.View:
					return GetViewMarkup(windowState, mimeTypes, navigationalState, userContextKey);
				case PortletMode.Edit:
					return GetEditMarkup(windowState, mimeTypes);
				case PortletMode.Help:
					return GetHelpMarkup(windowState, mimeTypes);
				case PortletMode.Preview:
					return GetPreviewMarkup(windowState, mimeTypes);
				default:
					throw new ArgumentOutOfRangeException("mode");
			}
		}

		public BlockingInteractionResponse PerformBlockingInteraction
			(Dictionary<string, string> formParameters, string interactionState, MimeType[] mimeTypes, PortletMode mode = PortletMode.View, PortletWindowState windowState = PortletWindowState.Normal, UploadContext[] uploadContexts = null)
		{
			ValidateParameters(mode, windowState, mimeTypes);

			switch (mode)
			{
				case PortletMode.View:
					return PerformViewInteraction(formParameters, interactionState, windowState, mimeTypes, uploadContexts);
				case PortletMode.Edit:
					return PerformEditInteraction(formParameters, interactionState, windowState, mimeTypes);
				case PortletMode.Help:
					return PerformHelpInteraction(formParameters, interactionState, windowState, mimeTypes);
				case PortletMode.Preview:
					return PerformPreviewInteraction(formParameters, interactionState, windowState, mimeTypes);
				default:
					throw new ArgumentOutOfRangeException("mode");
			}
		}

		/// <summary>
		/// Implementation of the <see cref="PerformBlockingInteraction"/> in the <see cref="PortletMode.View"/> mode.</summary>
		protected virtual BlockingInteractionResponse PerformViewInteraction(Dictionary<string, string> formParameters, string interactionState, PortletWindowState windowState, MimeType[] mimeTypes, UploadContext[] uploadContexts)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Implementation of the <see cref="PerformBlockingInteraction"/> in the <see cref="PortletMode.Edit"/> mode.</summary>
		protected virtual BlockingInteractionResponse PerformEditInteraction(
			Dictionary<string, string> formParameters, string interactionState, PortletWindowState windowState,
			MimeType[] mimeTypes)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Implementation of the <see cref="PerformBlockingInteraction"/> in the <see cref="PortletMode.Help"/> mode.</summary>
		protected virtual BlockingInteractionResponse PerformHelpInteraction(
			Dictionary<string, string> formParameters, string interactionState, PortletWindowState windowState,
			MimeType[] mimeTypes)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Implementation of the <see cref="PerformBlockingInteraction"/> in the <see cref="PortletMode.Preview"/> mode.</summary>
		protected virtual BlockingInteractionResponse PerformPreviewInteraction(
			Dictionary<string, string> formParameters, string interactionState, PortletWindowState windowState,
			MimeType[] mimeTypes)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Implementation of the <see cref="GetMarkup"/> in the <see cref="PortletMode.View"/> mode.</summary>
		protected virtual MarkupResponse GetViewMarkup(PortletWindowState windowState, MimeType[] mimeTypes, string navigationalState, string userContextKey)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Implementation of the <see cref="GetMarkup"/> in the <see cref="PortletMode.Edit"/> mode.</summary>
		protected virtual MarkupResponse GetEditMarkup(PortletWindowState windowState, MimeType[] mimeTypes)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Implementation of the <see cref="GetMarkup"/> in the <see cref="PortletMode.Preview"/> mode.</summary>
		protected virtual MarkupResponse GetPreviewMarkup(PortletWindowState windowState, MimeType[] mimeTypes)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Implementation of the <see cref="GetMarkup"/> in the <see cref="PortletMode.Help"/> mode.</summary>
		protected virtual MarkupResponse GetHelpMarkup(PortletWindowState windowState, MimeType[] mimeTypes)
		{
			throw new NotSupportedException();
		}


		protected void ValidateParameters(PortletMode mode, PortletWindowState windowState,
		                                  MimeType[] mimeTypes)
		{
			if ((from d in Descriptor.Markups
			     where mimeTypes.Contains(d.MimeType) && d.Modes.Contains(mode)
			           && d.WindowStates.Contains(windowState)
			     select d).Count() == 0)
				throw new ArgumentException(
					string.Format(
						@"Following parameters does not supported by portlet:
mode: {0}, windowState: {1}, mimeType: {2}.",
						mode, windowState, string.Join(",", mimeTypes)));
		}
	}
}