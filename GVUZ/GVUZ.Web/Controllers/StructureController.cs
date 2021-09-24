using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using FogSoft.Helpers;
using GVUZ.Helper;
using GVUZ.Helper.MVC;
using GVUZ.Model.Institutions;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.Helpers;
using GVUZ.Web.Portlets;
using GVUZ.Web.Portlets.Searches;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels;
using GVUZ.Model.Entrants;
using Microsoft.Practices.ServiceLocation;
using GVUZ.DAL.Dapper.Repository.Model.Structure;
using GVUZ.DAL.Dapper.Repository.Interfaces.Structure;
using GVUZ.Model.Helpers;

namespace GVUZ.Web.Controllers
{
	[AuthorizeAdm(Roles = UserRole.EduUser)]
	[MenuSection("Institution")]
	public class StructureController : BaseController
	{
		[GeneratorPortletLink(typeof(PortletLinkHelper), "InstitutionTabLink", MethodParams = new object[] { PortletType.InstitutionStructureTab, 1 })]
		public ActionResult Index()
		{
			return View("Index");
		}

        IStructureRepository structureRepository;
        public StructureController()
        {
            this.structureRepository = new StructuresRepository();
        }

        /// <summary>
        /// Установка нового идентификатора ОО в приложении при соответствующем переключении на вкладке струтуры ОО
        /// </summary>
        /// <param name="institutionId">Идентификатор ОО</param>
        [Authorize]
        public void SetInstitution(int? institutionId)
        {
            if (institutionId != null)
             InstitutionHelper.SetInstitutionID((int)institutionId);
        }

        /// <summary>
        /// Поличить идентификаторы всех ОО, являющихся филиалами данного ОО
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFilials()
        {
            int institutionID = InstitutionHelper.MainInstitutionID;
            string userName = UserHelper.GetAuthenticatedUserName();

            DAL.Dto.StructureInfoDto data = new DAL.Dto.StructureInfoDto();

            //var key = string.Format("TreeStructureFilials_{0}", institutionID);
            //var cache = ServiceLocator.Current.GetInstance<ICache>();
            var model = structureRepository.GetStructure(InstitutionID, userName);

            //cache.Insert(key, model);
            return new AjaxResultModel { Data = model };

            //            var filials = new int[0];
            //            int institutionID = InstitutionHelper.MainInstitutionID;

            //            var key = string.Format("TreeStructureFilials_{0}", institutionID);
            //            var cache = ServiceLocator.Current.GetInstance<ICache>();
            //            var model = cache.Get<AjaxResultModel>(key, null);
            //#if DEBUG
            //            model = null;
            //#endif
            //            if (model != null)
            //                return model;

            //            using (EntrantsEntities context = new EntrantsEntities())
            //            {
            //                int? esrpOrgID = context.Institution.Where(x => x.InstitutionID == institutionID).Select(y => y.EsrpOrgID).FirstOrDefault();
            //                if ((esrpOrgID != null) && (esrpOrgID != 0))
            //                {
            //                    filials = context.Institution.Where(x => x.MainEsrpOrgId == esrpOrgID && x.StatusId == 1).Select(x => x.InstitutionID).ToArray();
            //                }
            //            }

            //            model = new AjaxResultModel { Data = filials };
            //            cache.Insert(key, model);
            //            return model;
        }

		/// <summary>
		/// Получить детей для указанного узла. Второй параметр обозначает ОО, для которого строится дерево ()
		/// </summary>

  //      public ActionResult TreeStructure(int? structureItemID, int? institutionId)
		//{
		//    var key = string.Format("TreeStructure_{0}_{1}_{2}", 
  //              InstitutionHelper.MainInstitutionID,
  //              institutionId ?? -1000, structureItemID ?? -1000);
  //          var cache = ServiceLocator.Current.GetInstance<ICache>();
  //          var model = cache.Get<ActionResult>(key, null);
  //          if (model != null)
  //              return model;

		//	bool isReadonly = false;
		//	if (HttpContext != null && HttpContext.User != null) //не проверяем в случае внешних источников (тесты, портлеты)
  //              isReadonly = GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.InstitutionDataDirection);
		//	using (var context = new InstitutionsEntities())
		//	{
		//		// если запрашивается идентификатор -1, то возвращаем объект JsonInstitutionSearchResult
		//		if (structureItemID == -1)
		//		{
		//			var result = new JsonInstitutionSearchResult();

		//			var itemsStructure = context.InstitutionStructure
		//				.Include(x => x.InstitutionItem)
		//				.Include(x => x.Children)
  //                      .Where(x => x.InstitutionItem.InstitutionID == (institutionId == null ? InstitutionID : institutionId))
		//				.OrderBy(x => x.InstitutionItem.Name)
		//				.Select(x => new
		//				{
		//					TreeItemViewModel = new TreeItemViewModel
		//					{
		//						ItemID = x.InstitutionStructureID,
		//						Name = x.InstitutionItem.Name,
		//						IsLeaf = x.Children.Count == 0,
		//						CanAdd = x.InstitutionItem.ItemTypeID != (int)(InstitutionItemType.Direction) && !isReadonly,
		//						CanDelete = !isReadonly,
		//						CanEdit = !isReadonly,
		//					},
		//					Items = x.Children
		//				});

