using System;
using System.Data.SqlClient;
using System.Linq;
using GVUZ.Helper;
using GVUZ.Model;
using GVUZ.Model.Entrants;
using GVUZ.Web.ViewModels.Administration.Catalogs;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Методы работы с иностранными языками (справочники)
	/// </summary>
	public static class ForeignLanguageExtensions
	{
		/// <summary>
		/// Создаём иностранный язык
		/// </summary>
		public static AjaxResultModel CreateForeignLanguage(this EntrantsEntities dbContext, AddForeignLanguageViewModel model)
		{
			bool isEdit = model.LanguageID > 0;
			ForeignLanguageType language;
			if (isEdit)
			{
				language = dbContext.ForeignLanguageType.FirstOrDefault(x => x.LanguageID == model.LanguageID);
				if (language == null)
					return new AjaxResultModel("Иностранный язык не найден");
			}
			else
				language = new ForeignLanguageType();

			if (String.IsNullOrWhiteSpace(model.Name))
				return new AjaxResultModel("Необходимо ввести название иностранного языка");
			language.Name = model.Name;											
			
			if (!isEdit)
				dbContext.ForeignLanguageType.AddObject(language);
			
			try
			{
				dbContext.SaveChanges();
			}
			catch (Exception ex)
			{
				var inner = ex.InnerException as SqlException;				
				if (inner != null && inner.Message.Contains("UK_ForeignLanguageType_Name"))
				{
					return new AjaxResultModel().SetIsError("Name", "Введенный иностранный язык уже добавлен");
				}
				
				throw;
			}

			return new AjaxResultModel
			{
				Data = new ForeignLanguagesViewModel.ForeignLanguageData
				       	{
							LanguageID = language.LanguageID,
							Name = language.Name				       		
				       	}				
			};
		}
	
		/// <summary>
		/// Загружаем язык
		/// </summary>
		public static AddForeignLanguageViewModel LoadForeignLanguage(this EntrantsEntities dbContext, AddForeignLanguageViewModel model)
		{
            var language = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.ForeignLanguageType).First(x => x.Key == model.LanguageID);			
			model.LanguageID = language.Key;
			model.Name = language.Value;
			return model;
		}

		/// <summary>
		/// Удаляем язык
		/// </summary>
		public static AjaxResultModel DeleteForeignLanguage(this EntrantsEntities dbContext, int? languageID)
		{
			var language = dbContext.ForeignLanguageType.First(x => x.LanguageID == languageID);
			
			if (language == null)
				return new AjaxResultModel("Язык не найден");
			
			dbContext.ForeignLanguageType.DeleteObject(language);

			try
			{
				dbContext.SaveChanges();
			}
			catch (Exception ex) //если используется в заявлениях, будет ошибка
			{				
				var inner = ex.InnerException as SqlException;
				if (inner != null && inner.Message.Contains("FK_EntrantLanguage_ForeignLanguageType"))
				{					
					return new AjaxResultModel("Удаление невозможно. Данный язык используется в заявлении.");
				}

				throw;
			}

			return new AjaxResultModel();
		}
	}
}