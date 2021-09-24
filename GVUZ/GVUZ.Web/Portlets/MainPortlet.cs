using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using FogSoft.Helpers;
using FogSoft.WSRP;
using FogSoft.WSRP.Portlets;
using GVUZ.Model.Helpers;
using GVUZ.Web.Controllers;
using GVUZ.Web.Helpers;
using Microsoft.Practices.ServiceLocation;
using GVUZ.Web.Portlets.Markup;

namespace GVUZ.Web.Portlets
{
	[Portlet("gvuz.Search", DisplayName = "����� ���������������� ���������� display",
		Title = "����� ���������������� ���������� title", ShortTitle = "�����")]
	[PortletMode(MimeType.TextHtml, PortletMode.View)]
	[PortletWindowState(MimeType.TextHtml, PortletWindowState.Normal)]
	public class MainPortlet : MvcPortlet
	{
		public MainPortlet(PortletDescriptor descriptor)
			: base(descriptor)
		{
		}

		protected override BlockingInteractionResponse PerformViewInteraction(Dictionary<string, string> formParameters, string interactionState, PortletWindowState windowState, MimeType[] mimeTypes, UploadContext[] uploadContexts)
		{
			ISession session = ServiceLocator.Current.GetInstance<ISession>();

			if (!String.IsNullOrEmpty(interactionState))
			{
				//������� interactionState � ������ ��� ��������� � GetViewMarkup
				session.SetValue("interactionState", interactionState);

				(new SearchMarkup(new UserInfo())).ClearCheckboxes(interactionState);

				//��� �������� ����� ����� ajax ����� � ������ json
				string ajaxContent = (new CommonMarkup(new UserInfo())).ProcessInteractionAjax(interactionState, uploadContexts);
				session.SetValue("json", ajaxContent);
			}
			//������� formParameters � ������ ��� ��������� � GetViewMarkup
			if (formParameters != null)
			{
				foreach (var t in formParameters)
					session.SetValue(t.Key, t.Value);
			}

			return null;
		}

		protected override MarkupResponse GetViewMarkup(PortletWindowState windowState, MimeType[] mimeTypes, string navigationalState, string userContextKey)
		{
			/*Activation of the URL will result in an invocation of getMarkup. 
				* This mechanism permits a Portlet's markup to contain URLs, which do not involve changes to local state, 
				* to avoid the overhead of two-step processing by directly invoking getMarkup. The URL MAY specify 
				* the wsrp-navigationalState and/or wsrp-navigationalValues portlet URL parameters, whose values the 
				* Consumer MUST supply in the opaqueValue and publicValues fields of the NavigationalContext structure, respectively. */
			//            ISession session = LightServiceServiceLocator.Current.GetInstanceInstance<ISession>();

			ISession session = ServiceLocator.Current.GetInstance<ISession>();	
			NavigationHelper navigation = new NavigationHelper(navigationalState);
			
			// ������� �� ������ �����
			if (navigation.Module == PortletType.ApplicationGoBack)
			{
				navigationalState = session.GetValue("backNavigationalState", "");
				navigation = new NavigationHelper(navigationalState);
			}
			
			var user = (new JavaScriptSerializer()).Deserialize<UserInfo>(userContextKey);
			
			string interactionState = session.GetValue("interactionState", "");
			string itemString;
			string mimeType = "text/html";
			var commonMarkup = new CommonMarkup(user);
			var applicationMarkup = new ApplicationMarkup(user);
			var entrantMarkup = new EntrantMarkup(user);
			var institutionMarkup = new InstitutionMarkup(user);
			var searchMarkup = new SearchMarkup(user);

			MenuItemsType menuItemType = (MenuItemsType) 0;
			string pageTitle = "";
			const string searchTitle = "�����";

			int applicationID = session.GetValue(ApplicationController.ApplicationSessionKey, 0);
			int institutionID = InstitutionHelper.GetInstitutionID(true);

			if (navigation.Module == PortletType.Institution)
			{
				itemString = institutionMarkup.InstitutionPage(institutionID, navigation.Tab, interactionState, ref mimeType);
				pageTitle = "�������� �� ��������������� ����������";
			}
			else if (navigation.Module == PortletType.SearchAgain)
			{
				itemString = searchMarkup.GetSearchPage(searchMarkup.SearchInformerBlock(), searchMarkup.SearchBlock());
				menuItemType = MenuItemsType.Search;
				pageTitle = searchTitle;
			}
			else if (navigation.Module == PortletType.PersonalRecords)
			{
				itemString = entrantMarkup.PersonalRecordsPage(navigation.Tab, user, navigation.Mode, interactionState, ref mimeType);
				menuItemType = MenuItemsType.Personal;
				pageTitle = "������ ������";
			}
			else if (navigation.Module == PortletType.Application)
			{
				itemString = applicationMarkup.ApplicationPage(applicationID, navigation.Mode, navigation.Tab, navigation.TabNo, interactionState, user, ref mimeType);
				pageTitle = "";
			}
			else if (navigation.Module == PortletType.ApplicationList)
			{
				// ��� back ������ �� ������ ���������
				session.SetValue("backNavigationalState", navigationalState);
				itemString = applicationMarkup.ApplicationListPage(user, ref mimeType);
				menuItemType = MenuItemsType.Applications;
				pageTitle = "������ ���������";
			}
			else if (navigation.Module == PortletType.ApplicationView)
			{
				itemString = applicationMarkup.ApplicationViewPage(applicationID, navigation.Tab, interactionState, user, ref mimeType);
				pageTitle = "�������� ���������";
			}
			else if (navigation.Module == PortletType.SearchModule)
			{
				itemString = !String.IsNullOrEmpty(interactionState) ?
					searchMarkup.SearchPage(interactionState, ref mimeType) :
					commonMarkup.MainPage(searchMarkup.SearchBlock());
				menuItemType = MenuItemsType.Search;
				pageTitle = searchTitle;
			}
			else
			{
				//������� ��������
				itemString = !String.IsNullOrEmpty(interactionState) ? 
					searchMarkup.SearchPage(interactionState, ref mimeType) : 
					commonMarkup.MainPage(searchMarkup.SearchBlock());

				// ��� back ������ �� ������� ��������� ���������� �����
				session.SetValue("backNavigationalState", navigationalState);

				pageTitle = searchTitle;
			}
			// �������� � ������ interactionState
			session.SetValue("interactionState", "");

			// � ������� ���� ����������� ������� (������� � �����) ��� �������� �� ��������� ��������� �� �������� �� ���������.
			// � ��������� ������� �� ���������.
			if (mimeType == "text/html")
				itemString = commonMarkup.MainMenu(menuItemType, pageTitle) + itemString;

			MarkupResponse markupResponse =
								new MarkupResponse
								{
									markupContext =
											new MarkupContext
											{
												locale = ServiceLocator.Current.GetInstance<IConfigurationService>().GetLocale(),
												itemString = itemString,
												mimeType = mimeType
											}
								}
								;
			return markupResponse;
		}
	}
}