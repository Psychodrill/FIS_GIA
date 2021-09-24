using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBS.Replicator.Entities.ERBD
{
    public class ERBDCompositionInfo
    {
        public ERBDCompositionInfo(Guid participantId, byte pagesCount)
        {
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

        public readonly Guid? ParticipantId;

        public readonly bool Parsed;
        public readonly byte[] Barcode;
        public string BarcodeStr { get { return DataHelper.BytesToString( Barcode); } }

        public readonly byte PagesCount; 
    }
}
