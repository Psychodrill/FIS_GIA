using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace FogSoft.Helpers
{
	public enum DateEnumerationStep
	{
		Day,
		Month,
		Year
	}

	/// <summary>
	/// Describes shift method for the start date in <see cref="DateHelper.NormalizeInterval"/> method.</summary>
	public enum DateShift
	{
		/// <summary>
		/// <see cref="DateTime.MinValue"/> used as start day.</summary>
		MinDate = 1,
		/// <summary>
		/// Nearest Monday used as start day.</summary>
		/// <remarks>You can specify another day in <see cref="DateHelper.Initialize"/>.</remarks>
		WorkWeek = 2
	}

	/// <summary>
	/// Provides several format methods.</summary>
	[DebuggerStepThrough]
	public static class DateHelper
	{
		private static string _invalidDateRangeMessage = "Invalid date range.";
		/// <summary>
		/// Соответствие месяцев и кварталов.</summary>
		private static readonly Dictionary<int, int> Quarters =
			new Dictionary<int, int>
				{
					{1, 1},
					{2, 1},
					{3, 1},
					{4, 2},
					{5, 2},
					{6, 2},
					{7, 3},
					{8, 3},
					{9, 3},
					{10, 4},
					{11, 4},
					{12, 4}
				};

		private static DayOfWeek _startOfWeek = DayOfWeek.Monday;

		public static void Initialize(string invalidDateRangeMessage, DayOfWeek startOfWeek = DayOfWeek.Monday)
		{
			if (string.IsNullOrEmpty(invalidDateRangeMessage)) throw new ArgumentNullException("invalidDateRangeMessage");
			_invalidDateRangeMessage = invalidDateRangeMessage;
			_startOfWeek = startOfWeek;
		}

		
		/// <summary>
		/// Возвращает дату в заданном формате.</summary>
		/// <remarks>Описание форматов: http://msdn.microsoft.com/en-us/library/az4se3k1.aspx
		/// http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx.
		/// Дополнительно можно использовать "$d" для короткой записи года.</remarks>
		public static string Format(this DateTime value, string format = "d", IFormatProvider formatProvider = null)
		{
			if (value == DateTime.MinValue) return "";

			if (format == "$d")
			{
				var s = value.Year.ToString("0000");
				// replace, потому что год может быть на разной позиции
				return value.ToString("d").Replace(s, s.Substring(2, 2));
			}

			return value.ToString(format, formatProvider);
		}

		/// <summary>
		/// Возвращает дату в заданном формате. Подробности в описании метода <see cref="Format(System.DateTime,string,IFormatProvider)"/></summary>
		public static string Format(this DateTime? value, string format = "d", IFormatProvider formatProvider = null)
		{
			if (value.HasValue)
				return Format(value.Value, format);
			return String.Empty;
		}

		/// <summary>
		/// Возвращает дату в заданном формате. Подробности в описании метода <see cref="Format(System.DateTime,string,IFormatProvider)"/></summary>
		public static string FormatDate(object dateObject, string format)
		{
			if (dateObject == null) return String.Empty;

			return ((DateTime) dateObject).Format(format);
		}

		public static int GetQuarter(this DateTime date)
		{
			return Quarters[date.Month];
		}

		public static int GetQuarter(int month)
		{
			return Quarters[month];
		}

		/// <summary>
		/// Normalizes date interval (additionally exchanges result dates if end less than start).</summary>
		/// <param name="startDate">Optional start date.</param>
		/// <param name="endDate">Optional end date</param>
		/// <param name="start">Start date if specified or default start date depending on <see cref="defaultShift"/> parameter.</param>
		/// <param name="end">End date if specified or <see cref="DateTime.Today"/>.</param>
		/// <param name="defaultShift">One of the <see cref="DateShift"/> values.</param>
		public static void NormalizeInterval(DateTime? startDate, DateTime? endDate, out DateTime start, out DateTime end,
		                                     DateShift defaultShift = DateShift.MinDate)
		{
			if (!Enum.IsDefined(typeof(DateShift), defaultShift)) throw new ArgumentOutOfRangeException("defaultShift");

			end = endDate ?? DateTime.Today;

			if (startDate.HasValue)
			{
				start = startDate.Value;
			}
			else
			{
				switch (defaultShift)
				{
					case DateShift.MinDate:
						start = DateTime.MinValue;
						break;
					case DateShift.WorkWeek:
						DateTime dt = end.AddDays(-7);
						while (dt.DayOfWeek != _startOfWeek)
							dt = dt.AddDays(-1);
						start = dt;
						break;
					default:
						throw new InvalidOperationException();
				}
			}

			if (start > end)
			{
				DateTime dt = end;
				end = start;
				start = dt;
			}
		}

		/// <summary>
		/// Returns abbreviated day name for the specified <see cref="CultureInfo"/>.</summary>
		/// <remarks>If <see cref="CultureInfo"/> is not specified, <see cref="CultureInfo.CurrentUICulture"/> used.</remarks>
		public static string GetDayOfWeek(this DateTime value, CultureInfo culture = null)
		{
			if (culture == null)
				culture = CultureInfo.CurrentUICulture;
			return culture.DateTimeFormat.GetAbbreviatedDayName(
				culture.Calendar.GetDayOfWeek(value));
		}

		/// <summary>
		/// Returns abbreviated day name and day of month number for the specified <see cref="CultureInfo"/>.</summary>
		/// <remarks>If <see cref="CultureInfo"/> is not specified, <see cref="CultureInfo.CurrentUICulture"/> used.</remarks>
		public static string FormatDayOfWeek(this DateTime value, CultureInfo culture = null)
		{
			return value.GetDayOfWeek(culture) + ", " + value.Day.ToString("00");
		}

		/// <summary>
		/// Enumerates date interval by days.</summary>
		public static IEnumerable<DateTime> EnumerateTo(this DateTime start, DateTime end, 
		                                                DateEnumerationStep step = DateEnumerationStep.Day)
		{
			if (start > end)
				throw new ArgumentOutOfRangeException(_invalidDateRangeMessage);
			switch (step)
			{
				case DateEnumerationStep.Day:
					for (DateTime dt = start; dt <= end; dt = dt.AddDays(1))
						yield return dt;
					break;
				case DateEnumerationStep.Month:
					for (DateTime dt = start; dt <= end; dt = dt.AddMonths(1))
						yield return dt;
					break;
				case DateEnumerationStep.Year:
					for (DateTime dt = start; dt <= end; dt = dt.AddYears(1))
						yield return dt;
					break;
				default:
					throw new ArgumentOutOfRangeException("step");
			}
		}
	}
}
