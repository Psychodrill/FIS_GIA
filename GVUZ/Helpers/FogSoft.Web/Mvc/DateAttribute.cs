using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FogSoft.Web.Mvc
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
	public class DateAttribute : DataTypeAttribute
	{
		private string _compareDatePattern;
		private Match _match;
		public string CompareDatePattern
		{
			get { return _compareDatePattern; }
			set
			{
				_compareDatePattern = value;
				if (Regex.IsMatch(CompareDatePattern, @"^([<>]?\=?)(today|now|\d\d\d\d\-\d\d\-\d\d)$"))
					_compareDatePattern += "-0d";
				_match = Regex.Match(CompareDatePattern, @"^([<>]?\=?)(today|now|\d\d\d\d\-\d\d\-\d\d)([\-\+])(\d+)(y|m|d)$");
				if(!_match.Success)
					throw new ArgumentException("Invalid compare date pattern");
			}
		}

		public DateAttribute(string compareDatePattern) : base(DataType.Date)
		{
			ErrorMessageResourceType = typeof (Errors);
			ErrorMessageResourceName = "DateInvalid";
			CompareDatePattern = compareDatePattern;
		}

		public override bool IsValid(object value)
		{
			if(value == null) return true;
			if (!(value is DateTime))
				return false;
			DateTime checkDate;
			if (_match.Groups[2].Value == "today")
				checkDate = DateTime.Today;
			else if (_match.Groups[2].Value == "now")
				checkDate = DateTime.Now;
			else
				checkDate = DateTime.ParseExact(_match.Groups[1].Value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
			int mult = 1;
			if (_match.Groups[3].Value == "-")
				mult = -1;
			int gVal = Convert.ToInt32(_match.Groups[4].Value);
			if (_match.Groups[5].Value == "y")
				checkDate = checkDate.AddYears(mult * gVal);
			if (_match.Groups[5].Value == "m")
				checkDate = checkDate.AddMonths(mult * gVal);
			if (_match.Groups[5].Value == "d")
				checkDate = checkDate.AddDays(mult * gVal);
			DateTime val = (DateTime)value;
			switch (_match.Groups[1].Value)
			{
				case "=": return val == checkDate;
				case ">": return val > checkDate;
				case ">=": return val >= checkDate;
				case "<=": return val <= checkDate;
				case "<": return val < checkDate;
			}
			return true;
		}

		private readonly object _typeID = new object();
		public override object TypeId
		{
			get
			{
				return _typeID;
			}
		}
	}
}
