using System;
using System.Web.Mvc;

namespace FogSoft.Web.Mvc.Binders
{
    public class DateTimeBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (result == null || string.IsNullOrEmpty(result.AttemptedValue))
                return null;
            DateTime time;
            if (DateTime.TryParse(result.AttemptedValue, out time))
                return time;

            bindingContext.ModelState.AddModelError
                (bindingContext.ModelName, String.Format("\"{0}\" is invalid.", bindingContext.ModelName));

            return null;
        }
    }
}