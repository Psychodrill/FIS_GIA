using System;

namespace GVUZ.Helper
{
    public class GeneratorPortletLinkAttribute : Attribute
    {
        public GeneratorPortletLinkAttribute(Type portletClass, string portletClassMethod) : this()
        {
            PortletLinkClass = portletClass;
            PortletLinkMethod = portletClassMethod;
        }

        protected GeneratorPortletLinkAttribute()
        {
            MethodParams = new object[0];
            UseControllersArgs = true;
        }

        public Type PortletLinkClass { get; set; }
        public string PortletLinkMethod { get; set; }
        public object[] MethodParams { get; set; }
        public bool UseControllersArgs { get; set; }


        public virtual string ExecuteMethod(object[] controllerArgs)
        {
            object[] res;
            if (UseControllersArgs)
            {
                res = new object[controllerArgs.Length + MethodParams.Length];
                Array.Copy(controllerArgs, 0, res, 0, controllerArgs.Length);
                Array.Copy(MethodParams, 0, res, controllerArgs.Length, MethodParams.Length);
            }
            else
                res = MethodParams;
            return PortletLinkClass.GetMethod(PortletLinkMethod).Invoke(null, res) as string;
        }
    }
}