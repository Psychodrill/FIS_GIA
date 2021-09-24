using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GVUZ.Model.RecommendedLists;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.ViewModels.RecommendedLists;
using GVUZ.Helper;
using FogSoft.Helpers;
using GVUZ.Web.ViewModels;
using System.IO;
using System.Xml;
using System.Text;

namespace GVUZ.Web.Controllers
{
    public class RecommendedListsController : BaseController
    {
        public ActionResult GetRecommendedList()
        {
            using (var dbContext = new RecommendedListsEntities())
            {
                return View("RecommendedLists", dbContext.FillList(InstitutionID, null, new RecommendedListsShowParametersViewModel() { PageToShow = 0, SortDirection = 1, Filter = null })); //Сортировка по умолчанию - по ПК
            }
        }

        public ActionResult SortRecommendedList(RecommendedListsShowParametersViewModel parameters)
        {
            using (var dbContext = new RecommendedListsEntities())
            {
                return new AjaxResultModel() { Data = dbContext.FillList(InstitutionID, null, parameters) };
            }
        }

        public ActionResult FilterRecommendedList(RecommendedListsShowParametersViewModel parameters)
        {
            using (var dbContext = new RecommendedListsEntities())
            {
                return new AjaxResultModel() { Data = dbContext.FillList(InstitutionID, null, parameters) };
            }
        }

        public ActionResult DeleteListElement(int? recListId)
        {
            if (!recListId.HasValue)
                return new AjaxResultModel("Элемент списка для удаления не определён");

            using (var dbContext = new RecommendedListsEntities())
            {
                return dbContext.DeleteRecommendedListElement(recListId.Value);
            }
        }

        public ActionResult IncludeApplicationInRecommendedListPage(int? appId)
        {
            if (!appId.HasValue)
                return new EmptyResult();

            using (var dbContext = new RecommendedListsEntities())
            {
                var Model = dbContext.FillIncludeApplicationModel(appId.Value);
                return PartialView("RecommendedLists/IncludeApplicationInRecList", Model);
            }
        }

        public ActionResult SaveRecommendedLists(ApplicationIncludeInRecListViewModel model)
        {
            var stage1RecommendedCount = model.Stage1List.Count(x => x.Recommended);
            var stage2RecommendedCount = model.Stage2List.Count(x => x.Recommended);

            if (stage1RecommendedCount + stage2RecommendedCount == 0)
                return new AjaxResultModel("Для включения в список рекомендованных должно быть отмечено хотя бы одно условие приёма");

            using (RecommendedListsEntities dbContext = new RecommendedListsEntities())
            {
                try
                {
                    dbContext.IncludeApplicationInRecommendedLists(model);
                    return new AjaxResultModel();
                }
                catch (ArgumentNullException nullEx)
                {
                    LogHelper.Log.Error(nullEx);
                    return new AjaxResultModel(nullEx.Message);
                }
                catch (Exception ex)
                {
                    LogHelper.Log.Error(ex);
                    return new AjaxResultModel("При включении заявления в список рекомендованных произошла ошибка. Обратитесть к администратору");
                }
            }

        }

        public ActionResult DownloadRecommendedList(FilterValuesModel currentFilter)
        {
            var showParams = new RecommendedListsShowParametersViewModel() { Filter = currentFilter, PageToShow = -1, SortDirection = 0 };
            var tempFileName = Path.GetTempFileName();

            using (var dbContext = new RecommendedListsEntities())
            {
                var data = dbContext.FillList(InstitutionID, null, showParams);
                byte[] fileData;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (XmlTextWriter writer = new XmlTextWriter(ms, Encoding.UTF8))
                    {

                        writer.WriteStartDocument();
                        writer.WriteStartElement("RecommendedLists");

                        WriteRecommendedList(writer, data.RecommendedLists);

                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                        writer.Flush();

                        ms.Position = 0;
                        fileData = new byte[ms.Length];
                        ms.Read(fileData, 0, (int)ms.Length);
                    }
                }
                using (FileStream fs = new FileStream(tempFileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(fileData, 0, fileData.Length);
                    fs.Flush();
                }
                
            }
            return new AjaxResultModel() { Data = tempFileName };
        }

        public ActionResult GetList(string filePath)
        {
            var fileName = "Список лиц, рекомендованных к зачислению.xml";
            return new InstitutionApplicationController.FixedFileResult(filePath, "text/xml", fileName);
        }

        private void WriteRecommendedList(XmlTextWriter writer, RecommendedListViewModel[] data)
        {
            var orderdData = data.OrderBy(x => x.Stage).ThenBy(x => x.ApplicationNumber).ToArray();
            var currentStage = -1;
            var currentApp = string.Empty;

            foreach (var item in orderdData)
            {
                if (currentStage != item.Stage) // Новый этап - закрыть всё предыдущее и открыть новый
                {
                    if (currentStage != -1) // Не первый элемент - надо закрывать
                    {
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }

                    // А открыть новый этап - в любом случае
                    writer.WriteStartElement("RecommendedList");
                    writer.WriteStartElement("Stage");
                    writer.WriteString(item.Stage.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("RecLists");

                    currentStage = item.Stage;
                    currentApp = string.Empty;
                }

                if (currentApp != item.ApplicationNumber) // - новое заявление - закрыть заявление и начать новое
                {
                    if (currentApp != string.Empty) // не первый элемент - закрываем предыдущий
                    {
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }

                    // Открыть - всегда
                    writer.WriteStartElement("RecList");
                    writer.WriteStartElement("Application");
                    WriteDataElement(writer, "ApplicationNumber", item.ApplicationNumber);
                    WriteDataElement(writer, "RegistrationDate", item.AppRegistrationDate);
                    writer.WriteEndElement();
                    writer.WriteStartElement("FinSourceAndEduForms");

                    currentApp = item.ApplicationNumber;
                }

                // Остальное пишем всегда
                writer.WriteStartElement("FinSourceAndEduForm");
                WriteDataElement(writer, "EducationalLevelID", item.EduLevelId);
                WriteDataElement(writer, "EducationFormID", item.EduFormId);
                WriteDataElement(writer, "CompetitiveGroupID", item.CompetitiveGroupUID);
                WriteDataElement(writer, "DirectionID", item.DirectionId);
                writer.WriteEndElement();
            }
        }

        private void WriteDataElement(XmlTextWriter writer, string elementName, object elementValue)
        {
            writer.WriteStartElement(elementName);
            writer.WriteValue(elementValue == null ? "" : elementValue);
            writer.WriteEndElement();
        }
    }
}
