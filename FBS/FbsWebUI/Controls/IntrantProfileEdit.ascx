<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IntrantProfileEdit.ascx.cs" Inherits="Fbs.Web.Controls.IntrantProfileEdit" %>

<% if (HttpContext.Current.User.IsInRole("EditSelfAccount")) { %>

<asp:ValidationSummary runat="server" DisplayMode="BulletList" 
    EnableClientScript="false" ValidationGroup="IntrantProfile"
    HeaderText="<p>Произошли следующие ошибки:</p>"/>

<table class="form">
    <tr>
        <td class="left">Логин</td>
        <td class="text"><asp:Literal runat="server" ID="litUserName"/></td>
    </tr>

    <tr>
        <td class="left">Фамилия</td>
        <td><asp:TextBox runat="server" ID="txtLastName" CssClass="txt long"/></td>
    </tr>

    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLastName" 
        EnableClientScript="false" Display="None" ValidationGroup="IntrantProfile"
        ErrorMessage='Поле "Фамилия" обязательно для заполнения' /> 

    <tr>
        <td class="left">Имя</td>
        <td><asp:TextBox runat="server" ID="txtFirstName" CssClass="txt long"/></td>
    </tr>

    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName" 
        EnableClientScript="false" Display="None" ValidationGroup="IntrantProfile"
        ErrorMessage='Поле "Имя" обязательно для заполнения' /> 

    <tr>
        <td class="left">Отчество</td>
        <td><asp:TextBox runat="server" ID="txtPatronymicName" CssClass="txt long"/></td>
    </tr>

    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPatronymicName" 
        EnableClientScript="false" Display="None" ValidationGroup="IntrantProfile"
        ErrorMessage='Поле "Отчество" обязательно для заполнения' /> 

    <tr>
        <td class="left">Телефон&nbsp;(с&nbsp;указанием&nbsp;кода&nbsp;города)</td>
        <td><asp:TextBox runat="server" ID="txtPhone" CssClass="txt small"/></td>
    </tr>
    
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPhone" 
        EnableClientScript="false" Display="None" ValidationGroup="IntrantProfile"
        ErrorMessage='Поле "Телефон" обязательно для заполнения' /> 
    
    <tr>
        <td class="left">E-mail</td>
        <td><asp:TextBox runat="server" ID="txtEmail" CssClass="txt small"/></td>
    </tr>

    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" 
        EnableClientScript="false" Display="None" ValidationGroup="IntrantProfile"
        ErrorMessage='Поле "E-mail" обязательно для заполнения' /> 

    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" 
        EnableClientScript="false" Display="None" ValidationGroup="IntrantProfile"
        ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*\.{0,1}@([0-9a-zA-Z]*[-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$" 
        ErrorMessage='Поле "E-mail" заполнено неверно' />     

    <tr>
		<td colspan="2" class="box-submit">
		<asp:Button runat="server" ID="btnUpdate" Text="Сохранить" CssClass="bt" 
		    onclick="btnUpdate_Click" ValidationGroup="IntrantProfile" />
		</td>
    </tr>
</table> 

<input type="hidden" name="state" />

<script language="javascript" type="text/javascript">
    InitConfirmation('', '<%= Request.Form["state"] %>');
</script>

<% } else { %>

<p>Вы не имеете прав для редактирования регистрационных данных.<br />
    Перейдите на страницу просмотра <a href="/Profile/View.aspx" 
    title="Регистрационные данные">регистрационных данных</a>.</p>

<% } %>