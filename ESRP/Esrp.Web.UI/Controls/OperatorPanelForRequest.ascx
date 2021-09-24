<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperatorPanelForRequest.ascx.cs"
	Inherits="Esrp.Web.Controls.OperatorPanelForRequest" %>

<div class="form">
	<p class="title">Оператор
        &nbsp;<asp:Image ID="imgEditStatus" runat="server" ImageUrl="../Common/Images/Warning.png" Visible="false"/>
    </p>
	<div class="form_in">
		<p>Комментарий<br /><asp:TextBox ID="txtComments" runat="server" MaxLength="1024" Rows="5"  EnableViewState="false"
					TextMode="MultiLine" Width="100%"></asp:TextBox></p>
		<p><asp:Button ID="btnEditComment" runat="server" Text="Сохранить" onclick="btnEditComment_Click" EnableViewState="false" /></p>
	</div>
</div>