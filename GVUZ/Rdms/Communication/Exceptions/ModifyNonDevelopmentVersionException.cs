using System;
using System.Runtime.Serialization;

namespace Rdms.Communication.Exceptions
{
	[Serializable]
	public class ModifyNonDevelopmentVersionException : Exception
	{
		public ModifyNonDevelopmentVersionException()
		{
		}

		public ModifyNonDevelopmentVersionException(string message) : base(message)
		{
		}

		public ModifyNonDevelopmentVersionException(string message, Exception inner) : base(message, inner)
		{
		}

		protected ModifyNonDevelopmentVersionException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}