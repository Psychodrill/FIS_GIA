<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemindPassword.aspx.cs" Inherits="Esrp.Web.RemindPassword" 
    MasterPageFile="~/Common/Templates/Main.master" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<form runat="server">
    <asp:ValidationSummary CssClass="error_block"  runat="server" DisplayMode="BulletList" EnableClientScript="false"
        HeaderText='<p class="l8">Произошли следующие ошибки:</p>' />
    <div class="col_in">
	    <div class="statement edit">
            <p class="title">Напомнить пароль</p>
            <div class="statement_table">
                <table width="100%">
                    <tr>
                        <th>E-mail</th>
                        <td width="1"><asp:TextBox runat="server" ID="txtEmail" CssClass="txt short" /></td>
                    </tr>

                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" 
                        EnableClientScript="false" Display="None"
                        ErrorMessage='Поле "E-mail" обязательно для заполнения' />

                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail"
                         EnableClientScript="false" Display="None"
                        ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*\.{0,1}@([0-9a-zA-Z]*[-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$" 
                        ErrorMessage='Поле "E-mail" заполнено неверно' />  
                        <asp:CustomValidator ID="validEmailValidator" runat="server" ErrorMessage="Пользователь с таким E-Mail не найден" Display="None" OnServerValidate="validateEmailExistance"/>
                 </table>
                <p class="save">
                    <asp:Button runat="server" id="btnRemind" Text="Напомнить пароль" CssClass="bt" onclick="btnRemind_Click"/>
                </p>
            </div>
        </div>
    </div>
</form>
</asp:Content>
