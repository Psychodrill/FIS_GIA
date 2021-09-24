using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace GVUZ.CompositionExportModel
{
    [DataContract]
    public class CompositionRequestItem
    {
        [DataMember]
        public string CompositionPaths { get; set; }
        [DataMember]
        public int EntrantID { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public string DocumentSeries { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public string CompositionId { get; set; }
        [DataMember]
        public Guid ParticipantId { get; set; }
        [DataMember]
        public int UseYear { get; set; }


        public override string ToString()
        {
            return string.Format("{0}_{1}_{2}_{3}_{4}", LastName, FirstName, MiddleName, DocumentSeries, DocumentNumber);
        }
    }


}
