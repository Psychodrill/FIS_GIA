using System;
using System.Net.Mail;
using System.Text;

namespace FogSoft.Helpers.Mail
{
    public static class MailExtensions
    {
        public static MailAddress FixMailAddressDisplayName(this MailAddress address, Encoding encoding)
        {
            if (address == null) throw new ArgumentNullException("encoding");
            if (encoding == null) throw new ArgumentNullException("encoding");
            if (string.IsNullOrEmpty(address.DisplayName)) return address;

            return new MailAddress(address.Address, FixMailAddressDisplayName(address.DisplayName), encoding);
        }

        public static string FixMailAddressDisplayName(string address)
        {
            return address == null ? null : address.Replace(' ', '\xA0');
        }
    }
}