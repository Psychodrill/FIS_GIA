<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Institutions.InstitutionInfoViewModel>" %>
<%@ Import Namespace="GVUZ.Web.Portlets" %>
<%@ Register TagPrefix="gv" TagName="tabcontrol" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>

<div class="gvuzPortlet">

    <a href="<%= PortletLinkHelper.SearchLink() %>">Вернуться к результатам поиска</a><br />

    <div class="gvuzTab">
        <gv:tabcontrol runat="server" id="tabControl" />
    </div>
    <%= Model.TabContent %>

    <%--<table>
<tr>
<td><%= Html.LabelFor(m=>m.InstitutionName) %></td>
<td><%= Html.TextBoxFor(m => m.InstitutionName, new Dictionary<string, object> { { "disabled", "disabled" }, { "class", "searchField" } })%></td>
</tr>
</table>--%>

</div>