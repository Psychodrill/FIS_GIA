<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IntrantProfileEdit.ascx.cs"
    Inherits="Esrp.Web.Controls.IntrantProfileEdit" %>
<% if (HttpContext.Current.User.IsInRole("EditSelfAccount"))
   { %>
<asp:ValidationSummary CssClass="error_block" runat="server" DisplayMode="BulletList"
    EnableClientScript="false" ValidationGroup="IntrantProfile" HeaderText="<p>Произошли следующие ошибки:</p>" />
    <div class="statement_table">
        <table width="100%">
            <tr>
                <th style="border-top-color: #fff;">
                    Логин/E-mail
                </th>
                <td style="border-top-color: #fff;">
                    <asp:Literal runat="server" ID="litUserName" />
                </td>
            </tr>
            <tr>
                <th>
                    Фамилия
                </th>
                <td width="1">
                    <asp:TextBox runat="server" ID="txtLastName" CssClass="txt long" />
                </td>
            </tr>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLastName" EnableClientScript="false"
                Display="None" ValidationGroup="IntrantProfile" ErrorMessage='Поле "Фамилия" обязательно для заполнения' />
            <tr>
                <th>
                    Имя
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtFirstName" CssClass="txt long" />
                </td>
            </tr>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName" EnableClientScript="false"
                Display="None" ValidationGroup="IntrantProfile" ErrorMessage='Поле "Имя" обязательно для заполнения' />
            <tr>
                <th>
                    Отчество
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtPatronymicName" CssClass="txt long" />
                </td>
            </tr>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPatronymicName"
                EnableClientScript="false" Display="None" ValidationGroup="IntrantProfile" ErrorMessage='Поле "Отчество" обязательно для заполнения' />
            <tr>
                <th>
                    Телефон&nbsp;(с&nbsp;указанием&nbsp;кода&nbsp;города)
                </th>
                <td>
                    <asp:TextBox runat="server" ID="txtPhone" CssClass="txt small" />
                </td>
            </tr>
            <tr>
                <td colspan="2" class="box-submit" style="border-bottom-color: #fff;">
                    <asp:Button runat="server" ID="btnUpdate" Text="Сохранить" CssClass="bt" OnClick="btnUpdate_Click"
                        ValidationGroup="IntrantProfile" />
                </td>
            </tr>
        </table>
    </div>
<input type="hidden" name="state" />
<script language="javascript" type="text/javascript">
    InitConfirmation('', '<%= Request.Form["state"] %>');
</script>
<% }
   else
   { %>
<p>
    Вы не имеете прав для редактирования регистрационных данных.<br />
    Перейдите на страницу просмотра <a href="/Profile/View.aspx" title="Регистрационные данные">
        регистрационных данных</a>.</p>
<% } %>