<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountKeyEditSuccess.aspx.cs" 
    Inherits="Esrp.Web.Profile.AccountKeyEditSuccess"
    MasterPageFile="~/Common/Templates/Personal.master" %>
    
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <p>Ключ доступа “<%= AccountKeyCode %>” изменен успешно.</p>
    <p>
        <a href='/Profile/AccountKeyEdit.aspx?key=<%= AccountKeyCode %>'>Продолжить редактирование</a><br />
        <a href='/Profile/AccountKeyList.aspx'>Список ключей доступа</a><br />
        <a href='/Profile/View.aspx'>Регистрационный данные</a>
    </p>
</asp:Content>
