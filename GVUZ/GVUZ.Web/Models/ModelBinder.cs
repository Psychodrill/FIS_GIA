using System;
using System.Globalization;
using System.Web.Mvc;

namespace GVUZ.Web.Models
{
	public class ModelBinder : IModelBinder
	{
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			DateTime modelValue = Convert.ToDateTime(bindingContext.Model, CultureInfo.CurrentUICulture);
			
			if (modelValue == DateTime.MinValue)
			{
				ValueProviderResult result = controllerContext.Controller.ValueProvider.GetValue(
					bindingContext.ModelName);

				DateTime resDate;
				if (DateTime.TryParse(result.AttemptedValue, CultureInfo.CurrentUICulture, DateTimeStyles.None, out resDate))
					return resDate;
				
				bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Указан некорректный формат даты.");
				return result.RawValue;
			}

			return modelValue;		
		}
	}
}