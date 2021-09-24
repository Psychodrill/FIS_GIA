<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountKeyEditSuccess.aspx.cs" 
    Inherits="Fbs.Web.Administration.Accounts.Administrators.AccountKeyEditSuccess" 
    MasterPageFile="~/Common/Templates/Administration.Master" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <p>Ключ доступа “<%= AccountKeyCode %>” для пользователя “<%= Login %>” изменен успешно.</p>
    <p>
        <a href='/Administration/Accounts/Administrators/AccountKeyEdit.aspx?login=<%= Login %>&key=<%= AccountKeyCode %>'>Продолжить редактирование ключа доступа</a><br />
        <a href='/Administration/Accounts/Administrators/AccountKeyList.aspx?login=<%= Login %>'>Список ключей доступа пользователя</a><br />
        <a href='/Administration/Accounts/Administrators/Edit.aspx?login=<%= Login %>'>Продолжить редактирование пользователя</a>
    </p>
    <p>
        <a href="/Administration/Accounts/Administrators/List.aspx">Список пользователей</a><br />
        <a href="/Administration/Accounts/Administrators/Create.aspx">Создать пользователя</a>
    </p> 
</asp:Content>