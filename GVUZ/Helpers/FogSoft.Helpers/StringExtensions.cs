using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace FogSoft.Helpers
{
	/// <summary>
	/// Extension methods for strings.</summary>
	[DebuggerStepThrough]
	public static class StringExtensions
	{
		private static readonly Regex ReplaceNonLettersRegex = new Regex(@"[^a-zA-Z]", RegexOptions.Compiled);
		public static string FormatWith(this string pattern, params object[] prms)
		{
			return string.Format(pattern, prms);
		}

		/// <summary>
		/// Returns whether or not specified value equals at least one specified argument.</summary>
		/// <remarks>Uses <see cref="StringComparison.Ordinal"/> to compare strings.</remarks>
		public static bool In(this string value, params string[] args)
		{
			return value.In(StringComparison.Ordinal, args);
		}

		public static StringBuilder AppendWith(this StringBuilder builder, string[] values, string separator = ",", bool addLastSeparator = false)
		{
			if (builder == null) throw new ArgumentNullException("builder");
			if (values.IsNullOrEmpty()) return builder;
			int oldLength = builder.Length;

			foreach (string value in values)
				builder.Append(value).Append(separator);

			if (!addLastSeparator && builder.Length > oldLength)
				builder.Length -= separator.Length;

			return builder;
		}

		/// <summary>
		/// Returns whether or not specified value equals at least one specified argument.</summary>
		public static bool In(this string value, StringComparison comparison, params string[] args)
		{
			if (value == null) throw new ArgumentNullException("value");
			if (args == null || args.Length == 0) return false;
			foreach (string s in args)
			{
				if (string.Equals(value, s, comparison))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Replaces all non-letter characters with specified character[s].</summary>
		public static string ReplaceNonLetters(this string value, string replaceWith = "_")
		{
			if (value == null) throw new ArgumentNullException("value");

			return ReplaceNonLettersRegex.Replace(value, replaceWith);
		}
	}
}
