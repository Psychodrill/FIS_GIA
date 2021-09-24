<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IntrantProfileView.ascx.cs" Inherits="Fbs.Web.Controls.IntrantProfileView" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Fbs.Web" %>
<table class="form f600">
    <tr>
        <td class="left">Логин</td>
        <td class="text"><%= CurrentUser.Login %></td>
    </tr>
    <tr>
        <td class="left">Ф. И. О.</td>
        <td class="text"><%= CurrentUser.GetFullName() %></td>
    </tr>
    <tr>
        <td class="left">Телефон</td>
        <td class="text"><%= CurrentUser.Phone %></td>
    </tr>
    <tr>
        <td class="left">E-mail</td>
        <td class="text"><%= CurrentUser.Email %></td>
    </tr>
    <tr>
	    <td colspan="2" class="box-submit">
       <%-- <%= (HttpContext.Current.User.IsInRole("EditSelfAccount") ? 
            "<a href=\"/Profile/Edit.aspx\" title=\"Изменить регистрационные данные\">Изменить</a>" : "<br/>")%>--%>
	    </td>
    </tr>  
</table>