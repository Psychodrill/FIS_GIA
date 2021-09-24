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
using Admin.Models.DBContext;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;

namespace Admin.Controllers
{
    [Authorize]
    public class InstitutionController : Controller
    {
        
        private readonly ApplicationContext db;
        public InstitutionController(ApplicationContext context)
        {
            db = context;
        }
        
        public IActionResult Index ()
        {            
            return PartialView();
        }

        public IActionResult EditInstitutionData()
        {
            return PartialView();
        }

        public IActionResult SearchInstitutions(string search, string mode)
        {
        int search_mode = (!String.IsNullOrEmpty(mode)) ? Convert.ToInt32(mode) : 1;
        List<InstitutionDetail> list = new List<InstitutionDetail>();
            list = CommonRepository.SearchInstitutions(search.Trim(),db);
            ViewBag.Institution = list;
            if(search_mode == 2)
            {
                return PartialView("SearchInstitutionForYear");
            }
            return PartialView();
        }
        
         public IActionResult LoadInstitutionDetails(string Id)
         {
            int InstitutionId = (!String.IsNullOrEmpty(Id)) ? Convert.ToInt32(Id) : 0;
            Institution institution = InstitutionRepository.GetInstitutesDetails(InstitutionId, db);

            if (institution == null)
            {
                return Content("Не найдены данные по ОО");
            }
            ViewBag.License = InstitutionRepository.GetLicense(InstitutionId, db);
            ViewBag.Accreditation = InstitutionRepository.GetAccreditation(InstitutionId, db);

            return PartialView(institution);
         }

        public IActionResult SaveInstitutionDetails(string InstitutionId, string LicenseNumber, string LicenseDate, string Accreditation)
        {
            //Обновление лицензии
            DateTime licenseDate = Convert.ToDateTime(LicenseDate);
            int institutionId = Convert.ToInt32(InstitutionId);
            string license_result = InstitutionRepository.SaveLicense(institutionId, LicenseNumber, licenseDate, db);
            //Обновление аккредитации
            string accreditation_result = InstitutionRepository.SaveAccreditation(institutionId, Accreditation, db);

            return Content(license_result + "</br>" + accreditation_result);
        }

        public IActionResult UploadLicense (IFormFile upload, int InstitutionId)
        {
            string ret = "";
            InstitutionLicense license = InstitutionRepository.GetLicense(InstitutionId, db); 
            InstitutionAttachment attachment = InstitutionRepository.GetAttachment(upload);
            attachment.InstitutionLicense = license;
            db.InstitutionAttachment.Add(attachment);
            //license.InstitutionAttachment = attachment;
            db.SaveChanges();

            return Content(ret);
        }

        // public IActionResult UploadAccreditation (IFormFile upload)
        // {
        //     string ret = "";
        //     string upload_result = InstitutionRepository.UploadFile(upload, db);

        //     return Content(ret);
        // }

         public IActionResult DelBadDocs()
         { 
            return PartialView(); 
         }
        
         public IActionResult LoadInstitutionYear(string Id, string Name)
        {
            int InstitutionId = (!String.IsNullOrEmpty(Id)) ? Convert.ToInt32(Id) : 0;
            List<string> years = new List<string>();
            years = CommonRepository.GetYears(InstitutionId, db);
            ViewBag.Years = years;
            ViewBag.Id = Id;
            ViewBag.Name = Name;

            return PartialView();
        }

        public IActionResult LoadInstitutionDocs(string Id, string Name, string Year)
        {
            int InstitutionId = (!String.IsNullOrEmpty(Id)) ? Convert.ToInt32(Id) : 0;
            string InstitutionName = (!String.IsNullOrEmpty(Name)) ? Name : "";
            int InstitutionYear = (!String.IsNullOrEmpty(Year)) ? Convert.ToInt32(Year) : 0;

            List<InstitutionDocs> documents = new List<InstitutionDocs>();
            documents = InstitutionRepository.GetDocuments(InstitutionId, InstitutionYear, db);
            ViewBag.Documents = documents;

            return PartialView();
        }

        public IActionResult InstitutionDocRemove(string Id)
        {


            return Content("");
        }


    }
}
