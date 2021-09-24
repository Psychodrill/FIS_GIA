<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Register TagName="PersonalRecordsData" TagPrefix="gv" Src="~/Views/Shared/Portlets/Entrants/PersonalRecordsData.ascx" %>
<%@ Register TagName="ApplicationWizardButtons" TagPrefix="gv" Src="~/Views/Shared/Portlets/Applications/ApplicationWizardButtons.ascx" %>

<gv:ApplicationWizardButtons ID="ApplicationWizardButtons1" runat="server" ApplicationStep="PersonalData" IsTop="true" />
<gv:PersonalRecordsData runat="server" />
<gv:ApplicationWizardButtons runat="server" ApplicationStep="PersonalData" />
