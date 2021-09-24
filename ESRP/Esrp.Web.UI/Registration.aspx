<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="Esrp.Web.Registration"
    MasterPageFile="~/Common/Templates/Main.master" ValidateRequest="False" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="Esrp.Core.Systems" %>
<%@ Import Namespace="Esrp.Web" %>
<%@ Register TagPrefix="esrp" TagName="InformationSystemList" Src="~/Controls/InformationSystemsRegistration.ascx" %>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <script language="javascript" type="text/javascript">
        $(document).ready(function ()
        {
            var params = {
                changedEl: 'select',
                visRows: 7,
                scrollArrows: true
            }
            cuSel(params);

            function SetRadiBtnStyle(idtable)
            {
                $(idtable + ' tr td').css('border-top-color', '#fff');
                $(idtable + ' tr td').css('border-bottom-color', '#fff');
                $(idtable + ' td').css('padding-bottom', '0');
                $(idtable + ' td').css('padding-left', '0');
            }
        });
    </script>
    <form runat="server">
    <% if (!this.User.Identity.IsAuthenticated)
       { %>
    <h1>
        Шаг 1 из 3: "Ввод информации о пользователе"</h1>
    <br />
    <p class="l8" style="padding-bottom: 10px!important;">
        Заполняя данную форму, я даю свое согласие на обработку и публикацию своих персональных
        данных в общедоступных источниках персональных данных в целях обеспечения мероприятий
        по регистрации в информационных системах Рособрнадзора и ФГБУ «Федеральный центр
        тестирования».</p>
    <p class="l8" style="padding-bottom: 10px!important;">
        В случае указания в форме персональных данных третьих лиц, я гарантирую, что указанные
        данные являются общедоступными либо соответствующее согласие этих лиц на обработку
        их персональных данных мною получено.</p>
    <p class="l8" style="padding-bottom: 10px!important;">
        Данное согласие является бессрочным и может быть отозвано только на основании моего
        личного заявления.</p>
    <p class="l8" style="padding-bottom: 10px!important;">
        Поля, помеченные символом <span style="color: rgb(215, 5, 5) !important;">
            <%=this.Page.Required()%></span>, обязательны для заполнения!
        <asp:Panel runat="server" ID="pnlNotification" Visible="false" Width="630px">
            <div class="statement edit">
                <div class="top">
                    <div class="l">
                    </div>
                    <div class="r">
                    </div>
                    <div class="m">
                    </div>
                </div>
                <div class="cont" style="background: none;">
                    <div class="">
                        <p>
                            <img src="/Common/Images/notice-block-notice.gif" width="22" height="22" alt="!"
                                class="ico-notice" />
                            Ваша организация не найдена в справочнике.
                            <br />
                            Для продолжения регистрации заполните все поля данной регистрационной формы.
                            <br />
                            Организация будет добавлена в справочник после проверки введенных Вами данных администратором
                            <%=GeneralSystemManager.GetSystemName(2)%>.
                        </p>
                    </div>
                </div>
                <div class="bottom">
                    <div class="l">
                    </div>
                    <div class="r">
                    </div>
                    <div class="m">
                    </div>
                </div>
            </div>
        </asp:Panel>
    </p>
    <asp:ValidationSummary CssClass="error_block" runat="server" ID="vsErrors" DisplayMode="BulletList"
        EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />
    <%--<fieldset class="block">--%>
    <br>
    <div class="statement_table">
        <h2>
            Общая информация об организации</h2>
        <table width="100%">
            <tr>
                <th style="border-top-color: #fff;">
                    Полное наименование организации (без организационно-правовой формы)
                </th>
                <td width="1" style="border-top-color: #fff;">
                    <table width="100%" cellpadding="0" cellspacing="0" style="padding: 0px!important;">
                        <tr>
                            <td style="border-bottom: 0px; border-top: 0px; padding-top: 0px; padding-left: 0px;
                                padding-bottom: 0px; padding-right: 10px;">
                                <asp:TextBox runat="server" ID="TbFullName" CssClass="textareaoverflowauto" autocomplete="off"
                                    TextMode="MultiLine" Rows="5" Height="100px" MaxLength="1000" Width="410" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TbFullName"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "Полное наименование организации" обязательно для заполнения' />
                            </td>
                            <td width="100px" style="border-bottom: 0px; border-top: 0px; vertical-align: top!important;
                                padding-top: 0;">
                                <asp:Button ID="btnChangeOrg" runat="server" PostBackUrl="/SelectOrg.aspx?BackUrl=./Registration.aspx"
                                    Text="Выбрать" Width="100px" ToolTip="Выбор организации" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server" id="RowOrgType">
                <th>
                    Тип организации
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="DDLOrgTypes" CssClass="sel long" DataValueField="Id"
                        DataTextField="Name" AppendDataBoundItems="true">
                        <asp:ListItem Value="">&lt;Не задано&gt;</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr runat="server" id="RowOPF">
                <th>
                    Организационно-правовая форма
                </th>
                <td style="vertical-align: middle;">
                    <asp:DropDownList runat="server" ID="DDLOPF" CssClass="sel">
                        <asp:ListItem Value="">&lt;Не задано&gt;</asp:ListItem>
                        <asp:ListItem Text="Государственный" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Негосударственный" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr runat="server" id="RowRegion">
                <th>
                    Субъект Российской Федерации, на территории которого находится организация
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="DDLOrganizationRegion" AppendDataBoundItems="true"
                        CssClass="sel" DataValueField="Id" DataTextField="Name">
                        <asp:ListItem Value="">&lt;Не задано&gt;</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    Город
                </th>
                <td colspan="2">
                    <asp:TextBox Width="520" runat="server" ID="TBTownName" CssClass="textareaoverflowauto"
                        Rows="3" Height="50px" TextMode="MultiLine" MaxLength="255" />
                </td>
            </tr>
            <tr runat="server" id="RowJurAddress">
                <th>
                    Юридический адрес
                </th>
                <td colspan="2">
                    <asp:TextBox Width="520" runat="server" ID="TBJurAddress" CssClass="textareaoverflowauto"
                        Rows="3" Height="50px" TextMode="MultiLine" MaxLength="255" />
                </td>
            </tr>
            <tr>
                <th>
                    ОГРН
                </th>
                <td class="text">
                    <asp:TextBox runat="server" ID="tbOGRN" CssClass="txt small" MaxLength="13" />
                </td>
            </tr>
            <tr>
                <th>
                    ИНН
                </th>
                <td class="text">
                    <asp:TextBox runat="server" ID="tbINN" CssClass="txt small" MaxLength="10" />
                </td>
            </tr>
            <tr>
                <th>
                    КПП
                </th>
                <td colspan="2">
                    <asp:TextBox runat="server" ID="tbKPP" CssClass="txt long" MaxLength="9" />
                </td>
            </tr>
            <tr runat="server" id="RowPhone">
                <th>
                    Телефон
                </th>
                <td>
                    <asp:TextBox runat="server" ID="TBPhone" CssClass="txt small" MaxLength="100" />
                </td>
            </tr>
            <tr runat="server" id="RowFax">
                <th>
                    Факс
                </th>
                <td colspan="2">
                    <asp:TextBox runat="server" ID="TBFax" CssClass="txt small" MaxLength="100" />
                </td>
            </tr>
        </table>
    </div>
    <%--</fieldset>--%>
    <esrp:InformationSystemList runat="server" ID="informationSystemList"></esrp:InformationSystemList>
    <asp:Button runat="server" ID="btnRegister" Text="Сохранить" CssClass="bt" OnClick="btnRegister_Click"
        Width="100px" />
    <% }
       else
       { %>
    <p class="l8">
        Вы уже зарегистрированы.<br />
        Перейти на страницу <a href="/Profile/View.aspx" title="Регистрационные данные">регистрационных
            данных</a>.</p>
    <% } %>
    </form>
</asp:Content>
