<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IntrantProfileView.ascx.cs"
    Inherits="Esrp.Web.Controls.IntrantProfileView" %>
<%@ Import Namespace="Esrp.Web" %>
<%@ Import Namespace="Esrp.Core.Systems" %>
<div class="statement_table statement">
    <table width="100%">
        <tr>
            <th style="border-top-color: #fff;">
                Логин/E-mail
            </th>
            <td  style="border-top-color: #fff;">
                <%= CurrentUser.Login %>
            </td>
        </tr>
        <tr>
            <th>
                Ф. И. О.
            </th>
            <td >
                <%= CurrentUser.GetFullName() %>
            </td>
        </tr>
        <tr>
            <th>
                Телефон
            </th>
            <td >
                <%= CurrentUser.Phone %>
            </td>
        </tr>
        <% if (GeneralSystemManager.IsOpenSystem())
           {%>
            <tr>
                <td colspan="2" class="box-submit" style="border-bottom-color: #fff;">
                    <%=
                       (HttpContext.Current.User.IsInRole("EditSelfAccount")
                            ? "<a href=\"/Profile/Edit.aspx\" class=\"link_btn\" title=\"Изменить регистрационные данные\">Изменить</a>"
                            : "<br/>") %>
                </td>
            </tr>
        <% } %>
    </table>
</div>