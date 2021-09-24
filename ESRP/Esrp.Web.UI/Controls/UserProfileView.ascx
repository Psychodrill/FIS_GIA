<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfileView.ascx.cs"
    Inherits="Esrp.Web.Controls.UserProfileView" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Core.Systems" %>
<%@ Import Namespace="Esrp.Web" %>
<%--<p class="statement_menu" >
    <a href="/Profile/ChangePassword.aspx" class="gray"><span>Изменить пароль</span></a>
</p>--%>
<asp:Button ID="Button1" runat="server" PostBackUrl="/WebUserAccount/ChangePassword.aspx" Width="130px"
                    Text="Изменить пароль" ToolTip="Изменить пароль"  />
<div class="statement_table statement1" style="margin-top: 10px;">
    <table width="100%">
        <tr>
            <th >
                Логин/E-mail
            </th>
            <td >
                <%=this.CurrentUser.Login%>
            </td>
        </tr>
        <% if (this.CurrentUser.GetNewStatusName() != "Действующий")
           {%>
        <tr>
            <th>
                Текущий шаг регистрации
            </th>
            <td>
                <b>
                    <%=this.CurrentUser.GetStatusName()%>
                </b>
                <br />
                <br />
                <%=this.CurrentUser.GetViewStatusDescription()%>
            </td>
        </tr>
        <% } %>
        <tr>
            <th>
                Система
            </th>
            <td>
                <asp:Label runat="server" ID="lblSystem" />
            </td>
        </tr>
        <tr>
            <th>
                Состояние
            </th>
            <td>
                <%=this.CurrentUser.GetNewStatusName()%>
            </td>
        </tr>

        <%--FIS-1725 (09.06.2017)--%>
        <%--<tr runat="server" id="trTimeConnectionToSecureNetwork" visible="false">
            <th>
                <asp:Label runat="server" ID="lblTimeConnectionToSecureNetworkTitle">Согласованный срок подключения к защищенной сети</asp:Label>
            </th>
            <td>
                <asp:Label runat="server" ID="lblTimeConnectionToSecureNetwork"></asp:Label>
            </td>
        </tr>--%>
        <%--<tr runat="server" id="trTimeEnterInformationInFIS" visible="false">
            <th>
                <asp:Label runat="server" ID="lblTimeEnterInformationInFISTitle">Согласованный срок внесения сведений в ФИС ЕГЭ и приема</asp:Label>
            </th>
            <td>
                <asp:Label runat="server" ID="lblTimeEnterInformationInFIS"></asp:Label>
            </td>
        </tr>--%>
        <tr>
            <th>
                <span>Полное наименование организации<br />
                    (без организационно-правовой формы)</span>
            </th>
            <td>
                <% if (GeneralSystemManager.CanViewSubordinateOrganizations(this.User.Identity.Name) && this.CurrentUser.Status == UserAccount.UserAccountStatusEnum.Activated)
                   { %>
                <a href="/Administration/Organizations/UserDepartments/OrgCardInfo.aspx?OrgId=<%=this.OrganizationId%>">
                    <%=this.CurrentUser.OrganizationName%></a>
                <% }
                   else if (GeneralSystemManager.CanChangeRCModel(this.User.Identity.Name) && this.CurrentUser.Status == UserAccount.UserAccountStatusEnum.Activated)
                   { %>
                <a href="/Administration/Organizations/UserDepartments/OrgCardInfo.aspx?OrgId=<%=this.OrganizationId%>">
                    <%=this.CurrentUser.OrganizationName%></a>
                <% }
                   else if (Account.CheckRole(this.CurrentUser.Login, "EditSelfOrganization") && this.CurrentUser.Status == UserAccount.UserAccountStatusEnum.Activated)
                   { %>
                <a href="/Administration/Organizations/Administrators/OrgCard_Edit.aspx?act=SelfEdit">
                    <%=this.CurrentUser.OrganizationName%></a>
                <% }
                   else
                   {%>
                <%=this.CurrentUser.OrganizationName%>
                <% } %>
            </td>
        </tr>
        <tr>
            <th>
                Тип организации
            </th>
            <td>
                <%=this.CurrentUser.OrgTypeName%>
            </td>
        </tr>
        <tr>
            <th>
                ОПФ организации
            </th>
            <td>
                <%=this.CurrentUser.OrganizationIsPrivate.GetValueOrDefault()?"Частный":"Государственный"%>
            </td>
        </tr>
        <tr>
            <th>
                Субъект Российской Федерации, на территории которого находится организация
            </th>
            <td>
                <%=this.CurrentUser.OrganizationRegionName%>
            </td>
        </tr>
        <tr>
            <th>
                Город
            </th>
            <td>
                <%=this.CurrentUser.TownName%>
            </td>
        </tr>
        <tr>
            <th>
                Учредитель (для ССУЗов, ВУЗов и РЦОИ)
            </th>
            <td runat="server" id="tdFounders" class='founders-cell'>
            </td>
        </tr>
        <tr>
            <th>
                Юридический адрес
            </th>
            <td>
                <%=this.CurrentUser.OrganizationAddress%>
            </td>
        </tr>
        <tr id="trModel" visible="false" runat="server">
            <th>
                Модель приемной кампании:
            </th>
            <td>
                <asp:Literal runat="server" ID="lblModelName"></asp:Literal>
            </td>
        </tr>
        <tr>
            <th>
                Ф. И. О. руководителя организации
            </th>
            <td>
                <%=this.CurrentUser.OrganizationChiefName%>
            </td>
        </tr>
        <tr>
            <th>
                Телефон
            </th>
            <td>
                <%=this.CurrentUser.OrganizationPhone%>
            </td>
        </tr>
        <tr>
            <th>
                Факс
            </th>
            <td>
                <%=this.CurrentUser.OrganizationFax%>
            </td>
        </tr>
        <tr>
            <th>
                ОГРН
            </th>
            <td>
                <%=this.CurrentUser.OrganizationOGRN%>
            </td>
        </tr>
        <tr>
            <th>
                ИНН
            </th>
            <td>
                <%=this.CurrentUser.OrganizationINN%>
            </td>
        </tr>
        <tr>
            <th>
                КПП
            </th>
            <td>
                <%=this.CurrentUser.KPP%>
            </td>
        </tr>
        <tr>
            <th>
                Ф. И. О. лица, ответственного за работу с
                <asp:Label runat="server" ID="lblSystemNamesForFio" />
            </th>
            <td>
                <%=this.CurrentUser.GetFullName()%>
            </td>
        </tr>
        <tr>
            <th style="border-bottom-color: #fff;">
                Телефон лица, ответственного за работу с
                <asp:Label runat="server" ID="lblSystemNamesForPhone" />
            </th>
            <td style="border-bottom-color: #fff;">
                <%=this.CurrentUser.Phone%>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="box-submit">
                <% if (this.CurrentUser.CanEdit)
                   {%>
                <asp:Button runat="server" PostBackUrl="/Profile/Edit.aspx" Width="100px" Text="Изменить"
                    ToolTip="Изменить регистрационные данные" />
                <% }
                   else
                   {%><br />
                <% }%>
            </td>
        </tr>
        <tr>
            <th style="border-bottom-color: #fff;">
                Заявка на регистрацию
            </th>
            <td style="border-bottom-color: #fff;">
                <%=
                (this.IsRegistrationDocumentExists
                     ? "<a target=_blank href=\"/Profile/ConfirmedDocumentView.aspx\" title=\"Просмотр заявки на регистрацию\">просмотр</a>"
                     : "не загружен")%>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="box-submit">
                <% if (this.CurrentUser.CanEditRegistrationDocument)
                   {%>
                <asp:Button runat="server" PostBackUrl="/Profile/DocumentUpload.aspx" Width="100px"
                    Text="Изменить" ToolTip="Загрузить заявку на регистрацию" />
                <% }
                   else
                   {%><br />
                <% }%>
            </td>
        </tr>
    </table>
</div>
