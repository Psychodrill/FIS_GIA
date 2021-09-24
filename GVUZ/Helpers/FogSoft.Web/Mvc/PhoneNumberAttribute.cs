using System;
using System.ComponentModel.DataAnnotations;
using FogSoft.Helpers.Validators;

namespace FogSoft.Web.Mvc
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public class PhoneNumberAttribute : DataTypeAttribute
	{

		public PhoneNumberAttribute() : base(DataType.PhoneNumber)
		{
			ErrorMessageResourceType = typeof (Errors);
			ErrorMessageResourceName = "InvalidPhoneNumber";
		}

		public override bool IsValid(object value)
		{
			if(value == null || String.IsNullOrEmpty(value.ToString())) return true;
			return PhoneNumberValidator.IsValid(value.ToString());
		}
	}
}
