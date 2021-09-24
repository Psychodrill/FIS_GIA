namespace Ege.Check.App.Web.ModelBinders
{
    using System;
    using System.Globalization;
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;

    public class DateTimeModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var date = value.ConvertTo(typeof (DateTime), CultureInfo.CurrentCulture);
            bindingContext.Model = date;
            return true;
        }
    }
}
