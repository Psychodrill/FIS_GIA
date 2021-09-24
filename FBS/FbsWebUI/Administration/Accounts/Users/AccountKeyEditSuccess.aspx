<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountKeyEditSuccess.aspx.cs" 
    Inherits="Fbs.Web.Administration.Accounts.Users.AccountKeyEditSuccess" 
    MasterPageFile="~/Common/Templates/Administration.Master" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <p>Ключ доступа “<%= AccountKeyCode %>” для пользователя “<%= Login %>” изменен успешно.</p>
    <p>
        <a href='/Administration/Accounts/Users/AccountKeyEdit.aspx?login=<%= Login %>&key=<%= AccountKeyCode %>'>Продолжить редактирование ключа доступа</a><br />
        <a href='/Administration/Accounts/Users/AccountKeyList.aspx?login=<%= Login %>'>Список ключей доступа пользователя</a><br />
        <a href='/Administration/Accounts/Users/Edit.aspx?login=<%= Login %>'>Продолжить редактирование пользователя</a>
    </p>
    <p>
        <a href="/Administration/Accounts/Users/List.aspx">Список пользователей</a><br />
        <a href="/Administration/Accounts/Users/Create.aspx">Создать пользователя</a>
    </p> 
</asp:Content>