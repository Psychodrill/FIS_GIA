<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.MainPortletViewModel>" %>

<div class="gvuzPortlet">

    <%= Model.Search %>

    <%--<table class="portletsContainer">
<tr>
    <td class="informerRequests"><%= Model.InformerRequests%></td>
    <td class="informerSearch"><%= Model.InformerSearch%></td>
</tr>
<tr>
    <td colspan="2" class="informerMessages"><%= Model.InformerMessages%></td>
</tr>
</table>--%>
</div>