using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FogSoft.Helpers;
using System.Collections.Generic;
using GVUZ.Web.SQLDB;
using GVUZ.Web.ViewModels;
using GVUZ.Helper;
using GVUZ.Web.ContextExtensionsSQL;

namespace GVUZ.Web.Controllers {

	public class DictionaryController:BaseController {

		// GET: /Dictionary/id
		public ActionResult Get (string dic, string key, string filter) {
			IEnumerable<DictionaryModel> list=null;
			DictionaryFilterModel f= new DictionaryFilterModel();
			f.Dictionary=dic;
			f.Key=key;
			try {
				list=DictionarySQL.GetDictionary(f);
			} catch(Exception e) {
				return new AjaxResultModel(e.ToString());
			}
			return new AjaxResultModel { Data=list };
		}

		[HttpPost]
		public ActionResult GetByFilter(DictionaryFilterModel f) {
			IEnumerable<DictionaryModel> list=null;
			try {
				//app=EntrantApplicationSQL.GetApplication(id.Value);
				list=DictionarySQL.GetDictionary(f);
			} catch(Exception e) {
				return new AjaxResultModel(e.ToString());
			}
			return new AjaxResultModel { Data=list };
		}

		[Authorize]
		[HttpPost]
		public ActionResult GetQualifications(string filter) {
			IEnumerable<string> list=null;
			try {
				list=DictionarySQL.GetQualifications(filter);
			} catch(Exception e) {
				return new AjaxResultModel(e.ToString());
			}
			return new AjaxResultModel { Data=list };
		}
		[Authorize]
		[HttpPost]
		public ActionResult GetSpecialityByQualification(string Qualification) {
			IEnumerable<string> list=null;
			try {
				list=DictionarySQL.GetSpecialityByQualification(Qualification);
			} catch(Exception e) {
				return new AjaxResultModel(e.ToString());
			}
			return new AjaxResultModel { Data=list };
		}

	}
}