using System;
using System.Net;

namespace Esrp.Web.Administration.IPCheck
{
    public class IPAddress_My : IPAddress, IComparable<IPAddress_My>
    {
        public new static IPAddress_My Parse(string ipString)
        {
            return new IPAddress_My(IPAddress.Parse(ipString).GetAddressBytes());
        }

        public static bool TryParse(string ipString, out IPAddress_My address)
        {
            IPAddress addr = null;

            if (IPAddress.TryParse(ipString, out addr))
            {
                address = new IPAddress_My(addr.GetAddressBytes());
                return true;
            }
            address = null;
            return false;
        }

        IPAddress_My(byte[] address):base(address)
        {
            m_Adderess_My = Adderess_My_Calc();
        }

        public ulong m_Adderess_My;
        public ulong Adderess_My_Calc()
        {
            byte[] addr = this.GetAddressBytes();
            ulong returnVal = 0;
            for (int i = 0; i < addr.Length; i++)
                returnVal += ((ulong) addr[i] << (8*(addr.Length - 1 - i)));

            return returnVal;
        }
        public ulong Adderess_My
        {
            get { return m_Adderess_My; }
        }


        #region IComparable<IPAddress_My> Members

        public int CompareTo(IPAddress_My other)
        {
            return Adderess_My.CompareTo(other==null?0:other.Adderess_My);
        }

        #endregion
    }
}
