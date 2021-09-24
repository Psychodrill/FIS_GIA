using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FogSoft.Helpers;
using FogSoft.WSRP;
using GVUZ.Helper;
using GVUZ.Model.Institutions;
using GVUZ.Model.Benefits;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.Helpers;
using GVUZ.Web.Portlets;
using Rdms.Communication.Exceptions;

namespace GVUZ.Web.Controllers {
	[ValidateInput(false)]
	public class BaseController:Controller {
		/// <summary>
		/// Институт пользователя
		/// </summary>
		public int InstitutionID {
			get {
				return InstitutionHelper.GetInstitutionID(IsInsidePortlet);
			}
		}

		public bool IsInsidePortlet {
			get { return ControllerContext==null||ControllerContext.HttpContext.InsidePortlet(); }
		}

		private readonly Dictionary<string,object> _customParameters=new Dictionary<string,object>();

		public Dictionary<string,object> CustomParameters {
			get { return _customParameters; }
		}

		/// <summary>
		/// Добавить ошибки в модель
		/// </summary>
		protected void AddModelError(string message,string key="") {
			ModelState.AddModelError(key,message);
		}

		/// <summary>
		/// Добавить ошибки в модель
		/// </summary>
		protected void AddModelErrors(Dictionary<string,string> messages) {
			foreach(var message in messages)
				ModelState.AddModelError(message.Key,message.Value);
		}

		/// <summary>
		/// Вернуть ошибки json'ом
		/// </summary>
		protected JsonResult JsonReturnModelErrors(Dictionary<string,string> messages) {
			AddModelErrors(messages);
			return Json(new AjaxResultModel(ModelState));
		}

		/// <summary>
		/// Обработка исключений
		/// </summary>
		protected override void OnException(ExceptionContext filterContext) {
			filterContext.ExceptionHandled=true;
			LogHelper.Log.Error(filterContext.Exception);

			if(filterContext.Exception is UICustomException) {
				filterContext.HttpContext.Response.Clear();
				filterContext.HttpContext.Response.Write(filterContext.Exception.Message);
				return;
			}

			base.OnException(filterContext);
			filterContext.Result=Redirect(Url.Generate<ErrorController>(m => m.HttpError()));
		}

		/// <summary>
		/// Псевдо-json для добавления файлов аяксом
		/// </summary>
		public class FileJsonResult:ActionResult {
			private readonly JsonResult _result=new JsonResult();

			public FileJsonResult(object data) {
				_result.Data=data;
			}

			public override void ExecuteResult(ControllerContext context) {
				bool isXhr=context.HttpContext.Request.ServerVariables["HTTP_X_REQUESTED_WITH"]=="XMLHttpRequest";
				HttpResponseBase response=context.HttpContext.Response;
				if(!isXhr)
					response.Write("<textarea>");
				_result.ExecuteResult(context);
				if(!isXhr) {
					response.ContentType="text/html";
					response.Write("</textarea>");
				}
			}
		}

		public static int GetMaxAllowedFileLength() {
			//TODO: после получения подтверждения на размер вставить корректное значение
			return ConfigurationManager.AppSettings["MaxPostFileLength"].To(10000);
		}

		/// <summary>
		/// Принять файл по и отдать attachment ID. Метод не должен использоваться, заменён на <see cref="ReceiveFile1"/>
		/// </summary>
		[HttpPost]
		public virtual ActionResult ReceiveFile() {
			if(Request==null||Request.Files==null||Request.Files.Count==0||Request.Files[0].ContentLength==0)
				return new FileJsonResult(new { FileID=0 });

			HttpPostedFileBase file=Request.Files[0];
			if(file.ContentLength>GetMaxAllowedFileLength()*1024)
				return new FileJsonResult(new { FileID=0,BigSize=true });

			using(var context=new InstitutionsEntities()) {
				int attachmentID=context.CreateAttachment(file);
				return new FileJsonResult(new { FileID=attachmentID });
			}
		}

