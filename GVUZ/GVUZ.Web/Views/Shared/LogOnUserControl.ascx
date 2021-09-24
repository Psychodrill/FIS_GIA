<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="GVUZ.Model.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Security" %>

<%
    if (Request.IsAuthenticated) {
%>

    <table>
        <tr>
            <td>
                <% if (InstitutionHelper.InstitutionName != null) {%>
                        <span class="line1"><%= InstitutionHelper.InstitutionName %></span>
                <%}%>
                <span class="line1 primary">
                    <%= UserHelper.GetCurrentFullNameNonEmpty()%>
                </span>
                <% if (UserRole.CurrentUserInRole(UserRole.FBDAdmin)) {%>
                    <span class="line1"><%:Html.ActionLink("Перейти к списку ОО", "List", "InstitutionAdmin")%></span>
                <%}%>
            </td>
            <td>
                <span title="Выйти из аккаунта" class="logoutSpan" onclick="location.href='<%: Url.Action("LogOff", "Account") %>'"></span>
            </td>
        </tr>

    </table>
    
	
    <%--Отключил ссылку на личный кабинет на данном этапе dchaplygin https://redmine.armd.ru/issues/21517
    <span class="line2">
        <%: Html.ActionLink("Личный кабинет", "Personal", "Account")%>&nbsp;&nbsp;
    </span>--%>
<%
    }
%>
