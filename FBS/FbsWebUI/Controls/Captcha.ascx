<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Captcha.ascx.cs" Inherits="Fbs.Web.Controls.Captcha" %>
    <asp:Image runat="server" ID="captchaImage" />
    <br />
    <br />
	<p>
    	<span>Введите код подтверждения:</span>
        <br />
        <br />
		<asp:TextBox id="CodeNumberTextBox" runat="server"></asp:TextBox>
        <asp:CustomValidator ID="validator" Display="None" runat="server" OnServerValidate="captchaValidate" />
	</p>