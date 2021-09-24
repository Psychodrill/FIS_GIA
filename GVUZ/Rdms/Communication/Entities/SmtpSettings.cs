using System.Runtime.Serialization;

namespace Rdms.Communication.Entities
{
	[DataContract]
	public class SmtpSettings
	{
		[DataMember]
		public string Host { get; set; }

		[DataMember]
		public int Port { get; set; }

		[DataMember]
		public string FromAddress { get; set; }

		[DataMember]
		public string Login { get; set; }

		[DataMember]
		public string Password { get; set; }

		[DataMember]
		public bool UseSsl { get; set; }
	}
}