using System;
using System.Runtime.Serialization;

namespace Rdms.Communication.Exceptions
{
	[Serializable]
	public class IllegalDeletingException : Exception
	{
		private readonly bool _messageComplete;

		public bool MessageComplete
		{
			get { return _messageComplete; }
		}

		public IllegalDeletingException()
		{
		}

		public IllegalDeletingException(string message) : base(message)
		{
		}

		public IllegalDeletingException(string message, bool messageComplete)
			: base(message)
		{
			_messageComplete = messageComplete;
		}

		public IllegalDeletingException(string message, Exception inner) : base(message, inner)
		{
		}

		protected IllegalDeletingException(
			SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			if (info != null)
				_messageComplete = info.GetBoolean("_messageComplete");
		}

		public override void GetObjectData(
			SerializationInfo info,
			StreamingContext context)
		{
			base.GetObjectData(info, context);
			if (info != null)
				info.AddValue("_messageComplete", _messageComplete);
		}
	}
}