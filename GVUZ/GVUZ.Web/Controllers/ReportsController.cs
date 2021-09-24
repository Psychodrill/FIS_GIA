using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ARMTRS.Controls;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels.Reports;
using Plat.Common.Reporting;
using Plat.Common.Reporting.Generation;
using Plat.Subsystems.Reporting.Entities;
using Plat.Subsystems.Reporting.Lifecycle;
using Plat.Subsystems.Universal.Entities;
using Plat.Subsystems.Universal.Lifecycle;
using Plat.Subsystems.Universal.Metadata;
using Plat.Subsystems.Universal.Metadata.Dal;
using Plat.Subsystems.Universal.Metadata.ListsUI;
using Plat.WebForms.Components.Reporting;
using GVUZ.Web.Helpers;
using Plat.Abstractions.Entities;

namespace GVUZ.Web.Controllers
{
    [Authorize(Roles = UserRole.EduAdmin)]
    public class ReportsController : BaseController
    {
        public ActionResult Index()
        {
            if (!ConfigHelper.ShowInstitutionReports())
                return HttpNotFound();
            return View(GetModel());
        }

        public ActionResult Report(string reportCode)
        {
            if (!ConfigHelper.ShowInstitutionReports())
                return HttpNotFound();
            return View("Index", GetModel(reportCode));
        }

        [HttpPost]
        public ActionResult GenerateReport(ReportsViewModel model)
        {
            if (!ConfigHelper.ShowInstitutionReports())
                return HttpNotFound();

            GenerationFileFormats fileFormat;
            if ((String.IsNullOrEmpty(model.ReportCode)) || (String.IsNullOrEmpty(model.ReportFileFormat)) || (!Enum.TryParse<GenerationFileFormats>(model.ReportFileFormat, out fileFormat)))
                return View("Index", GetModel(model.ReportCode));

            ActionResult result = GetReportFile(model.ReportCode, fileFormat, model.ReportParameters);
            if (result == null)
                return View("Index", GetModel(model.ReportCode));
            return result;
        }

        private ReportsViewModel GetModel(string reportCode = null, IEnumerable<ReportParameter> postedParameters=null)
        {
            ReportsViewModel model = new ReportsViewModel();
            AppendMenuElements(model);

            if (!String.IsNullOrEmpty(reportCode))
            {
                AppendReportData(model, reportCode);
                if (postedParameters != null)
                {
                    model.MergeParameters(postedParameters);
                }
            }
            return model;
        }

        private ActionResult GetReportFile(string reportCode, GenerationFileFormats fileFormat, IEnumerable<ReportParameter> postedParameters)
        {
            if (String.IsNullOrEmpty(reportCode))
                throw new ArgumentNullException("reportCode");
            if (postedParameters == null)
                throw new ArgumentNullException("parameters");

            StoredTemplateService storedTemplateService = StoredTemplateService.CreateService();
            StoredTemplate template = storedTemplateService.GetPublishedByCode(reportCode);
            if (template == null)
                return null;

            IEnumerable<IdentifiedParameter> parameters = GetParametersForReport(postedParameters, storedTemplateService.GetAllParameters(template).Select(x => x.Key));
            GenerationResult generationResult = storedTemplateService.GenerateFile(template.Id, parameters, fileFormat);
            if (generationResult.GenerationResultType == GenerationResultTypes.Binary || generationResult.GenerationResultType == GenerationResultTypes.StringOrBinary)
                return new FixedFileResult(generationResult.BinaryResult, GetMimeType(generationResult.FileExtension), String.Format("{0}.{1}", generationResult.FileName, generationResult.FileExtension));
            else
            {
                ReportsViewModel model = GetModel(reportCode, postedParameters);
                model.ReportHTML = generationResult.StringResult;
                return View("Index", model);
            }
        }

