<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Fbs.Web.Administration.Accounts.Users.Edit"
    MasterPageFile="~/Common/Templates/Administration.Master" %>

<%@ Import Namespace="Fbs.Core" %>
<%@ Import Namespace="Fbs.Web" %>
<%--
    Права доступа к странице определяются в web.config секцией: 
    <location path="Administration/Accounts/Users/Edit.aspx">
--%>
<asp:Content runat="server" ContentPlaceHolderID="cphHead">

    <script src="/Common/Scripts/Confirmation.js" type="text/javascript"></script>

</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphActions">
    <div class="h10">
    </div>
    <div class="border-block">
        <div class="tr">
            <div class="tt">
                <div>
                </div>
            </div>
        </div>
        <div class="content">
            <ul>
                <% if (User.IsInRole("ActivateDeactivateUsers") &&
       CurrentUser.status != UserAccount.UserAccountStatusEnum.Activated)
                   { %>
                <li><a href="/Administration/Accounts/Users/Activate.aspx?Login=<%= Login %>" title="Активировать">
                    Активировать</a></li>
                <% } %>
                <% if (User.IsInRole("ActivateDeactivateUsers") &&
       CurrentUser.status != UserAccount.UserAccountStatusEnum.Deactivated)
                   { %>
                <li><a href="/Administration/Accounts/Users/Deactivate.aspx?Login=<%= Login %>" title="Отключить">
                    Отключить</a></li>
                <% } %>
                <% if (User.IsInRole("ActivateDeactivateUsers"))
                   { %>
                <li><a href="/Administration/Accounts/Users/SendToRevision.aspx?Login=<%= Login %>"
                    title="Отправить на доработку">Отправить на доработку</a></li>
                <% } %>
                <% if (User.IsInRole("ActivateDeactivateUsers") &&
       CurrentUser.status != UserAccount.UserAccountStatusEnum.Deactivated)
                   { %>
                <li><a href="/Administration/Accounts/Users/RemindPassword.aspx?Login=<%= Login %>"
                    title="Напомнить пароль">Напомнить пароль</a></li>
                <% } %>
                <li><a href="/Administration/Accounts/Users/ChangePassword.aspx?Login=<%= Login %>"
                    title="Изменить пароль">Изменить пароль</a></li>
            </ul>
            <div class="split">
            </div>
            <ul>
                <li><a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>" title="История изменений"
                    class="gray">История изменений</a></li>
                <li><a href="/Administration/Accounts/Users/AuthenticationHistory.aspx?Login=<%= Login %>"
                    title="История аутентификаций" class="gray">История аутентификаций</a></li>
            </ul>
        </div>
        <div class="br">
            <div class="tt">
                <div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <form runat="server">
    <asp:ValidationSummary runat="server" DisplayMode="BulletList" EnableClientScript="false"
        HeaderText="<p>Произошли следующие ошибки:</p>" />
    <table class="form">
        <tr>
            <td class="left">
                Логин
            </td>
            <td class="text">
                <asp:Literal runat="server" ID="litUserName" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="left">
                Состояние
            </td>
            <td class="text">
                <b>
                    <asp:Literal runat="server" ID="litStatus" /></b>
                <br />
                <asp:Literal runat="server" ID="litStatusDescription" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="left">
                Полное&nbsp;наименование&nbsp;организации
            </td>
            <td>
                <asp:HyperLink runat="server" Visible="false" ID="OrganizationName_hyperlynk"></asp:HyperLink>
                <asp:TextBox runat="server" ID="OrganizationName_txt" CssClass="txt long" MaxLength="1000"/>
                <asp:HiddenField runat="server" ID="OrganizationName_hiddenField" />
            </td>
            <td>
                <asp:Button ID="btnChangeOrg" runat="server" Text="Выбрать" Width="100px" ToolTip="Выбор организации" />
            </td>
        </tr>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="OrganizationName_txt"
            EnableClientScript="false" Display="None" ID="OrganizationName_txt_validator"
            ErrorMessage='Поле "Полное наименование организации" обязательно для заполнения' />
        <tr>
            <td class="left">
                Краткое&nbsp;наименование&nbsp;организации
            </td>
            <td colspan="2">
                <asp:TextBox runat="server" ID="TBShortName" CssClass="txt long" Rows="5" Height="100" MaxLength="500"/>
            </td>
        </tr>
        <tr>
            <td class="left">
                Субъект Российской Федерации, на территории которого находится организация
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlOrganizationRegion" AppendDataBoundItems="true"
                    CssClass="sel long" DataValueField="Id" DataTextField="Name" Width="370px">
                    <asp:ListItem Value="0">&lt;Не задано&gt;</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td rowspan="13">
            </td>
        </tr>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlOrganizationRegion"
            EnableClientScript="false" Display="None" ErrorMessage='Поле "Субъект Российской Федерации" обязательно для заполнения' />
        <tr>
            <td class="left">
                Тип ОУ
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlEducationInstitutionType" CssClass="sel small"
                    AppendDataBoundItems="true" DataValueField="Id" DataTextField="Name">
                    <asp:ListItem Value="">&lt;Не задано&gt;</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlEducationInstitutionType"
            EnableClientScript="false" Display="None" ErrorMessage='Поле "Тип ОУ" обязательно для заполнения' />
        <tr>
            <td class="left">
                Вид ОУ
            </td>
            <td>
                <asp:DropDownList runat="server" ID="DLLOrgKind" CssClass="sel small" AppendDataBoundItems="true"
                    DataValueField="Id" DataTextField="Name" >
                    <asp:ListItem Value="">&lt;Не задано&gt;</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="VReqOrgKind" ControlToValidate="DLLOrgKind" ErrorMessage='Поле "Вид ОУ" обязательно для заполнения' Display="None" EnableClientScript="false"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="left">
                Организационно-правовая форма ОУ
            </td>
            <td>
                <asp:DropDownList runat="server" ID="DDLOPF" CssClass="sel small">
                    <asp:ListItem Value="0">Государственный</asp:ListItem>
                    <asp:ListItem Value="1">Негосударственный</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="left">
                Является филиалом
            </td>
            <td>
                <asp:CheckBox runat='server' ID="ChIsFilial" />
            </td>
        </tr>
        <tr>
            <td class="left">
                ИНН
            </td>
            <td>
                <asp:TextBox runat="server" ID="TBINN" CssClass="txt small" MaxLength="10"/>
                   <asp:RequiredFieldValidator ID="VReqINN" runat="server" ControlToValidate="TBINN"
                    EnableClientScript="false" Display="None" ErrorMessage='Поле "ИНН" обязательно для заполнения' />
                    <asp:RegularExpressionValidator runat="server" ID="VRegexINN" ValidationExpression="[0-9]{10}"
                    ControlToValidate="TBINN"
                    EnableClientScript="false" Display="None" ErrorMessage='Поле "ИНН" заполнено неверно' />
    
            </td>
        </tr>
        <tr>
            <td class="left">
                ОГРН
            </td>
            <td>
                <asp:TextBox runat="server" ID="TBOGRN" CssClass="txt small" MaxLength="13" />
                   <asp:RequiredFieldValidator ID="VReqOGRN" runat="server" ControlToValidate="TBOGRN"
                    EnableClientScript="false" Display="None" ErrorMessage='Поле "ОГРН" обязательно для заполнения' />
                    <asp:RegularExpressionValidator runat="server" ID="VRegexOGRN" ValidationExpression="[0-9]{13}"
                    ControlToValidate="TBOGRN"
                    EnableClientScript="false" Display="None" ErrorMessage='Поле "ОГРН" заполнено неверно' />
            </td>
        </tr>
        <tr>
            <td class="left">
                Учредитель (для&nbsp;ССУЗов&nbsp;и&nbsp;ВУЗов)
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtOrganizationFounderName" CssClass="txt long" MaxLength="255"/>
            </td>
        </tr>
        <asp:RequiredFieldValidator runat="server" ID="VReqFounderName" ControlToValidate="txtOrganizationFounderName"
            EnableClientScript="false" Display="None" ErrorMessage='Поле "Учредитель" обязательно для заполнения' />
        <tr>
            <td class="left">
                Должность руководителя организации
            </td>
            <td>
                <asp:TextBox runat="server" ID="TbDirectorPosition" CssClass="txt long" MaxLength="255"/>
            </td>
        </tr>
        <tr>
            <td class="left">
                Ф.И.О. руководителя организации
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtOrganizationChiefName" CssClass="txt long" MaxLength="255"/>
            </td>
        </tr>
        <asp:RequiredFieldValidator ID="VReqDirectorName" runat="server" ControlToValidate="txtOrganizationChiefName"
            EnableClientScript="false" Display="None" ErrorMessage='Поле "ФИО руководителя организации" обязательно для заполнения' />
        <tr>
            <td class="left">
                Фактический адрес
            </td>
            <td>
                <asp:TextBox runat="server" ID="TbFactAddress" CssClass="txt long" MaxLength="255"/>
                <asp:RequiredFieldValidator ID="VReqFactAddress" runat="server" ControlToValidate="TbFactAddress"
                    EnableClientScript="false" Display="None" ErrorMessage='Поле "Фактический адрес" обязательно для заполнения' />
            </td>
        </tr>
        <tr>
            <td class="left">
                Юридический адрес
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtOrganizationAddress" CssClass="txt long" MaxLength="255"/>
            </td>
        </tr>
        <asp:RequiredFieldValidator runat="server" ID="VReqLawAddress" ControlToValidate="txtOrganizationAddress"
            EnableClientScript="false" Display="None" ErrorMessage='Поле "Юридический адрес" обязательно для заполнения' />
        <tr>
            <td class="left">
                Свидетельство об аккредитации
            </td>
            <td>
                <asp:TextBox runat="server" ID="TBAccred" CssClass="txt long" MaxLength="255"/>
            </td>
        </tr>
        <tr>
            <td class="left">
                Код города
            </td>
            <td>
                <asp:TextBox runat="server" ID="TbPhoneCode" CssClass="txt small" MaxLength="10"/>
            </td>
        </tr>
        <tr>
            <td class="left">
                Телефон
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtOrganizationPhone" CssClass="txt small" MaxLength="100"/>
            </td>
        </tr>
        <asp:RequiredFieldValidator runat="server" ID="VReqOrgPhone" ControlToValidate="txtOrganizationPhone"
            EnableClientScript="false" Display="None" ErrorMessage='Поле "Телефон руководителя организации" обязательно для заполнения' />
        <tr>
            <td class="left">
                Факс
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtOrganizationFax" CssClass="txt small" MaxLength="100"/>
            </td>
        </tr>
        <tr>
            <td class="left">
                EMail
            </td>
            <td>
                <asp:TextBox runat="server" ID="TBEMail" CssClass="txt small" MaxLength="100"/>
                 <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="TBEMail" EnableClientScript="false"
            Display="None" ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$"
            ErrorMessage='Поле "EMail" заполнено неверно' />
            </td>
        </tr>
        <tr>
            <td class="left">
                Сайт
            </td>
            <td>
                <asp:TextBox runat="server" ID="TBSite" CssClass="txt small" MaxLength="40"/>
            </td>
        </tr>
        <tr>
            <td class="left">
                Ф.И.О. лица, ответственного за работу с ФБС
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtFullName" CssClass="txt long" MaxLength="255"/>
            </td>
        </tr>
        <asp:RequiredFieldValidator runat="server" ID="VReqUserName" ControlToValidate="txtFullName" EnableClientScript="false"
            Display="None" ErrorMessage='Поле "Ф.И.О. лица, ответственного за работу с ФБС" обязательно для заполнения' />
        <tr>
            <td class="left">
                Телефон лица, ответственного за работу с ФБС
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtPhone" CssClass="txt small" MaxLength="255" />
            </td>
        </tr>
        <asp:RequiredFieldValidator runat="server" ID="VReqUserPhone" ControlToValidate="txtPhone" EnableClientScript="false"
            Display="None" ErrorMessage='Поле "Телефон лица, ответственного за работу с ФБС" обязательно для заполнения' />
        <tr>
            <td class="left">
                E-mail лица, ответственного за работу с ФБС
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtEmail" CssClass="txt small" MaxLength="255"/>
            </td>
        </tr>
        <asp:RequiredFieldValidator runat="server" ID="VReqUserEMail" ControlToValidate="txtEmail" EnableClientScript="false"
            Display="None" ErrorMessage='Поле "E-mail лица, ответственного за работу с ФБС" обязательно для заполнения' />
        <asp:RegularExpressionValidator runat="server" ID="VRegexUserEMail" ControlToValidate="txtEmail" EnableClientScript="false"
            Display="None" ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$"
            ErrorMessage='Поле "E-mail лица, ответственного за работу с ФБС" заполнено неверно' />
        <tr>
            <td class="left">
                Текущая заявка на регистрацию
            </td>
            <td>
                <%= (IsRegistrationDocumentExists ?
                    String.Format("<a target=_blank href=\"/Profile/ConfirmedDocumentView.aspx?login={0}\" title=\"Просмотр заявки на регистрацию\">просмотр</a>", HttpUtility.UrlEncode(CurrentUser.login)) : 
                "не загружен")  %>
            </td>
        </tr>
        <tr>
            <td class="left">
                Заявка на регистрацию
            </td>
            <td>
                <asp:FileUpload ID="fuRegistrationDocument" runat="server" CssClass="long" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="box-submit">
                <input type="button" class="bt" value="Вернуться" onclick="window.location='./View.aspx?login=<%= CurrentUser.login %>';" />&nbsp;&nbsp;
                <asp:Button runat="server" ID="BtAddToOrgs" Text="Добавить организацию" CssClass="bt"
                    OnClick="BtAddToOrgs_Click" />
                <asp:Button runat="server" ID="btnUpdate" Text="Сохранить" CssClass="bt" OnClick="btnUpdate_Click" />
            </td>
        </tr>
    </table>
    <input type="hidden" name="state" />

    <script language="javascript" type="text/javascript">
    InitConfirmation('', '<%= Request.Form["state"] %>');
    </script>

    </form>
</asp:Content>
