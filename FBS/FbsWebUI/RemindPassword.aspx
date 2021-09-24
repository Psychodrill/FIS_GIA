<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemindPassword.aspx.cs" Inherits="Fbs.Web.RemindPassword" 
    MasterPageFile="~/Common/Templates/Loginless.master" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<form runat="server">

    <asp:ValidationSummary runat="server" DisplayMode="BulletList" EnableClientScript="false"
        HeaderText='<p class="l8">Произошли следующие ошибки:</p>' />

    <table class="form">
        <tr>
            <td class="left"><span>E-mail</span></td>
            <td><asp:TextBox runat="server" ID="txtEmail" CssClass="txt short" /></td>
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
            <asp:Button runat="server" id="btnRemind" Text="Напомнить пароль" CssClass="bt" onclick="btnRemind_Click" />
			</td>
        </tr>
    </table>

</form>
</asp:Content>
