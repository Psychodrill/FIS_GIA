using System;
using System.Runtime.Serialization;

namespace Rdms.Communication.Exceptions
{
	[Serializable]
	public class UniqueConstraintViolationException : Exception
	{
		public UniqueConstraintViolationException()
		{
		}

		public UniqueConstraintViolationException(string message) : base(message)
		{
		}

		public UniqueConstraintViolationException(string message, Exception inner) : base(message, inner)
		{
		}

		protected UniqueConstraintViolationException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}