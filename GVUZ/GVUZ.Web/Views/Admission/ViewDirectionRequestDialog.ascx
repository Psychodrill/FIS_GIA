<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.InstitutionDirectionRequestViewModel>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>

<table style="widows: 100%" cellpadding="3">
    <colgroup>
        <col style="width: 30%" />
        <col style="width: 70%" />
    </colgroup>
    <tr>
        <td style="font-weight: bold;vertical-align: top"><%= Html.TableLabelFor(m => m.DirectionName) %></td>
        <td><%= Model.DirectionName %></td>
    </tr>
    <tr>
        <td style="font-weight: bold;vertical-align: top"><%= Html.TableLabelFor(m => m.OperationType) %></td>
        <td><%= Model.OperationType %></td>
    </tr>
    <tr>
        <td style="font-weight: bold;vertical-align: top"><%= Html.TableLabelFor(m => m.RequestDate) %></td>
        <td><%= Model.RequestDate %></td>
    </tr>
    <tr>
        <td style="font-weight: bold;vertical-align: top"><%= Html.TableLabelFor(m => m.Comment) %></td>
        <td><%= Model.Comment %></td>
    </tr>
</table>