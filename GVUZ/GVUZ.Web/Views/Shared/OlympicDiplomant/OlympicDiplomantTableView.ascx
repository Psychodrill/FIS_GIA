
<%@ Control Language="C#" Inherits="ViewUserControl<OlympicDiplomantListViewModel>" %>

<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="GVUZ.DAL.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.OlympicDiplomant" %>

<table class="gvuzDataGrid" id="gvuzDataGrid">
    <thead>
        <tr>
            <th>
            </th>
            <th>
                <span>№</span>
            </th>
            <th>
                <span class="linkSumulator" id="col2"  onclick="DoSort(this, 2)" ><%= Html.LabelFor(model => model.Data.FirstOrDefault().OlympicDiplomantDocument.LastName) %></span>
            </th>
            <th>
                <span class="linkSumulator" id="col3"  onclick="DoSort(this, 3)"  ><%= Html.LabelFor(model => model.Data.FirstOrDefault().OlympicDiplomantDocument.FirstName) %></span>
            </th>
            <th>
                <span class="linkSumulator" id="col4"  onclick="DoSort(this, 4)"  ><%= Html.LabelFor(model => model.Data.FirstOrDefault().OlympicDiplomantDocument.MiddleName) %></span>
            </th>
            <th>
                <span class="linkSumulator" id="col5"   onclick="DoSort(this, 5)" >Реквизиты документа</span>
            </th>
            <th>
                <span class="linkSumulator" id="col6"  onclick="DoSort(this, 6)"   ><%= Html.LabelFor(model => model.Data.FirstOrDefault().FormNumber) %></span>
            </th>
            <th>
                <span class="linkSumulator" id="col7"   onclick="DoSort(this, 7)" >Реквизиты диплома</span>
            </th>
            <th>
                <span class="linkSumulator" id="col8"  onclick="DoSort(this, 8)"   ><%= Html.LabelFor(model => model.Data.FirstOrDefault().ResultLevelID) %></span>
            </th>
            <th>
                <span class="linkSumulator" id="col9" onclick="DoSort(this, 9)"  ><%= Html.LabelFor(model => model.Data.FirstOrDefault().OlympicDiplomantStatus.Name) %></span>
            </th>
            <th id="thAction" style="width: 1%;">Действие</th>
        </tr>
    </thead>

    <tbody>
        <% 
            var list = Model.Data.ToList();
            foreach (var record in Model.PagedData) 
            { 
                int index = list.IndexOf(record); 
                index++; 
        %>
        <tr itemid="<%= record.OlympicDiplomantID %>">
            <td><span><input type="checkbox" class="checkFind" /></span></td>
            <td><span class="" title="Редактировать"><%= index %></span></td>
            <td><span class="btnEdit linkSumulator" title="Редактировать"><%= record.OlympicDiplomantDocument.LastName %></span></td>
            <td><span class="" title="Редактировать"><%= record.OlympicDiplomantDocument.FirstName %></span></td>
            <td><span class="" title="Редактировать"><%= record.OlympicDiplomantDocument.MiddleName %></span></td>
            <td><span class="" title="Редактировать"><%= record.OlympicDiplomantDocument.DocumentSeries %> <%= record.OlympicDiplomantDocument.DocumentNumber %></span></td>
            <td><span class="" title="Редактировать"><%= record.FormNumber %></span></td>
            <td><span class="" title="Редактировать"><%= record.DiplomaSeries %> <%= record.DiplomaNumber %></span></td>
            <td><span class="" title="Редактировать"><%= record.OlympicDiplomType.Name %></span></td>
            <td><span class="" title="Редактировать"><%= record.OlympicDiplomantStatus.Name %></span></td>
            <td><a href="#" class="btnEdit" title="Редактировать"></a><a href="#" class="btnDelete" title="Удалить"></a></td>
        </tr>
        <% } %>
    </tbody>

    <tfoot>
        <tr>
            <th colspan="11">
                <div>
                    <%= Html.PagedListPager((IPagedList)Model.PagedData, 
                        page => Url.Action("Index","OlympicDiplomant", 
                        new { page }),
                        PagedListRenderOptions.EnableUnobtrusiveAjaxReplacing(
                        new AjaxOptions() { HttpMethod = "GET", UpdateTargetId = "tablerows" }))%>
                </div>
            </th>
        </tr>
    </tfoot>

</table>

<script type="text/javascript">
    $('#totalcount').text('<%=Model.PagedData.TotalItemCount%>');
    $('#pagenumber').text('<%=Model.PagedData.PageNumber%>');
    
    var filtered = '<%=Model.Filter.Filtered%>';

    if (filtered == 'True')
        $('#messageFilter').show();
    else
        $('#messageFilter').hide();


</script>
