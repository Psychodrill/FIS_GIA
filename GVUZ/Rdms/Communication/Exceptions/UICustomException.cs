using System;
using System.Runtime.Serialization;

namespace Rdms.Communication.Exceptions
{
	[Serializable]
	public class UICustomException : Exception
	{
		public UICustomException()
		{
		}

		public UICustomException(string message) : base(message)
		{
		}

		public UICustomException(string message, Exception inner) : base(message, inner)
		{
		}

        protected UICustomException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}