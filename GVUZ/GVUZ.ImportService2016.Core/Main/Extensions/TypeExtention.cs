using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GVUZ.ImportService2016.Core.Main.Extensions
{
    public static class TypeExtention
    {
        public static string GetDescription<T>(this T type) where T : class
        {
            foreach (DescriptionAttribute attrib in type.GetType().GetCustomAttributes(typeof(DescriptionAttribute), true))
                return attrib.Description;

            return string.Empty;
        }
    }
}
