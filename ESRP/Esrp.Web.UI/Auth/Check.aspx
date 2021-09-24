<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Check.aspx.cs" Inherits="Esrp.Web.Auth.Check" MasterPageFile="~/Common/Templates/Loginless.master" %>
<%@ Import Namespace="Esrp.Core.Systems" %>


<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphContent">

    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>

<form id="Form1" runat="server">
        
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLogin" 
        EnableClientScript="false" Display="None"
        ErrorMessage='Поле "Логин" обязательно для заполнения' /> 

    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword" 
        EnableClientScript="false" Display="None"
        ErrorMessage='Поле "Пароль" обязательно для заполнения' />

    <asp:CustomValidator ID="cvInvalidCredentials" runat="server" 
        EnableClientScript="false" Display="None"
        ErrorMessage='Неверный логин пользователя или пароль.' />
        
    <div class="form centered">
        <p class="title">Для входа в cистему введите логин и пароль</p>
        <asp:ValidationSummary ID="ValidationSummary1"  CssClass="error_block" runat="server" DisplayMode="BulletList" EnableClientScript="false"
        HeaderText="<p>Произошли следующие ошибки:</p>"/> 
        <div class="form_in">
		    <p>Логин<br /><asp:TextBox runat="server" ID="txtLogin" CssClass="txt short" /></p>
		    <p>Пароль<br /><asp:TextBox runat="server" ID="txtPassword" TextMode="Password" CssClass="txt short" /></p>
		    <p><asp:Button runat="server" id="btnLogin" Text="Войти" onclick="btnLogin_Click" CssClass="bt" />
            <%--<asp:Button runat="server" id="btnCancel" Text="Вернуться" onclick="btnCancel_Click" CssClass="bt" /></p>--%>
            <p><asp:PlaceHolder runat="server" ID="trRememberMe">
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
                </asp:PlaceHolder></p>
            <% if (GeneralSystemManager.IsOpenSystem())
               {%>
	                <p><a href="/RemindPassword.aspx" class="un">Напомнить пароль</a></p>       
                    <p><a href="/Registration.aspx" class="un">Регистрация</a></p>
            <% } %>	
        </div>
    </div>

    <script type="text/javascript">
        InitNotice();
    </script>

</form>
</asp:Content>
