
<%@ Control Language="C#" Inherits="ViewUserControl<IEnumerable<OlympicDiplomantDocument>>" %>

<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="GVUZ.DAL.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.OlympicDiplomant" %>
<%@ Import Namespace="GVUZ.Data.Model" %>

<table class="gvuzDataGrid" id="canceledTable">
    <thead>
        <tr>
            <%--<th><span>ID</span></th>--%>
            <th><span>ФИО</span></th>
            <th><span>Тип документа</span></th>
            <th><span>Серия</span></th>
            <th><span>Номер</span></th>
            <th><span>Дата выдачи</span></th>
            <th><span>Дата рождения</span></th>
            <th id="thAction" style="width: 1%;">Действие</th>
        </tr>
    </thead>

    <tbody>
        <% foreach (var record in Model) {  %>

    <% 
            string dateStr = ""; 
            if(record.DateIssue != null)
                dateStr = record.DateIssue.Value.ToShortDateString();

            string dateBirth = ""; 
            if(record.BirthDate != null)
                dateBirth = record.BirthDate.Value.ToShortDateString();
    %>

        <tr itemtype="record" itemid="<%= record.OlympicDiplomantDocumentID %>">
            <%--<td><span itemtype="OlympicDiplomantDocumentID" class="btnEditS linkSumulator" title="Редактировать"><%= record.OlympicDiplomantDocumentID %></span></td>--%>
            <td style="display:none"><span itemtype="LastName" class="btnEditS linkSumulator" title="Редактировать"><%= record.LastName %></span></td>
            <td style="display:none"><span itemtype="FirstName" class="btnEditS linkSumulator" title="Редактировать"><%= record.FirstName %></span></td>
            <td style="display:none"><span itemtype="MiddleName" class="btnEditS linkSumulator" title="Редактировать"><%= record.MiddleName %></span></td>
            <td><span itemtype="FIO" class="btnEditS linkSumulator" title="Редактировать"><%= record.LastName %> <%= record.FirstName %> <%= record.MiddleName %></span></td>
            <td style="display:none"><span itemtype="IdentityDocumentTypeID" class="btnEditS linkSumulator" title="Редактировать"><%= record.IdentityDocumentTypeID %></span></td>
            <td><span itemtype="IdentityDocumentTypeName" class="btnEditS linkSumulator" title="Редактировать"><%= record.IdentityDocumentType.Name %></span></td>
            <td><span itemtype="DocumentSeries" class="btnEditS linkSumulator" title="Редактировать"><%= record.DocumentSeries %></span></td>
            <td><span itemtype="DocumentNumber" class="btnEditS linkSumulator" title="Редактировать"><%= record.DocumentNumber %></span></td>
            <td  style="display:none"><span itemtype="OrganizationIssue" class="btnEditS linkSumulator" title="Редактировать"><%= record.OrganizationIssue %></span></td>
            <td><span itemtype="DateIssue" class="btnEditS linkSumulator" title="Редактировать"><%= dateStr %></span></td>
            <td><span itemtype="BirthDate" class="btnEditS linkSumulator" title="Редактировать"><%= dateBirth %></span></td>
            <td><a href="#" class="btnEditS" title="Редактировать"></a><a href="#" class="btnDeleteS" title="Удалить"></a></td>
        </tr>
        <% } %>
    </tbody>

    <tfoot>
    </tfoot>

</table>    