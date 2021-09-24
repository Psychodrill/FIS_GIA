using System;
using System.Runtime.Serialization;

namespace FogSoft.WSRP
{
	public class PortletException : Exception
	{
		public PortletException(string message) : base(message)
		{
		}

		public PortletException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected PortletException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}