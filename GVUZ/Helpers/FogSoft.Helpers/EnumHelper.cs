using System;
using System.Diagnostics;

namespace FogSoft.Helpers
{
	/// <summary>
	/// Simplifies common tasks for enums. Use with caution for performance-critical code.</summary>
	[DebuggerStepThrough]
	public static class EnumHelper
	{
		public static void EnsureDefined<T>(T value)
		{
			if (!IsDefined(value))
				throw new ArgumentOutOfRangeException("value");
		}

		public static bool IsDefined<T>(T value)
		{
			return Enum.IsDefined(typeof(T), value);
		}

		public static T BitwiseOr<T>(T value, T flag)
		{
			return (T)((object)(Convert.ToInt32(value) | Convert.ToInt32(flag)));
		}

		public static T BitwiseAnd<T>(T value, T flag)
		{
			return (T)((object)(Convert.ToInt32(value) & Convert.ToInt32(flag)));
		}

		/// <summary>
		/// Checks that both value produce non-zero result of bitwise AND. Use with caution for performance-critical code.</summary>
		/// <remarks>Providing non-enum types for "T" not checked (the reason is performance degradation).</remarks>
		public static bool IsBitwiseMatch<T>(T value, T flag)
		{
			return (Convert.ToInt32(value) & Convert.ToInt32(flag)) != 0;
		}

		/// <summary>
		/// Checks that both value produce non-zero result of bitwise AND. Use with caution for performance-critical code.</summary>
		/// <remarks>Providing non-enum types for "T" not checked (the reason is performance degradation).</remarks>
		public static bool IsBitwiseMatch<T>(T value, int flag)
		{
			return (Convert.ToInt32(value) & flag) != 0;
		}

		/// <summary>
		/// Returns whether or not specified value equals at least one specified argument.</summary>
		public static bool In<T>(this T value, params T[] list)
		{
			if (list == null) throw new ArgumentNullException("list");
			foreach (T item in list)
			{
				if (Equals(item, value))
					return true;
			}
			return false;
		}
	}
}
