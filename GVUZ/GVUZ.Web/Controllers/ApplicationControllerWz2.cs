using System;
using System.Web.Mvc;
using GVUZ.Helper;
using System.Data.SqlClient;
using GVUZ.Web.ContextExtensionsSQL;
using GVUZ.Data.Helpers;
using System.Collections.Generic;
using GVUZ.Web.ViewModels;
using GVUZ.Web.Helpers;
using System.IO;
using System.Configuration;
using System.Text;
using GVUZ.DAL.Dapper.Repository.Model;
using FogSoft.Helpers;

namespace GVUZ.Web.Controllers
{

    public partial class ApplicationController : BaseController
    {

        #region Wz3
        // GET: /Application/
        public ActionResult Wz3(int? id = 0)
        {
            if (!id.HasValue) { return new EmptyResult(); }

            try
            {
                var Wz3 = ApplicationSQL.GetApplicationWz3(id.Value, false);
                Wz3.IsFromKrym = ApplicationSQL.CheckFromKrym(Wz3.ApplicationID);
                return PartialView("ApplicationWz3", Wz3);
            }
            catch (Exception ex)
            {
                return new AjaxResultModel(ex.ToString());
            }
        }

        public ActionResult AppEntTestDocGet(int? EntTestItemId, int? ApplicationId)
        {
            try
            {
                if (EntTestItemId == null || ApplicationId == null) { return new AjaxResultModel("Не задары параметры запроса!"); }
                //Модифицирцем d проставляем d.ID и снова возвращае 
                EntranceTestItem EntTestItem = ApplicationSQL.AppEntTestItemDocGet(ApplicationId.Value, EntTestItemId, null);
                return new AjaxResultModel { Data = EntTestItem };
            }
            catch (Exception ex)
            {
                return new AjaxResultModel(ex.ToString());
            }
        }

