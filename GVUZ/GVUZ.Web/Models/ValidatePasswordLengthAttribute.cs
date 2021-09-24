using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;

namespace GVUZ.Web.Models
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false,
		Inherited = true)]
	public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
	{
		private const string DefaultErrorMessage = "'{0}' должен быть как минимум {1} символов.";
		private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

		public ValidatePasswordLengthAttribute()
			: base(DefaultErrorMessage)
		{
		}

		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
			                     name, _minCharacters);
		}

		public override bool IsValid(object value)
		{
			string valueAsString = value as string;
			return (valueAsString != null && valueAsString.Length >= _minCharacters);
		}
	}
}