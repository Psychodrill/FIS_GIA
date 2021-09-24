<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Searches.SearchPortletViewModel>" %>

<div class="gvuzPortlet">
    <%--<table class="portletsContainer">
<tr>
    <td class="informerSearch"><%= Model.InformerSearch%></td>
    <td class="search"><%= Model.Search%></td>
</tr>
</table>--%>
    <%= Model.Search %>
</div>