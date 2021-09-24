using System;
using System.Runtime.Serialization;

namespace Rdms.Communication.Exceptions
{
	[Serializable]
	public class LoginAlreadyInUseException : Exception
	{
		public LoginAlreadyInUseException()
		{
		}

		public LoginAlreadyInUseException(string message) : base(message)
		{
		}

		public LoginAlreadyInUseException(string message, Exception inner) : base(message, inner)
		{
		}

		protected LoginAlreadyInUseException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}