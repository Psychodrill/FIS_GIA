<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountKeyCreate.aspx.cs"
    Inherits="Esrp.Web.Administration.Accounts.Users.AccountKeyCreate" MasterPageFile="~/Common/Templates/Administration.Master" %>
     <%@ Register Src="~/Controls/DatePickerControl.ascx" TagPrefix="uc" TagName="DatePicker" %>
<asp:Content runat="server" ContentPlaceHolderID="cphHead">
    <script type="text/javascript" src="/Common/Scripts/CalendarPopup.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <div class="left_col">
        <form id="Form2" runat="server">
        <asp:ValidationSummary CssClass="error_block" ID="ValidationSummary2" runat="server"
            DisplayMode="BulletList" EnableClientScript="true" HeaderText="<p>Произошли следующие ошибки:</p>" />
        <div class="col_in">
            <div class="statement edit">
                <p class="title">
                    Логин/Е-mail&nbsp;<%= CurrentUserLogin %></p>
                <p class="back">
                    <a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
                <p class="statement_menu">
                    <a href="/Administration/Accounts/Users/Edit<%= GetUserKeyCode() %>.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>">
                        <span>Изменить</span></a> <a href="/Administration/Accounts/Users/ChangePassword.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>"
                            class="gray" title="Изменить пароль"><span>Изменить пароль</span></a> <a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>"
                                title="История изменений" class="gray"><span>История изменений</span></a>
                    <a href="/Administration/Accounts/Users/AuthenticationHistory.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>"
                        title="История аутентификаци" class="gray"><span>История аутентификаций</span></a>
                    <a href="/Administration/Accounts/Users/AccountKeyList.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>"
                        title="Ключи доступа" class="active"><span>Ключи доступа</span></a>
                </p>
                <div class="clear">
                </div>
                <div class="statement edit">
                    <p class="title">
                        Создать новый ключ доступа</p>
                </div>
                <div class="statement_table">
                    <table width="700">
                        <tr>
                            <th>
                                Действителен с
                            </th>
                            <td nowrap>
                                <uc:DatePicker runat="server" ID="txtDateFrom" MinDate="01.01.2000" />
                             
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Действителен по
                            </th>
                            <td nowrap>
                                <uc:DatePicker runat="server" ID="txtDateTo" MinDate="01.01.2000" />
                                
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Активировать ключ
                            </th>
                            <td nowrap>
                                <asp:CheckBox runat="server" ID="cbIsActive" />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Button runat="server" ID="btnCreate" Text="Создать" OnClick="btnCreate_Click" />
            </div>
        </div>
        <div id="CalendarContainer" style="position: absolute; visibility: hidden; background-color: white;
            z-index: 99;">
        </div>
        </form>
    </div>
</asp:Content>
