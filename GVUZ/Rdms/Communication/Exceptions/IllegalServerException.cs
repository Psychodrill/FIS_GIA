using System;
using System.Runtime.Serialization;

namespace Rdms.Communication.Exceptions
{
	[Serializable]
	public class IllegaServerException : Exception
	{
		public IllegaServerException()
		{
		}

		public IllegaServerException(string message) : base(message)
		{
		}

		public IllegaServerException(string message, Exception inner) : base(message, inner)
		{
		}

		protected IllegaServerException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}