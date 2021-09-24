using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esrp.Core.ApplicationFCT
{
    public class ApplicationFCT
    {

#region ctors
        public ApplicationFCT()
        {
            ID = 0;
        }


        public ApplicationFCT(int OrgID, int ApplicationID)
        {
            ID = ApplicationID;
            OrganizationID = OrgID;
        }

 #endregion

        public int ID
        {
            get;
            set;
        }

        public int OrganizationID
        {
            get; set;
        }

        public int FillingStage
        {
            get; set;
        }

        public string ScanOrderContentType
        {
            get; set;
        }

        public byte[] ScanOrder
        {
            get;
            set;
        }


        public string PersonFullName
        {
            get; set;
        }

        public string PersonPosition
        {
            get; set;
        }

        public string PersonWorkPhone
        {
            get; set;
        }

        public string PersonMobPhone
        {
            get; set;
        }

        public string PersonEmail
        {
            get; set;
        }

        public bool IsThereAttestatK1More
        {
            get; set;
        }

        public int NumARMs
        {
            get; set;
        }

        public int NumPDNs
        {
            get; set;
        }

        public int? DictOperationSystemID
        {
            get; set;
        }

        // ???? Is it need dict names

        public bool? IsIPStatic
        {
            get; set;
        }

        public string IPAddress
        {
            get; set;
        }

        public string IPMask4ARMs
        {
            get; set;
        }

        public string FISLogin
        {
            get; set;
        }

        public int? DictAntivirusID
        {
            get; set;
        }

        public int? DictUnauthAccessProtectID
        {
            get; set;
        }


        public int? DictElectronicLockID
        {
            get; set;
        }

        public int? DictTNScreenID
        {
            get; set;
        }

        public string IP4TNS
        {
            get; set;
        }


        public int? DictVipNetCryptoID
        {
            get; set;
        }

        public string DictAntivirusName
        {
            get; set;
        }

        public string DictElectronicLockName
        {
            get; set;
        }

        public string DictOperationSystemName
        {
            get; set;
        }

        public string DictTNScreenName
        {
            get; set;
        }

        public string DictUnauthAccessProtectName
        {
            get; set;
        }

        public string DictVipNetCryptoName
        {
            get; set;
        }


    }
}
