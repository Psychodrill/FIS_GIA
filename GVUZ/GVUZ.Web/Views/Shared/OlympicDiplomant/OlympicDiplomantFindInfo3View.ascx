<%@ Control Language="C#" Inherits="ViewUserControl<FindPersonResultModel>" %>

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

<div class="container">
    <div>
        <h3>Для победителя/призера олимпиады найдено несколькозаписей в базе данных участников ЕГЭ</h3>
    </div>
</div>

<table>

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
        <% foreach (var record in Model.Persons) {   %>

        <% 
            string BirthDayStr = ""; 
            if(record.BirthDay != null)
                BirthDayStr = record.BirthDay.Value.ToShortDateString();
        %>

        <tr itemid="<%= record.PersonId %>">
            <td><input name="filsel" type="radio" itemid="<%= record.PersonId %>"/></td>
            <td><%= record.LastName %> <%= record.FirstName %> <%= record.Patronymic %></td>
            <td><%= BirthDayStr %></td>
            <td><%= record.DocumentTypeName %></td>
            <td><%= record.DocumentSeries %> <%= record.DocumentNumber %></td>
            <%--<td><%= record.UseYear %></td>--%>
        </tr>

        <% } %>

    </tbody>
</table>

<script type="text/javascript">
    StatusId = '<%= Model.Status %>';
</script>
