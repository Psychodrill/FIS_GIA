using System.Globalization;
using System.Web.Mvc;

namespace GVUZ.Web.ViewModels.InstitutionAchievements
{
    public class InstitutionAchievementModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor.PropertyType == typeof (decimal) ||
                propertyDescriptor.PropertyType == typeof (decimal?))
            {
                var v = bindingContext.ValueProvider.GetValue(propertyDescriptor.Name);
                if (v != null)
                {
                    decimal d;
                    if (decimal.TryParse(v.AttemptedValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out d))
                    {
                        propertyDescriptor.SetValue(bindingContext.Model, d);
                    }
                }
            }
            else
            {
                base.BindProperty(controllerContext, bindingContext, propertyDescriptor);    
            }
        }
    }
}