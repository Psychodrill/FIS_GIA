<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Fbs.Web.Login"
    MasterPageFile="~/Common/Templates/Loginless.master" %>

<%@ Import Namespace="Fbs.Web" %>
<%@ Register TagPrefix="c" TagName="LoginControl" Src="~/LoginControl.ascx" %>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">

    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>

    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>

    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>

    <% // стандартный механизм не позволяет обрабатывать ошибку 401. поэтому выведу сообщение руками. %>
    <% if (!String.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
       { %>
    <p class="l8">
        У вас нет прав для просмотра выбранной страницы.<br />
        Пожалуйста, авторизуйтесь.</p>
    <% } %>
    <p class="l8">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </p>
    <table class="form" style="width: 100%" id="Table1">
        <tr>
            <td class="box-submit">
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <c:LoginControl runat="server" ID="loginControl" />
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        InitNotice();
    </script>

</asp:Content>
