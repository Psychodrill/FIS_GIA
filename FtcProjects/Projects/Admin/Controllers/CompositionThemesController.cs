using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.DBContext;
using Admin.Models;
using Admin.Models.DBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;

namespace Admin.Controllers
{
    [Authorize]
    public class CompositionThemesController : Controller
    {
        private readonly ApplicationContext db;
        public CompositionThemesController(ApplicationContext context)
        {
            db = context;
        }

        public ActionResult Index()
        {
            return PartialView();
        }

        
        public ActionResult AddFile()
        {
            return View(new CompositionFileViewModel());
        }
        
        [HttpPost]
        public ActionResult AddFile(IFormFile uploadFile)
        {           
            if (uploadFile != null)
            {
                var CmpThemesList = new List<CompositionFileViewModel>();
                Stream ss = uploadFile.OpenReadStream();
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                using (TextFieldParser parser = new TextFieldParser(ss, Encoding.GetEncoding(1251)))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(";");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        CmpThemesList.Add( new CompositionFileViewModel
                        {

                            Year = fields[0],
                            Code = fields[1],
                            Name = fields[2]

                        });
                    }
                    CmpThemesList = CmpThemesList.Skip(1).ToList();
                }
                return PartialView("ThemesList", CmpThemesList);
            };
            return PartialView("Index");   
            
        }

      
        [ValidateAntiForgeryToken]
        public ActionResult SaveFile(List<CompositionFileViewModel> model)
        {
            var dbModel = new List<CompositionThemes>();
            if (model != null) {

                foreach (var row in model)
                {
                    dbModel.Add(new CompositionThemes
                    {
                        Name = row.Name,
                        Year = int.Parse(row.Year),
                        ThemeID = int.Parse(row.Code)
                    });
                }
                db.CompositionThemes.AddRange(dbModel);
                db.SaveChanges();
            }
            return RedirectToAction("Index") ;
        }

    }
}