		//			foreach (var structureItem in itemsStructure)
		//			{
		//				result.Objects.Add(structureItem.TreeItemViewModel.ItemID.ToString(), 
		//					structureItem.TreeItemViewModel);
		//				if (structureItem.Items.Count > 0)
		//					result.Children.Add(structureItem.TreeItemViewModel.ItemID.ToString(), 
		//						structureItem.Items.Select(x => x.InstitutionStructureID).ToArray());
		//			}

		//			/*foreach (var child in children)
		//			{
		//				result.Children.Add(child.ItemID.ToString(), child.Items.Select(x => x.InstitutionStructureID).ToArray());
		//			}*/

		//		    model = Json(result);
  //                  cache.Insert(key, model, Int32.MaxValue);
  //                  return model;
		//		}

		//		// если запрашивается идентификатор 0, то возвращаем рутовый элемент
		//		if (structureItemID == 0)
		//		{
		//			var rootStructure = context.InstitutionStructure
		//				.Include(s => s.Children)
		//				.Include(s => s.InstitutionItem)
		//				.Include(s => s.InstitutionItem.Institution)
  //                      .FirstOrDefault(x => x.InstitutionItem.InstitutionID == (institutionId ?? InstitutionID));
		//		    if (rootStructure == null)
		//		    {
  //                      model = Json(new TreeItemViewModel());
  //                      cache.Insert(key, model, Int32.MaxValue);
		//		        return model;
		//		    }

		//		    model = Json(new TreeItemViewModel
		//				{
		//					ItemID = rootStructure.InstitutionStructureID,
		//					Name = rootStructure.InstitutionItem.Institution.FullName,
		//					IsLeaf = rootStructure.Children.Count == 0,
		//					CanAdd = !isReadonly
		//				});
  //                  cache.Insert(key, model, Int32.MaxValue);
		//		    return model;
		//		}
				
		//		var instItems = context.InstitutionStructure.Where(
		//			x => 
		//				// open tree structure
		//				structureItemID > 0 && x.ParentID == structureItemID)
		//			.OrderBy(x => x.InstitutionItem.Name)
		//			.Select(x => new TreeItemViewModel
		//				{
		//					ItemID = x.InstitutionStructureID,
		//					Name = x.InstitutionItem.Name,
		//					IsLeaf = x.Children.Count == 0,
		//					CanAdd = x.InstitutionItem.ItemTypeID != (int)(InstitutionItemType.Direction) && !isReadonly,
		//					CanDelete = !isReadonly,
		//					CanEdit = !isReadonly,
		//				});

  //              model = Json(instItems.ToArray());
  //              cache.Insert(key, model, Int32.MaxValue);
		//		return model;
		//	}
		//}

	 //   private void ClearCache()
	 //   {
  //          int institutionID = InstitutionHelper.MainInstitutionID;
  //          var cache = ServiceLocator.Current.GetInstance<ICache>();
  //          cache.RemoveAllWithPrefix(new []
  //              {
  //                  string.Format("TreeStructureFilials_{0}", institutionID),
  //                  string.Format("TreeStructure_{0}_", institutionID)
  //              });
	 //   }

	 //   [HttpPost]
		//[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		//public ActionResult AddItem(int? structureItemID)
		//{
		//	using (InstitutionsEntities dbContext = new InstitutionsEntities())
		//	{
		//	    if (structureItemID.HasValue)
		//	    {
  //                  ClearCache();
		//	        return PartialView("Structure/AddItem", dbContext.FillInitialStructure(new AddStructureItemViewModel(structureItemID.Value)));
		//	    }
		//	}
		    
		//	return new ContentResult { Content = "No Data" };
		//}

		//[HttpPost]
		//[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		//public ActionResult EditItem(int? structureItemID)
		//{
		//	if (structureItemID.HasValue)
		//	{
		//		using (InstitutionsEntities dbContext = new InstitutionsEntities())
		//		{
		//			EditStructureItemViewModel model = new EditStructureItemViewModel(structureItemID.Value);
		//			dbContext.LoadStructure(model);
		//			return PartialView("Structure/AddItem", model);
		//		}
		//	}
            
		//	return new ContentResult { Content = "No Data" };
		//}

		//[HttpPost]
		//[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		//public ActionResult SaveItem(EditStructureItemViewModel model)
		//{
		//	if (!ModelState.IsValid)
		//		return new AjaxResultModel(ModelState);

		//	using (InstitutionsEntities dbContext = new InstitutionsEntities())
		//	{
  //              ClearCache();
		//		return dbContext.UpdateStructure(model);
		//	}
		//}

		//[HttpPost]
		//[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		//public ActionResult CreateItem(AddStructureItemViewModel model)
		//{
		//	if (!ModelState.IsValid)
		//		return new AjaxResultModel(ModelState);

		//	using (InstitutionsEntities dbContext = new InstitutionsEntities())
		//	{
  //              ClearCache();
		//		return dbContext.AddStructure(model);
		//	}
		//}
		
		//public ActionResult DeleteItem(int? structureItemID)
		//{
		//	using (var context = new InstitutionsEntities())
		//	{
  //              ClearCache();
		//		return Json(context.DeleteStructure(structureItemID));
		//	}
		//}
	}
}
