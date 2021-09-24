<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Applications.ApplicationViewModel>" %>
<%@ Register TagPrefix="gv" TagName="apptabcontrol" Src="~/Views/Shared/Common/ApplicationTabControl.ascx" %>
<div class="gvuzPortlet divstatement">
<% if (Model.CanView) {%>
<gv:apptabcontrol runat="server"/>
<%= Model.Content %>
<%} else {%>
<div class="field-validation-error"><%: Model.DenyMessage %></div>
<%}%>
</div>