<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditSuccess.aspx.cs" 
    Inherits="Fbs.Web.Administration.Accounts.Users.EditSuccess" 
    MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Import Namespace="Fbs.Core" %>
<%@ Import Namespace="Fbs.Web" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <p>Пользователь “<%= CurrentUser.Login %>” изменен успешно.</p>
    <p><a href='/Administration/Accounts/Users/RemindPassword.aspx?login=<%= CurrentUser.Login %>'>Напомнить пароль</a><br />
    <% if (User.IsInRole("ActivateDeactivateUsers")) 
   { %>
    <a href='/Administration/Accounts/Users/Activate.aspx?login=<%= CurrentUser.Login %>'>Активировать</a>
    <% } %>
    <br />
    <a href='/Administration/Accounts/Users/Edit.aspx?login=<%= CurrentUser.Login %>'>Продолжить редактирование</a></p>
    <p><a href="/Administration/Accounts/Users/List.aspx">Список пользователей</a><br />
    <a href="/Administration/Accounts/Users/Create.aspx">Создать пользователя</a></p> 
</asp:Content>
