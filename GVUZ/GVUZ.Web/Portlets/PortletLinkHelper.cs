using System;
using FogSoft.Helpers;
using GVUZ.Web.Helpers;

namespace GVUZ.Web.Portlets
{
    public static class PortletLinkHelper
    {
        private const string UrlTypeLink = "render";
		private const string UrlTypeAction = "blockingAction";

		private const string UrlTypeAjax = "clientRender";
		private const string UrlTypeAjaxAction = "clientBlockingAction";

		//basic templates
        private const string PortletLinkTemplate = "wsrp_rewrite?wsrp-urlType={0}&wsrp-navigationalState={1}/wsrp_rewrite";
        private const string PortletActionTemplate = "wsrp_rewrite?wsrp-urlType={0}&wsrp-interactionState={1}/wsrp_rewrite";

		//module.tab.Id.tabNo.mode
		private const string NavigationStateTemplate = "{0}.{1}.id{2}.tab{3}.{4}";

		//если передать отрицательное значение, то номер таба будет браться из сессии
		private static string TabNo (int tabNo)
		{
			return tabNo < 0 ? "" : tabNo.ToString();
		}

		public static string NavigationState(string module, string tab, string id = "0", int tabNo = -1, string mode = PortletType.ViewMode)
		{
			return NavigationStateTemplate.FormatWith(module, tab, id, TabNo(tabNo), mode);
		}

		private static string InstitutionState(string id = "0", int tabNo = -1)
		{
			return NavigationStateTemplate.FormatWith(PortletType.Institution, PortletType.InstitutionInfoTab, id, TabNo(tabNo), "");
		}

		private static string InstitutionTabState(string tab, string id = "0", int tabNo = -1)
		{
			return NavigationStateTemplate.FormatWith(PortletType.Institution, tab, id, TabNo(tabNo), "");
		}

		private static string PersonalRecordsState(int tabNo = -1, string mode = PortletType.ViewMode)
		{
			return NavigationStateTemplate.FormatWith(PortletType.PersonalRecords, PortletType.PersonalRecordsDataTab, "0", TabNo(tabNo), mode);
		}

		private static string PersonalRecordsTabState(string tab, int tabNo = -1, string mode = PortletType.ViewMode)
		{
			return NavigationStateTemplate.FormatWith(PortletType.PersonalRecords, tab, "0", TabNo(tabNo), mode);
		}

		private static string ApplicationState(string id = "", string step = PortletType.ApplicationStepSending)
		{
			return NavigationStateTemplate.FormatWith(PortletType.Application, "", id, "", step);
		}

		/*private static string ApplicationSendingTabState(string tab = "", int tabNo = -1)
		{
			return NavigationStateTemplate.FormatWith(PortletType.Application, tab, "", TabNo(tabNo), PortletType.ApplicationStepSending);
		}
*/
		private static string ApplicationViewState(string id)
		{
			return NavigationStateTemplate.FormatWith(PortletType.ApplicationView, PortletType.ApplicationViewCommonTab, id, "", "");
		}

		private static string ApplicationViewTabState(string id, string tab = "", int tabNo = -1)
		{
			return NavigationStateTemplate.FormatWith(PortletType.ApplicationView, tab, id, TabNo(tabNo), "");
		}

		/// <summary>
		/// Линк на список заявлений </summary>
		public static string ApplicationListLink()
		{
			return Link(NavigationStateTemplate.FormatWith(PortletType.ApplicationList, "", "", "", ""));
		}

		/// <summary>
		/// Ссылка Назад </summary>
		public static string ApplicationGoBack()
		{
			return Link(NavigationStateTemplate.FormatWith(PortletType.ApplicationGoBack, "", "", "", ""));
		}
		
		/// <summary>
		/// Линк на подачу заявления </summary>
		/// <param name="id">номер заявления (если пустой то из сесси)</param>
		/// <param name="step">шаг заявления</param>
		public static string ApplicationLink(int id = 0, string step = PortletType.ApplicationStepPersonalData)
		{
			return Link(ApplicationState(id == 0 ? "" : id.ToString(), step));
		}

