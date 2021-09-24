<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditIS.aspx.cs" Inherits="Esrp.Web.Administration.Accounts.Users.EditIS"
    MasterPageFile="~/Common/Templates/Administration.Master" %>

<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>
<%--
    Права доступа к странице определяются в web.config секцией: 
    <location path="Administration/Accounts/SupportUsers/Edit.aspx">
--%>
<asp:Content runat="server" ContentPlaceHolderID="cphHead">
    
    
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <div class="left_col">
        <form id="Form2" runat="server">
        <asp:ValidationSummary CssClass="error_block" ID="ValidationSummary2" runat="server"
            DisplayMode="BulletList" EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />
        <div class="col_in">
            <div class="statement edit">
                <p class="title">
                    Логин/Е-mail&nbsp;<%= CurrentUser.Login %></p>
                <p class="back">
                    <a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
                <p class="statement_menu">
                    <a href="#" onclick="$('#<%= btnUpdate.ClientID %>').click();" class="active"><span>
                        Сохранить</span></a> <a href="/Administration/Accounts/Users/ChangePassword.aspx?Login=<%= Login %>&UserKey=IS"
                            title=""><span>Изменить пароль</span></a> <a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>&UserKey=IS"
                                title="История изменений" class="gray"><span>История изменений</span></a>
                    <a href="/Administration/Accounts/Users/AuthenticationHistory.aspx?Login=<%= Login %>&UserKey=IS"
                        title="История аутентификаци" class="gray"><span>История аутентификаций</span></a>
                    <a href="/Administration/Accounts/Users/AccountKeyList.aspx?Login=<%= Login %>&UserKey=IS"
                        title="Ключи доступа" class="gray"><span>Ключи доступа</span></a>
                </p>
                <div class="clear">
                </div>
                <div class="statement_table">
                    <table width="100%">
                        <tr>
                            <th>
                                Фамилия
                            </th>
                            <td width="1">
                                <asp:TextBox runat="server" ID="txtLastName" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLastName"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "Фамилия" обязательно для заполнения' />
                        <tr>
                            <th>
                                Имя
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtFirstName" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFirstName"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "Имя" обязательно для заполнения' />
                        <tr>
                            <th>
                                Отчество
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtPatronymicName" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFirstName"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "Отчество" обязательно для заполнения' />
                        <tr>
                            <th>
                                Телефон
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtPhone" />
                            </td>
                        </tr>
                        <asp:PlaceHolder ID="phAccessGroup" runat="server" Visible="true">
                            <tr>
                                <th style="vertical-align: top;">
                                    Группа
                                </th>
                                <td>
                                    <asp:CheckBoxList runat="server" ID="cblGroup" AppendDataBoundItems="true" RepeatColumns="3"
                                        DataValueField="Id" DataTextField="Name" CssClass="checkbox-box-list"/>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <tr>
                            <td colspan="2" style="border-bottom: 0px">
                                <asp:Button runat="server" ID="btnUpdate" Text="Сохранить" OnClick="btnUpdate_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <input type="hidden" name="state" />
        </form>
    </div>
    <script language="javascript" type="text/javascript">
        InitConfirmation('', '<%= Request.Form["state"] %>');
    </script>
</asp:Content>
