using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GVUZ.Helper.ExternalValidation
{
    public class EgeResultAndStatus
    {
        public const string TransferError = "001";
        public const string InvalidPin = "002";
        public const string Succeded = "003";

        private static readonly HashSet<string> Codes = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
            {TransferError, InvalidPin, Succeded};

        private string _statusCode;

        public EgeResultAndStatus()
        {
        }

        public EgeResultAndStatus(EgeResult egeResult, string code)
        {
            if (egeResult == null) throw new ArgumentNullException("egeResult");

            StatusCode = code;
            Result = egeResult;
        }

        [XmlElement("StatusCode", IsNullable = false)]
        public string StatusCode
        {
            get { return _statusCode; }
            set
            {
                if (!Codes.Contains(value)) throw new ArgumentOutOfRangeException("value");
                _statusCode = value;
            }
        }

        [XmlElement("ResultXml", IsNullable = false)]
        public EgeResult Result { get; set; }
    }
}