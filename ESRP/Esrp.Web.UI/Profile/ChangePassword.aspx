<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs"
    Inherits="Esrp.Web.Personal.Profile.ChangePassword" MasterPageFile="~/Common/Templates/Main.master" %>

<%@ OutputCache VaryByParam="none" Duration="1" %>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <form runat="server">
    <asp:ValidationSummary CssClass="error_block" runat="server" DisplayMode="BulletList"
        EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />
    <div class="statement_table" style="margin-top: 0px;">
        <table width="100%">
            <tr>
                <th>
                    Старый&nbsp;пароль
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtOldPassword" TextMode="Password" CssClass="txt short" />
                </td>
            </tr>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOldPassword" EnableClientScript="false"
                Display="None" ErrorMessage='Поле "Старый пароль" обязательно для заполнения' />
            <asp:CustomValidator ID="cvWrongOldPassword" runat="server" ControlToValidate="txtOldPassword"
                ValidateEmptyText="false" EnableClientScript="false" Display="None" OnServerValidate="cvWrongOldPassword_ServerValidate"
                ErrorMessage="Старый пароль не верен" />
            <tr>
                <th>
                    Новый&nbsp;пароль
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtNewPassword" TextMode="Password" CssClass="txt short" />
                </td>
            </tr>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNewPassword" EnableClientScript="false"
                Display="None" ErrorMessage='Поле "Новый пароль" обязательно для заполнения' />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtNewPassword"
                EnableClientScript="false" Display="None" ValidationExpression="^(?!^[0-9]*$)(?!^[a-zA-Z]*$)(?!^[a-z0-9]*$)(?!^[A-Z0-9]*$)^([a-zA-Z0-9]{8,})$"
                ErrorMessage='Поле "Новый пароль" должно содержать цифры, строчные, заглавные буквы и его длина должна превышать 8 символов' />
            <tr>
                <th>
                    Новый&nbsp;пароль&nbsp;еще&nbsp;раз
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtNewPasswordConfirm" TextMode="Password" CssClass="txt short" />
                </td>
            </tr>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNewPasswordConfirm"
                EnableClientScript="false" Display="None" ErrorMessage='Поле "Новый пароль еще раз" обязательно для заполнения' />
            <asp:CompareValidator runat="server" ControlToValidate="txtNewPasswordConfirm" ControlToCompare="txtNewPassword"
                EnableClientScript="false" Type="String" Display="None" ErrorMessage='Не совпадает значение полей "Новый пароль" и "Новый пароль еще раз"' />
            <tr>
                <td colspan="2" class="box-submit"  style="border-bottom-color: #fff;">
                    <asp:Button runat="server" ID="btnLogin" Text="Изменить пароль" CssClass="bt" OnClick="btnLogin_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</asp:Content>
