<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Register TagName="EntrantLanguageEdit" TagPrefix="gv" Src="~/Views/Shared/Portlets/Entrants/EntrantLanguageEdit.ascx" %>
<%@ Register TagName="ApplicationWizardButtons" TagPrefix="gv" Src="~/Views/Shared/Portlets/Applications/ApplicationWizardButtons.ascx" %>
	
<gv:ApplicationWizardButtons ID="ApplicationWizardButtons1" runat="server" ApplicationStep="Languages" IsTop="true" />
<gv:EntrantLanguageEdit runat="server" />
<gv:ApplicationWizardButtons runat="server" ApplicationStep="Languages" />