		/// <summary>
		/// Принять файл и отдать fileID
		/// </summary>
		[HttpPost]
		public virtual ActionResult ReceiveFile1() {
			if(Request==null||Request.Files==null||Request.Files.Count==0||Request.Files[0].ContentLength==0)
				return new FileJsonResult(new { FileID=(Guid?)null });

			HttpPostedFileBase file=Request.Files[0];
			if(file.ContentLength>GetMaxAllowedFileLength()*1024)
				return new FileJsonResult(new { FileID=(Guid?)null,BigSize=true });

            LogHelper.Log.Debug("before context");
            try
            {
                using (var context = new InstitutionsEntities())
                {
                    int attachmentID = context.CreateAttachment(file);
                    LogHelper.Log.DebugFormat("attachmentID: {0}", attachmentID);

                    var f = context.Attachment.Where(x => x.AttachmentID == attachmentID).Select(x => (Guid?)x.FileID).FirstOrDefault();
                    LogHelper.Log.DebugFormat("f: {0}", f.IsDbNullOrNull() ? "null" : f.ToString());


                    Guid ? fileID = (Guid?)context.Attachment.Where(x => x.AttachmentID == attachmentID).Select(x => (Guid?)x.FileID).FirstOrDefault();
                    return new FileJsonResult(new { FileID = fileID });
                }
            }
            catch (Exception ex)
            {
                var exL = ex as System.Reflection.ReflectionTypeLoadException;
                LogHelper.Log.DebugFormat("after context:\n {0}\n {1}\n {2}\n {3}", ex.Message, ex.StackTrace
                    , exL != null ? exL.Message : ""
                    , exL != null && exL.LoaderExceptions != null ? string.Join(",", exL.LoaderExceptions.Select(t => t.Message)) : ""
                    );
                //return null;
                throw ex;
            }
		}

		public virtual ActionResult ReceiveFile2(int year,int instid) {
			if(Request==null||Request.Files==null||Request.Files.Count==0||
				// ReSharper disable PossibleNullReferenceException
				 Request.Files[0].ContentLength==0)
				return new FileJsonResult(new { FileID=(Guid?)null });
			HttpPostedFileBase file=Request.Files[0];
			/*if (file.ContentLength > GetMaxAllowedFileLength() * 1024)
				 return new FileJsonResult(new { FileID = (Guid?)null, BigSize = true });
			// ReSharper restore PossibleNullReferenceException
			*/
			using(var context=new BenefitsEntities()) {
				int attachmentID=context.CreateAttachment1(file,year,instid);
				//int attachmentID = context.CreateAttachment(file);
				Guid? fileID=null;
				//int fileID = context.AdmissionRules.Where(x => x.RecordID == attachmentID).Select(x => x.RecordID).LastOrDefault();
				//Guid? fileID = context.ExecuteStoreQuery<Guid>(@"SELECT [FileID] FROM AdmissionRules WHERE RecordID={0}", attachmentID).Single();
				return new FileJsonResult(new { FileID=fileID });
			}
		}

		public virtual ActionResult Delete(int YeaRi,int Insti) {
			using(var context=new BenefitsEntities()) {
				context.DeleteAttachment1(YeaRi,Insti);
				//int attachmentID = context.CreateAttachment(file);
				Guid? fileID=null;
				//int fileID = context.AdmissionRules.Where(x => x.RecordID == attachmentID).Select(x => x.RecordID).LastOrDefault();
				//Guid? fileID = context.ExecuteStoreQuery<Guid>(@"SELECT [FileID] FROM AdmissionRules WHERE RecordID={0}", attachmentID).Single();
				return new FileJsonResult(new { FileID=fileID });
			}
		}

		/// <summary>
		/// Получить файл через портлет
		/// </summary>
		[HttpPost]
		public virtual ActionResult ReceivePortletFile(UploadContext[] uploadContexts) {
			var receivedFile=new PortletFileHelper.UploadFile(uploadContexts);

			if(receivedFile.Content==null)
				return new FileJsonResult(new { FileID=(Guid?)null });

			if(receivedFile.Content.Length>GetMaxAllowedFileLength()*1024)
				return new FileJsonResult(new { FileID=(Guid?)null,BigSize=true });

			using(var context=new InstitutionsEntities()) {
				int attachmentID=context.CreatePortletAttachment(receivedFile);
				Guid? fileID=context.Attachment.Where(x => x.AttachmentID==attachmentID).Select(x => x.FileID).Single();
				return new FileJsonResult(new { FileID=fileID });
			}
		}

