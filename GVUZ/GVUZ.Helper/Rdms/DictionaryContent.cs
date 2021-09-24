using System;
using System.Xml.Linq;
using Rdms.Communication.Entities;

namespace GVUZ.Helper.Rdms
{
    public class DictionaryContent
    {
        public DictionaryContent(string message)
        {
            ErrorMessage = message;
        }

        public DictionaryContent(VersionDescription versionDescription, XDocument content)
        {
            if (versionDescription == null) throw new ArgumentNullException("versionDescription");
            if (content == null) throw new ArgumentNullException("content");
            Content = content;
            VersionDescription = versionDescription;
        }

        public DictionaryContent(VersionDescription versionDescription)
        {
            VersionDescription = versionDescription;
        }

        public string ErrorMessage { get; private set; }
        public XDocument Content { get; private set; }
        public VersionDescription VersionDescription { get; private set; }

        public bool HasErrors
        {
            get { return !string.IsNullOrEmpty(ErrorMessage); }
        }

        public bool ShouldImportContent
        {
            get { return Content != null; }
        }
    }
}