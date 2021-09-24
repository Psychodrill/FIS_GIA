using System;
using System.Data.SqlClient;
using System.Linq;
using GVUZ.Helper;
using GVUZ.Model.Entrants;
using GVUZ.Web.ViewModels.Administration.Catalogs;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Методы для работы с минимальными баллами по ЕГЭ (справочники)
	/// </summary>
	public static class SubjectEgeMinValueExtensions
	{
		/// <summary>
		/// Обновить данные по предмету
		/// </summary>
		public static AjaxResultModel UpdateSubjectsMinEgeScores(this EntrantsEntities dbContext, SubjectsAndEgeMinScoreViewModel model)
		{
			if (model.SubjectsAndScores == null || model.SubjectsAndScores.Length == 0)
			{
				return new AjaxResultModel("Ошибка сохранения. Обратитесь к администратору.");
			}

			var scoresList = model.SubjectsAndScores.ToList();
			if (scoresList.Any(scoreData => scoreData.MinValue == null))
			{
				return new AjaxResultModel().SetIsError("Error", "Ошибка сохранения. Проверьте правильность заполнения.");
			}

			foreach (var scoreData in scoresList)
			{
				var data = scoreData;
				dbContext.SubjectEgeMinValue.Where(x => x.SubjectID == data.SubjectID)
					.ToList().ForEach(dbContext.SubjectEgeMinValue.DeleteObject);
			}

			scoresList.ForEach(x => 
				dbContext.SubjectEgeMinValue.AddObject(new SubjectEgeMinValue
				                                       	{
				                                       		SubjectID = x.SubjectID, 
															MinValue = x.MinScore
				                                       	}));
			try
			{
				dbContext.SaveChanges();
			}
			catch (Exception ex)
			{
				var inner = ex.InnerException as SqlException;				
				if (inner != null)
				{
					return new AjaxResultModel("Ошибка сохранения. Обратитесь к администратору.");
				}

				throw;
			}

			return new AjaxResultModel();
		}
	}
}