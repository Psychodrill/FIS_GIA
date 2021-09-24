using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GVUZ.DAL.Helpers
{
    static class ValidateINNHelper
    {
        static int GetCheckNumber(string INN)
        {
            int[] multipliers = { 2, 4, 10, 3, 5, 9, 4, 6, 8, 0 };

            int[] innn = INN.Select((c, i) => (c - 48) * multipliers[i++]).ToArray();

            int crc = innn.Sum();

            return crc %= 11;
        }

        internal static bool IsValidINN(string INN)
        {
            var crc = GetCheckNumber(INN);

            return (crc == int.Parse(INN.Substring(9)));
        }
    }
}