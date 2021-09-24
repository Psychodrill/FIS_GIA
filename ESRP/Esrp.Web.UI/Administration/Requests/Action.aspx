<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Action.aspx.cs" 
Inherits="Esrp.Web.Administration.Requests.Action"
	MasterPageFile="~/Common/Templates/Administration.Master" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<form runat="server">
<div class="left_col">
    
        <div class="col_in">
			<div class="statement edit" >
							
				<p class="title">Заявление на регистрацию № <%= RequestID%></p>
				<p class="back"><a href='/Administration/Requests/RequestForm.aspx?RequestID=<%= RequestID %>'><span class="un">Вернуться</span></a></p>
		        <div class="clear"></div>
            
        <p style="color:Red;" runat="server" id="pErrorMessage"></p>    
		<asp:PlaceHolder runat="server" ID="phActivate">
            <div class="statement_in statement1">
                <p><strong><span runat="server" id="spanConfirmMessage"></span></strong></p>
            </div>
            <p class="save">
				<asp:Button runat="server" ID="btnAction" CssClass="bt" onclick="btnAction_Click" />
			</p>
		</asp:PlaceHolder>
		<asp:PlaceHolder runat="server" ID="phRevision">
            <div class="statement_in statement1" style="width: 610px;">
			    <p><strong>Пожалуйста, введите причину отправки на доработку заявки под номером "<%= RequestID %>"</strong></p>
				<p><asp:TextBox runat="server" ID="txtCause" TextMode="MultiLine" Rows="5" CssClass="textareaoverflowauto" Width="600"  /></p>
				<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCause"
					EnableClientScript="false" Display="None" ErrorMessage='Поле "Причина" обязательно для заполнения' />
            </div>
			<p class="save">
				<asp:Button runat="server" ID="btnSendToRevision" Text="Отправить на доработку" CssClass="bt"
							OnClick="btnSendToRevision_Click" />
			</p>

		</asp:PlaceHolder>
        
		<asp:PlaceHolder runat="server" ID="phSuccess" Visible="false">
            <div class="statement_in statement1" style="border-bottom: 0px;">
			    <p>Действие выполнено успешно.</p>
            </div>
		</asp:PlaceHolder>
    </div>
    </div>
</div>
</form>
</asp:Content> 