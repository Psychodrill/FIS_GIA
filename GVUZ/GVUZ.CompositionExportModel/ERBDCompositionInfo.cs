using GVUZ.CompositionExportModel.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GVUZ.CompositionExportModel
{
    [DataContract, Serializable]
    public class ERBDCompositionInfo
    {
        public ERBDCompositionInfo() { }

        public ERBDCompositionInfo(string key, Guid participantId, byte pagesCount)
        {
            Key = key;
            ParticipantId = participantId;
            PagesCount = pagesCount;
        }

        public ERBDCompositionInfo(string infoRow)
        {
            if ((!infoRow.Contains("_")) && (!infoRow.Contains(":")))
            {
                Parsed = false;
            }
            else
            {
                string barcode = infoRow.Split('_')[0];
                Barcode = DataHelper.StringToBytes(barcode);
                byte pagesCount;
                if (Byte.TryParse(infoRow.Split(':').Last(), out pagesCount))
                {
                    PagesCount = pagesCount;
                    Parsed = true;
                }
                else
                {
                    Parsed = false;
                }
            }
        }
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public Guid? ParticipantId { get; set; }
        [DataMember]
        public bool Parsed { get; set; }
        [DataMember]
        public byte[] Barcode { get; set; }
        [DataMember]
        public byte PagesCount { get; set; }

        //[DataMember]
        //public string BarcodeStr { get { return DataHelper.BytesToString(Barcode); } }
    }
}
