<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfileView.ascx.cs" Inherits="Fbs.Web.Controls.UserProfileView" %>
<%@ Import Namespace="Fbs.Web" %>
<%@ Import Namespace="Fbs.Core" %>
   
<%--   <% if (CurrentUser.Status == Fbs.Core.UserAccount.UserAccountStatusEnum.Consideration)
   {%>
    <p>Для улучшения качества работы службы поддержки пользователей Подсистемы ФИС &laquo;Результаты ЕГЭ&raquo;, просим Вас ответить <a href='http://opros.fbsege.ru/'>на следующие вопросы</a>.</p>
    <%} %>--%>
    
<table class="form f600">
    <tr>
        <td class="left">Логин</td>
        <td class="text"><%= CurrentUser.Login%></td>
    </tr>
     <% if (CurrentUser.GetNewStatusName() != "Действующий")
   { %>
      <tr>
        <td class="left">Текущий шаг регистрации</td>
        <td class="text"><b><%= CurrentUser.GetStatusName() %> </b> <br/><br/>
                        <%= CurrentUser.GetViewStatusDescription() %></td>
    </tr>
    <% } %>
    <tr>
    <tr>
        <td class="left">Состояние</td>
        <td class="text">
            <%= CurrentUser.GetNewStatusName()%>
        </td>
    </tr>             
    <tr>
      <td colspan="2" class="box-submit"><br/></td>
    </tr>            
    <tr>
        <td class="left"><span>Полное наименование организации<BR/>(без организационно-правовой формы)</span></td>
        <td class="text">
            <% if (UserAccount.CheckRole(CurrentUser.Login, "EditSelfOrganization")) { %>
                <a href="/Administration/Organizations/Administrators/OrgCard_Edit.aspx?act=SelfEdit"><%= CurrentUser.OrganizationName %></a>
            <% } else {%>
                <%= CurrentUser.OrganizationName %>
            <% } %>
        </td>
    </tr>
    <tr>
        <td class="left">Субъект Российской Федерации, на территории которого находится организация</td>
        <td class="text"><%= CurrentUser.OrganizationRegionName%></td>
    </tr>
    <tr>
        <td class="left">Учредитель (для ссузов, вузов и РЦОИ)</td>
        <td class="text"><%= CurrentUser.OrganizationFounderName%></td>
    </tr>
    <tr>
        <td class="left">Юридический адрес</td>
        <td class="text"><%= CurrentUser.OrganizationAddress%></td>
    </tr>
    <tr>
        <td class="left">Ф. И. О. руководителя организации</td>
        <td class="text"><%= CurrentUser.OrganizationChiefName%></td>
    </tr>
    <tr>
        <td class="left">Телефон руководителя организации</td>
        <td class="text"><%= CurrentUser.OrganizationPhone%></td>
    </tr>
    <tr>
        <td class="left">Факс</td>
        <td class="text"><%= CurrentUser.OrganizationFax%></td>
    </tr>
    <tr>
        <td class="left">Ф. И. О. лица, ответственного за работу с ФБС</td>
        <td class="text"><%= CurrentUser.GetFullName() %></td>            
    </tr>
    <tr>
        <td class="left">Телефон лица, ответственного за работу с ФБС</td>
        <td class="text"><%= CurrentUser.Phone%></td>
    </tr>
    <tr>
        <td class="left">E-mail лица, ответственного за работу с ФБС</td>
        <td class="text"><%= CurrentUser.Email%></td>
    </tr>
</table>

