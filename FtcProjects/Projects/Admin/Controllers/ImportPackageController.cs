using Admin.DBContext;
using Admin.Models.DBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Controllers
{
    [Authorize]
    public class ImportPackageController : Controller
    {
        private ApplicationContext db { get; }

        public ImportPackageController(ApplicationContext _db)
        {
            db = _db;
        }


        public IActionResult Index()
        {
            return PartialView();
        }

        public IActionResult Search()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<ImportPackage> Search(long packageId)
        {
            ImportPackage package = await db.ImportPackage.Where(q => q.PackageId == packageId).FirstOrDefaultAsync();

            if (package == null)
                throw new Exception("Package id is required");

            return package;
        }

        [HttpPost]
        public async Task<ImportPackage> Save(ImportPackage package)
        {
            return db.ImportPackage.Update(package).Entity;
        }
    }
}
