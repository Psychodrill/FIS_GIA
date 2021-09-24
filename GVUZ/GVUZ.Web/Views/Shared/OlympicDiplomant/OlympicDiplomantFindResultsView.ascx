<%@ Control Language="C#" Inherits="ViewUserControl<IEnumerable<RVIPersonIdentDocs>>" %>

<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Data.Model" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.OlympicDiplomant" %>
<%@ Import Namespace="GVUZ.Data.Helpers" %>

<table class="gvuzDataGrid" >
    <thead>
        <tr>
            <td>Выбор</td>
            <td>ФИО</td>
            <td>Дата рождения</td>
            <td>Тип документа</td>
            <td>Реквизиты документа</td>
            <%--<td>Год сдачи ЕГЭ</td>--%>
        </tr>
    </thead>
    <tbody>
        <% foreach (var record in Model) { %>

        <% 
            string BirthDayStr = ""; 
            if(record.RVIPersons.BirthDay != null)
                BirthDayStr = record.RVIPersons.BirthDay.Value.ToShortDateString();

            string docStr = "";
            if(record.RVIDocumentTypes != null)
                docStr = record.RVIDocumentTypes.DocumentTypeName;

        %>

        <tr itemid="<%= record.PersonId %>">
            <td><input name="filsel" type="radio" itemid="<%= record.PersonId %>"/></td>
            <td><%= record.RVIPersons.NormSurname%> <%= record.RVIPersons.NormName%> <%= record.RVIPersons.NormSecondName%></td>
            <td><%= BirthDayStr %></td>
            <td><%= docStr %></td>
            <td><%= record.DocumentSeries %> <%= record.DocumentNumber %></td>
            <%--<td><%= record.RVIPersons.UseYear %></td>--%>
        </tr>
        <% } %>
    </tbody>
</table>

