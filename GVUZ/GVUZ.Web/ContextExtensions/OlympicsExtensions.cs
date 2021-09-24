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
	/// Методы для работы с олимпиадами (справочники)
	/// </summary>
	public static class OlympicsExtensions
	{
		/// <summary>
		/// Создать новую олимпиаду
		/// </summary>
		public static AjaxResultModel CreateOlympic(this EntrantsEntities dbContext, AddOlympicViewModel model)
		{
			//model.PrepareForSave(dbContext);

			//bool isEdit = model.OlympicID > 0;
			//OlympicType olympic;
			//if (isEdit)
			//{
			//	olympic = dbContext.OlympicType.FirstOrDefault(x => x.OlympicID == model.OlympicID);
			//	if (olympic == null)
			//		return new AjaxResultModel("Олимпиада не найдена");
			//}
			//else
			//	olympic = new OlympicType();
			
			//if (String.IsNullOrWhiteSpace(model.Name))
			//	return new AjaxResultModel("Необходимо ввести название олимпиады");

			//if (model.OlympicNumber == null)
			//	return new AjaxResultModel().SetIsError("OlympicNumber", "Поле № олимпиады обязательно для заполнения.");

			//olympic.Name = model.Name;
			//olympic.OlympicLevelID = model.OlympicLevelID;
		 //   olympic.OlympicYear = model.OlympicYear;
			//olympic.OlympicNumber = model.OlympicNumber.Value;
			//olympic.OrganizerName = model.OrganizerName;			

			//if (!isEdit)
			//	dbContext.OlympicType.AddObject(olympic);

			//if (isEdit)
			//{
			//	var links = dbContext.OlympicTypeSubjectLink.Where(x => x.OlympicID == olympic.OlympicID);
			//	foreach (var olympicTypeSubjectLink in links)
			//	{
			//		dbContext.OlympicTypeSubjectLink.DeleteObject(olympicTypeSubjectLink);
			//	}				
			//}

			//foreach (var subject in model.Subjects)
			//{
			//	dbContext.OlympicTypeSubjectLink.AddObject(
			//		new OlympicTypeSubjectLink 
			//		{ 
			//			OlympicType = olympic, 
			//			SubjectID = subject.SubjectID,
   //                     SubjectLevelID = subject.SubjectLevelID
			//		});
			//}

			//try
			//{
			//	dbContext.SaveChanges();
			//}
			//catch (Exception ex)
			//{
			//	var inner = ex.InnerException as SqlException;
			//	if (inner != null && inner.Message.Contains("UK_OlympicType_OlympicNumber"))
			//	{
			//		return new AjaxResultModel().SetIsError("OlympicNumber", "Олимпиада с таким номером уже добавлена");
			//	}

			//	throw;
			//}

			return new AjaxResultModel
			{
				Data = new OlympicsViewModel.OlympicData
				{
					//OlympicID = olympic.OlympicID,
					//Name = olympic.Name,
					//OlympicLevelID = olympic.OlympicLevelID,
     //               OlympicLevelName = olympic.OlympicLevel != null ? olympic.OlympicLevel.Name : 
     //                   string.Join(", ", olympic.OlympicTypeSubjectLink.Select(c => 
     //                       c.OlympicLevel.Name).Distinct().OrderBy(c => c).ToArray()),
     //               OlympicYear = olympic.OlympicYear,
					//OlympicNumber = olympic.OlympicNumber,
					//OrganizerName = olympic.OrganizerName
				}
			};			
		}

		/// <summary>
		/// Проинициализировать модель олимпиады
		/// </summary>
		public static AddOlympicViewModel InitOlympic(this EntrantsEntities dbContext, AddOlympicViewModel model)
		{
            var list = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.OlympicLevel)
				.Where(x => x.Key != 1) //все уровни
				.OrderBy(x => x.Value).Select(x => new { ID = x.Key, Name = x.Value }).ToArray();
		    model.Levels = list;

#warning ХАРДКОД!!!! ХАРДКОД!!!! ХАРДКОД!!!! ХАРДКОД!!!! ХАРДКОД!!!!
            if (model.OlympicYear != 2012)
                model.SubjectLevels = list.Select(c => c.Name).ToArray();

			model.FillData(dbContext);
			return model;
		}

		/// <summary>
		/// Загрузить олимпиаду
		/// </summary>
		public static AddOlympicViewModel LoadOlympic(this EntrantsEntities dbContext, AddOlympicViewModel model)
		{
			model = dbContext.InitOlympic(model);
			var olympic = dbContext.OlympicType.First(x => x.OlympicID == model.OlympicID);
			
			//model.OlympicID = olympic.OlympicID;
			//model.Name = olympic.Name;
			//model.OlympicLevelID = olympic.OlympicLevelID;
		 //   model.OlympicYear = olympic.OlympicYear;
			//model.OlympicNumber = olympic.OlympicNumber;
			//model.OrganizerName = olympic.OrganizerName;

			return model;
		}

		/// <summary>
		/// Удалить олимпиаду
		/// </summary>
		public static AjaxResultModel DeleteOlympic(this EntrantsEntities dbContext, int? olympicID)
		{
			var olympic = dbContext.OlympicType.First(x => x.OlympicID == olympicID);

			if (olympic == null)
				return new AjaxResultModel("Олимпиада не найдена");

			dbContext.OlympicType.DeleteObject(olympic);

			try
			{
				dbContext.SaveChanges();
			}
			catch (Exception ex)
			{
				var inner = ex.InnerException as SqlException;
				if (inner != null && inner.Message.Contains("FK_BenefitItemCOlympicType_OlympicType"))
				{
					return new AjaxResultModel("Удаление невозможно. Данная олимпиада используется в заявлении.");
				}

				throw;
			}

			return new AjaxResultModel();
		}
	}
}