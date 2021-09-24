using System;
using System.Linq;
using System.Web.Mvc;

namespace GVUZ.Helper
{
    public class AjaxResultModel : ActionResult
    {
        public const string DataError = "Ошибка в данных";
        private readonly string _combinedErrorMessage;

        public AjaxResultModel()
        {
       }

        public AjaxResultModel(string message)
        {
            IsError = !String.IsNullOrEmpty(message);
            Message = message;
        }

        public AjaxResultModel(ModelStateDictionary modelState)
        {
            var array = modelState.Where(x => x.Value.Errors.Count > 0).Select(
                x => new{
                        ControlID = x.Key.Replace('.', '_').Replace('[', '_').Replace(']', '_'),
                        ErrorMessage = String.Join("<br/>", x.Value.Errors.Select(y => y.ErrorMessage))
                    }).ToArray();
            if (array.Length > 0)
            {
                Data = array;
                IsError = true;
            }
            _combinedErrorMessage = String.Join("\r\n", array.Select(x => x.ErrorMessage.Replace("<br/>", "\r\n")));
        }

        public bool IsError { get; set; }
        public string Message { get; private set; }
        public object Data { get; set; }
        public object Extra { get; set; }

        public JsonResult Json()
        {
            return new JsonResult {Data = this, JsonRequestBehavior=JsonRequestBehavior.AllowGet};
        }

        public AjaxResultModel SetMessageFromErrors()
        {
            Message = _combinedErrorMessage;
            return this;
        }

        public AjaxResultModel SetIsError(string controlID, string errorMessage)
        {
            Data = new[] {new {ControlID = controlID, ErrorMessage = errorMessage}};
            IsError = true;
            return this;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            Json().ExecuteResult(context);
        }
    }
}