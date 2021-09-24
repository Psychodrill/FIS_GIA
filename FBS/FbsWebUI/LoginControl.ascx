<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginControl.ascx.cs"
    Inherits="Fbs.Web.LoginControl" %>
<div style="width: 250px;" align="center">
    <% if (Context.User.Identity.IsAuthenticated)
       {%>
    <div class="user-block">
        <a href="/Profile/View.aspx" class="user" title="Регистрационные данные">Рег.&nbsp;данные</a>
        <a href="/Logout.aspx" class="exit" title="Выход">Выход</a>
    </div>
    <% }
       else
       { %>
    <div align="right" style="padding-right: 25px;">
        <form runat="server" id="formLogin">
        <span>
            <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
            <asp:Button runat="server" ID="btnLogin" Text="Войти в систему" OnClick="btnLogin_Click"
                CssClass="bt" />
        </span>
        </form>
        <br>
        <% if (System.Configuration.ConfigurationManager.AppSettings["EnableOpenedFbs"] == "True") { %>
            <div align="right">
                <asp:HyperLink ID="hlRegistration" runat="server" CssClass="user" ForeColor="#666666">Регистрация</asp:HyperLink><br />
                <asp:HyperLink ID="hlRemindPassword" runat="server" CssClass="exit" ForeColor="#666666">Напомнить пароль</asp:HyperLink>
            </div>
        <% } %>
    </div>
    <% } %>
</div>
