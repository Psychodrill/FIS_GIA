<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Register TagName="PersonalRecordsAddress" TagPrefix="gv" Src="~/Views/Shared/Portlets/Entrants/PersonalRecordsAddress.ascx" %>
<%@ Register TagName="ApplicationWizardButtons" TagPrefix="gv" Src="~/Views/Shared/Portlets/Applications/ApplicationWizardButtons.ascx" %>

<gv:ApplicationWizardButtons ID="ApplicationWizardButtons1" runat="server" ApplicationStep="Address" IsTop="true" />
<gv:PersonalRecordsAddress runat="server" />
<gv:ApplicationWizardButtons runat="server" ApplicationStep="Address" />
