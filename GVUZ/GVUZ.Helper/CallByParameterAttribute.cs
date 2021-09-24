using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace GVUZ.Helper
{
    /// <summary>
    ///     ������� ��� ���� � ����������� submit-��������, ������� ������ ���������� ��� Action � ��������� "Call_".
    /// </summary>
    /// <remarks>�� ������� http://blog.ashmind.com/2010/03/15/multiple-submit-buttons-with-asp-net-mvc-final-solution/ </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CallByParameterAttribute : ActionNameSelectorAttribute
    {
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            if (actionName.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                return true;

            HttpRequestBase request = controllerContext.RequestContext.HttpContext.Request;

            // �������� ��� Image Button (������ �������������� ������ ������� ������)
            return request.Form["Call_" + methodInfo.Name] != null;
        }
    }
}