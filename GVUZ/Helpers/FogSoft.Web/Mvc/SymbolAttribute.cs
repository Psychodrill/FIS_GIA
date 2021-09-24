using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FogSoft.Web.Mvc
{
	public enum SymbolType
	{
		// only digits
		Numeric = 1,
		// digits and letters
		AlphaNumeric = 2,
		// first number than letter characters
		NumberLetter = 3,
		// digits, letters and dash
		AlphaNumericDash = 4,
	}

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public class SymbolAttribute : ValidationAttribute
	{
		private readonly SymbolType _symbolType;
		public SymbolAttribute(SymbolType symbolType)
		{
			_symbolType = symbolType;
			ErrorMessageResourceType = typeof(Errors);			
		}

		public override bool IsValid(object value)
		{
			if (value == null) return true;

			switch (_symbolType)
			{
				case SymbolType.Numeric:
					ErrorMessageResourceName = "InvalidNumeric";
					return new Regex(@"^\d*$", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).IsMatch(value.ToString());				
				case SymbolType.AlphaNumeric:
					ErrorMessageResourceName = "InvalidAlphaNumeric";
					return new Regex(@"^[\s\w]*$", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).IsMatch(value.ToString());
				case SymbolType.NumberLetter:
					ErrorMessageResourceName = "InvalidNumberLetter";
					return new Regex(@"^\d+[\s]?[\w]?$", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).IsMatch(value.ToString());
				case SymbolType.AlphaNumericDash:
					ErrorMessageResourceName = "InvalidAlphaNumericDash";
					return new Regex(@"^[\s\w-]*$", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).IsMatch(value.ToString());
			}

			return base.IsValid(value);
		}
	}
}
