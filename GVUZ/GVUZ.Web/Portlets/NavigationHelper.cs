using System.Collections.Generic;
using FogSoft.Helpers;
using Microsoft.Practices.ServiceLocation;
using GVUZ.Web.Controllers;
using GVUZ.Web.Helpers;

namespace GVUZ.Web.Portlets
{
	public class NavigationHelper
	{
		public string Module { get; private set; }
		public string Tab { get; private set; }
		public int ID { get; private set; }
		public int TabNo { get; private set; }
		public string Mode { get; private set; }
	
		public NavigationHelper(string navigationalState)
		{
			List<string> link = ParseLink(navigationalState);
			SetSessionValues(link);
			Module = link[0];
			Tab = link[1];
			ID = GetSessionValue("ID", 0);
			Mode = link[4];
			TabNo = GetSessionValue("currentTab", 0);
		}

		private static void SetSessionValues(List<string> link)
		{
			SetSessionValue("ID", link[2]);
			int id = GetSessionValue("ID", 0);
			SetSessionValue("currentTab", link[3]);
			if (id!=0)
			{
                InstitutionHelper.MainInstitutionID = id;
				InstitutionHelper.SetInstitutionID(id);
				SetSessionValue(ApplicationController.ApplicationSessionKey, id.ToString()); //используется в заявлениях
			}
			
		}

		private static List<string> ParseLink(string navigationalState)
		{
			navigationalState = navigationalState ?? "";
			var link = new List<string> { navigationalState, "", "0", "0", "" };
			string[] states = navigationalState.Split('.');
			if (states.Length > 0)
				link[0] = states[0];
			if (states.Length > 1)
				link[1] = states[1];
			if (states.Length > 2)
				link[2] = states[2].Replace("id", "");
			if (states.Length > 3)
				link[3] = states[3].Replace("tab", "");
			if (states.Length > 4)
				link[4] = states[4];
			return link;
		}
		
		/// <summary>
		/// Устанавливает значение для текущей сессии.</summary>
		private static void SetSessionValue(string name, string value)
		{
			if (name != "" && value != "")
			{
				ISession session = ServiceLocator.Current.GetInstance<ISession>();
				session.SetValue(name, value);
			}
		}

		/// <summary>
		/// Возвращает значение из текущей сессии.</summary>
		private static int GetSessionValue(string name, int defaultValue)
		{
			ISession session = ServiceLocator.Current.GetInstance<ISession>();
			return session.GetValue(name, defaultValue).To(0);
		}
	}
}