        public ActionResult AppEntTestDocCommonSave(EntrantTestItemDocument d)
        {
            try
            {
                //Модифицирцем d проставляем d.ID и снова возвращае 
                ApplicationSQL.AppEntTestDocCommonSave(d);
                return new AjaxResultModel { Data = d };
            }
            catch (Exception ex)
            {
                return new AjaxResultModel(ex.ToString());
            }
        }
        public ActionResult AppEntTestDocCommonDel(int? aetdID)
        {
            if (aetdID == null) { return new AjaxResultModel("Не задары параметры запроса!"); }
            try
            {
                ApplicationSQL.AppEntTestDocCommonDel(aetdID.Value);
                return new AjaxResultModel { Data = "OK" };
            }
            catch (Exception ex)
            {
                return new AjaxResultModel(ex.ToString());
            }
        }
        public ActionResult AppEntTestDocSave(EntrantTestItemDocument d)
        {
            if (d == null) { return new AjaxResultModel("Не задары параметры запроса!"); }

            if (d.InstitutionDocumentTypeID != null)
            {
                if (d.InstitutionDocumentTypeID > 0)
                {
                    if ((d.InstitutionDocumentDate.HasValue ? d.InstitutionDocumentDate.Value.Year : 0) > DateTime.Now.Year)
                    {
                        return new AjaxResultModel { Extra = "DateError" };
                    }
                    if ((d.InstitutionDocumentDate.HasValue ? d.InstitutionDocumentDate.Value.Year : 0) == DateTime.Now.Year)
                    {
                        if ((d.InstitutionDocumentDate.HasValue ? d.InstitutionDocumentDate.Value.Month : 0) >
                            DateTime.Now.Month)
                        {
                            return new AjaxResultModel { Extra = "DateError" };
                        }
                        if ((d.InstitutionDocumentDate.HasValue ? d.InstitutionDocumentDate.Value.Month : 0) ==
                            DateTime.Now.Month)
                        {
                            if ((d.InstitutionDocumentDate.HasValue ? d.InstitutionDocumentDate.Value.Day : 0) >
                                DateTime.Now.Day)
                            {
                                return new AjaxResultModel { Extra = "DateError" };
                            }
                        }
                    }
                    if ((d.InstitutionDocumentDate.HasValue ? d.InstitutionDocumentDate.Value.Year : 0) <=
                        DateTime.Now.Year - 100)
                    {
                        return new AjaxResultModel { Extra = "DateError" };
                    }
                }
            }
            try
            {       //Модифицирцем d проставляем d.ID и снова возвращае 
                if (d.SourceID == 3)
                {
                    // Проверить по базе ФБС, заполнить @HasEge, @EgeResultValue
                }
                ApplicationSQL.AppEntTestDocSave(d);
                var I = ApplicationSQL.AppEntTestItemDocGet(d.ApplicationID.Value, d.EntranceTestItemID, d.ID);
                return new AjaxResultModel { Data = I };
            }
            catch (Exception ex)
            {
                return new AjaxResultModel(ex.ToString());
            }
        }
        public ActionResult AppEntTestDocDel(int? aetdID)
        {
            if (aetdID == null)
            {
                return new AjaxResultModel("Не задары параметры запроса!");
            }
            try
            {
                EntranceTestItem I = ApplicationSQL.AppEntTestDocDel(aetdID.Value);
                return new AjaxResultModel { Data = I };
            }
            catch (Exception ex)
            {
                return new AjaxResultModel(ex.ToString());
            }
        }
        public ActionResult AppEntTestDocValueSave(EntrantTestItemDocument d)
        {
            try
            {
                //Модифицирцем d проставляем d.ID и снова возвращае 
                ApplicationSQL.AppEntTestDocValueSave(d);
                var I = ApplicationSQL.AppEntTestItemDocGet(d.ApplicationID.Value, d.EntranceTestItemID, d.ID);
                return new AjaxResultModel { Data = I };
            }
            catch (Exception ex)
            {
                return new AjaxResultModel(ex.ToString());
            }
        }
        public ActionResult AppEntTestDocEgeValueSave(EntrantTestItemDocument d)
        {
            try
            {
                //Модифицирцем d проставляем d.ID и снова возвращае 

                //var I=ApplicationSQL.AppEntTestItemDocGet(d.EntranceTestItemID.Value, d.ApplicationID.Value);
                return new AjaxResultModel { Data = ApplicationSQL.AppEntTestDocEgeValueSave(d) };
            }
            catch (Exception ex)
            {
                return new AjaxResultModel(ex.ToString());
            }
        }
        // SelectEntDocsList
        //int? applicationID, int? entranceTestItemID, int? docSourceID, 
        public ActionResult SelectEntDocsList(int? ApplicationID, int? EntranceTestItemID, int? SourceID, int? GroupID)
        {
            try
            {
                if (ApplicationID == null || SourceID == null || GroupID == null)
                {
                    return new AjaxResultModel("Не заданы параметры запроса!");
                }
                //Модифицируем d проставляем d.ID и снова возвращае 
                var ds = ApplicationSQL.SelectEntDocsList(ApplicationID.Value, EntranceTestItemID, SourceID.Value, GroupID.Value);
                return new AjaxResultModel { Data = ds };
            }
            catch (Exception ex)
            {
                return new AjaxResultModel(ex.ToString());
            }
        }



        //public ActionResult AppEntTestDocEGESave(EntrantTestItemDocument d) {
        //   try {
        //      //Модифицирцем d проставляем d.ID и снова возвращае 
        //      ApplicationSQL.AppEntTestDocEGESave(d);
        //      var I=ApplicationSQL.AppEntTestItemDocGet(ApplicationId.Value, d.EntranceTestItemID, d.ID);
        //      return new AjaxResultModel { Data=I };
        //   } catch(Exception ex) {
        //      return new AjaxResultModel(ex.ToString());
        //   }
        //}

        #endregion

        [HttpPost]
        public ActionResult GetComposition(int? ApplicationID)
        {
            try
            {
                var res = ApplicationSQL.GetCompositionResults(ApplicationID.Value);
                var CompositionResult = ApplicationSQL.GetComposition(ApplicationID.Value);
                return new AjaxResultModel { Data = CompositionResult, Extra = res };
            }
            catch
            {
                return new AjaxResultModel("Превышено время ожидания");
            }
        }

