<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.OlympicDiplomant.OlympicDiplomantImportViewModel>" %>

<%--<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Security" %>--%>


<div style="display:none;"><span id="debugInfo"><%=Model.DebugInfo %></span></div>
<h2><%= Model.ErrorMessage%></h2>

<div>
    Добавлено записей: <span style="color:crimson"><b><%= Model.InsertCount %></b></span> <span style="margin-left:30px;">Обновлено записей:</span>  <span style="color:crimson;"><b><%= Model.UpdateCount %></b></span> <span style="margin-left:30px;">Записей с ошибками:</span> <span style="color:crimson;"><b><%= Model.ErrorCount %></b></span>  
</div>


<%if(Model.Columns.Count() > 0) { %>

<hr />

<div>
    <table>
        <thead>
            <tr>
                <td>Номер строки</td>  
                <td>Информация</td>
            </tr>
        </thead>

        <tbody>
            <%foreach (var record in Model.Columns) { %>
            <tr>
                <td><%= record.Index %></td>
                <td><%= record.Info %></td>
            </tr>
            <% } %>        
        </tbody>
    </table>
</div>

<hr />
<% } %>        


