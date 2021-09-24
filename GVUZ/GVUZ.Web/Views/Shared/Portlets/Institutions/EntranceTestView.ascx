<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.EntranceTestViewModelC>" %>
<%@ Import Namespace="GVUZ.Model.Institutions" %>
<%@ Import Namespace="GVUZ.Web.ViewModels" %>
<div id="dialogCaption" style="display: none"><%: "" %></div>
<% if (!Model.HideMainType)
   { %>
    <table class="gvuzDataGrid" cellpadding="3">
        <thead>
            <tr style="height: 20px;">
                <th style="width: 40%"><%= Html.LabelFor(x => x.EntranceTestID) %></th>
                <th style="width: 25%"><%= Html.LabelFor(x => x.Form) %></th>
                <th style="width: 25%"><%= Html.LabelFor(x => x.MinScore) %></th>
            </tr>
        </thead>
        <tbody>
            <% foreach (EntranceTestViewModelC.EntranceTestItemData item in Model.TestItems)
               { %>
                <tr>
                    <td><%: item.TestName %></td>
                    <td><%: item.Form %></td>
                    <td><%: item.Value %></td>
                </tr>
            <% } %>
        </tbody>
    </table>
<% } %>
<% if (Model.CreativeTestTypeID != 0 && (Model.CreativeTestItems.Length > 0 || Model.HideMainType))
   { %>
    <h4><%= Model.CreativeTestItemName %></h4>
    <table class="gvuzDataGrid" cellpadding="3">
        <thead>
            <tr style="height: 20px;">
                <th style="width: 40%"><%= Model.CreativeTestTypeID == EntranceTestType.AttestationType ? Html.LabelFor(x => x.AttestationType) : Html.LabelFor(x => x.EntranceTestID) %></th>
                <th style="width: <%= Model.HideMainType ? 50 : 25 %>%"><%= Html.LabelFor(x => x.Form) %></th>
                <% if (!Model.HideMainType)
                   { %><th style="width: 25%"><%= Html.LabelFor(x => x.MinScore) %></th><% } %>
            </tr>
        </thead>
        <tbody>
            <% foreach (EntranceTestViewModelC.EntranceTestItemData item in Model.CreativeTestItems)
               { %>
                <tr>
                    <td><%: item.TestName %></td>
                    <td><%: item.Form %></td>
                    <% if (!Model.HideMainType)
                       { %><th style="width: 25%"><%: item.Value %></th><% } %>
                </tr>
            <% } %>
        </tbody>
    </table>
<% } %>
<% if (Model.CustomTestTypeID != 0 && (Model.CustomTestItems.Length > 0 || Model.HideMainType))
   { %>
    <h4><%= Model.CustomTestItemName %></h4>
    <table class="gvuzDataGrid" cellpadding="3">
        <thead>
            <tr style="height: 20px;">
                <th style="width: 40%"><%= Model.CustomTestTypeID == EntranceTestType.AttestationType ? Html.LabelFor(x => x.AttestationType) : Html.LabelFor(x => x.EntranceTestID) %></th>
                <th style="width: <%= Model.HideMainType ? 50 : 25 %>%"><%= Html.LabelFor(x => x.Form) %></th>
                <% if (!Model.HideMainType)
                   { %><th style="width: 25%"><%= Html.LabelFor(x => x.MinScore) %></th><% } %>
            </tr>
        </thead>
        <tbody>
            <% foreach (EntranceTestViewModelC.EntranceTestItemData item in Model.CustomTestItems)
               { %>
                <tr>
                    <td><%: item.TestName %></td>
                    <td><%: item.Form %></td>
                    <% if (!Model.HideMainType)
                       { %><th style="width: 25%"><%: item.Value %></th><% } %>
                </tr>
            <% } %>
        </tbody>
    </table>
<% } %>