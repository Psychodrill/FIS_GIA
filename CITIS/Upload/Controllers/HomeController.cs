using System.Web.Mvc;
//using GVUZ.Web.Auth;
using System.Xml.Linq;
using GVUZ.Web.Auth;

namespace upload.Controllers
{
    public class HomeController : Controller
    {
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}

        [HttpGet]
        public ActionResult Test()
        {
            string UserName = "mesteruh@gmail.com";
            string Password = "7qCPfD";


            var checkResult = CheckEsrpAuth.CheckUserAccess(UserName, Password);


            if (checkResult == 0)
            {

                return Json("test complited successfull", JsonRequestBehavior.AllowGet);
            }
            else if (checkResult == -1)
            {
                return Json("Test fail", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Teste Else", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost, ValidateInput(false)]        
        public ActionResult Test2(string a)
        {

            string UserName= "mesteruh@gmail.com";
            string Password= "7qCPfD";

            var profilesXElementPar = XElement.Parse(a);

            var checkResult = CheckEsrpAuth.CheckUserAccess(UserName, Password);


            if (checkResult == 0)
            {

               return Json(profilesXElementPar.Element("composition").Value.ToString(), JsonRequestBehavior.AllowGet);
            }
            else if (checkResult == -1)
            {
                return Json("Test fail", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Teste Else", JsonRequestBehavior.AllowGet);
            }

            


            

        }
    }
}