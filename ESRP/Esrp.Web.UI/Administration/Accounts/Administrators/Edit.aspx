<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" 
    Inherits="Esrp.Web.Administration.Accounts.Administrators.Edit"
    MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>

<%--
    Права доступа к странице определяются в web.config секцией: 
    <location path="Administration/Accounts/SupportUsers/Edit.aspx">
--%>

<asp:Content runat="server" ContentPlaceHolderID="cphHead">
    <script src="/Common/Scripts/Confirmation.js" type="text/javascript"></script>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphActions">
    <div class="h10"></div>
    <div class="border-block">
        <div class="tr"><div class="tt"><div></div></div></div>
        <div class="content">
            <ul>
            <li><a href="/Administration/Accounts/Administrators/ChangePassword.aspx?Login=<%= Login %>" 
                title="Изменить пароль">Изменить пароль</a></li>
            </ul>
            <div class="split"></div>
            <ul>
            <li><a href="/Administration/Accounts/Administrators/History.aspx?Login=<%= Login %>" 
                title="История изменений" class="gray">История изменений</a></li>
            <li><a href="/Administration/Accounts/Administrators/AuthenticationHistory.aspx?Login=<%= Login %>" 
                title="История аутентификаци" class="gray">История аутентификаций</a></li>
<% if (User.IsInRole("EditAdministratorAccount")) { %>
        <li><a href="/Administration/Accounts/Administrators/AccountKeyList.aspx?Login=<%= Login %>" 
            title="Ключи доступа" class="gray">Ключи доступа</a></li>
<% } %>                
            <ul>
        </div>
    <div class="br"><div class="tt"><div></div></div></div>
    </div> 
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<form runat="server">

    <asp:ValidationSummary CssClass="error_block"  runat="server" DisplayMode="BulletList" EnableClientScript="false"
        HeaderText="<p>Произошли следующие ошибки:</p>"/>

    <table class="form">
        <tr>
            <td class="left">Логин</td>
            <td class="left"><%= CurrentUser.Login %></td>
        </tr>
        
        <tr>
            <td class="left">Фамилия</td>
            <td><asp:TextBox runat="server" ID="txtLastName" CssClass="txt long"/></td>
        </tr>
        
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLastName" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "Фамилия" обязательно для заполнения' />   
            
        <tr>
            <td class="left">Имя</td>
            <td><asp:TextBox runat="server" ID="txtFirstName" CssClass="txt long"/></td>
        </tr>
        
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "Имя" обязательно для заполнения' />   
                  
        <tr>
            <td class="left">Отчество</td>
            <td><asp:TextBox runat="server" ID="txtPatronymicName" CssClass="txt long"/></td>
        </tr>

        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPatronymicName" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "Отчество" обязательно для заполнения' />  

        <tr>
            <td class="left">Телефон</td>
            <td><asp:TextBox runat="server" ID="txtPhone" CssClass="txt small"/></td>
        </tr>      

        <tr>
            <td class="left">E-mail</td>
            <td><asp:TextBox runat="server" ID="txtEmail" CssClass="txt small"/></td>
        </tr>

        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "E-mail" обязательно для заполнения' /> 

        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" 
            EnableClientScript="false" Display="None"
            ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*\.{0,1}@([0-9a-zA-Z]*[-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$" 
            ErrorMessage='Поле "E-mail" заполнено неверно' />     

        <tr>
			<td colspan="2" class="box-submit">
			<asp:Button runat="server" ID="btnUpdate" Text="Сохранить" CssClass="bt" 
			    onclick="btnUpdate_Click" />
			</td>
        </tr> 
    </table>        

    <input type="hidden" name="state" />

</form>

<script language="javascript" type="text/javascript">
    InitConfirmation('', '<%= Request.Form["state"] %>');
</script>
</asp:Content>
