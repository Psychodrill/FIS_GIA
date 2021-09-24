<%@ Control Language="C#"
    Inherits="System.Web.Mvc.ViewUserControl<GVUZ.DAL.Dapper.ViewModel.Olympics.OlympicsListViewModel>" %>

<%@ Import Namespace="GVUZ.DAL.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<table class="gvuzDataGrid" id="gvuzDataGrid">
    <thead>
        <tr>
            <th>
                <span class="linkSumulator" id="col1" onclick="DoSort(this, 1)"><%= Html.LabelFor(model => model.Description.OlympicNumber) %></span>
            </th>
            <th>
                <span class="linkSumulator" id="col2" onclick="DoSort(this, 2)"><%= Html.LabelFor(model => model.Description.Name) %></span>
            </th>
            <th>
                <span class="linkSumulator" id="col3" onclick="DoSort(this, 3)"><%= Html.LabelFor(model => model.Description.ProfileName) %></span>
            </th>
            <th>
                <span class="linkSumulator" id="col4" onclick="DoSort(this, 4)"><%= Html.LabelFor(model => model.Description.OrganizerName) %></span>
            </th>
            <th>
                <span class="linkSumulator" id="col5" onclick="DoSort(this, 5)"><%= Html.LabelFor(model => model.Description.OlympicYear) %></span>
            </th>
            <th id="thAction" style="width: 1%;">Действие</th>
        </tr>
    </thead>

    <tbody>
        <% foreach (var record in Model.PagedData)
            { %>
        <tr itemid="<%= record.OlympicTypeProfileID %>">
            <td><%= record.OlympicNumber %></td>
            <td><span class="btnEdit linkSumulator" title="Редактировать"><%= record.Name %></span></td>
            <td><%= record.ProfileName %></td>
            <td><%= record.OrganizerName %></td>
            <td><%= record.OlympicYear %></td>
            <td><a href="#" class="btnEdit" title="Редактировать"></a><a href="#" class="btnDelete" title="Удалить"></a></td>
        </tr>
        <% } %>
    </tbody>

    <tfoot>
        <tr>
            <th colspan="10">
                <div>
                    <%= Html.PagedListPager((IPagedList)Model.PagedData, 
                        page => Url.Action("OlympicsCatalog","Administration", 
                        new { page, Model.Filter.Year, Model.Filter.Name, Model.Filter.Profile, 
                        Model.Filter.Level, Model.Filter.Organizer, Model.Filter.Subject, 
                        Model.Filter.Vosh, Model.Filter.Sort }),
                        PagedListRenderOptions.EnableUnobtrusiveAjaxReplacing(
                        new AjaxOptions() { HttpMethod = "GET", UpdateTargetId = "tablerows", 
                        OnComplete = "PagedOnComplete", OnBegin="PagedOnBegin" }))%>
                </div>
            </th>
        </tr>
    </tfoot>

</table>

<script type="text/javascript">
    $('#totalcount').text('<%=Model.PagedData.TotalItemCount%>');


    var filtered = '<%=Model.Filter.Filtered%>';

    if (filtered == 'True')
        $('#messageFilter').show();
    else
        $('#messageFilter').hide();

</script>