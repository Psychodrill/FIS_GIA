using System;
using FogSoft.Helpers;
using GVUZ.Model.Courses;
using GVUZ.Model.Helpers;
using GVUZ.Model.Institutions;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.Controllers;
using GVUZ.Web.Portlets.Institutions;
using GVUZ.Web.ViewModels;
using Microsoft.Practices.ServiceLocation;
using Institution = GVUZ.Model.Institutions.Institution;

namespace GVUZ.Web.Portlets.Markup
{
    public class InstitutionMarkup : CommonMarkup
    {
        public InstitutionMarkup(UserInfo currentUser) : base(currentUser)
        {
        }

        /// <summary>
        ///     Сведения об образовательном учреждении с выбранным табом
        /// </summary>
        public string InstitutionPage(int id, string tab, string interactionState, ref string mimeType)
        {
            /*if (!String.IsNullOrEmpty(interactionState))
			{
				itemString = institutionMarkup.InstitutionTabAjax(interactionState, session.GetValue("structureItemID", 0));
				mimeType = "application/json";
			}
			else
			{
				itemString = institutionMarkup.InstitutionPage(navigation.ID, navigation.Tab);
			}*/
            var cwe = new ControllerWrapperExecutor();
            var session = ServiceLocator.Current.GetInstance<ISession>();

            string tabContent = "";
            if (String.IsNullOrEmpty(interactionState))
            {
                using (var context = new InstitutionsEntities())
                {
                    Institution institution = context.LoadInstitution(id);

                    if (tab == PortletType.InstitutionInfoTab)
                    {
                        tabContent = new ControllerWrapper().RenderView(
                            "Common/CommonInfoControl",
                            new InstituteCommonInfoViewModel(institution, false));
                    }
                    else if (tab == PortletType.InstitutionStructureTab)
                    {
                        tabContent = new ControllerWrapper().RenderView(
                            "Portlets/Institutions/InstitutionStructure",
                            new InstitutionStructureViewModel());
                    }
                    else if (tab == PortletType.InstitutionReceiveTab)
                    {
                        tabContent = new ControllerWrapper().RenderView(
                            "Portlets/Institutions/EntranceTestTree",
                            new EntranceTestTreeViewModel());
                    }
                    else if (tab == PortletType.InstitutionFacilityTab)
                    {
                        //removed old code
                    }
                    else if (tab == PortletType.InstitutionCourcesTab)
                    {
                        using (var dbContext = new CoursesEntities())
                        {
                            var model = new PreparatoryCourseViewModel(institution.InstitutionID);
                            dbContext.FillPreparatoryCourses(model);
                            tabContent = new ControllerWrapper().RenderView(
                                "Common/PreparatoryCourseView",
                                model);
                        }
                    }
                }
            }
            else
            {
                mimeType = "application/json";
                return InstitutionTabAjax(interactionState,
                                          session.GetValue(ApplicationController.StructureItemSessionKey, 0), cwe);
            }

            /* ISession session = LightServiceServiceLocator.Current.GetInstanceInstance<ISession>();
			int tid = session.GetValue("institutionId", 0);*/
            string itemString = new ControllerWrapper().RenderView(
                "Portlets/Institutions/InstitutionInfo",
                new InstitutionInfoViewModel
                    {
                        TabContent = tabContent
                    });
            return itemString;
        }

        public string InstitutionTabAjax(string interactionState, int structureItemID, ControllerWrapperExecutor cwe)
        {
            string itemString = "";

            if (interactionState == PortletType.InstitutionTreeStructure)
            {
                //itemString = cwe.DoViewControllerAction<StructureController>(x => x.TreeStructure(null, null),
                //                                                             structureItemID);
                /*ControllerWrapper controllerWrapper = new ControllerWrapper(new StructureController());
				JsonResult jsonResult = ((JsonResult)new StructureController().TreeStructure(structureItemID));
				jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
				jsonResult.ExecuteResult(controllerWrapper.Controller.ControllerContext);
				itemString = controllerWrapper.Controller.Response.Output.ToString();*/
            }
            else if (interactionState == PortletType.InstitutionFullTreeStructure)
            {
                //itemString = GetItemString<StructureController, int?>(-1, new StructureController().TreeStructure);
                //itemString = cwe.DoViewControllerAction<StructureController>(x => x.TreeStructure(null, null), -1);
            }
            else if (interactionState == PortletType.InstitutionTreeEntranceTest)
            {
                itemString = cwe.DoViewControllerAction<EntranceTestController>(x => x.TreeStructureView(null),
                                                                                structureItemID);
                /*ControllerWrapper controllerWrapper = new ControllerWrapper(new EntranceTestController());
				JsonResult jsonResult = ((JsonResult)new EntranceTestController().TreeStructureView(structureItemID));
				jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
				jsonResult.ExecuteResult(controllerWrapper.Controller.ControllerContext);
				itemString = controllerWrapper.Controller.Response.Output.ToString();*/
            }
            else if (interactionState == PortletType.InstitutionFullTreeEntranceTest)
            {
                //itemString = cwe.DoViewControllerAction<EntranceTestController>(x => x.TreeStructure(null), -1);
            }
            else if (interactionState == PortletType.InstitutionTreeEntranceTestView)
            {
                //itemString = cwe.DoViewControllerAction<EntranceTestController>(x => x.ViewEntranceTest(null), structureItemID);
            }
            return itemString;
        }

/*
		private static string GetItemString<TController, T>(T objectValue,
															Func<T, ActionResult> controllerAction) where TController : Controller, new()
		{
			ControllerWrapper controllerWrapper = new ControllerWrapper(new TController());
			JsonResult jsonResult = ((JsonResult)controllerAction.Invoke(objectValue));
			jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
			jsonResult.ExecuteResult(controllerWrapper.Controller.ControllerContext);
			return controllerWrapper.Controller.Response.Output.ToString();
		}*/
    }
}