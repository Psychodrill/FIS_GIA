using System;
using System.Data.SqlClient;
using System.Linq;
using GVUZ.Helper;
using GVUZ.Model;
using GVUZ.Model.Entrants;
using GVUZ.Model.Helpers;
using GVUZ.Web.Portlets.Entrants;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Методы для работы с языками абитуриентов. Сейчас не вводятся
	/// </summary>
	public static class EntrantLanguageExtensions
	{
		/// <summary>
		/// Загружаем языки
		/// </summary>
		public static EntrantLanguageViewModel FillLanguages(this EntrantsEntities dbContext, EntrantLanguageViewModel model, EntrantKey key)
		{
			int entrantID = key.GetEntrantID(dbContext, false);
			if (entrantID == 0 && key.ApplicationID > 0)
				model.ShowDenyMessage = true;
            model.LanguageList = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.ForeignLanguageType).OrderBy(x => x.Value).Select(x => new { ID = x.Key, x.Value }).ToArray();
			//для просмотра и редактирования в разные модели пишем
			if (!model.IsView)
			{
				model.LanguageData = dbContext.EntrantLanguage
					.Where(x => x.EntrantID == entrantID)
					.OrderBy(x => x.ForeignLanguageType.Name)
					.Select(x => x.LanguageID).ToArray();
			}
			else
			{
				model.LanguageDataView = dbContext.EntrantLanguage
					.Where(x => x.EntrantID == entrantID)
					.OrderBy(x => x.ForeignLanguageType.Name)
					.Select(x => x.ForeignLanguageType.Name).ToArray();
			}

			model.EntrantID = entrantID;
			return model;
		}

		/// <summary>
		/// Сохраняем выбранные иностранные языки
		/// </summary>
		public static AjaxResultModel SaveLanguages(this EntrantsEntities dbContext, EntrantLanguageViewModel model, UserInfo userInfo)
		{
			var currentData = SaveLanguagesInternal(dbContext, model, model.EntrantID);
			if (currentData.IsError) return currentData;
			if (model.ApplicationID > 0)
			{
				int origEntrantID =
					dbContext.Application.Where(x => x.ApplicationID == model.ApplicationID).Select(x => x.EntrantID).FirstOrDefault();
				//ставим те же данные, что и у текущего #30337
				if (origEntrantID > 0 && origEntrantID != model.EntrantID)
					SaveLanguagesInternal(dbContext, model, origEntrantID);
			}

			return currentData;
		}

		/// <summary>
		/// Внутреннее сохранение иностранных языков
		/// </summary>
		private static AjaxResultModel SaveLanguagesInternal(EntrantsEntities dbContext, EntrantLanguageViewModel model, int entrantID)
		{
			Entrant entrant =
				dbContext.Entrant.Where(x => x.EntrantID == entrantID).FirstOrDefault();
			if (entrant == null)
				return new AjaxResultModel("Не найден абитуриент");
			foreach (var l in dbContext.EntrantLanguage.Where(x => x.EntrantID == entrant.EntrantID))
				dbContext.EntrantLanguage.DeleteObject(l);
			if (model.LanguageData != null)
			{
				foreach (int langID in model.LanguageData)
				{
					EntrantLanguage el = new EntrantLanguage { Entrant = entrant };
					int tmpLangID = langID;
					el.ForeignLanguageType = dbContext.ForeignLanguageType.Single(x => x.LanguageID == tmpLangID);
					dbContext.EntrantLanguage.AddObject(el);
				}
			}

			try
			{
				dbContext.SaveChanges();
			}
			catch (Exception ex)
			{
				SqlException inner = ex.InnerException as SqlException;
				if (inner != null && inner.Message.Contains("UK_EntrantLanguage"))
					return new AjaxResultModel("Дублируются языки");
				throw;
			}

			return new AjaxResultModel();
		}
	}
}