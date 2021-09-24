using System;
using System.Data.SqlClient;
using System.Linq;
using GVUZ.Helper;
using GVUZ.Model.Entrants;
using GVUZ.Web.ViewModels.Administration.Catalogs;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Методы для работы с квалификациями (справочники)
	/// </summary>
	public static class QualificationsExtensions
	{
		/// <summary>
		/// Создать новую квалификацию
		/// </summary>
		public static AjaxResultModel CreateQualification(this EntrantsEntities dbContext, AddQualificationViewModel model)
		{
			bool isEdit = model.QualificationID > 0;
            //QualificationType qualification;
            //if (isEdit)
            //{
            //    qualification = dbContext.QualificationType
            //        .Where(x => x.QualificationID == model.QualificationID).FirstOrDefault();
            //    if (qualification == null)
            //        return new AjaxResultModel("Квалификация не найдена");
            //}
            //else
            //    qualification = new QualificationType();

            //if (String.IsNullOrWhiteSpace(model.Name))
            //    return new AjaxResultModel("Необходимо ввести название квалификации/степени");

            //qualification.Name = model.Name;
            //qualification.Code = model.Code;
			
            //if (!isEdit)
            //    dbContext.QualificationType.AddObject(qualification);

			try
			{
				dbContext.SaveChanges();
			}
			catch (Exception ex)
			{
				var inner = ex.InnerException as SqlException;
				if (inner != null && inner.Message.Contains("UK_QualificationType_Code"))
				{					
					return new AjaxResultModel().SetIsError("Code", "Уже добалена квалификация/степень с указанным кодом");
				}

				throw;
			}

			return new AjaxResultModel
			{
				Data = new QualificationsViewModel.QualificationData
				{
                    //QualificationID = qualification.QualificationID,
                    //Name = qualification.Name,
                    //Code = qualification.Code
				}
			};
		}

		/// <summary>
		/// Загрузить квалификацию
		/// </summary>
		public static AddQualificationViewModel LoadQualification(this EntrantsEntities dbContext, AddQualificationViewModel model)
		{
            //var qualification = dbContext.QualificationType.First(x => x.QualificationID == model.QualificationID);

            //model.QualificationID = qualification.QualificationID;
            //model.Name = qualification.Name;
            //model.Code = qualification.Code;			

			return model;
		}

		/// <summary>
		/// Удалить квалификацию
		/// </summary>
		public static AjaxResultModel DeleteQualification(this EntrantsEntities dbContext, int? qualificationID)
		{
            //var qualification = dbContext.QualificationType.First(x => x.QualificationID == qualificationID);

            //if (qualification == null)
            //    return new AjaxResultModel("Квалификация/степень не найдена");

            //dbContext.QualificationType.DeleteObject(qualification);

			try
			{
				dbContext.SaveChanges();
			}
			catch (Exception ex)
			{
				var inner = ex.InnerException as SqlException;
				if (inner != null)
				{
					return new AjaxResultModel(inner.Message);
				}

				throw;
			}

			return new AjaxResultModel();
		}
	}
}