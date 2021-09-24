using System;
using System.Collections.Generic;

namespace Fbs.Web.Administration.IPCheck
{
    public class IPChecker
    {
        List<IPRange> m_ranges=new List<IPRange>();
        public IPChecker(string ipRangesStr)
        {
            Init(ipRangesStr);
        }
        private void Init(string ipRangesStr)
        {
            string[] ipRanges_split = ipRangesStr.Split(new char[] { ';', ' '});
            m_ranges.Clear();
            foreach(string range in ipRanges_split)
            {
                IPRange ipRange = new IPRange(range);
                if (ipRange.IsValid())
                    m_ranges.Add(ipRange);
            }
        }

        /// <summary>
        /// Адрес принадлежит какому-либо диапазону?
        /// </summary>
        public bool IsInRage(IPAddress_My address)
        {
            foreach(IPRange range in m_ranges)
            {
                if (range.IsInRage(address))
                {
                    return true;
                }
            }
            return false;
        }

        private static IPChecker m_IPChecker_forAdmin = new IPChecker(Config.IPRangesForAdmins);
        /// <summary>
        /// Находится ли данный IP-адрес в списке разрешённых для администрирования
        /// </summary>
        /// <param name="address">ip-адрес пользователя</param>
        public static bool IsAdminIP_InAllowedRage(IPAddress_My address)
        {
            return m_IPChecker_forAdmin.IsInRage(address);
        }

       /// <summary>
        /// Находится ли данный IP-адрес в списке разрешённых для администрирования
        /// </summary>
        /// <param name="addressStr">строка: ip-адрес пользователя</param>
        public static bool IsAdminIP_InAllowedRage(string addressStr)
        {
            IPAddress_My address;
            if (IPAddress_My.TryParse(addressStr, out address))
            {
                return IsAdminIP_InAllowedRage(address);
            }
            return false;
        }

        private static IPChecker m_IPChecker_forOuterSite = new IPChecker(Config.IPRangesForOuterSite);
        /// <summary>
        /// Находится ли данный IP-адрес в списке разрешённых для внешних сайтов
        /// </summary>
        /// <param name="address">ip-адрес пользователя</param>
        public static bool CheckOuterSite(IPAddress_My address)
        {
            return m_IPChecker_forOuterSite.IsInRage(address);
        }

        public static bool CheckOuterSite(string addressStr)
        {
            IPAddress_My address;
            if (IPAddress_My.TryParse(addressStr, out address))
            {
                return CheckOuterSite(address);
            }
            return false;
        }


        /// <summary>
        /// Находится ли данный IP-адрес в списке разрешённых для администрирования
        /// </summary>
        /// <param name="addressStr">строка: ip-адрес пользователя</param>
        public static bool IsAdminIP_InAllowedRage(string addressStr, out string error)
        {
            IPAddress_My address;
            error = "";
            if (IPAddress_My.TryParse(addressStr, out address))
            {
                return IsAdminIP_InAllowedRage(address);
            }

            error=string.Format("IP-Адрес '{0}' невозможно разобрать.", addressStr);
            return false;
            
        }
    }
}