		/// <summary>
		/// Линк на просмотр заявления </summary>
		/// <param name="id">номер заявления (если пустое - id заявления из сессии)</param>
		public static string ApplicationViewLink(int id = 0)
		{
			return Link(ApplicationViewState(id == 0 ? "" : id.ToString()));
		}

		/// <summary>
		/// Линк на просмотр заявления c выбранным табом </summary>
		/// <param name="id">номер заявления (если пустое - id заявления из сессии)</param>
		/// <param name="tab">Тип таба из PortletType</param>
		/// <param name="tabNo">Номер таба для подсветки, если -1, то смотрим в сессии</param>
		public static string ApplicationViewTabLink(string tab, int tabNo = -1, string id = "")
		{
			return Link(ApplicationViewTabState(id, tab, tabNo));
		}

    	/// <summary>
		/// Линк на Личное дело (на первый таб) </summary>
		public static string PersonalRecordsLink()
		{
			return Link(PersonalRecordsState(0));
		}

		/// <summary>
		/// Линк на Личное дело - на указанныый таб </summary>
		/// <param name="tab">Тип таба из PortletType</param>
		/// <param name="tabNo">Номер таба для подсветки, если -1, то смотрим в сессии</param>
		/// <param name="mode">ViewMode/EditMode</param>
		public static string PersonalRecordsTabLink(string tab, int tabNo = -1, string mode = PortletType.ViewMode)
		{
			return Link(PersonalRecordsTabState(tab, tabNo, mode));
		}

		/// <summary>
		/// Линк на Личное дело - таб Личные данные</summary>
		public static string PersonalRecordsDataLink()
		{
			return PersonalRecordsTabLink(PortletType.PersonalRecordsDataTab);
		}

		/// <summary>
		/// Линк на Личное дело - таб Личные данные в режиме редактирования</summary>
		public static string PersonalRecordsEditDataLink()
		{
			return PersonalRecordsTabLink(PortletType.PersonalRecordsDataTab, -1, PortletType.EditMode);
		}

		/// <summary>
		/// Линк на Личное дело - таб Адрес</summary>
		public static string PersonalRecordsAddressLink()
		{
			return PersonalRecordsTabLink(PortletType.PersonalRecordsAddressTab);
		}

		/// <summary>
		/// Линк на Личное дело - таб Адрес в режиме редактирования</summary>
		public static string PersonalRecordsEditAddressLink()
		{
			return PersonalRecordsTabLink(PortletType.PersonalRecordsAddressTab , - 1, PortletType.EditMode);
		}

		/// <summary>
		/// Линк на Личное дело - таб Иностранные языки</summary>
		public static string PersonalRecordsLanguageLink()
		{
			return PersonalRecordsTabLink(PortletType.PersonalRecordsLanguageTab);
		}

		/// <summary>
		/// Линк на Личное дело - таб Иностранные языки в режиме редактирования</summary>
		public static string PersonalRecordsEditLanguageLink()
		{
			return PersonalRecordsTabLink(PortletType.PersonalRecordsLanguageTab, -1, PortletType.EditMode);
		}

		//basic links

		/// <summary>
		/// Основной метод для генерации линков (для href)</summary>
		/// <param name="navigationalState">navigationalState для определения страницы (разбирается через NavigationHelper)</param>
        public static string Link(string navigationalState)
        {
            return PortletLinkTemplate.FormatWith(UrlTypeLink, navigationalState);
        }

		/*/// <summary>
		/// Метод для генерации линков для Ajax</summary>
		/// <param name="navigationalState">navigationalState из PortletType</param>
		public static string AjaxLink(string navigationalState)
		{
			return PortletLinkTemplate.FormatWith(UrlTypeAjax, navigationalState);
		}*/

