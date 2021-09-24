<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewIS.aspx.cs" 
    Inherits="Esrp.Web.Administration.Accounts.Users.ViewIS"
    MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Import Namespace="Esrp.Web" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web.Administration.SqlConstructor.UserAccounts" %>

<asp:Content runat="server" ContentPlaceHolderID="cphActions">
    <div class="h10"></div>
    <div class="border-block">
        <div class="tr"><div class="tt"><div></div></div></div>
        <div class="content">
        <ul>
        <li><a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>&UserKey=IS" 
            title="История изменений" class="gray">История изменений</a></li>
        <li><a href="/Administration/Accounts/Users/AuthenticationHistory.aspx?Login=<%= Login %>&UserKey=IS" 
            title="История аутентификаций" class="gray">История аутентификаций</a></li>
        </ul>
        </div>
    <div class="br"><div class="tt"><div></div></div></div>
    </div> 
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
   <table class="form f600">
        <tr>
            <td class="left">Логин/Е-mail</td>
            <td class="text"><%= CurrentUser.Login %></td>
        </tr>
        <tr>
		    <td colspan="2" class="box-submit"><br/></td>
        </tr>            
        <tr>
            <td class="left">Ф. И. О.</td>
            <td class="text"><%= CurrentUser.GetFullName() %></td>            
        </tr>
        <tr>
            <td class="left">Группы</td>
            <td class="text"><%= SqlConstructor_GetUsersIS.GetUserGroupNames(CurrentUser.Login) %></td>
        </tr>

        <tr>
            <td class="left">Телефон</td>
            <td class="text"><%= CurrentUser.Phone %></td>
        </tr>
        <%--<tr>
            <td class="left">E-mail</td>
            <td class="text"><%= CurrentUser.Email %></td>
        </tr>--%>
        
        <tr>
		    <td colspan="2" class="box-submit"><br /></td>
        </tr>            
    </table>
</asp:Content>
