using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Classes;
using static WebApplication1.AppLogic.InstitutionProcessor;

namespace WebApplication1.Controllers
{
    public class SearchController : Controller
    {

        public ActionResult Search()
        {
            ViewBag.InstitutionInfo = new List<InstitutionFullInfo<DateTime>>();
            InstitutionViewModel vmm = new InstitutionViewModel();
            return View(vmm);
        }

        public ActionResult Results(InstitutionViewModel model, int id = 0)

        {
            var institutionId = model.InstitutionID;

            if (id != 0)
                institutionId = id;

            var institutionName = model.FullName;

            var data = SearchInstitution(institutionId, institutionName);

            List<InstitutionViewModel> insts = new List<InstitutionViewModel>();

            foreach (var row in data)
            {
                insts.Add(new InstitutionViewModel
                {
                    InstitutionID = row.InstitutionID,
                    FullName = row.FullName,
                    BriefName = row.BriefName
                });
            }

            ViewBag.InstitutionInfo = insts;

            return View("Search");
        }
       
    }
}