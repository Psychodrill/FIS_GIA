namespace Fbs.Web.Administration.IPCheck
{
    public class IPRange
    {
        public readonly IPAddress_My StartAddress;
        public readonly IPAddress_My EndAddress;
        public IPRange(string rangeStr)
        {
            string[] rangeSplit = rangeStr.Split(new char[] {'-'});
            if (rangeSplit.Length==1)
            {
                IPAddress_My.TryParse(rangeSplit[0], out StartAddress);
                IPAddress_My.TryParse(rangeSplit[0], out EndAddress);
            }
            if (rangeSplit.Length > 1)
            {
                IPAddress_My addr1, addr2;
                IPAddress_My.TryParse(rangeSplit[0], out addr1);
                IPAddress_My.TryParse(rangeSplit[1], out addr2);
                if ((addr1 != null) && (addr2 != null) && (addr1.CompareTo(addr2)<=0 ))
                {
                    StartAddress = addr1;
                    EndAddress = addr2;
                }
                else
                {
                    StartAddress = addr2;
                    EndAddress = addr1;
                }
            }
        }

        public bool IsValid()
        {
            if ((StartAddress == null) || (EndAddress == null))
                return false;
            return true;
        }

        public bool IsInRage(IPAddress_My address)
        {
            if ((StartAddress!=null)&&(EndAddress!=null))
            {
                return (StartAddress.CompareTo(address)<=0) && (address.CompareTo(EndAddress)<=0);
            }
            return false;
        }
    }
}
