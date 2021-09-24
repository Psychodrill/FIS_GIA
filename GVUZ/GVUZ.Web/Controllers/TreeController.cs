using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using GVUZ.Model.Institutions;
using GVUZ.Web.Helpers;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels;

namespace GVUZ.Web.Controllers
{
	[AuthorizeAdm(Roles = UserRole.EduAdmin)]
	public class TreeController : Controller
	{
		public ActionResult Index()
		{
			return View("Index");
		}

		public ActionResult SendPage1()
		{
			return JavaScript("<b>Hello,world1</b>");
		}

		public ActionResult SendPage2()
		{
			return JavaScript("<b>Hello,world2</b>");
		}

		public ActionResult TreeStructure(int? structureItemID)
		{
			using (var context = new InstitutionsEntities())
			{
				// если запрашивается идентификатор 0, то возвращаем рутовый элемент
				if (structureItemID == 0)
				{
					var rootStructure = context.InstitutionStructure
						.Include(s => s.Children)
						.Include(s => s.InstitutionItem)
						.Include(s => s.InstitutionItem.Institution)
						.First(x => x.InstitutionItem.InstitutionID == 2);

					return Json(new ClickableTreeItemViewModel
					{
						ItemID = rootStructure.InstitutionStructureID,
						Name = rootStructure.InstitutionItem.Institution.FullName,
						IsLeaf = rootStructure.Children.Count == 0,
						CanClick = true
					});
				}

				var instItems = context.InstitutionStructure.Where(
					x =>
						// open tree structure
						structureItemID > 0 && x.ParentID == structureItemID)
					.OrderBy(x => x.InstitutionItem.Name)
					.Select(x => new ClickableTreeItemViewModel
					{
						ItemID = x.InstitutionStructureID,
						Name = x.InstitutionItem.Name,
						IsLeaf = x.Children.Count == 0,
					});

				// [ { "ItemID" : "1", Name :  }, { "ItemID} ]

				return Json(instItems.ToArray());
			}
		}

		// по факту не используется
		public ActionResult Test()
		{
			return null;
		}
	}
}
