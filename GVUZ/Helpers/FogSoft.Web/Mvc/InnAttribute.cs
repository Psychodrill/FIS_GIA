using System;
using System.ComponentModel.DataAnnotations;
using FogSoft.Helpers.Validators;

namespace FogSoft.Web.Mvc
{
	/// <summary>
	/// Атрибут для проверки ИНН. <seealso cref="InnValidator"/></summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public class InnAttribute : ValidationAttribute
	{
		public bool ForOrganization { get; set; }

		public InnAttribute(bool forOrganization)
		{
			ForOrganization = forOrganization;
			ErrorMessageResourceType = typeof(Errors);
			ErrorMessageResourceName = "InvalidInn";
		}

		public override bool IsValid(object value)
		{
			if (value != null && !(value is string))
				return false;

			return InnValidator.IsValid((string) value, ForOrganization);
		}
	}
}
