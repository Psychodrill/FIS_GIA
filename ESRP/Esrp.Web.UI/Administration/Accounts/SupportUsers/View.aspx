<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" 
    Inherits="Esrp.Web.Administration.Accounts.Support.View"
    MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Import Namespace="Esrp.Web" %>
<%@ Import Namespace="Esrp.Core" %>

<asp:Content runat="server" ContentPlaceHolderID="cphActions">
    <div class="h10"></div>
    <div class="border-block">
        <div class="tr"><div class="tt"><div></div></div></div>
        <div class="content">
        <ul>
        <li><a href="/Administration/Accounts/SupportUsers/History.aspx?Login=<%= Login %>" 
            title="История изменений" class="gray">История изменений</a></li>
        <li><a href="/Administration/Accounts/SupportUsers/AuthenticationHistory.aspx?Login=<%= Login %>" 
            title="История аутентификаций" class="gray">История аутентификаций</a></li>
        </ul>
        </div>
    <div class="br"><div class="tt"><div></div></div></div>
    </div> 
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
   <table class="form f600">
        <tr>
            <td class="left">Логин</td>
            <td class="text"><%= CurrentUser.Login%></td>
        </tr>
        <tr>
		    <td colspan="2" class="box-submit"><br/></td>
        </tr>            
        <tr>
            <td class="left">Ф. И. О.</td>
            <td class="text"><%= CurrentUser.GetFullName()%></td>            
        </tr>
        <tr>
            <td class="left">Телефон</td>
            <td class="text"><%= CurrentUser.Phone%></td>
        </tr>
        <tr>
            <td class="left">E-mail</td>
            <td class="text"><%= CurrentUser.Email%></td>
        </tr>
        <tr>
		    <td colspan="2" class="box-submit"><br /></td>
        </tr>            
    </table>
</asp:Content>
