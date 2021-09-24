<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" 
    Inherits="Esrp.Web.Administration.Accounts.Administrators.ChangePassword"
    MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<form runat="server">

    <asp:Panel runat="server" ID="pwdChangePanel">
        <p>
            Внимание вы собираетесь сменить пароль для данного пользователя!!!
        </p>
	    <asp:Button runat="server" ID="btnUpdate" Text="Сменить" CssClass="bt" 
			        onclick="btnUpdate_Click" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pwdChangeSuccessPanel" Visible="false">
        <p>
            Смена пароля для данного пользователя была инициированна.
        </p>
    </asp:Panel>
		      
</form>
</asp:Content>
