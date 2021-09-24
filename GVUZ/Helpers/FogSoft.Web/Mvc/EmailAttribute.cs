using System;
using System.ComponentModel.DataAnnotations;

namespace FogSoft.Web.Mvc
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public class EmailAttribute : RegularExpressionAttribute
	{
		public EmailAttribute() : base(@"^[\w_\.-]+@(\w+(\-*\w+)*\.){1,7}[a-zA-Zа-яА-Я]{2,7}$")
		{
			ErrorMessageResourceType = typeof(Errors);
			ErrorMessageResourceName = "InvalidEmail";
		}
	}
}
