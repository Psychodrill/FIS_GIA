using System;
using System.Runtime.Serialization;

namespace Rdms.Communication.Exceptions
{
	[Serializable]
	public class NameAlreadyInUseException : Exception
	{
		public NameAlreadyInUseException()
		{
		}

		public NameAlreadyInUseException(string message) : base(message)
		{
		}

		public NameAlreadyInUseException(string message, Exception inner) : base(message, inner)
		{
		}

		protected NameAlreadyInUseException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}