        [Authorize]
        public ActionResult ViewComposition(int? id)
        {
            if (!id.HasValue)
                return new AjaxResultModel { };

            var model = new CompositionViewModel() { ApplicationId = id.Value };

            string CompositionOldDrive = ConfigurationManager.AppSettings["CompositionOldDrive"];
            string CompositionNewDrive = ConfigurationManager.AppSettings["CompositionNewDrive"];

            var i = 1;
            foreach (var item in ApplicationSQL.GetComposition(id.Value))
            {
                if (item.CompositionPaths != null)
                
                foreach (var p in item.CompositionPaths.Split(';'))
                    {
                        var s = p.Trim();
                        if (CompositionNewDrive != null && CompositionNewDrive != "") { 
                            s = s.Replace(CompositionOldDrive, CompositionNewDrive);
                        }
                        if (s != "")
                        {
                            model.CompositionPages.Add(new CompositionPage
                            {
                                Id = i++,
                                Path = Convert.ToBase64String(Encoding.UTF8.GetBytes(s))
                            });
                        }
                    }
            }

            return View("ApplicationViewComposition", model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult LoadComposition(string path)
        {
            if (path == null)
                return new AjaxResultModel { IsError = true, Data = "Файл не найден" };

            try
            {
                string user = ConfigurationManager.AppSettings["CompositionUser"];
                string password = ConfigurationManager.AppSettings["CompositionPassword"];
                // попробую увеличить GetFileTryCount так как в массовой загрузки грузит только на 7 раз
                int GetFileTryCount = 10; // FogSoft.Helpers.AppSettings.Get("Composition.GetFileTryCount", 3);
                int GetFileTryDelay = 2; // FogSoft.Helpers.AppSettings.Get("Composition.GetFileTryDelay", 2);

                // ?
                var p = Encoding.UTF8.GetString(Convert.FromBase64String(path));

                string file = string.Empty;
                //var nio = new NetworkIO(user, password);
                int tryNumber = 1;
                while (tryNumber <= GetFileTryCount)
                {
                    try
                    {
                        file = new NetworkIO("", "").ReadImageBytes(p);
                        LogHelper.Log.Debug(string.Format("Успешный факт анонимного получения бланка сочинения: {0}", p));
                        break;
                    }
                    catch (IOException ioEx)
                    {
                        LogHelper.Log.Error(string.Format("Ошибка анонимного получения бланка сочинения: {0},  попытка: {1}", p, tryNumber), ioEx);
                        try
                        {
                            file = new NetworkIO(user, password).ReadImageBytes(p);
                            LogHelper.Log.Debug(string.Format("Успешный факт анонимного получения бланка сочинения: {0}", p));
                            break;
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Log.Error(string.Format("Ошибка авторизованного получения бланка сочинения: {0},  попытка: {1}", p, tryNumber), ex);
                            try
                            {
                                file = new NetworkIO(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()).ReadImageBytes(p);
                            }
                            catch (Exception iEx)
                            {
                                LogHelper.Log.Error(string.Format("Ошибка криво-авторизованного получения бланка сочинения: {0},  попытка: {1}", p, tryNumber), iEx);
                                //throw iEx;
                            }
                        }
                    }
                    finally
                    {
                        LogHelper.Log.Error(string.Format("Ошибка получения бланка сочинения: {0},  попытка: {1}", p, tryNumber));
                        tryNumber++;
                        if (tryNumber >= GetFileTryCount)
                        {
                            throw new Exception("Ошибка доступа к хранилищу бланков сочинений!\n Пожалуйста, попробуйте позднее!");
                        }

                        System.Threading.Thread.Sleep(1000 * GetFileTryDelay);
                    }
                }


                return new AjaxResultModel { Data = file };
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += " " + ex.InnerException.Message;
                }
                return new AjaxResultModel { IsError = true, Data = message };
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult GetDocInfo(int? docId)
        {
            if (docId == null)
                return new AjaxResultModel { IsError = true, Data = "Не заданы параметры запроса!" };

            try
            {
                return new AjaxResultModel { Data = new ApplicationRepository().GetDocInfo(docId.Value).Info };
            }
            catch (Exception ex)
            {
                return new AjaxResultModel { IsError = true, Data = ex.Message };
            }
        }

    }
}