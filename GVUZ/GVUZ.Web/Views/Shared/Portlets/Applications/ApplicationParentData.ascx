<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Register TagName="PersonalRecordsData" TagPrefix="gv" Src="~/Views/Shared/Portlets/Entrants/PersonalRecordsData.ascx" %>
<%@ Register TagName="ApplicationWizardButtons" TagPrefix="gv" Src="~/Views/Shared/Portlets/Applications/ApplicationWizardButtons.ascx" %>
<gv:ApplicationWizardButtons ID="ApplicationWizardButtons2" runat="server" ApplicationStep="ParentData" IsTop="true" />
<gv:PersonalRecordsData runat="server" />
<gv:ApplicationWizardButtons ID="ApplicationWizardButtons1" runat="server" ApplicationStep="ParentData" />