        private void AppendMenuElements(ReportsViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            IEnumerable<UniversalEntity> menuElements = null;
            try
            {
                CatalogElements.ReloadMenuElements();
                CatalogElements.ReloadMenuItems();
                menuElements = CatalogElements.MenuElements;
                string menuElementCode = ConfigHelper.InstitutionReportsRootCode();
                if (!String.IsNullOrEmpty(menuElementCode))
                {
                    string menuElementId = String.Empty;
                    UniversalEntity containerMenuItem = CatalogElements.MenuItems.Where(x => x.GetCode() == menuElementCode).FirstOrDefault();
                    if (containerMenuItem != null)
                    {
                        menuElementId = containerMenuItem.Id;
                        menuElements = menuElements.Where(x => x.GetMenuItemId() == menuElementId);
                    }
                    else
                    {
                        menuElements = new List<UniversalEntity>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось получить данные о структуре меню", ex);
            }

            foreach (UniversalEntity menuElement in menuElements)
            {
                string reportCode = menuElement.GetContentCode();
                if (!String.IsNullOrEmpty(reportCode))
                {
                    model.MenuElements.Add(new MenuElement() { Text = menuElement.GetText(), ReportCode = reportCode });
                }
            }
        }

        private const string InstitutionIdParameter = "instId_auto";
        private const string LoginParameter = "login_auto";
        private void AppendReportData(ReportsViewModel model, string reportCode)
        {
            if (model == null)
                throw new ArgumentNullException("model");
            if (String.IsNullOrEmpty(reportCode))
                throw new ArgumentNullException("reportCode");

            StoredTemplateService storedTemplateService = StoredTemplateService.CreateService();
            StoredTemplate template = storedTemplateService.GetPublishedByCode(reportCode);
            if (template == null)
                return;

            model.ReportName = template.Name;
            model.ReportCode = template.Code;
            foreach (ReportParameter parameter in GetParamentersForGUI(storedTemplateService.GetAllParameters(template)))
            {
                model.ReportParameters.Add(parameter);
            }

            foreach (GenerationFileFormats fileFormat in storedTemplateService.GetFileFormats(template.Type))
            {
                //if (fileFormat == GenerationFileFormats.Html)
                //    continue;//Пока не реализован
                model.ReportFileFormats.Add(new SelectListItem()
                {
                    Value = fileFormat.ToString(),
                    Text = fileFormat.ToString().Replace("ExcelXml","Excel")
                });
            }
        }

        private IEnumerable<ReportParameter> GetParamentersForGUI(IEnumerable<KeyValuePair<ParameterIdentifier, DBFieldInfoWithInput>> parameterInfos)
        {
            if (parameterInfos == null)
                throw new ArgumentNullException("parameterInfos");

            //Надо собрать все параметры отчета с различными DBName, без дублирования
            List<ReportParameter> result = new List<ReportParameter>();
            foreach (KeyValuePair<ParameterIdentifier, DBFieldInfoWithInput> parameterInfo in parameterInfos)
            {
                if ((parameterInfo.Key.ParameterDBName.ToLower() == LoginParameter.ToLower())
                    || (parameterInfo.Key.ParameterDBName.ToLower() == InstitutionIdParameter.ToLower()))
                    continue;

                if (!result.Any(x => x.DBName == parameterInfo.Key.ParameterDBName))
                {
                    ReportParameter parameter = new ReportParameter();
                    parameter.DBName = parameterInfo.Key.ParameterDBName;
                    parameter.Text = parameterInfo.Value.EnsuredUIName;
                    parameter.InputType = parameterInfo.Value.InputType;

                    if (parameterInfo.Value.InputType == RegisteredParametersInputs.CDropDownInput)
                    {
                        string entityId = parameterInfo.Value.InputTypeArguments;
                        if (!String.IsNullOrEmpty(entityId))
                        {
                            IDalMetadata dalMetadata = MetadataManager.GetDalMetadataByListId(entityId);
                            IListUIMetadata listMetadata = MetadataManager.GetListUIMetadataByListId(entityId);
                            if ((dalMetadata != null) && (listMetadata != null) && (listMetadata.Columns.Any(obj => obj.IsNameColumn)))
                            {
                                UniversalFinderService<UniversalEntity> finderService = new UniversalFinderService<UniversalEntity>(dalMetadata, null, false);
                                IEnumerable<UniversalEntity> entities = null;
                                EntitySearchQuery<UniversalEntity> searchQuery=new EntitySearchQuery<UniversalEntity>();
                                searchQuery.RawQuery.Add(new KeyValuePair<string,object>(LoginParameter ,User.Identity.Name));
                                searchQuery.RawQuery.Add(new KeyValuePair<string,object>(InstitutionIdParameter ,InstitutionID));
                                try
                                {
                                    entities = finderService.GetEntities(searchQuery);
                                }
                                catch (Exception ex) 
                                {
                                    throw ex;
                                }

                                if (entities != null)
                                {
                                    string nameField = listMetadata.Columns.First(obj2 => obj2.IsNameColumn).UniqueDBName;
                                    parameter.ItemsForSelect.AddRange(entities.Select(x => new SelectListItem() { Value = x.Id, Text = x.GetString(nameField) }));
                                    parameter.ItemsForSelectLoaded = true;
                                }
                            }
                        }
                    }

                    result.Add(parameter);
                }
            }
            return result;
        }

        private IEnumerable<IdentifiedParameter> GetParametersForReport(IEnumerable<ReportParameter> postedParameters, IEnumerable<ParameterIdentifier> expectedParameterIdentifiers)
        {
            if (postedParameters == null)
                throw new ArgumentNullException("postedParameters");

            //Надо размножить все введенные параметры так, чтобы каждый DBName повторялся, при необходимости, для всех частей отчета
            List<IdentifiedParameter> result = new List<IdentifiedParameter>();
            foreach (ParameterIdentifier parameterIdentifier in expectedParameterIdentifiers)
            {
                if (postedParameters.Any(x => x.DBName == parameterIdentifier.ParameterDBName))
                {
                    ReportParameter postedParameter = postedParameters.First(x => x.DBName == parameterIdentifier.ParameterDBName);
                    IdentifiedParameter parameter = new IdentifiedParameter();
                    parameter.Identifier = parameterIdentifier;
                    parameter.Value = postedParameter.Value;
                    result.Add(parameter);
                }
                else if (parameterIdentifier.ParameterDBName.ToLower() == LoginParameter.ToLower())
                {
                    IdentifiedParameter parameter = new IdentifiedParameter();
                    parameter.Identifier = parameterIdentifier;
                    parameter.Value = User.Identity.Name;
                }
                else if (parameterIdentifier.ParameterDBName.ToLower() == InstitutionIdParameter.ToLower())
                {
                    IdentifiedParameter parameter = new IdentifiedParameter();
                    parameter.Identifier = parameterIdentifier;
                    parameter.Value = InstitutionID;
                }
            }
            return result;
        }

        private string GetMimeType(string fileExtension)
        {
            string result = "application/octet-stream";
            switch (fileExtension)
            {
                case "xml": result = "text/xml"; break;
                case "rtf": result = "text/rtf"; break;
                case "pdf": result = "application/pdf"; break;
                case "xls": result = "application/octet-stream"; break;
                case "html": result = "text/html"; break;
            }
            return result;
        }
    }
}
