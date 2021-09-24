using System;
using FogSoft.Helpers;
using FogSoft.WSRP;
using GVUZ.Model.Helpers;
using GVUZ.Web.Controllers;
using Microsoft.Practices.ServiceLocation;

namespace GVUZ.Web.Portlets.Markup
{
    public class CommonMarkup
    {
        public CommonMarkup(UserInfo currentUser)
        {
            if (currentUser == null) throw new ArgumentNullException("currentUser");
            CurrentUser = currentUser;
        }

        public UserInfo CurrentUser { get; private set; }

        /// <summary>
        ///     Стартовая страница
        /// </summary>
        public string MainPage(string informerRequests, string informerSearch, string informerMessages)
        {
            string itemString = new ControllerWrapper().RenderView(
                "Portlets/MainPortlet",
                new MainPortletViewModel
                    {
                        InformerRequests = informerRequests,
                        InformerSearch = informerSearch,
                        InformerMessages = informerMessages
                    });
            return itemString;
        }

        /// <summary>
        ///     Стартовая страница только с поиском
        /// </summary>
        public string MainPage(string search)
        {
            string itemString = new ControllerWrapper().RenderView(
                "Portlets/MainPortlet",
                new MainPortletViewModel
                    {
                        Search = search
                    });
            return itemString;
        }

        public string MainMenu(MenuItemsType menuItemType, string pageTitle)
        {
            string itemString = new ControllerWrapper().RenderView(
                "Portlets/MainMenuControl",
                new MainMenuViewModel
                    {
                        MenuItems = menuItemType,
                        Title = pageTitle
                    });
            return itemString;
        }

        /// <summary>
        ///     обработка ajax-запросов с uploadContexts (загрузка файла)
        /// </summary>
        public string ProcessInteractionAjax(string interactionState, UploadContext[] uploadContexts)
        {
            string ajaxContent = "";

            if (interactionState == PortletType.ReceiveFile)
                ajaxContent = ControllerWrapper.DoFileReceive<EntrantController>(uploadContexts);

            return ajaxContent;
        }

        /// <summary>
        ///     обработка ajax-запросов
        /// </summary>
        public string ProcessAjax(string interactionState, ControllerWrapperExecutor cwe)
        {
            var session = ServiceLocator.Current.GetInstance<ISession>();
            string ajaxContent = "";

            if (interactionState == PortletType.SavePersonalData)
                ajaxContent = cwe.DoViewControllerAction<EntrantController>(x => x.SavePersonalData(null),
                                                                            session.GetValue("model", ""));
            else if (interactionState == PortletType.ReceiveFile)
                ajaxContent = session.GetValue("json", "");
            else if (interactionState == PortletType.SavePersonalAddress)
                ajaxContent = cwe.DoViewControllerAction<EntrantController>(x => x.SavePersonalAddress(null),
                                                                            session.GetValue("model", ""));
            else if (interactionState == PortletType.RegionsList)
                ajaxContent = cwe.DoViewControllerAction<EntrantController>(x => x.RegionsList(null),
                                                                            session.GetValue("countryID", 0));
            else if (interactionState == PortletType.ChangeRegion)
                ajaxContent = cwe.DoViewControllerAction<EntrantController>(x => x.ChangeRegion(null),
                                                                            session.GetValue("area", 1) == 1
                                                                                ? session.GetValue("regionID", 0)
                                                                                : session.GetValue("fRegionID", 0));
            else if (interactionState == PortletType.CitiesList)
                ajaxContent = cwe.DoViewControllerAction<EntrantController>(x => x.CitiesList(null, ""),
                                                                            session.GetValue("regionID", 0),
                                                                            session.GetValue("q", ""));
            else if (interactionState == PortletType.CitiesList2)
                ajaxContent = cwe.DoViewControllerAction<EntrantController>(x => x.CitiesList(null, ""),
                                                                            session.GetValue("fRegionID", 0),
                                                                            session.GetValue("q", ""));
            else if (interactionState == PortletType.AddDocument)
                ajaxContent = cwe.DoViewControllerAction<EntrantController>(x => x.AddDocument(null, null, null, null),
                                                                            session.GetValue("entrantID", 0),
                                                                            session.GetValue("documentTypeID", 0));
            else if (interactionState == PortletType.EditDocument)
                ajaxContent = cwe.DoViewControllerAction<EntrantController>(x => x.EditDocument(null),
                                                                            session.GetValue("entrantDocumentID", 0));
            else if (interactionState == PortletType.ViewDocument)
                ajaxContent = cwe.DoViewControllerAction<EntrantController>(x => x.ViewDocument(0),
                                                                            session.GetValue("entrantDocumentID", 0));
            else if (interactionState == PortletType.DeleteDocument)
                ajaxContent = cwe.DoViewControllerAction<EntrantController>(x => x.DeleteDocument(null),
                                                                            session.GetValue("entrantDocumentID", 0));
            else if (interactionState == PortletType.SaveDocumentAuto)
                ajaxContent = cwe.DoViewControllerAction<EntrantController>(x => x.SaveDocumentAuto(null),
                                                                            session.GetValue("model", ""));
            else if (interactionState == PortletType.SaveEntrantLanguages)
                ajaxContent = cwe.DoViewControllerAction<EntrantController>(x => x.SaveEntrantLanguages(null),
                                                                            session.GetValue("model", ""));
            else if (interactionState == PortletType.SaveApplicationDocuments)
                ajaxContent = cwe.DoViewControllerAction<ApplicationController>(x => x.SaveApplicationDocuments(null),
                                                                                session.GetValue("model", ""));
            else if (interactionState == PortletType.SaveApplicationCheck)
                ajaxContent = cwe.DoViewControllerAction<ApplicationController>(x => x.SaveApplicationCheck(null),
                                                                                session.GetValue("model", ""));
            else if (interactionState == PortletType.SendEntrantApplication)
                ajaxContent = cwe.DoViewControllerAction<ApplicationController>(x => x.SendEntrantApplication(null),
                                                                                session.GetValue("model", ""));
                //else if (interactionState == PortletType.SaveApplicationEntranceTest)
                //	ajaxContent = cwe.DoViewControllerAction<ApplicationController>(x => x.SaveApplicationEntranceTest(null), session.GetValue("model", ""));
            else if (interactionState == PortletType.GetApplicationSendingParentsTab)
                ajaxContent = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationSendingParentsTab());
            else if (interactionState == PortletType.GetApplicationSendingAddressTab)
                ajaxContent = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationSendingAddressTab());
            else if (interactionState == PortletType.GetApplicationSendingDocumentsTab)
                ajaxContent = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationSendingDocumentsTab());
            else if (interactionState == PortletType.GetApplicationSendingTestsTab)
                ajaxContent = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationSendingTestsTab());
            else if (interactionState == PortletType.GetApplicationSendingLanguagesTab)
                ajaxContent = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationSendingLanguagesTab());
            return ajaxContent;
        }
    }
}