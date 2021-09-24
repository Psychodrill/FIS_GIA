using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using FogSoft.Helpers;

namespace FogSoft.Patcher
{
    public static class XmlPatcher
    {
        public static string GetAttribute(this XElement element, string name, string defaultValue = null)
        {
            if (element == null) throw new ArgumentNullException("element");
            if (name == null) throw new ArgumentNullException("name");
            XAttribute attribute = element.Attribute(name);
            if (attribute == null)
            {
                if (defaultValue == null)
                    throw new ArgumentException("{0} not found for {1}.".FormatWith(name, element.Name));
                return defaultValue;
            }
            return attribute.Value;
        }

        public static void Patch(string configurationPath, params string[] targets)
        {
            if (string.IsNullOrEmpty(configurationPath)) throw new ArgumentNullException("configurationPath");
            if (targets == null || targets.Length == 0) throw new ArgumentNullException("targets");

            XDocument config = XDocument.Load(configurationPath);
            if (config == null || config.Root == null)
                throw new ArgumentException("Cannot load cofiguration file '{0}'.".FormatWith(configurationPath));

            List<Node> nodes = (from e in config.Root.Elements() select CreateNode(e)).ToList();
            List<ReplaceTextNode> replaces = nodes.OfType<ReplaceTextNode>().Select(x => x).ToList();
            foreach (string target in targets)
            {
                XDocument document;
                using (XmlReader reader = XmlReader.Create(target))
                {
                    document = XDocument.Load(reader, LoadOptions.PreserveWhitespace);
                    XmlNamespaceManager namespaceManager =
                        reader.NameTable == null ? null : new XmlNamespaceManager(reader.NameTable);

                    foreach (Node node in nodes)
                    {
                        if (node is ReplaceTextNode) continue;

                        node.Apply(document, namespaceManager);
                    }
                }

                if (replaces.Count == 0)
                    document.Save(target);
                else
                {
                    Encoding encoding = Encoding.GetEncoding(document.Declaration.Encoding);
                    var builder = new StringBuilder(1024*1024*64);
                    using (XmlWriter writer = XmlWriter.Create(new PatchWriter(builder, encoding)))
                        document.Save(writer);

                    foreach (ReplaceTextNode replace in replaces)
                        builder.Replace(replace.OldValue, replace.NewValue);

                    File.WriteAllText(target, builder.ToString(), encoding);
                }
            }
        }

        private static Node CreateNode(XElement element)
        {
            switch (element.Name.ToString().ToLower())
            {
                case "namespace":
                    return new NamespaceNode(element.GetAttribute("prefix"), element.GetAttribute("uri"));
                case "replace":
                    return new ReplaceTextNode(element.GetAttribute("oldValue"), element.GetAttribute("newValue"));
                case "append":
                    return new AppendNode(element.GetAttribute("xpath"), element.GetAttribute("name"),
                                          element.GetAttribute("value"));
                case "delete":
                    return new DeleteElementNode(element.GetAttribute("xpath"));
                case "remove":
                    return new RemoveAttributeNode(element.GetAttribute("xpath"), element.GetAttribute("name"));
                default:
                    throw new ArgumentException("Element {0} does not supported.".FormatWith(element.Name));
            }
        }

        private class AppendNode : Node
        {
            public AppendNode(string xPath, string name, string value)
            {
                XPath = xPath;
                Name = name;
                Value = value;
            }

            private string XPath { get; set; }
            private string Name { get; set; }
            private string Value { get; set; }

            public override void Apply(XDocument document, XmlNamespaceManager namespaceManager)
            {
                foreach (XElement element in document.XPathSelectElements(XPath, namespaceManager))
                {
                    XName name = GetXName(Name, namespaceManager);
                    IEnumerable<XAttribute> attributes = element.Attributes(name);
                    if (attributes.Count() == 0)
                    {
                        element.Add(new XAttribute(name, Value));
                    }
                    else
                    {
                        if (attributes.First().Value != Value)
                            throw new InvalidOperationException
                                ("Element {0} already contains attribute {1} with different value {2}."
                                     .FormatWith(element.Name, Name, attributes.First().Value));
                    }
                }
            }
        }

        private class DeleteElementNode : Node
        {
            public DeleteElementNode(string xPath)
            {
                XPath = xPath;
            }

            private string XPath { get; set; }

            public override void Apply(XDocument document, XmlNamespaceManager namespaceManager)
            {
                IEnumerable<XElement> elements = document.XPathSelectElements(XPath, namespaceManager);
                elements.Remove();
            }
        }

        private class NamespaceNode : Node
        {
            public NamespaceNode(string prefix, string uri)
            {
                Prefix = prefix;
                Uri = uri;
            }

            private string Prefix { get; set; }
            private string Uri { get; set; }

            public override void Apply(XDocument document, XmlNamespaceManager namespaceManager)
            {
                if (namespaceManager == null) throw new ArgumentNullException("namespaceManager");
                namespaceManager.AddNamespace(Prefix, Uri);
            }
        }

        private abstract class Node
        {
            public abstract void Apply(XDocument document, XmlNamespaceManager namespaceManager);

            protected static XName GetXName(string name, XmlNamespaceManager namespaceManager)
            {
                int i = name.IndexOf(":");
                if (i < 0)
                    return XName.Get(name);

                string namespaceName = name.Substring(0, i);
                if (namespaceName.StartsWith("{"))
                    return XName.Get(name);

                string uri = namespaceManager.LookupNamespace(namespaceName);
                if (uri == null)
                    throw new InvalidOperationException("Namespace '{0}' does not found.".FormatWith(namespaceName));
                return XName.Get(name.Substring(i + 1), uri);
            }
        }

        private class PatchWriter : StringWriter
        {
            private readonly Encoding _encoding;

            public PatchWriter(StringBuilder sb, Encoding encoding) : base(sb)
            {
                _encoding = encoding;
            }

            public override Encoding Encoding
            {
                get { return _encoding; }
            }
        }

        private class RemoveAttributeNode : Node
        {
            public RemoveAttributeNode(string xPath, string name)
            {
                XPath = xPath;
                Name = name;
            }

            private string XPath { get; set; }
            private string Name { get; set; }

            public override void Apply(XDocument document, XmlNamespaceManager namespaceManager)
            {
                foreach (XElement element in document.XPathSelectElements(XPath, namespaceManager))
                {
                    element.Attributes(GetXName(Name, namespaceManager)).Remove();
                }
            }
        }

        private class ReplaceTextNode : Node
        {
            public ReplaceTextNode(string oldValue, string newValue)
            {
                OldValue = Decode(oldValue);
                NewValue = Decode(newValue);
            }

            public string OldValue { get; private set; }
            public string NewValue { get; private set; }

            private static string Decode(string value)
            {
                return
                    value.Replace("\\n", "\n")
                         .Replace("\\r", "\r")
                         .Replace("\\t", "\t")
                         .Replace("&amp;", "&")
                         .Replace("&lt;", "<");
            }

            public override void Apply(XDocument document, XmlNamespaceManager namespaceManager)
            {
                throw new InvalidOperationException("Replace should be applied later.");
            }
        }
    }
}