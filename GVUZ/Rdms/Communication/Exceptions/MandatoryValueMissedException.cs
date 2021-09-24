using System;
using System.Runtime.Serialization;

namespace Rdms.Communication.Exceptions
{
	[Serializable]
	public class MandatoryValueMissedException : Exception
	{
		public MandatoryValueMissedException()
		{
		}

		public MandatoryValueMissedException(string message) : base(message)
		{
		}

		public MandatoryValueMissedException(string message, Exception inner) : base(message, inner)
		{
		}

		protected MandatoryValueMissedException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}