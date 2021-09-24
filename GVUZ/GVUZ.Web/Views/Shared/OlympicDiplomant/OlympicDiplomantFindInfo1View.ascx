<%@ Control Language="C#" Inherits="ViewUserControl<FindPersonResultModel>" %>

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
        <h3>Победитель/призер олимпиады найден в базе данных участников ЕГЭ</h3>
    </div>
    <div>
        ФИО: <b> <%= Model.Persons.FirstOrDefault().LastName %> <%= Model.Persons.FirstOrDefault().FirstName %>  <%= Model.Persons.FirstOrDefault().Patronymic %></b>
    </div>
    <div>
        Дата рождения: <b><%= Model.Persons.FirstOrDefault().BirthDay %></b>
    </div>
    <div>
        Тип документа, удостоверяющего личность: <b><%= Model.Persons.FirstOrDefault().DocumentTypeName %></b>
    </div>
    <div>
        Реквизиты документа, удостоверяющего личность: <b><%= Model.Persons.FirstOrDefault().DocumentNumber %> <%= Model.Persons.FirstOrDefault().DocumentSeries %></b>
    </div>
    <div>
        <%--Год сдачи ЕГЭ: <b><%= Model.Persons.FirstOrDefault().UseYear %></b>--%>
    </div>

    <input style="display:none" name="filsel" type="radio" checked="checked" id="filNotFind" itemid="<%= Model.Persons.FirstOrDefault().PersonId%>"/>
</div>

<script type="text/javascript">
    StatusId = '<%= Model.Status %>';
</script>



