using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using FogSoft.Helpers;

namespace FogSoft.Web.Mvc
{
	public static class MvcValidationHelper
	{
		public static string GetDisplayName(this PropertyInfo property)
		{
			if (property == null) throw new ArgumentNullException("property");
			DisplayAttribute[] attributes = property.GetAttributes<DisplayAttribute>(allowMultiple: false, throwIfNotFound: false);
			if (attributes.Length > 0)
				return attributes[0].Name;
			return property.Name;
		}
	}
}
