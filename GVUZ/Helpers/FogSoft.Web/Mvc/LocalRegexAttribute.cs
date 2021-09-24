using System;
using System.ComponentModel.DataAnnotations;

namespace FogSoft.Web.Mvc
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
	public class LocalRegexAttribute : RegularExpressionAttribute
	{
		public LocalRegexAttribute(string pattern) : base(pattern)
		{
			ErrorMessageResourceType = typeof(Errors);
			ErrorMessageResourceName = "PropertyValueInvalidRegex";
			
		}
	}
}
