using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GVUZ.CompositionExportModel
{
    [DataContract, Serializable]
    public class ERBDCompositionInfoList
    {
        [DataMember]
        public DateTime ActualDate { get { return _actualDate; } set { _actualDate = value; } }
        private DateTime _actualDate = DateTime.Now;

        [DataMember]
        public List<ERBDCompositionInfo> Items { get { return _items; } set { _items = value; } }
        private List<ERBDCompositionInfo> _items = new List<ERBDCompositionInfo>();
    }
}
