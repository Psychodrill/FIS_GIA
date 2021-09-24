using System;
using System.Data.SqlClient;
using System.Linq;
using GVUZ.Helper;
using GVUZ.Model.Entrants;
using GVUZ.Web.ViewModels.Administration.Catalogs;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Творческие направления
	/// </summary>
	public static class EntranceTestCreativeDirectionExtensions
	{
		/// <summary>
		/// Создание творческого направления
		/// </summary>
		public static AjaxResultModel CreateEnranceTestCreativeDirection(this EntrantsEntities dbContext, AddCreativeDirectionViewModel model)
		{
			if (String.IsNullOrWhiteSpace(model.Name))
				return new AjaxResultModel().SetIsError("Name", "Необходимо ввести название направления подготовки.");

			var spaceIndex = model.Name.IndexOf(' ');
			if (spaceIndex == -1)
				return new AjaxResultModel().SetIsError("directionNameTrick",
														"Некорректно введено название направления (используйте значения из выпадающего списка).");

			var code = model.Name.Substring(0, spaceIndex);
			var name = model.Name.Substring(spaceIndex + 1);			
			
			Direction direction = dbContext.Direction
				.Where(x => x.Code == code || x.Name == name).FirstOrDefault();							

			if (direction == null)
				return new AjaxResultModel().SetIsError("Name", "Введенное направление подготовки не найдено (используйте значения из выпадающего списка).");

			var testCreativeDirection = new EntranceTestCreativeDirection { Direction = direction };
			dbContext.EntranceTestCreativeDirection.AddObject(testCreativeDirection);

			try
			{
				dbContext.SaveChanges();
			}
			catch (Exception ex)
			{
				var inner = ex.InnerException as SqlException;
				if (inner != null && inner.Message.Contains("PK_EntranceTestCreativeDirections"))
				{					
					return new AjaxResultModel().SetIsError("Name", "Данное направление подготовки уже добавлено.");
				}

				throw;
			}

			return new AjaxResultModel
			{
				Data = new DirectionData
				{
					DirectionID = direction.DirectionID,
					Name = direction.Name,
					Code = direction.Code
				}
			};
		}

		/// <summary>
		/// Загрузка модели
		/// </summary>
		public static AddCreativeDirectionViewModel InitEntranceTestCreativeDirection(this EntrantsEntities dbContext, AddCreativeDirectionViewModel model)
		{			
			model.FillData(dbContext);
			return model;
		}

		/// <summary>
		/// Удаление творческого направления
		/// </summary>
		public static AjaxResultModel DeleteEnranceTestCreativeDirection(this EntrantsEntities dbContext, int? directionID)
		{
			var direction = dbContext.EntranceTestCreativeDirection.First(x => x.DirectionID == directionID);

			if (direction == null)
				return new AjaxResultModel("Направление подготовки не найдено.");

			dbContext.EntranceTestCreativeDirection.DeleteObject(direction);

			try
			{
				dbContext.SaveChanges();
			}
			catch (Exception ex)
			{
				var inner = ex.InnerException as SqlException;				
				if (inner != null)
				{
					return new AjaxResultModel("Невозможно удалить направление подготовки");
				}

				throw;
			}

			return new AjaxResultModel();
		}
	}
}