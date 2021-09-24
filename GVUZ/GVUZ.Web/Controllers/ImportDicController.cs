using System.Linq;
using System.Web.Mvc;
using GVUZ.Helper;
using GVUZ.Helper.MVC;
using GVUZ.Helper.Rdms;
using GVUZ.Web.Helpers;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels;

namespace GVUZ.Web.Controllers
{
	[Authorize(Roles = UserRole.RonNsi), MenuSection("Import")]
	public class ImportDicController : BaseController
	{
		public ActionResult ImportDictionaries()
		{			
			return View("ImportDictionaries", new ImportDictionaryViewModel(
				DictionaryNames.GetSortedByName().Select(x =>
					new ImportDictionaryItem
						{
							DictionaryID = (int)x.Key,
							DictionaryName = x.Value
						})));
		}

		[HttpPost]
		public AjaxResultModel SyncDictionary(int? dictionaryID)
		{
			string result;
			if (dictionaryID != null)
				result = NsiHelper.Import((Dictionary)dictionaryID);
			else
				result = "Не указан идентификатор справочника";

			return new AjaxResultModel { Data = result };
		}
	}
}