		/// <summary>
		/// Метод для генерации линков для Ajax</summary>
		/// <param name="interactionState">interactionState из PortletType</param>
		public static string AjaxActionLink(string interactionState)
		{
			return PortletActionTemplate.FormatWith(UrlTypeAjaxAction, interactionState);
		}

		/// <summary>
		/// Основной метод для генерации action для форм</summary>
		/// <param name="interactionState">interactionState из PortletType</param>
        public static string Action(string interactionState)
        {
            return PortletActionTemplate.FormatWith(UrlTypeAction, interactionState);
        }


		/// <summary>
		/// Линк на просмотр института </summary>
		/// <param name="id">id института</param>
        public static string InstitutionLink(string id)
        {
			return Link(InstitutionState(id));
        }

		/// <summary>
		/// Линк на просмотр института - для дерева </summary>
        public static string InstitutionLinkJsPattern()
        {
			return Link(InstitutionState("%1$s"));
        }

		/// <summary>
		/// Линк на просмотр института - на указанный таб </summary>
		/// <param name="id">id института</param>
		/// <param name="tab">Тип таба из PortletType</param>
		[Obsolete]
        public static string InstitutionTabNameLink(string id, string tab)
        {
			return Link(InstitutionTabState(tab, id));
        }

		/// <summary>
		/// Линк на просмотр института по id из сессии - на указанный таб </summary>
		/// <param name="tab">Тип таба из PortletType</param>
		[Obsolete]
		public static string InstitutionTabLink2(string tab)
        {
			return Link(InstitutionTabState(tab, InstitutionHelper.GetInstitutionID(true).ToString()));
        }

		/// <summary>
		/// Линк на просмотр института по id из сессии - на указанный таб </summary>
		/// <param name="tab">Тип таба из PortletType</param>
		/// <param name="tabNo">Номер таба для подсветки</param>
		public static string InstitutionTabLink(string tab, int tabNo)
		{
			return Link(InstitutionTabState(tab, InstitutionHelper.GetInstitutionID(true).ToString(), tabNo));
		}

		// institution ajax links
		public static string InstitutionTreeStructureAjaxLink()
		{
			return AjaxActionLink(PortletType.InstitutionTreeStructure);
		}

		public static string InstitutionFullTreeStructureAjaxLink()
		{
			return AjaxActionLink(PortletType.InstitutionFullTreeStructure);
		}

		public static string InstitutionTreeEntranceTestAjaxLink()
		{
			return AjaxActionLink(PortletType.InstitutionTreeEntranceTest);
		}

		public static string InstitutionFullTreeEntranceTestAjaxLink()
		{
			return AjaxActionLink(PortletType.InstitutionFullTreeEntranceTest);
		}

		public static string InstitutionTreeEntranceTestViewAjaxLink()
		{
			return AjaxActionLink(PortletType.InstitutionTreeEntranceTestView);
		}
		
		public static string InstitutionSearchByNameAjaxLink()
		{
			return AjaxActionLink(PortletType.InstitutionSearchByName);
		}

		public static string InstitutionDirectionSearchAjaxLink()
		{
			return AjaxActionLink(PortletType.InstitutionDirectionSearch);
		}

		public static string InstitutionDirectionCodeSearchAjaxLink()
		{
			return AjaxActionLink(PortletType.InstitutionDirectionCodeSearch);
		}

		public static string InstitutionRegionSearchAjaxLink()
		{
			return AjaxActionLink(PortletType.InstitutionRegionSearch);
		}

		//search links

		/// <summary>
		/// action для формы поиска института </summary>
        public static string SearchAction()
        {
            return Action(PortletType.Search);
        }

		/// <summary>
		/// Ссылка для повторного поиска (параметры берем из сессии) </summary>
		public static string SearchLink()
		{
			return Link(PortletType.SearchAgain);
		}

		/// <summary>
		/// Ссылка на страницу поиска </summary>
		public static string SearchPage()
		{
			return Action(PortletType.SearchPage);
		}

    }

}