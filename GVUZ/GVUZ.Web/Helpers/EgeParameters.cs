using System.Xml.Serialization;
using AutoMapper;
using FogSoft.Helpers;
using GVUZ.Helper.ExternalValidation;

namespace GVUZ.Web.Helpers
{
	[AutoMapping(Source = typeof(EgeParameters), Destination = typeof(EgeQuery))]
	public class EgeParameters : IAutoMapping
	{
		[XmlElement("FirstName", IsNullable = false)]
		public string FirstName { get; set; }

		[XmlElement("LastName", IsNullable = false)]
		public string LastName { get; set; }

		[XmlElement("PatronymicName", IsNullable = false)]
		public string PatronymicName { get; set; }

		[XmlElement("PassportSeria", IsNullable = false)]
		public string PassportSeries { get; set; }

		[XmlElement("PassportNumber", IsNullable = false)]
		public string PassportNumber { get; set; }

		[XmlElement("CertificateNumber", IsNullable = false)]
		public string CertificateNumber { get; set; }

		[XmlElement("CheckCode", IsNullable = false)]
		public string CheckCode { get; set; }

		public void CreateMap(IConfiguration config)
		{
			config.CreateMap<EgeParameters, EgeQuery>()
				.ForMember(x => x.TypographicNumber, o => o.Ignore())
				.ForMember(x => x.PatronymicName, o => o.NullSubstitute(""));
		}
	}
}