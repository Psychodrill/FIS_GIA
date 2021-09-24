using System;
using System.Linq;
using System.Web.Mvc;
using FogSoft.Helpers;
using GVUZ.Helper;
using GVUZ.Helper.ExternalValidation;
using GVUZ.Helper.MVC;
using GVUZ.Model;
using GVUZ.Model.Applications;
using GVUZ.Model.Cache;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.ContextExtensions;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Model.Helpers;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.Filters;
using GVUZ.Web.Helpers;
using GVUZ.Web.Models;
using GVUZ.Web.Portlets;
using GVUZ.Web.Portlets.Applications;
using GVUZ.Web.Portlets.Entrants;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels;
using Microsoft.Practices.ServiceLocation;
using Application = GVUZ.Model.Entrants.Application;
using System.Collections.Generic;
using GVUZ.Model.ApplicationPriorities;
using GVUZ.Model.RecommendedLists;
using GVUZ.Web.ContextExtensionsSQL;

namespace GVUZ.Web.Controllers {
    public partial class ApplicationController : BaseController {

        #region Wz2
        [Authorize]
        public ActionResult Wz2(int? id = 0) {
            if(!id.HasValue) { return new EmptyResult(); }
            var Wz2 = ApplicationSQL.GetApplicationWz2(id.Value);
            return PartialView("ApplicationWz2", Wz2);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Wz2Save(int ApplicationID, int Step, int cheksPage)
        {
            try
            {
                //#DocumentsCheck - тут была проверка документов, убрана (FIS-1711)
                ApplicationSQL.Wz2Save(ApplicationID, Step);
                return new AjaxResultModel();
            }
            catch (Exception ex)
            {
                return new AjaxResultModel(ex.ToString());
            }
        }
        #endregion 

        [Authorize]
        [HttpPost]
        public ActionResult Wz3Save(AppResultsModel model)
        {
            try
            {
                ApplicationSQL.Wz3Save(model);
                return new AjaxResultModel();
            }
            catch (Exception ex)
            {
                return new AjaxResultModel(ex.ToString());
            }
        }


    }
}