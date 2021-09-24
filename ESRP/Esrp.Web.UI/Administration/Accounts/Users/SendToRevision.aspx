<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendToRevision.aspx.cs" 
    Inherits="Esrp.Web.Administration.Accounts.Users.SendToRevision"
    MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>

<%--
    Права доступа к странице определяются в web.config секцией: 
    <location path="Administration/Accounts/Users/SendToRevision.aspx">
--%>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<form runat="server">
    <asp:ValidationSummary CssClass="error_block"  runat="server" DisplayMode="BulletList" EnableClientScript="false"
        HeaderText="<p>Произошли следующие ошибки:</p>"/>
        
    <% if (!UserAccount.CheckNewUserAccountEmail(CurrentUser.Email)) {%>
    <p>
    Внимание: существуют 
    <a href="/Administration/Accounts/Users/List<%= GetUserKeyCode() %>.aspx?email=<%= HttpUtility.UrlEncode(CurrentUser.Email)%>">пользователи</a>
    с таким же e-mail.</p>
    <p><a href='/Administration/Accounts/Users/Edit<%= GetUserKeyCode() %>.aspx?login=<%= CurrentUser.Login %>'>Вернуться к редактированию</a></p>
    <%} %>
    
    <table class="form">
        <tr>
            <td class="left">Логин</td>
            <td class="text"><%= CurrentUser.Login %></td>
        </tr>
        <tr>
            <td class="left">Причина</td>
            <td><asp:TextBox runat="server" ID="txtCause" TextMode="MultiLine" Rows="5"  
                CssClass="txt-area long"/></td>
        </tr>
        
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCause" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "Причина" обязательно для заполнения' /> 
        
        <tr>
			<td colspan="2" class="box-submit">
			<asp:Button runat="server" ID="btnSendToRevision" Text="Отправить на доработку" CssClass="bt" 
			    onclick="btnSendToRevision_Click" />
			</td>
        </tr>       
    </table>        
</form>
</asp:Content>    
