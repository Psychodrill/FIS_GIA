<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewOU.aspx.cs" 
    Inherits="Esrp.Web.Administration.Accounts.Users.ViewOU"
    MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Import Namespace="Esrp.Web" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web.Administration.SqlConstructor.UserAccounts" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphActions">
    <div class="h10"></div>
    <div class="border-block">
        <div class="tr"><div class="tt"><div></div></div></div>
        <div class="content">
        <ul>
        <li><a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>&UserKey=OU" 
            title="История изменений" class="gray">История изменений</a></li>
        <li><a href="/Administration/Accounts/Users/AuthenticationHistory.aspx?Login=<%= Login %>&UserKey=OU" 
            title="История аутентификаций" class="gray">История аутентификаций</a></li>
        </ul>
        </div>
    <div class="br"><div class="tt"><div></div></div></div>
    </div> 
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphContent">
   <table class="form f600">
        <tr>
            <td class="left">Логин/E-mail</td>
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
            <td class="left">Доступ к системам</td>
            <td class="text"><asp:Label ID="lbSystemAccess" runat="server" /></td>
        </tr>

        <tr>
            <td class="left">Телефон</td>
            <td class="text"><%= CurrentUser.Phone %></td>
        </tr>
<%--
        <tr>
            <td class="left">E-mail</td>
            <td class="text"><%= CurrentUser.Email %></td>
        </tr>
--%>
        
        <tr>
		    <td colspan="2" class="box-submit"><br /></td>
        </tr>            
    </table>
</asp:Content>
