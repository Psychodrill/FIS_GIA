using System;
using GVUZ.Model.Entrants;
using GVUZ.Model.Helpers;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.Controllers;
using GVUZ.Web.Portlets.Applications;

namespace GVUZ.Web.Portlets.Markup
{
    public class ApplicationMarkup : CommonMarkup
    {
        public ApplicationMarkup(UserInfo currentUser) : base(currentUser)
        {
        }

        /// <summary>
        ///     Список заявлений
        /// </summary>
        public string ApplicationListPage(UserInfo user, ref string mimeType)
        {
            var cwe = new ControllerWrapperExecutor();
            cwe.CustomParameters["UserInfo"] = user;
            return cwe.DoViewControllerAction<EntrantController>(x => x.ApplicationListControl());
        }

        /// <summary>
        ///     Форма подачи заявления
        /// </summary>
        /// <param name="applicationNumber">номер заявления, либо 0 для нового</param>
        /// <param name="applicationStep">номер шага в заявлении</param>
        /// <param name="tab">выбранный таб (в шаге)</param>
        /// <param name="tabNo">номер таба</param>
        /// <param name="interactionState">для ajax</param>
        /// <param name="user">текущий пользователь</param>
        /// <param name="mimeType">для ajax</param>
        public string ApplicationPage(int applicationNumber, string applicationStep, string tab, int tabNo,
                                      string interactionState, UserInfo user, ref string mimeType)
        {
            string content = "";

            int entrantID;
            using (var dbContext = new EntrantsEntities())
            {
                entrantID = dbContext.GetEntrantID(user);
            }

            var cwe = new ControllerWrapperExecutor();
            cwe.CustomParameters["UserInfo"] = user;
            cwe.CustomParameters[ApplicationController.ApplicationSessionKey] = applicationNumber;


            if (!String.IsNullOrEmpty(interactionState))
            {
                mimeType = "application/json";
                return ProcessAjax(interactionState, cwe);
            }

            if (applicationStep == PortletType.ApplicationAdd)
            {
                content = cwe.DoViewControllerAction<ApplicationController>(x => x.AddApplication());
            }
            else if (applicationStep == PortletType.ApplicationStepPersonalData)
            {
                content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationPersonalDataStep());
            }
            else if (applicationStep == PortletType.ApplicationStepAddress)
            {
                content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationAddressStep());
            }
            else if (applicationStep == PortletType.ApplicationStepParents)
            {
                content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationParentDataStep());
            }
            else if (applicationStep == PortletType.ApplicationStepTests)
            {
                content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationEntranceTestStep());
            }
            else if (applicationStep == PortletType.ApplicationStepLanguage)
            {
                content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationLanguagesStep());
            }
            else if (applicationStep == PortletType.ApplicationStepDocuments)
            {
                content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationDocumentsStep());
            }
            else if (applicationStep == PortletType.ApplicationStepAdditional)
            {
                content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationAdditionalInfoStep());
            }
            else if (applicationStep == PortletType.ApplicationStepSending)
            {
                content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationSendingData());
            }

            return new ControllerWrapper().RenderView(
                "Portlets/Applications/Application",
                new ApplicationViewModel
                    {
                        Content = content,
                        EntrantID = entrantID
                    });
        }

        /// <summary>
        ///     Страница просмотра заявления
        /// </summary>
        /// <param name="applicationID">номер заявления</param>
        /// <param name="tab">выбранный таб</param>
        /// <param name="interactionState">ajax-действие</param>
        /// <param name="user">текущий пользователь</param>
        /// <param name="mimeType">для ajax-вызовов</param>
        /// <returns></returns>
        public string ApplicationViewPage(int applicationID, string tab, string interactionState, UserInfo user,
                                          ref string mimeType)
        {
            using (var dbContext = new EntrantsEntities())
            {
                string content = "";
                int entrantID = dbContext.GetEntrantID(user);

                var cwe = new ControllerWrapperExecutor();
                cwe.CustomParameters["UserInfo"] = user;
                cwe.CustomParameters[ApplicationController.ApplicationSessionKey] = applicationID;
                //session.GetValue(ApplicationController.ApplicationSessionKey, 0)

                if (!String.IsNullOrEmpty(interactionState))
                {
                    mimeType = "application/json";
                    return ProcessAjax(interactionState, cwe);
                }

                if (tab == PortletType.ApplicationViewCommonTab)
                    content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationViewCommonTab());
                else if (tab == PortletType.ApplicationViewPersonalTab)
                    content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationViewPersonalTab());
                else if (tab == PortletType.ApplicationViewAddressTab)
                    content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationViewAddressTab());
                else if (tab == PortletType.ApplicationViewDocumentsTab)
                    content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationViewDocumentsTab());
                else if (tab == PortletType.ApplicationViewTestsTab)
                    content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationViewTestsTab());
                else if (tab == PortletType.ApplicationViewLanguageTab)
                    content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationViewLanguagesTab());

                var model = new ApplicationViewModel
                    {
                        Content = content,
                        EntrantID = entrantID
                    };
                dbContext.FillApplicationView(model, user, applicationID);

                return new ControllerWrapper().RenderView(
                    "Portlets/Applications/ApplicationView",
                    model);
            }
        }
    }
}