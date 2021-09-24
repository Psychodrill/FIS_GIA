using System;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using GVUZ.TestServices.InformationService;
using GVUZ.TestServices.Properties;

namespace GVUZ.TestServices
{
	public partial class CheckForm : Form
	{
		private static readonly XmlSerializer EgeSerializer;
		public readonly static XmlSerializerNamespaces Namespaces;
		public readonly static XmlWriterSettings WriterSettings;
		
		static CheckForm()
		{
			EgeSerializer = new XmlSerializer(typeof(EgeResultAndStatus));

			Namespaces = new XmlSerializerNamespaces();
			Namespaces.Add("", "");
			WriterSettings = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = true };
		}

		public CheckForm()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			egeAddress.Text = Settings.Default.GVUZ_TestServices_InformationService_InformationService;
		}

		private void btnTestEge_Click(object sender, EventArgs e)
		{
			EgeResultAndStatus status;
			using (var service = new InformationService.InformationService())
			{
				service.Url = egeAddress.Text;
				EgeParameters parameters;
				if (useDevTests.Checked)
				{
					parameters = 
						new EgeParameters
							{
								FirstName = "Люсия",
								LastName = "Евстафьева",
								PatronymicName = "Николаевна",
								CertificateNumber = "21-234567008-10",
								PassportSeria = "4619",
								PassportNumber = "513316",
								CheckCode = egePinCode.Text
							};
				}
				else
				{
					parameters =
						new EgeParameters
							{
								FirstName = "Иван",
								LastName = "Иванов",
								PatronymicName = "Иванович",
								CertificateNumber = "01-000043475-10",
								PassportSeria = "1111",
								PassportNumber = "123123",
								CheckCode = egePinCode.Text
							};
				}
				status = service.GetEgeInformation("", parameters);
			}
			egeResult.Text = SerializeToString(status, EgeSerializer);
		}

		public static string SerializeToString(object instance, XmlSerializer serializer)
		{
			if (instance == null) throw new ArgumentNullException("instance");
			if (serializer == null) throw new ArgumentNullException("serializer");
			StringBuilder builder = new StringBuilder();
			using (XmlWriter writer = XmlWriter.Create(builder, WriterSettings))
			{
				serializer.Serialize(writer, instance, Namespaces);
			}
			return builder.ToString();
		}

		private void btnTestEgeRaw_Click(object sender, EventArgs e)
		{
			using (WebClient client = new WebClient())
			{
				client.Headers.Add(HttpRequestHeader.ContentType, "text/xml");
				client.Headers.Add("SOAPAction", "urn:fbd:v1/GetEgeInformation");
				client.Headers["Content-Type"] = "text/xml; charset=utf-8";
           
				client.Encoding = Encoding.UTF8;
				string request;
				if (useDevTests.Checked)
				{
					request = string.Format(@"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""
xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
<soap:Body><GetEgeInformation xmlns=""urn:fbd:v1"">
      <AppId></AppId>
      <AppXml>
        <FirstName>{0}</FirstName>
        <LastName>{1}</LastName>
		<PatronymicName>{2}</PatronymicName>
        <PassportSeria>{3}</PassportSeria>
        <PassportNumber>{4}</PassportNumber>
        <CertificateNumber>{5}</CertificateNumber>
        <CheckCode>{6}</CheckCode>
      </AppXml>
    </GetEgeInformation>
</soap:Body></soap:Envelope>", "Люсия", "Евстафьева", "", "4619", "513316", "21-234567008-10", egePinCode.Text);
				}
				else
				{
					request = string.Format(@"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""
xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
<soap:Body><GetEgeInformation xmlns=""urn:fbd:v1"">
      <AppId> </AppId>
      <AppXml>
        <FirstName>Иван</FirstName>
		<LastName>Иванов</LastName>
        <PatronymicName>Иванович</PatronymicName>
        <PassportSeria>1111</PassportSeria>
        <PassportNumber>123123</PassportNumber>  
        <CertificateNumber>01-000043475-10</CertificateNumber>  
        <CheckCode>{0}</CheckCode>
      </AppXml>
    </GetEgeInformation>
</soap:Body></soap:Envelope>", string.IsNullOrEmpty(egePinCode.Text) ? "962316" : egePinCode.Text);
				}

				egeResult.Text = client.UploadString(new Uri(egeAddress.Text, UriKind.Absolute), "POST", request);
			}
		}
	}
}