using System.Diagnostics;

namespace FogSoft.Helpers
{
	/// <summary>
	/// Extension methods for numbers.</summary>
	[DebuggerStepThrough]
	public static class NumberHelper
	{
		public static string Format(this decimal? value, string format = "N")
		{
			return value == null ? string.Empty : Format(value.Value, format);
		}

		public static string Format(this decimal value, string format = "N")
		{
			return value.ToString(format);
		}

		public static string Format(this float? value, string format = "N")
		{
			return value == null ? string.Empty : Format(value.Value, format);
		}

		public static string Format(this float value, string format = "N")
		{
			return value.ToString(format);
		}

		public static int? NullIfZero(this int value)
		{
			return value == 0 ? null : (int?)value;
		}

		public static decimal? NullIfZero(this decimal value)
		{
			return value == 0 ? null : (decimal?)value;
		}
	}
}
