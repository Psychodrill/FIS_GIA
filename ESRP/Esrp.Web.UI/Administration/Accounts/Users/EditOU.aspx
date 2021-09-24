<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditOU.aspx.cs" Inherits="Esrp.Web.Administration.Accounts.Users.EditOU"
    MasterPageFile="~/Common/Templates/Administration.Master" %>

<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>
<%@ Import Namespace="Esrp.Core.Systems" %>
<%--
    Права доступа к странице определяются в web.config секцией: 
    <location path="Administration/Accounts/SupportUsers/Edit.aspx">
--%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHead">
    
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphContent">
    <div class="left_col">
        <form id="Form1" runat="server">
        <asp:ValidationSummary CssClass="error_block" ID="ValidationSummary1" runat="server"
            DisplayMode="BulletList" EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />
        <div class="col_in">
            <div class="statement edit" >
                <p class="title">
                    Логин/Е-mail&nbsp;<%= CurrentUser.Login %></p>
                <p class="back">
                    <a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
                <p class="statement_menu">
                    <a href="#" onclick="$('#<%= btnUpdate.ClientID %>').click();" class="active">
                        <span>Сохранить</span>
                    </a> 
                    <a href="/Administration/Accounts/Users/ChangePassword.aspx?Login=<%= Login %>&UserKey=OU" title="">
                        <span>Изменить пароль</span>
                    </a> 
                    <%if (!GeneralSystemManager.CanChangeRCModel(this.User.Identity.Name))
                      { %>
                        <a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>&UserKey=OU" title="История изменений" class="gray">
                            <span>История изменений</span>
                        </a>
                        <a href="/Administration/Accounts/Users/AuthenticationHistory.aspx?Login=<%= Login %>&UserKey=OU" title="История аутентификаци" class="gray">
                            <span>История аутентификаций</span>
                        </a>
                        <a href="/Administration/Accounts/Users/AccountKeyList.aspx?Login=<%= Login %>&UserKey=OU" title="Ключи доступа" class="gray">
                            <span>Ключи доступа</span>
                        </a>
                    <%} %>
                </p>
                <div class="clear">
                </div>
                <div class="statement_table" style="width: 730px;">
                    <table>
                        <tr>
                            <th>
                                Фамилия
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtLastName" CssClass="txt long" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLastName"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "Фамилия" обязательно для заполнения' />
                        <tr>
                            <th>
                                Имя
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtFirstName" CssClass="txt long" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFirstName"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "Имя" обязательно для заполнения' />
                        <tr>
                            <th>
                                Отчество
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtPatronymicName" CssClass="txt long" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPatronymicName"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "Отчество" обязательно для заполнения' />
                        <tr runat="server" id="trAccessBlock">
                            <th>
                                Доступ к системе
                            </th>
                            <td class="text">
                                <asp:CheckBoxList runat="server" ID="cblSystems" AppendDataBoundItems="true" DataValueField="SystemID"
                                    DataTextField="Name" CssClass="checkbox-box-list selects-systems">
                                    </asp:CheckBoxList>   
                                <asp:CustomValidator runat="server" ID="cvSystems" EnableClientScript="false" Display="None"
                                    ErrorMessage='Выберите хотя бы одну информационную систему' />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Группы
                            </th>
                            <td>
                                <asp:CheckBoxList runat="server" ID="chblGroup" CssClass="selects-groups" AppendDataBoundItems="true"
                                    DataValueField="Code" DataTextField="Name">
                                </asp:CheckBoxList>
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
        $(document).ready(function ()
        {
            ToggleGroupsInput();
            $(".selects-systems input[type=checkbox]").click(function ()
            {
                ToggleGroupsInput();
            });
        });
        function ToggleGroupsInput()
        {
            //Некрасиво, но CheckBoxList не отображает value
            var fisChecked = false;
            $(".selects-systems input[type=checkbox]:checked + label").each(function (index, element)
            {
                if ($(element).text() == "ФИС ЕГЭ и приема")
                {
                    fisChecked = true;
                }
            });

            if (fisChecked)
            {
                $(".selects-groups").closest("tr").show();
            }
            else
            {
                $(".selects-groups").closest("tr").hide();
            }
        } 
    </script>
</asp:Content>
