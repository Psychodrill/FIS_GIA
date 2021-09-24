using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esrp.Utility
{
  public static   class Conversions
    {
        public static string ArrayToString(long[] array)
        {
            string[] result = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
                result[i] = array[i].ToString();
            return String.Join(",", result);
        }

        public static string ArrayToString(int[] array)
        {
            string[] result = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
                result[i] = array[i].ToString();
            return String.Join(",", result);
        }

        public static string ListToString(List<string > list)
        {
            string[] result = new string[list.Count ];
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].ToString();
            return String.Join(",", result);
        }

        public static string ListToString(List<int> list)
        {
            string[] result = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].ToString();
            return String.Join(",", result);
        }
    }
}
