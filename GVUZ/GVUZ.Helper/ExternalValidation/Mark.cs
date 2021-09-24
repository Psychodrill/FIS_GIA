using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;

namespace GVUZ.Helper.ExternalValidation
{
    [DebuggerDisplay("{SubjectName}: {SubjectMark}")]
    public class Mark
    {
        private static readonly CultureInfo RussianCulture = CultureInfo.GetCultureInfo("ru-RU");

        [XmlElement("subjectName")]
        public string SubjectName { get; set; }

        [XmlElement("subjectMark")]
        public string SubjectMark { get; set; }

        [XmlElement("subjectAppeal")]
        public string SubjectAppeal { get; set; }

        /// <summary>
        ///     Gets decimal mark (or -1 if cannot be parsed).
        /// </summary>
        [XmlIgnore]
        public decimal Value
        {
            get
            {
                decimal mark;
                if (Decimal.TryParse(SubjectMark, NumberStyles.Any, RussianCulture, out mark))
                    return mark;
                return -1;
            }
        }
    }
}