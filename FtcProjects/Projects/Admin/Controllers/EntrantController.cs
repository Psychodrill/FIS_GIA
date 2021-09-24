using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Admin.DBContext;
using Admin.Models;
using Admin.Data;
using Microsoft.AspNetCore.Authorization;

namespace Admin.Controllers
{
    [Authorize]
    public class EntrantController : Controller
    {
        private readonly ApplicationContext db;
        public EntrantController(ApplicationContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return PartialView();
        }

        public IActionResult Dublicates()
        {
            return PartialView();
        }

        public IActionResult SearchInstitutions(string search)
        {
            List<InstitutionDetail> list = new List<InstitutionDetail>();
            list = CommonRepository.SearchInstitutions(search.Trim(), db);
            ViewBag.Institution = list;

            return PartialView();
        }

        public ActionResult LoadInvalidEntrants(string Id, string searchString, int page = 1)
        {
            int pageSize = 15;
            int InstitutionId = (!String.IsNullOrEmpty(Id)) ? Convert.ToInt32(Id) : 0;

            List<EntrantViewModel> entrants = new List<EntrantViewModel>();
            entrants = EntrantRepository.GetEntrants(InstitutionId, db);

            if (!String.IsNullOrEmpty(searchString))
            {
                List<EntrantViewModel> searchEntrants = new List<EntrantViewModel>();
                foreach(var item in entrants)
                {
                    if(item.Entrant.LastName.Contains(searchString, System.StringComparison.CurrentCultureIgnoreCase))
                    searchEntrants.Add(item);
                }
                entrants = searchEntrants;
            }

            var count = entrants.Count();
            var items = entrants.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                EntrantViewModel = items
            };
            ViewBag.instID = InstitutionId;
            ViewBag.itemCount = count;

            return PartialView(viewModel);
        }

        public IActionResult EntrantRemove(string EntrantId)
        {
            try
            {
                int EntrID = (!String.IsNullOrEmpty(EntrantId)) ? Convert.ToInt32(EntrantId) : 0;
                if (EntrID > 0)
                {
                    EntrantRepository.EntrantRemove(EntrID);
                }
            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
            }
            return Content("Абитуриент удалён");
        }

    }
}