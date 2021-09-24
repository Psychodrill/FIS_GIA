<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Searches.SearchInformerViewModel>" %>
<%@ Import Namespace="GVUZ.Web.Portlets" %>
<%@ Import Namespace="GVUZ.Web.Portlets.Searches" %>
<%--<script type="text/javascript">
jQuery(document).ready(function(){
<%
    if (Model.InstitutionsList.Count > 0) 
    {
        %>
// --- Автозаполнение учреждений ---
    jQuery("#InstitutionName").autocompleteArray([
<%
        for (int i = 0; i < Model.InstitutionsList.Count-1; i++)
		    {
		%> 
        <%= "'"+Model.InstitutionsList[i]+"',"%>
<% 
            } 
            %>
        <%= "'"+Model.InstitutionsList[Model.InstitutionsList.Count-1]+"'"%>
],
		{
			delay:10,
			minChars:1,
			matchSubset:1,
			autoFill:true,
			maxItemsToShow:20
		}
);
// --- Автозаполнение учреждений ---

// --- Автозаполнение специальностей ---
    jQuery("#DirectionName").autocompleteArray([
<%
        for (int i = 0; i < Model.SpecialityList.Count-1; i++)
		    {
		%> 
        <%= "'"+Model.SpecialityList[i]+"',"%>
<% 
            } 
            %>
        <%= "'"+Model.SpecialityList[Model.SpecialityList.Count-1]+"'"%>
],
		{
			delay:10,
			minChars:1,
			matchSubset:1,
			autoFill:true,
			maxItemsToShow:20
		}
);
// --- Автозаполнение специальностей ---
<% 
    } %>
});
</script>--%>
<script type="text/javascript">
    function clearIt(obj) {
        if ((obj.name == 'DirectionName' && obj.value == '<%= Search.DirectionName %>')
            || (obj.name == 'InstitutionName' && obj.value == '<%= Search.NamePart %>')) {
            obj.value = '';
        }
        return false;
    }
</script>
<table class="informer">
    <form method="post" name="specialitySearchForm" action="<%= PortletLinkHelper.SearchAction() %>" target="_self">
        <tr>
            <td>
                <%= Html.LabelFor(m => m.DirectionName) %>:
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <%= Html.TextBoxFor(m => m.DirectionName, new Dictionary<string, object> {{"onclick", "clearIt(this);"}, {"class", "searchField"}}) %>
            </td>
            <td>
                <input type="submit" value="Найти" class="button" name="<%= SearchType.InformerDirection %>"/><br />
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.LabelFor(m => m.VuzCheckDirection) %>&nbsp;<input type="checkbox" name="VuzCheckDirection" value="true"<% if (Model.VuzCheckDirection)
                                                                                                                                   { %> checked="checked"<% } %>/>&nbsp;
                <%= Html.LabelFor(m => m.SsuzCheckDirection) %>&nbsp;<input type="checkbox" name="SsuzCheckDirection" value="true"<% if (Model.SsuzCheckDirection)
                                                                                                                                     { %> checked="checked" <% } %> />
            </td>
            <td>&nbsp;</td>
        </tr>

    </form>
    <form method="post" name="institutionsSearchForm" action="<%= PortletLinkHelper.SearchAction() %>" target="_self">

        <tr>
            <td>
                <%= Html.LabelFor(m => m.InstitutionName) %>:
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <%= Html.TextBoxFor(m => m.InstitutionName, new Dictionary<string, object> {{"onclick", "clearIt(this);"}, {"class", "searchField"}}) %>
            </td>
            <td>
                <input type="submit" value="Найти" class="button" name="<%= SearchType.InformerInstitution %>"/>
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.LabelFor(m => m.VuzCheckInstitution) %>&nbsp;<input type="checkbox" name="VuzCheckInstitution" value="true"<% if (Model.VuzCheckInstitution)
                                                                                                                                       { %> checked="checked"<% } %>/>&nbsp;
                <%= Html.LabelFor(m => m.SsuzCheckInstitution) %>&nbsp;<input type="checkbox" name="SsuzCheckInstitution" value="true"<% if (Model.SsuzCheckInstitution)
                                                                                                                                         { %> checked="checked" <% } %> />
            </td>
            <td>&nbsp;</td>
        </tr>
    </form>
</table>