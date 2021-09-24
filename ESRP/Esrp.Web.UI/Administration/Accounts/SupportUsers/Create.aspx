<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Create.aspx.cs" 
    Inherits="Esrp.Web.Administration.Accounts.Support.Create" 
    MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>

<%--
    Права доступа к странице определяются в web.config секцией: 
    <location path="Administration/Accounts/SupportUsers/Create.aspx">
--%>

<asp:Content runat="server" ContentPlaceHolderID="cphHead">
    <script src="/Common/Scripts/Confirmation.js" type="text/javascript"></script>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<form runat="server">

    <asp:ValidationSummary CssClass="error_block"  runat="server"  DisplayMode="BulletList" EnableClientScript="false"
        HeaderText="<p>Произошли следующие ошибки:</p>"/>

    <table class="form" style="width:100%;">
        <tr>
            <td class="left">Логин</td>
            <td><asp:TextBox runat="server" ID="txtLogin" CssClass="txt long"/></td>
        </tr>
        
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLogin" 
            EnableClientScript="false" Display="None" 
            ErrorMessage='Поле "Логин" обязательно для заполнения' />
            
        <asp:CustomValidator runat="server" ID="validLogin" EnableClientScript="false"
            ControlToValidate="txtLogin" Display="None" OnServerValidate="validLogin_ServerValidate"
            EnableViewState="false"  ErrorMessage='Логин "{0}" занят' />
            
        <tr>
            <td class="left">Фамилия</td>
            <td><asp:TextBox runat="server" ID="txtLastName" CssClass="txt long"/></td>
        </tr>
        
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLastName" 
            EnableClientScript="false" Display="None" SetFocusOnError="true"
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
