using System;
using System.ComponentModel.DataAnnotations;

namespace FogSoft.Web.Mvc
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
		AllowMultiple = false, Inherited = true)]
	public class LocalRangeAttribute : RangeAttribute
	{
		public LocalRangeAttribute(int minimum, int maximum) : base(minimum, maximum)
		{
			ErrorMessageResourceType = typeof (Errors);
			ErrorMessageResourceName = "PropertyValueInvalidRange";
		}

	}
}
