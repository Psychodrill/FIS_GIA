<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Main.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Esrp.Web.WebUserAccount.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContentModalDialog" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSecondLevelMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphThirdLevelMenu" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContent" runat="server">
<form id="Form1" runat="server">
    <asp:ValidationSummary ID="ValidationSummary1" CssClass="error_block"  runat="server" DisplayMode="BulletList" EnableClientScript="false"
        HeaderText='<p class="l8">Произошли следующие ошибки:</p>' />
    <div class="col_in">
	    <div class="statement edit">
            <p class="title">Сменить пароль</p>
            <div class="clear"></div>
            <div class="statement_table">
        <p>Для продолжения нажмите кнопку сменить пароль</p>
        <div style="margin-top:10px"></div>
        <p class="save">
            <asp:Button runat="server" id="btnChange" Text="Сменить пароль" CssClass="bt"/>
		</p>

            </div>
        </div>
    </div>
</form>
</asp:Content>
