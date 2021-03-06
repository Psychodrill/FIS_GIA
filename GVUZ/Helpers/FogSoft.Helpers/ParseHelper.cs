using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace FogSoft.Helpers
{
	/// <summary>
	/// Helps to parse and convert strings and objects.</summary>
	[DebuggerStepThrough]
	public static class ParseHelper
	{
		public static bool IsDbNullOrNull(this object value)
		{
			return value == null || value == DBNull.Value;
		}

		public static string GetStringOrNullIfEmpty(string value)
		{
			return string.IsNullOrEmpty(value) ? null : value;
		}

		/// <summary>
		/// Parses value to boolean with standard text (false,no,off/true,yes,on) and numeric (0/1) representations.</summary>
		public static bool ParseToBoolean(string value, bool defaultValue)
		{
			if (string.IsNullOrEmpty(value))
				return defaultValue;

			switch (value.ToLower())
			{
				case "true":
				case "yes":
				case "on":
					return true;
				case "false":
				case "no":
				case "off":
					return false;
			}

			int result;
			if (int.TryParse(value, NumberStyles.Integer, CultureInfo.CurrentCulture, out result))
			{
				switch (result)
				{
					case 1:
						return true;
					case 0:
						return false;
						/*default:
					return defaultValue;*/
				}
			}

			return defaultValue;
		}

		/// <summary>
		/// Converts specified value to the <see cref="T"/> (if needed).</summary>
		/// <param name="value">Value to convert.</param>
		/// <param name="defaultValue">Default value (if is <see cref="DBNull"/> or null).</param>
		/// <param name="provider">Optional <see cref="IFormatProvider"/>.</param>
		/// <param name="parameterName">Optional parameter name (to throw exception if <see cref="ensureExists"/>).</param>
		/// <param name="ensureExists">Ensures, that values not is <see cref="DBNull"/> or null.</param>
		/// <param name="createTypeDescriptorContext">Optional function to create <see cref="ITypeDescriptorContext"/>
		/// when all other conversions cannot be done.</param>
		/// <returns>Converted value/the same value/default value.</returns>
		public static T ConvertTo<T>
			(object value, T defaultValue = default(T),
			 IFormatProvider provider = null, string parameterName = null, bool ensureExists = false,
			 Func<object, ITypeDescriptorContext> createTypeDescriptorContext = null)
		{
			return
				(T)
				ConvertTo(typeof (T), value, defaultValue, provider, parameterName, ensureExists, false,
				          createTypeDescriptorContext);
		}

		/// <summary>
		/// Converts specified value to the specified type (if needed).</summary>
		/// <param name="targetType">Target type (to convert to this type).</param>
		/// <param name="value">Value to convert.</param>
		/// <param name="defaultValue">Default value (if is <see cref="DBNull"/> or null).</param>
		/// <param name="provider">Optional <see cref="IFormatProvider"/>.</param>
		/// <param name="parameterName">Optional parameter name (to throw exception if <see cref="ensureExists"/>).</param>
		/// <param name="ensureExists">Ensures, that values not is <see cref="DBNull"/> or null.</param>
		/// <param name="convertDefaultValue">Whether or not to convert default value (to ensure right type).</param>
		/// <param name="createTypeDescriptorContext">Optional function to create <see cref="ITypeDescriptorContext"/>
		/// when all other conversions cannot be done.</param>
		/// <returns>Converted value/the same value/default value.</returns>
		public static object ConvertTo
			(Type targetType, object value, object defaultValue,
			 IFormatProvider provider = null, string parameterName = null, bool ensureExists = false,
			 bool convertDefaultValue = true,
			 Func<object, ITypeDescriptorContext> createTypeDescriptorContext = null)
		{
			if (value == null || value == DBNull.Value)
			{
				if (ensureExists)
				{
					string message = (parameterName == null)
					                 	? "Required parameter not found."
					                 	: string.Format("Required parameter '{0}' not found.", parameterName);
					throw new ArgumentException(message);
				}
			}
			else
			{
				if (targetType.IsAssignableFrom(value.GetType())) return value;

				if (targetType == typeof (bool))
					return ParseToBoolean(value.ToString(), (bool) (defaultValue ?? default(bool)));

				if (targetType.IsEnum) return Enum.Parse(targetType, value.ToString());

				if (targetType == typeof (Guid)) return new Guid(value.ToString());

				Type nullableType = Nullable.GetUnderlyingType(targetType);
				if (nullableType != null)
				{
					Type underlyingType = nullableType.UnderlyingSystemType;

					if (underlyingType.IsAssignableFrom(value.GetType())) return value;

					// ReSharper disable RedundantCast
					if (underlyingType == typeof (bool))
						return ParseToBoolean(value.ToString(), (bool) ((object) (defaultValue) ?? (object) default(bool)));
					// ReSharper restore RedundantCast

					if (nullableType.IsEnum) return Enum.Parse(underlyingType, value.ToString());

					if (underlyingType == typeof (Guid)) return new Guid(value.ToString());

					return ChangeType(underlyingType, value, provider);
				}

				return ChangeType(targetType, value, provider);
			}

			if (!convertDefaultValue || targetType == typeof (object)) return defaultValue;

			return ConvertTo(targetType, defaultValue, null, provider, parameterName, false, false);
		}

		/// <summary>
		/// Changes type of the specified <see cref="value"/> to <see cref="targetType"/>.
		/// Usually you should not use this method directly (use <see cref="ConvertTo{T}"/> or <see cref="ConvertTo"/> instead).</summary>
		/// <param name="targetType">Target type (to convert to this type).</param>
		/// <param name="value">Value to convert.</param>
		/// <param name="provider">Optional <see cref="IFormatProvider"/>.</param>
		/// <param name="createTypeDescriptorContext">Optional function to create <see cref="ITypeDescriptorContext"/>
		/// when value does not support <see cref="IConvertible"/>.</param>
		/// <returns>Converted value.</returns>
		public static object ChangeType(Type targetType, object value, IFormatProvider provider = null,
		                                Func<object, ITypeDescriptorContext> createTypeDescriptorContext = null)
		{
			if (targetType == null) throw new ArgumentNullException("targetType");
			if (value == null) throw new ArgumentNullException("value");

			if (value is IConvertible)
			{
				try
				{
					return Convert.ChangeType(value, targetType, provider ?? CultureInfo.CurrentCulture);
				}
				catch (InvalidCastException ex)
				{
                    LogHelper.Log.Error(ex.Message, ex);
					// TODO: think about static Dictionary<Type, HashSet<Type>> HasConvertFrom to avoid try/catch (Eventually)
				}
			}

			if (createTypeDescriptorContext != null)
			{
				var context = createTypeDescriptorContext(value);
				var converter = TypeDescriptor.GetConverter(value);
				if (converter != null && converter.CanConvertTo(context, targetType))
				{
					return converter.ConvertTo(context, CultureInfo.CurrentCulture, value, targetType);
				}
				converter = TypeDescriptor.GetConverter(targetType);
				if (converter != null && converter.CanConvertFrom(context, value.GetType()))
				{
					return converter.ConvertFrom(context, CultureInfo.CurrentCulture, value);
				}
			}

			throw new InvalidOperationException(
				string.Format("Type cannot be changed from {0} to {1}.", value.GetType(), targetType));
		}

		/// <summary>
		/// Converts any object to the specified type (<see cref="ConvertTo{T}"/> for details).</summary>
		public static T To<T>(this object value, T defaultValue = default(T), IFormatProvider provider = null,
		                      string parameterName = null, bool ensureExists = false)
		{
			return ConvertTo(value, defaultValue, provider, parameterName, ensureExists);
		}	

        /// <summary>
        /// Конвертация строки в число с дробной частью. Вне зависимости от введённого разделителя
        /// </summary>
        /// <param name="str">Строка для конвертации</param>
        /// <returns>Число, если преобразование возможно.</returns>
        /// <exception cref="InvalidCastException">Если преобразование невозможно - ошибка InvalidArgumentExceptino</exception>
        public static decimal ToDecimal(this string str)
        {
            decimal result;
            try
            {
                result = decimal.Parse(str);
            }
            catch
            {
                string changedString;
                if (str.Contains("."))
                    changedString = str.Replace(".", ",");
                else if (str.Contains(",")) changedString = str.Replace(",", ".");
                     else throw new InvalidCastException("Строка не является предствалением числа");

                try
                {
                    result = decimal.Parse(changedString);
                }
                catch
                {
                    throw new InvalidCastException("Строка не является предствалением числа");
                }
            }

            return result;
        }
	}
}
