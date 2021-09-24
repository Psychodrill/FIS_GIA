using System;
using System.Text;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Esrp.Core;

namespace Esrp.Web
{
    public static class IntrantAccountExtentions
    {
        public static string GetFullName(this IntrantAccount account)
        {
            StringBuilder result = new StringBuilder();
            if (!string.IsNullOrEmpty(account.LastName))
                result.Append(account.LastName);
            if (!string.IsNullOrEmpty(account.FirstName))
            {
                if (result.Length > 0)
                    result.Append(" ");
                result.Append(account.FirstName);
            }
            if (!string.IsNullOrEmpty(account.PatronymicName))
            {
                if (result.Length > 0)
                    result.Append(" ");
                result.Append(account.PatronymicName);
            }
            return result.ToString();
        }

        /*public static string GetIpAddressesAsEdit(this IntrantAccount account)
        {
            if (account.IpAddresses == null)
                return null;
            return string.Join("\r\n", account.IpAddresses);
        }

        public static void SetIpAddressesAsEdit(this IntrantAccount account, string ipAddresses)
        {
            account.IpAddresses = ipAddresses.Split("\r\n,; ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        public static string GetIpAddressesAsView(this IntrantAccount account)
        {
            if (account.IpAddresses == null)
                return null;
            return string.Join("<br />", account.IpAddresses);
        }*/

        public static string GetStateName(bool? isActive)
        {
            if (isActive == null)
                return "Неизвестен";
            if ((bool)isActive)
                return "Действующий";
            else
                return "Отключенный";
        }
    }
}
