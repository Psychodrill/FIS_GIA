using System;
using System.ComponentModel.DataAnnotations;

namespace FogSoft.Web.Mvc
{
	/// <summary>
	/// Overrides <see cref="RequiredAttribute"/> with error message from <see cref="Errors"/>.</summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
		AllowMultiple = false, Inherited = true)]
	public sealed class LocalRequiredAttribute : RequiredAttribute
	{
		public LocalRequiredAttribute()
		{
			ErrorMessageResourceType = typeof (Errors);
			ErrorMessageResourceName = "Required";
		}
	}
}
