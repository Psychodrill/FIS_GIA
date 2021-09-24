<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Esrp.Web.Login"
    MasterPageFile="~/Common/Templates/Main.master" %>
<%@ Import Namespace="Esrp.Core.Systems" %>
<%@ Import Namespace="Esrp.Web" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">

    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>

<% // стандартный механизм не позволяет обрабатывать ошибку 401. поэтому выведу сообщение руками. %>
<% if (!String.IsNullOrEmpty(Request.QueryString["ReturnUrl"])) { %>
    <p><strong>
    У вас нет прав для просмотра выбранной страницы.<br/>
    Пожалуйста, авторизуйтесь.
    </strong></p>
<% } %>

<form runat="server">

    <asp:ValidationSummary  CssClass="error_block" runat="server" DisplayMode="BulletList" EnableClientScript="false"
        HeaderText="<p>Произошли следующие ошибки:</p>"/> 

    <% // форму логина скрою  %>
    <table class="form" style="display: none;">
    <tr>
        <td class="left">Логин</td>
        <td><asp:TextBox runat="server" ID="txtLogin" CssClass="txt short" /></td>
    </tr>

    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLogin" 
        EnableClientScript="false" Display="None"
        ErrorMessage='Поле "Логин" обязательно для заполнения' /> 
        
    <tr>
        <td class="left">Пароль</td>
        <td><asp:TextBox runat="server" ID="txtPassword" TextMode="Password" CssClass="txt short" /></td>
    </tr>

    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" 
        EnableClientScript="false" Display="None"
        ErrorMessage='Поле "Пароль" обязательно для заполнения' />

    <asp:CustomValidator ID="cvInvalidCredentials" runat="server" 
        EnableClientScript="false" Display="None"
        ErrorMessage='Неверный логин пользователя или пароль.' />

    <% // Скрою блок, если DisableRememberMe установлено в True  %>
    <tr runat="server" id="trRememberMe">
        <td class="left"><br/></td>
        <td>
            <asp:CheckBox runat="server" ID="cbPersistant" Text="Запомнить меня" />

            <div class="notice" id="LoginNotice">
            <div class="top"><div class="l"></div><div class="r"></div><div class="m"></div></div>
            <div class="cont">
            <div dir="ltr" class="hide" title="Свернуть" onclick="ToggleNoticeState(this);">x<span></span></div>
            <div class="txt-in">
                <p>В целях информационной безопасности флажок "Запомнить меня" рекомендуется 
                    использовать, если доступ к данному компьютеру имеется исключительно у лица,
                    ответственного за работу с <%= GeneralSystemManager.GetSystemName(2)%>.</p>
            </div>
            </div>
            <div class="bottom"><div class="l"></div><div class="r"></div><div class="m"></div></div>
            </div>
        </td>
    </tr>

    <tr>
		<td colspan="2" class="box-submit">
		<asp:Button runat="server" id="btnLogin" Text="Войти" onclick="btnLogin_Click" CssClass="bt" /><br />
        <% if (GeneralSystemManager.IsOpenSystem())
           {%>
                <a href="/Registration.aspx" class="user" title="Регистрация">Регистрация</a><br />
                <a href="/RemindPassword.aspx" class="exit" title="Напомнить пароль">Напомнить пароль</a>	
        <% } %>	
		</td>
    </tr>
    </table>

    <script type="text/javascript">
        InitNotice();
    </script>

</form>
</asp:Content>
