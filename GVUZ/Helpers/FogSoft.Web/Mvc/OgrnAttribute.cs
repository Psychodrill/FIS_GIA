using System;
using System.ComponentModel.DataAnnotations;
using FogSoft.Helpers.Validators;

namespace FogSoft.Web.Mvc
{
	/// <summary>
	/// Атрибут для проверки ОГРН. <seealso cref="OgrnValidator"/></summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public class OgrnAttribute : ValidationAttribute
	{
		public bool ForOrganization { get; set; }

		public OgrnAttribute(bool forOrganization)
		{
			ForOrganization = forOrganization;
			ErrorMessageResourceType = typeof(Errors);
			ErrorMessageResourceName = "InvalidOgrn";
		}

		public override bool IsValid(object value)
		{
			if (!(value is string) && value != null)
				return false;

			return OgrnValidator.IsValid((string) value, ForOrganization);
		}
	}
}
