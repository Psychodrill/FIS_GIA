<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendToRevisionSuccess.aspx.cs" 
    Inherits="Esrp.Web.Administration.Accounts.Users.SendToRevisionSuccess" 
    MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <p>Анкета пользователя “<%= CurrentUser.Login %>” отправлена на доработку успешно.<br />
    Уведомление отправлено на e-mail.</p>
    <p><a href='/Administration/Accounts/Users/Edit<%= GetUserKeyCode() %>.aspx?login=<%= CurrentUser.Login %>'>Продолжить редактирование </a></p>
    <p><a href="/Administration/Accounts/Users/List<%= GetUserKeyCode() %>.aspx">Список пользователей</a><br />
    <a href="/Administration/Accounts/Users/Create<%= GetUserKeyCode() %>.aspx">Создать пользователя</a></p>
</asp:Content>
