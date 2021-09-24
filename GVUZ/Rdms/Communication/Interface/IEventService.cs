using System;
using System.Collections.Generic;
using System.ServiceModel;
using Rdms.Communication.Entities;

namespace Rdms.Communication.Interface
{
	[ServiceContract]
	public interface IEventService
	{
		[OperationContract]
		List<EventDescription> GetEvents(DateTime? start);

		[OperationContract]
		void Clear();
	}
}