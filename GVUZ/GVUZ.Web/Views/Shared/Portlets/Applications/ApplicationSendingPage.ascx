<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Applications.ApplicationSendingViewModel>" %>
<%@ Register TagPrefix="gv" TagName="ApplicationSendingTab" Src="~/Views/Shared/Common/ApplicationSendingTabControl.ascx" %>
<%@ Register TagName="ApplicationSending" TagPrefix="gv" Src="~/Views/Shared/Portlets/Applications/ApplicationSending.ascx" %>

<%@ Register TagName="ApplicationWizardButtons" TagPrefix="gv" Src="~/Views/Shared/Portlets/Applications/ApplicationWizardButtons.ascx" %>

<gv:ApplicationWizardButtons ID="ApplicationWizardButtons1" runat="server" ApplicationStep="Sending" IsTop="true" />

<div class="gvuzPortlet">
<% if(Model.ShowDenyMessage) { %> <div>Невозможно редактировать данное заявление</div><script type="text/javascript">
																						function doSubmit() { return false; }</script>  <%} else { %>
<div id="divErrorTop" style="overflow:hidden"></div>
<gv:ApplicationSending runat="server" />
<% if(Model.IsDraft) {%>
<div class="gvuzTab">
	<gv:ApplicationSendingTab runat="server"/>
</div>
<% } %>
<% } %>
<div id="divErrorBottom" style="overflow:hidden"></div>
</div>
<br />
<gv:ApplicationWizardButtons ID="ApplicationWizardButtons2" runat="server" ApplicationStep="Sending" />
