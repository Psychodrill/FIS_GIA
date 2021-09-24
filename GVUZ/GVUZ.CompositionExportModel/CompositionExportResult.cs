using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GVUZ.CompositionExportModel
{
    [DataContract]
    public class CompositionExportResult
    {
        [DataMember]
        public string Log { get; set; }

        [DataMember]
        public byte[] File { get; set; }
        [DataMember]
        public bool HasData { get; set; }
    }
}
