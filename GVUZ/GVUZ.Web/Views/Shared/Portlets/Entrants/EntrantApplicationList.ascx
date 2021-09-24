<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Entrants.ApplicationListViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Portlets.Entrants" %>

<div id="documentAddDialog"></div>
<div id="content">
    <table class="gvuzDataGrid" cellpadding="3">
        <thead>
            <tr>
                <th>
                    <%= Html.LabelFor(x => x.AppDescr.InstitutionName) %>
                </th>
                <th>
                    <%= Html.LabelFor(x => x.AppDescr.RegistrationDate) %>
                </th>
                <th>
                    <%= Html.LabelFor(x => x.AppDescr.StatusName) %>
                </th>
                <th style="width: 40px">
                </th>
            </tr>
        </thead>
        <tbody>
            <% foreach (ApplicationListViewModel.ApplicationData app in Model.Applications)
               { %>
                <tr itemID="<%= app.ApplicationID %>">
                    <td><%: app.InstitutionName %></td>
                    <td><%: app.RegistrationDate.ToString("dd.MM.yyyy") %></td>
                    <td><%: app.StatusName %></td>
                    <td><a href="" class="btnView"><% ApplicationListViewModel.ApplicationData app1 = app; %><%= Url.GenerateLinkIf<ApplicationController>(c => c.ApplicationPersonalData(app1.ApplicationID), "редактировать", app.StatusID == 1) %><%= Url.GenerateLinkIf<ApplicationController>(c => c.ApplicationViewTab0App(app1.ApplicationID), "просмотр", app.StatusID != 1) %></a></td>
                </tr>
            <% } %>
        </tbody>
    </table>
</div>