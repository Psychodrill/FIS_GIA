<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Deactivate.aspx.cs" Inherits="Esrp.Web.Administration.Accounts.Users.Deactivate"
    MasterPageFile="~/Common/Templates/Administration.Master" %>

<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>
<%--
    Права доступа к странице определяются в web.config секцией: 
    <location path="Administration/Accounts/Users/Deactivate.aspx">
--%>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <div class="left_col">
        <form id="Form2" runat="server">
        <p style="color: Red;" runat="server" id="pErrorMessage">
        </p>
        <asp:ValidationSummary CssClass="error_block" ID="ValidationSummary1" runat="server"
            DisplayMode="BulletList" EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />
        <div class="col_in">
            <div class="statement edit">
                <p class="title">
                    Логин/Е-mail&nbsp;<%= CurrentOrgUser.login %></p>
                <p class="back">
                    <a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
                <p class="statement_menu">
                    <a href="/Administration/Accounts/Users/Edit<%= GetUserKeyCode() %>.aspx?Login=<%= Login %>"
                        class="gray" title="Изменить"><span>Изменить</span></a>
                    <% if (User.IsInRole("ActivateDeactivateUsers"))
                       {
                           if (CurrentOrgUser.CanBeActivated())
                           { %>
                    <a href="/Administration/Accounts/Users/Activate.aspx?Login=<%= Login %>" title="Активировать"
                        class="gray">Активировать</a>
                    <%     }
                           if (CurrentOrgUser.CanBeDeactivated())
                           { %>
                    <a href="/Administration/Accounts/Users/Deactivate.aspx?Login=<%= Login %>" title="Отключить"
                        class="active">Отключить</a>
                    <%     }
                       } %>
                    <% if (CurrentOrgUser.status != UserAccount.UserAccountStatusEnum.Deactivated)
                       { %>
                    <a href="/Administration/Accounts/Users/RemindPassword.aspx?Login=<%= Login %>" title="Напомнить пароль"
                        class="gray"><span>Напомнить пароль</span></a>
                    <% } %>
                    <a href="/Administration/Accounts/Users/ChangePassword.aspx?Login=<%= Login %>" class="gray"
                        title="Изменить пароль"><span>Изменить пароль</span></a> <a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>"
                            title="История изменений" class="gray"><span>История изменений</span></a>
                    <a href="/Administration/Accounts/Users/AuthenticationHistory.aspx?Login=<%= Login %>"
                        title="История аутентификаци" class="gray"><span>История аутентификаций</span></a>
                </p>
                <div class="clear">
                </div>
                <div class="statement_table">
                    <table width="100%">
                        <tr>
                            <th>Причина</th>
                            <td width="1">
                                <asp:TextBox runat="server" ID="txtCause" TextMode="MultiLine" Rows="5" CssClass="textareaoverflowauto" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCause" EnableClientScript="false"
                                    Display="None" ErrorMessage='Поле "Причина" обязательно для заполнения' />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="border-bottom: 0px">
                                <asp:Button runat="server" ID="btnDeactivate" Text="Отключить" CssClass="bt" OnClick="btnDeactivate_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        </form>
    </div>
</asp:Content>
