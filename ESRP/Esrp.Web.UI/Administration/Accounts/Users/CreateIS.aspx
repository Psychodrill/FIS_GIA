<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateIS.aspx.cs" Inherits="Esrp.Web.Administration.Accounts.Users.CreateIS"
    MasterPageFile="~/Common/Templates/Administration.Master" %>

<%--
    Права доступа к странице определяются в web.config секцией: 
    <location path="Administration/Accounts/SupportUsers/Create.aspx">
--%>
<asp:Content runat="server" ContentPlaceHolderID="cphHead">
    
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <div class="left_col">
        <form runat="server">
        <asp:ValidationSummary CssClass="error_block" runat="server" DisplayMode="BulletList"
            EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />
        <div class="col_in">
            <div class="statement edit">
                <p class="title">
                    Новый пользователь</p>
                <p class="back">
                    <a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
                <p class="statement_menu">
                    <a href="#" onclick="$('#<%= btnUpdate.ClientID %>').click();" class="active" handleclick="false">
                        <span>Сохранить</span>
                    </a>
                </p>
                <div class="clear">
                </div>
                <div class="statement_table">
                    <table style="width: 100%;">
                        <tr>
                            <th>
                                Логин/E-mail
                            </th>
                            <td width="1">
                                <asp:TextBox runat="server" ID="txtLogin" CssClass="txt long" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLogin" EnableClientScript="false"
                            Display="None" ErrorMessage='Поле "Логин/E-mail" обязательно для заполнения' />
                        <asp:CustomValidator runat="server" ID="vldLogin" EnableClientScript="false" ControlToValidate="txtLogin"
                            Display="None" OnServerValidate="vldLogin_ServerValidate" EnableViewState="false"
                            ErrorMessage='Логин "{0}" занят' />
                        <tr>
                            <th>
                                Фамилия
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtLastName" CssClass="txt long" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLastName" EnableClientScript="false"
                            Display="None" ErrorMessage='Поле "Фамилия" обязательно для заполнения' />
                        <tr>
                            <th>
                                Имя
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtFirstName" CssClass="txt long" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName" EnableClientScript="false"
                            Display="None" ErrorMessage='Поле "Имя" обязательно для заполнения' />
                        <tr>
                            <th>
                                Отчество
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtPatronymicName" CssClass="txt long" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPatronymicName"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "Отчество" обязательно для заполнения' />
                        <tr>
                            <th>
                                Группа
                            </th>
                            <td class="text">
                                <asp:CheckBoxList runat="server" ID="cblGroup" AppendDataBoundItems="true" DataValueField="Id"
                                    DataTextField="Name"  CssClass="checkbox-box-list" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Телефон
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtPhone" CssClass="txt small" />
                            </td>
                        </tr>
                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtLogin" EnableClientScript="false"
                            Display="None" ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*\.{0,1}@([0-9a-zA-Z]*[-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                            ErrorMessage='Поле "Логин/E-mail" заполнено неверно' />
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
        InitConfirmation('', '<%=this.Request.Form["state"]%>');
    </script>
</asp:Content>
