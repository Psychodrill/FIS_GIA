<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppSuccess.aspx.cs" Inherits="Esrp.Web.ApplicationFCT.AppSuccess"
MasterPageFile="~/Common/Templates/Main.master" ValidateRequest="False" MaintainScrollPositionOnPostback="true" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <form id="form1" runat="server">	
				<h1>Результат</h1>
				<div class="statement edit">               
					<p class="back" style="margin-bottom:20px;">
						<a href="AppStep2.aspx"><span class="un">Вернуться</span></a>
					</p>
				</div>
                <br/>
				<h2>Заявка успешно сформирована!</h2>
				<br/>
                <p>Пожалуйста, не изменяйте имя файла сформированной заявки. Это ускорит процесс ее обработки.</p>
                <br/>
                <br/>
				<asp:linkbutton runat="server" onclick="ShowApp">Посмотреть заявку в виде PDF файла</asp:linkbutton>	
    </form>
</asp:Content>