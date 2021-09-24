<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" 
    Inherits="Fbs.Web.Personal.Profile.ChangePassword" MasterPageFile="~/Common/Templates/Personal.master" %>
<%@ OutputCache VaryByParam="none" Duration="1" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<form runat="server">

    <asp:ValidationSummary runat="server" DisplayMode="BulletList" EnableClientScript="false"
        HeaderText="<p>Произошли следующие ошибки:</p>"/>

    <table class="form">
    <tr>
        <td class="left">Старый&nbsp;пароль</td>
        <td><asp:TextBox runat="server" ID="txtOldPassword" TextMode="Password" 
            CssClass="txt short"/></td>
    </tr>

    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOldPassword" 
        EnableClientScript="false" Display="None"
        ErrorMessage='Поле "Старый пароль" обязательно для заполнения' /> 

    <asp:CustomValidator ID="cvWrongOldPassword" runat="server"
        ControlToValidate="txtOldPassword" ValidateEmptyText="false"
        EnableClientScript="false" Display="None"
        OnServerValidate="cvWrongOldPassword_ServerValidate"
        ErrorMessage="Старый пароль не верен" />

    <tr>
        <td class="left">Новый&nbsp;пароль</td>
        <td><asp:TextBox runat="server" ID="txtNewPassword" TextMode="Password" 
            CssClass="txt short"/></td>
    </tr>

    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNewPassword" 
        EnableClientScript="false" Display="None"
        ErrorMessage='Поле "Новый пароль" обязательно для заполнения' />     

    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtNewPassword" 
        EnableClientScript="false" Display="None"
        ValidationExpression="^(?!^[0-9]*$)(?!^[a-zA-Z]*$)(?!^[a-z0-9]*$)(?!^[A-Z0-9]*$)^([a-zA-Z0-9]{8,})$" 
        ErrorMessage='Поле "Новый пароль" должно содержать цифры, строчные, заглавные буквы и его длина должна превышать 8 символов' />

    <tr>
        <td class="left">Новый&nbsp;пароль&nbsp;еще&nbsp;раз</td>
        <td><asp:TextBox runat="server" ID="txtNewPasswordConfirm" TextMode="Password" 
            CssClass="txt short"/></td>
    </tr>

    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNewPasswordConfirm" 
        EnableClientScript="false" Display="None"
        ErrorMessage='Поле "Новый пароль еще раз" обязательно для заполнения' />     

    <asp:CompareValidator runat="server" 
        ControlToValidate="txtNewPasswordConfirm" ControlToCompare="txtNewPassword" 
        EnableClientScript="false" Type="String" Display="None" 
        ErrorMessage='Не совпадает значение полей "Новый пароль" и "Новый пароль еще раз"' />     

    <tr>
		<td colspan="2" class="box-submit">
		<asp:Button runat="server" id="btnLogin" Text="Изменить пароль" CssClass="bt" 
		    onclick="btnLogin_Click" />
		</td>
    </tr>
    </table>    

</form>
</asp:Content>
