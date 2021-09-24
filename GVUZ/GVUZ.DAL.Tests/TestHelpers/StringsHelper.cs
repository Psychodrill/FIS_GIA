using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Tests.TestHelpers
{
    internal static class StringsHelper
    {
        public static string NormalizeNamePart(string namePart)
        {
            if (namePart == null)
                return null;

            return namePart
                .ToUpper()
                .Replace(" ", "")
                .Replace("Й", "И")
                .Replace("Ё", "Е");
        }
    }
}
