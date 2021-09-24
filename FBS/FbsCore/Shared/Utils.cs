using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fbs.Core.Shared
{
    public static class Utils
    {
        public static bool ParseGuid(string item, out Guid result)
        {
            try
            {
                result = new Guid(item);
            }
            catch
            {
                result = Guid.Empty;
                return false;
            }

            return true;
        }

        public static string GetNullString(object item)
        {
            if (item == null)
                return null;

            if (item.ToString().Trim().Length == 0)
                return null;
            else
                return item.ToString().Trim();
        }
    }
}
