using System;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FogSoft.Helpers
{
	[DebuggerStepThrough]
	public static class XmlSerializerHelper
	{
		public readonly static XmlSerializerNamespaces Namespaces;
		public readonly static XmlWriterSettings WriterSettings;
		public readonly static XmlWriterSettings IndentWriterSettings;

		static XmlSerializerHelper()
		{
			Namespaces = new XmlSerializerNamespaces();
			Namespaces.Add("", "");
			WriterSettings = new XmlWriterSettings {OmitXmlDeclaration = true, Indent = false};
			IndentWriterSettings = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = true };
		}

		public static string SerializeToString(object instance, XmlSerializer serializer, bool indent = false)
		{
			StringBuilder builder = new StringBuilder();
			AppendAsXml(builder, instance, serializer, indent);
			return builder.ToString();
		}

		public static StringBuilder AppendAsXml(this StringBuilder builder, object instance, XmlSerializer serializer, bool indent = false)
		{
			if (builder == null) throw new ArgumentNullException("builder");
			if (instance == null) throw new ArgumentNullException("instance");
			if (serializer == null) throw new ArgumentNullException("serializer");
			
			using (XmlWriter writer = XmlWriter.Create(builder, indent ? IndentWriterSettings : WriterSettings))
			{
				serializer.Serialize(writer, instance, Namespaces);
			}
			return builder;
		}
	}
}
