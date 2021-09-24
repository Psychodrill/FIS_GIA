using System;
using System.Data.SqlClient;
using System.Linq;
using GVUZ.Helper;
using GVUZ.Model.Entrants;
using GVUZ.Web.ViewModels.Administration.Catalogs;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Профильные направления
	/// </summary>
	public static class EntranceTestProfileDirectionExtensions
	{
		/// <summary>
		/// Сохраняем профильное направление
		/// </summary>
		public static AjaxResultModel CreateEntranceTestProfileDirection(this EntrantsEntities dbContext, AddProfileDirectionViewModel model)
		{
			if (String.IsNullOrWhiteSpace(model.Name))
				return new AjaxResultModel().SetIsError("Name", "Необходимо ввести название Вуза.");

			var institution = dbContext.Institution.Where(x => x.FullName == model.Name).FirstOrDefault();

			if (institution == null)
				return new AjaxResultModel().SetIsError("Name", "Указанный Вуз не найден.");

			var directionValidationResult = model.Validate();
			if (!String.IsNullOrWhiteSpace(directionValidationResult))
				return new AjaxResultModel().SetIsError("directionNameTrick", directionValidationResult);			

			foreach (var directionData in model.Directions)
			{
				var spaceIndex = directionData.Name.Trim().IndexOf(' ');
				if (spaceIndex == -1)
					return new AjaxResultModel().SetIsError("directionNameTrick",
					                                        "Некорректно введено название направления (используйте значения из выпадающего списка).");

				var code = directionData.Name.Substring(0, spaceIndex);
				var name = directionData.Name.Substring(spaceIndex + 1);
				var direction = dbContext.Direction.FirstOrDefault(x => x.Code == code || x.Name == name);
				if (direction == null)
				    return new AjaxResultModel().SetIsError("directionNameTrick",
				                                            "Одно из указанных направлений не найдено.");

				directionData.DirectionID = direction.DirectionID;
				directionData.Code = code;
				directionData.Name = name;
			}			
			
			if (model.IsEditMode)
			{				
				dbContext.EntranceTestProfileDirection
                    //.Where(x => x.InstitutionID == institution.InstitutionID)
                    .ToList().ForEach(
					dbContext.EntranceTestProfileDirection.DeleteObject);				
			}
			// create institute directions
			model.Directions.Select(directionData => 
				new EntranceTestProfileDirection
			        {
			           // InstitutionID = institution.InstitutionID, 
						DirectionID = directionData.DirectionID
					}).ToList().ForEach(dbContext.EntranceTestProfileDirection.AddObject);			

			try
			{
			    dbContext.SaveChanges();
			}
			catch (Exception ex)
			{
			    var inner = ex.InnerException as SqlException;
			    if (inner != null)
			    {
			        return new AjaxResultModel("Невозможно добавить направление");
			    }

			    throw;
			}

			return new AjaxResultModel
			{
			    Data = new ProfileDirectionViewModel.InstitutionProfileDirectionData
			    {
			        InstitutionID = institution.InstitutionID,
					Name = institution.FullName,
					Directions = model.Directions
			    }
			};			
		}

		/// <summary>
		/// Загружаем модель профильного направления по вузу
		/// </summary>
		public static AddProfileDirectionViewModel LoadProfileDirection(this EntrantsEntities dbContext, AddProfileDirectionViewModel model)
		{
			model = InitEntranceTestProfileDirection(dbContext, model);
			var institution = dbContext.Institution.First(x => x.InstitutionID == model.InstitutionID);			
						
			model.Name = institution.FullName;

			return model;
		}

		/// <summary>
		/// Начальная загрузка модели профильного направления
		/// </summary>
		public static AddProfileDirectionViewModel InitEntranceTestProfileDirection(this EntrantsEntities dbContext, AddProfileDirectionViewModel model)
		{
			model.FillData(dbContext);
			return model;
		}

		/// <summary>
		/// Удаляем профильное направление
		/// </summary>
		public static AjaxResultModel DeleteEnranceTestProfileDirection(this EntrantsEntities dbContext, int institutionID)
		{
            var directions = dbContext.EntranceTestProfileDirection;
				//.Where(x => x.InstitutionID == institutionID);

			if (!directions.Any())
				return new AjaxResultModel("Ошибка удаления.");

			foreach (var entranceTestProfileDirection in directions)
			{
				dbContext.EntranceTestProfileDirection.DeleteObject(entranceTestProfileDirection);
			}			

			try
			{
				dbContext.SaveChanges();
			}
			catch (Exception ex)
			{
				var inner = ex.InnerException as SqlException;
				if (inner != null)
				{
					return new AjaxResultModel("Невозможно удалить направление");
				}

				throw;
			}

			return new AjaxResultModel();
		}
	}
}