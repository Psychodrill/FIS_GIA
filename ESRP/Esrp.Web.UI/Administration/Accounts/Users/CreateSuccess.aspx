<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateSuccess.aspx.cs" 
    Inherits="Esrp.Web.Administration.Accounts.Users.CreateSuccess" 
    MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <div class="left_col">
        <div class="col_in">
            <div class="statement">
                <p class="title">Пользователь “<%= CurrentUser.Login %>” создан успешно.</p>
            </div>
            <div class="statement_in">
                <p>Уведомление отправлено на e-mail.</p>
                <p><a href='/Administration/Accounts/Users/Edit.aspx?login=<%= CurrentUser.Login %>'>Продолжить редактирование</a></p>
                <p><a href="/Administration/Accounts/Users/List.aspx">Список пользователей</a></p>
                <p><a href="/Administration/Accounts/Users/Create.aspx">Создать пользователя</a></p>
            </div>
        </div>
    </div> 
</asp:Content>
