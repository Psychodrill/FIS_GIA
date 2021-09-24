using System;
using GVUZ.Model.Entrants;
using GVUZ.Model.Helpers;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.Controllers;
using GVUZ.Web.Portlets.Entrants;

namespace GVUZ.Web.Portlets.Markup
{
    public class EntrantMarkup : CommonMarkup
    {
        public EntrantMarkup(UserInfo currentUser) : base(currentUser)
        {
        }

        /// <summary>
        ///     Личное дело абитуриента с выбранным табом
        /// </summary>
        public string PersonalRecordsPage(string tab, UserInfo user, string mode, string interactionState,
                                          ref string mimeType)
        {
            string content = "";
            int entrantID;

            using (var dbContext = new EntrantsEntities())
            {
                entrantID = dbContext.GetEntrantID(user);
            }

            var cwe = new ControllerWrapperExecutor();
            cwe.CustomParameters["UserInfo"] = user;

            if (String.IsNullOrEmpty(interactionState))
            {
                if (tab == PortletType.PersonalRecordsDataTab)
                {
                    content = (mode == PortletType.EditMode || entrantID == 0)
                                  ? cwe.DoViewControllerAction<EntrantController>(x => x.PersonalDataEdit())
                                  : cwe.DoViewControllerAction<EntrantController>(x => x.PersonalDataView());
                }
                if (entrantID != 0)
                {
                    if (tab == PortletType.PersonalRecordsAddressTab)
                    {
                        content = mode == PortletType.EditMode
                                      ? cwe.DoViewControllerAction<EntrantController>(x => x.PersonalAddressEdit())
                                      : cwe.DoViewControllerAction<EntrantController>(x => x.PersonalAddressView());
                    }
                    else if (tab == PortletType.PersonalRecordsDocumentsTab)
                    {
                        content = cwe.DoViewControllerAction<EntrantController>(x => x.PersonalDocumentsEdit());
                    }
                    else if (tab == PortletType.PersonalRecordsLanguageTab)
                    {
                        content = mode == PortletType.EditMode
                                      ? cwe.DoViewControllerAction<EntrantController>(x => x.PersonalLanguagesEdit())
                                      : cwe.DoViewControllerAction<EntrantController>(x => x.PersonalLanguagesView());
                    }
                    else if (tab == PortletType.PersonalRecordsRequestTab)
                        content = cwe.DoViewControllerAction<EntrantController>(x => x.ApplicationListControl());
                }
            }
            else
            {
                mimeType = "application/json";
                return ProcessAjax(interactionState, cwe);
            }

            return new ControllerWrapper().RenderView(
                "Portlets/Entrants/PersonalRecords",
                new PersonalRecordsViewModel
                    {
                        TabContent = content,
                        EntrantID = entrantID
                    });
        }
    }
}