		/// <summary>
		/// Отдать файл клиенту
		/// </summary>
		public virtual ActionResult GetFile(int? fileID) {
			using(InstitutionsEntities dbContext=new InstitutionsEntities()) {
				Attachment att=dbContext.Attachment.Where(x => x.AttachmentID==fileID).FirstOrDefault();
				if(att!=null) {
					return new FixedFileResult(dbContext.GetAttachmentBody(att.AttachmentID),att.MimeType,att.Name);
				}

				return new EmptyResult();
			}
		}

		/// <summary>
		/// Специальный тип результата возврата файла для корректного отображения русских букв в имени
		/// </summary>
		protected class FixedFileResult:ActionResult {
			private readonly byte[] _fileBody;
			private readonly string _contentType;
			private readonly string _fileName;

			public FixedFileResult(byte[] fileBody,string contentType,string fileName) {
				_fileBody=fileBody;
				_contentType=contentType;
				_fileName=fileName;
			}

			public override void ExecuteResult(ControllerContext context) {
				//if(!context.HttpContext.Request.UserAgent.Contains("MSIE"))
				//{
				//    FileContentResult res = new FileContentResult(_fileBody, _contentType);
				//    res.FileDownloadName = _fileName;
				//    res.ExecuteResult(context);
				//    return;
				//}
				if(context==null) {
					throw new ArgumentNullException("context");
				}

				HttpResponseBase response=context.HttpContext.Response;
				response.ContentType=_contentType;
				if(!string.IsNullOrEmpty(_fileName)) {
					//string headerValue = ContentDispositionUtil.GetHeaderValue(this.FileDownloadName);
					context.HttpContext.Response.AddHeader("Content-Disposition","attachment; filename="+HttpUtility.UrlEncode(_fileName,Encoding.UTF8).Replace("+","%20"));
				}

				WriteFile(response);
			}

			private void WriteFile(HttpResponseBase response) {
				response.OutputStream.Write(_fileBody,0,_fileBody.Length);
			}
		}

		//TODO: rename after refactoring old code
		/// <summary>
		/// Отдать файл клиенту
		/// </summary>
		public virtual ActionResult GetFile1(Guid? fileID) {
			using(InstitutionsEntities dbContext=new InstitutionsEntities()) {
				Attachment att=dbContext.Attachment.Where(x => x.FileID==fileID).FirstOrDefault();
				if(att!=null) {
					return new FixedFileResult(dbContext.GetAttachmentBody(att.AttachmentID),att.MimeType,att.Name);
					//return File(dbContext.GetAttachmentBody(att.AttachmentID), att.MimeType, att.Name);
				}

				return new EmptyResult();
			}
		}

		public virtual ActionResult GetFile2(int year) {
			using(BenefitsEntities dbContext=new BenefitsEntities()) {
				AdmissionRules att=dbContext.AdmissionRules.Where(x => x.Year==year&&x.InstitutionID==InstitutionID).FirstOrDefault();
				if(att!=null) {
					//return new FixedFileResult(dbContext.GetRulesBody(att.RecordID), "text/x-ms-contact", "user.contact");
					return File(dbContext.GetRulesBody(att.RecordID),att.MimeType,att.FileName);
					//return new EmptyResult();
				}
				//return new FixedFileResult(null, "other", "ahahahahaha");
				return new EmptyResult();
			}
		}

		public static bool CheckFile(int year,int Inst) {
			using(BenefitsEntities dbContext=new BenefitsEntities()) {
				int attId=dbContext.AdmissionRules.Where(x => x.Year==year&&x.InstitutionID==Inst).Select(obj => obj.RecordID).FirstOrDefault();
				if(attId!=0) {
					return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Создать модель нужного типа
		/// </summary>
		public TModel GetModel<TModel>() where TModel:class, new() {
			TModel model=(TModel)Activator.CreateInstance(typeof(TModel));
			return RefreshModel(model);
		}

		/// <summary>
		/// Обновить модель данными, пришедшими с клиента
		/// </summary>
		public TModel RefreshModel<TModel>(TModel model) where TModel:class, new() {
			base.TryUpdateModel(model);
			return model;
		}
	}
}