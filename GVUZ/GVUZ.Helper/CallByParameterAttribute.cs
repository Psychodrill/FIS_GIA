using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace GVUZ.Helper
{
    /// <summary>
    ///     Атрибут для форм с несколькими submit-кнопками, которые должны называться как Action с префиксом "Call_".
    /// </summary>
    /// <remarks>По мотивам http://blog.ashmind.com/2010/03/15/multiple-submit-buttons-with-asp-net-mvc-final-solution/ </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CallByParameterAttribute : ActionNameSelectorAttribute
    {
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            if (actionName.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                return true;

            HttpRequestBase request = controllerContext.RequestContext.HttpContext.Request;

            // подумать про Image Button (сейчас обрабатываются только обычные кнопки)
            return request.Form["Call_" + methodInfo.Name] != null;
        }
    }
}