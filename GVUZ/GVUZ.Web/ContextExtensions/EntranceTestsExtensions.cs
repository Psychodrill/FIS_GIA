using GVUZ.Helper;
using GVUZ.Model.Entrants;
using GVUZ.Web.ViewModels.Administration.Catalogs;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// ВИ для администрирования. Не доделано (скорее всего и не нужно никогда). Косвенное использование в админке. Поэтому класс оставлен
	/// </summary>
	public static class EntranceTestsExtensions
	{
		public static AjaxResultModel CreateTest(this EntrantsEntities dbContext, AddTestViewModel model)
		{			
			return new AjaxResultModel();
		}

		public static AddTestViewModel LoadTest(this EntrantsEntities dbContext, AddTestViewModel model)
		{			
			return model;
		}

		public static AddTestViewModel InitTest(this EntrantsEntities dbContext, AddTestViewModel model)
		{
			model.FillData(dbContext);
			return model;
		}

		public static AjaxResultModel DeleteTest(this EntrantsEntities dbContext, int id)
		{
			return new AjaxResultModel("Не поддерживается.");

			//var link = dbContext.DirectionSubjectLink
			//    .Where(x => x.ID== id).First();

			//if (link == null)
			//    return new AjaxResultModel("Ошибка удаления.");

			//dbContext.DirectionSubjectLink.DeleteObject(link);

			//try
			//{
			//    dbContext.SaveChanges();
			//}
			//catch (Exception ex)
			//{
			//    var inner = ex.InnerException as SqlException;
				
			//    if (inner != null)
			//    {
			//        return new AjaxResultModel(inner.Message);
			//    }

			//    throw;
			//}

			//return new AjaxResultModel();
		}
	}
}