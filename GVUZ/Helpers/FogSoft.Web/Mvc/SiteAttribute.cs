using System;
using System.ComponentModel.DataAnnotations;

namespace FogSoft.Web.Mvc
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public class SiteAttribute : RegularExpressionAttribute
	{
		public SiteAttribute()
			: base("^(http(s)?://)?(([\\w\\-]+(\\.[\\w\\-]+)*\\.[\\w][\\w]+(/[^\">]*)?)|(\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}))$")
		{
			ErrorMessageResourceType = typeof(Errors);
			ErrorMessageResourceName = "PropertyValueInvalidRegex";
		}
	}
}
