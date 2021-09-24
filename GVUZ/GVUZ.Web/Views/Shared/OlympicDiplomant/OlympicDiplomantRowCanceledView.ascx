<%@ Control Language="C#" Inherits="ViewUserControl<OlympicDiplomantDocument>" %>

<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="GVUZ.DAL.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Data.Model" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.OlympicDiplomant" %>

<tr itemtype="record" itemid="<%= Model.OlympicDiplomantDocumentID %>">

    <% 
            string dateStr = ""; 
            if(Model.DateIssue != null)
                dateStr = Model.DateIssue.Value.ToShortDateString();
    %>

    <%--<td><span itemtype="OlympicDiplomantDocumentID" class="btnEditS linkSumulator" title="Редактировать"><%= Model.OlympicDiplomantDocumentID %></span></td>--%>
    <td style="display:none"><span itemtype="LastName" class="btnEditS linkSumulator" title="Редактировать"><%= Model.LastName %></span></td>
    <td style="display:none"><span itemtype="FirstName" class="btnEditS linkSumulator" title="Редактировать"><%= Model.FirstName %></span></td>
    <td style="display:none"><span itemtype="MiddleName" class="btnEditS linkSumulator" title="Редактировать"><%= Model.MiddleName %></span></td>
    <td><span itemtype="FIO" class="btnEditS linkSumulator" title="Редактировать"><%= Model.LastName %> <%= Model.FirstName %> <%= Model.MiddleName %></span></td>
    <td style="display:none"><span itemtype="IdentityDocumentTypeID" class="btnEditS linkSumulator" title="Редактировать"><%= Model.IdentityDocumentTypeID %></span></td>
    <td><span itemtype="IdentityDocumentTypeName" class="btnEditS linkSumulator" title="Редактировать"><%= Model.IdentityDocumentType.Name %></span></td>
    <td><span itemtype="DocumentSeries" class="btnEditS linkSumulator" title="Редактировать"><%= Model.DocumentSeries %></span></td>
    <td><span itemtype="DocumentNumber" class="btnEditS linkSumulator" title="Редактировать"><%= Model.DocumentNumber %></span></td>
    <td style="display:none"><span itemtype="OrganizationIssue" class="btnEditS linkSumulator" title="Редактировать"><%= Model.OrganizationIssue %></span></td>
    <td><span itemtype="DateIssue" class="btnEditS linkSumulator" title="Редактировать"><%=  dateStr %></span></td>
    <td><span itemtype="BirthDate" class="btnEditS linkSumulator" title="Редактировать"><%= Model.BirthDate.Value.ToShortDateString() %></span></td>
    <td><a href="#" class="btnEditS" title="Редактировать"></a><a href="#" class="btnDeleteS" title="Удалить"></a></td>
</tr>
