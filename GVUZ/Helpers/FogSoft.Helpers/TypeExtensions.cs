using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace FogSoft.Helpers
{
	/// <summary>
	/// Extension methods for types (primarily to obtain attributes).</summary>
	[DebuggerStepThrough]
	public static class TypeExtensions
	{
        public static string GetDescription<T>(this T type) where T : class
        {
            foreach (DescriptionAttribute attrib in type.GetType().GetCustomAttributes(typeof(DescriptionAttribute), true))
                return attrib.Description;

            return string.Empty;
        }

        public static string GetEnumDescription<TEnum>(this TEnum item)
        {
            return typeof(TEnum).GetEnumDescription(Convert.ToInt32(item));
        }

        public static string GetEnumDescription(this Type enumType, int value)
        {
            foreach (FieldInfo field in enumType.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
            {
                foreach (DescriptionAttribute attrib in field.GetCustomAttributes(typeof(DescriptionAttribute), true))
                {
                    if ((int)field.GetValue(null) == value) return attrib.Description;
                }
            }
            return null;
        }

		public static T[] GetAttributes<T>(this Type type, bool allowMultiple = true, bool inherit = true,
			bool throwIfNotFound = true) where T : Attribute
		{
			if (type == null) throw new ArgumentNullException("type");
			T[] attributes = (T[])type.GetCustomAttributes(typeof(T), inherit);
			if (attributes.Length == 0)
			{
				if (throwIfNotFound)
					throw new ArgumentException(String.Format("There are no {0} attribute for the {1} type.",
						typeof(T).Name, type.Name));
				return attributes;
			}
			if (!allowMultiple && attributes.Length > 1)
				throw new ArgumentException(String.Format("There are more than one {0} attribute for the {1} type.",
						typeof(T).Name, type.Name));
			return attributes;
		}

		public static T[] GetAttributes<T>(this PropertyInfo info, bool allowMultiple = true, bool inherit = true,
			bool throwIfNotFound = true) where T : Attribute
		{
			if (info == null) throw new ArgumentNullException("info");
			T[] attributes = (T[])info.GetCustomAttributes(typeof(T), inherit);
			if (attributes.Length == 0)
			{
				if (throwIfNotFound)
					throw new ArgumentException(String.Format("There are no {0} attribute for the {1} type.",
						typeof(T).Name, info.Name));
				return attributes;
			}
			if (!allowMultiple && attributes.Length > 1)
				throw new ArgumentException(String.Format("There are more than one {0} attribute for the {1} type.",
						typeof(T).Name, info.Name));
			return attributes;
		}

		/// <summary>
		/// Replaces '.' and '+' by underscore signs.</summary>
		public static string NormalizeName(this Type type)
		{
			if (type == null) throw new ArgumentNullException("type");
			return (type.FullName ?? type.Name).Replace('.', '_').Replace("+", "__");
		}
	}
}
