using System;
using System.Data.SqlClient;
using System.Linq;
using GVUZ.Helper;
using GVUZ.Model.Entrants;
using GVUZ.Web.ViewModels.Administration.Catalogs;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Общеобразовательные предметы (справочники)
	/// </summary>
	public static class GeneralSubjectsExtesions
	{
		/// <summary>
		/// Создаём предмет
		/// </summary>
		public static AjaxResultModel CreateGeneralSubject(this EntrantsEntities dbContext, AddGeneralSubjectViewModel model)
		{
			bool isEdit = model.SubjectID > 0;
			Subject subject;
			if (isEdit)
			{
				subject = dbContext.Subject.FirstOrDefault(x => x.SubjectID == model.SubjectID);
				if (subject == null)
					return new AjaxResultModel("Общеобразовательный предмет не найден");
			}
			else
				subject = new Subject();

			if (String.IsNullOrWhiteSpace(model.Name))
				return new AjaxResultModel("Необходимо ввести название общеобразовательного предмета");

			subject.Name = model.Name;

			if (!isEdit)
				dbContext.Subject.AddObject(subject);

			try
			{
				dbContext.SaveChanges();
			}
			catch (Exception ex)
			{
				var inner = ex.InnerException as SqlException;
				if (inner != null && inner.Message.Contains("UK_Subject_Name"))
				{
					return new AjaxResultModel().SetIsError("Name", "Введенный общеобразовательный предмет уже добавлен");
				}

				throw;
			}

			return new AjaxResultModel
			       	{
			       		Data = new GeneralSubjectsViewModel.GeneralSubjectData
			       		       	{
			       		       		SubjectID = subject.SubjectID,
			       		       		Name = subject.Name
			       		       	}
			       	};
		}

		/// <summary>
		/// Загружаем предмет
		/// </summary>
		public static AddGeneralSubjectViewModel LoadGeneralSubject(this EntrantsEntities dbContext,
		                                                            AddGeneralSubjectViewModel model)
		{
			var subject = dbContext.Subject.First(x => x.SubjectID == model.SubjectID);

			model.SubjectID = subject.SubjectID;
			model.Name = subject.Name;

			return model;
		}

		/// <summary>
		/// Удаляем предмет
		/// </summary>
		public static AjaxResultModel DeleteGeneralSubject(this EntrantsEntities dbContext, int? subjectID)
		{
			var subject = dbContext.Subject.First(x => x.SubjectID == subjectID);

			if (subject == null)
				return new AjaxResultModel("Общеобразовательный предмет не найден");

			dbContext.Subject.DeleteObject(subject);

			try
			{
				dbContext.SaveChanges();
			}
			catch (Exception ex) //в зависимости от ошибок удаления, сообщаем человеческую ошибку
			{
				var inner = ex.InnerException as SqlException;
				if (inner != null && inner.Message.Contains("FK_OlympicTypeSubjectLink_Subject"))
				{
					return new AjaxResultModel("Невозможно удалить. Данный общеобразовательный предмет указан в олимпиаде.");
				}

				if (inner != null && inner.Message.Contains("FK_EntranceTestItem_Subject"))
				{
					return
						new AjaxResultModel("Невозможно удалить. Данный общеобразовательный предмет указан во вступительных испытаниях.");
				}

				if (inner != null && inner.Message.Contains("FK_DirectionSubjectLink_Subject"))
				{
					return
						new AjaxResultModel(
							"Невозможно удалить. Cуществуют направления подготовки, использующие данный предмет в качестве вступительного испытания.");
				}

				throw;
			}

			return new AjaxResultModel();
		}
	}
}