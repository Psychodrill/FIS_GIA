<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HistoryVersion.aspx.cs"
    Inherits="Esrp.Web.Administration.Accounts.Users.HistoryVersion" MasterPageFile="~/Common/Templates/Administration.Master" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Core.Systems" %>
<%@ Import Namespace="Esrp.Web" %>
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
                    <% if ("IS".Equals(Request.QueryString["UserKey"]) || "OU".Equals(Request.QueryString["UserKey"]))
                       { %>
                    <a href="/Administration/Accounts/Users/Edit<%= GetUserKeyCode() %>.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>"
                        class="gray"><span>Изменить</span></a> <a href="/Administration/Accounts/Users/ChangePassword.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>"
                            title="Изменить пароль" class="gray"><span>Изменить пароль</span></a> <a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>"
                                title="История изменений" class="active"><span>История изменений</span></a>
                    <a href="/Administration/Accounts/Users/AuthenticationHistory.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>"
                        title="История аутентификаци" class="gray"><span>История аутентификаций</span></a>
                    <a href="/Administration/Accounts/Users/AccountKeyList.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>"
                        title="Ключи доступа" class="gray"><span>Ключи доступа</span></a>
                    <% }
                       else
                       { %>
                    <a href="/Administration/Accounts/Users/Edit<%= this.GetUserKeyCode() %>.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>">
                        <span>Изменить</span></a>
                    <% if (User.IsInRole("ActivateDeactivateUsers"))
                       {
                           if (CurrentOrgUser.CanBeActivated())
                           { %>
                    <a href="/Administration/Accounts/Users/Activate.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>"
                        title="Активировать" class="gray">Активировать</a>
                    <%     }
                                       if (CurrentOrgUser.CanBeDeactivated())
                                       { %>
                    <a href="/Administration/Accounts/Users/Deactivate.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>"
                        title="Отключить" class="gray">Отключить</a>
                    <%     }
                                   } %>
                    <% if (CurrentOrgUser.status != UserAccount.UserAccountStatusEnum.Deactivated)
                       { %>
                    <a href="/Administration/Accounts/Users/RemindPassword.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>"
                        title="Запросить смену пароля" class="gray"><span>Запросить смену пароля</span></a>
                    <% } %>
                    <a href="/Administration/Accounts/Users/ChangePassword.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>"
                        class="gray" title="Изменить пароль"><span>Изменить пароль</span></a> <a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>"
                            title="История изменений" class="active"><span>История изменений</span></a>
                    <a href="/Administration/Accounts/Users/AuthenticationHistory.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>"
                        title="История аутентификаци" class="gray"><span>История аутентификаций</span></a>
                    <% } %>
                </p>
                <div class="clear">
                </div>
                <asp:Repeater runat="server" ID="rpHistoryVersion" DataSourceID="dsHistoryVersion">
                    <ItemTemplate>
                        <div class="statement edit">
                            <p class="title">
                                Редакция №
                                <%#Eval("VersionId")%>
                                от
                                <%# (DateTime)Eval("UpdateDate") %></p>
                        </div>
                        <div class="statement_table">
                            <table width="100%">
                                <tr>
                                    <th>
                                        Редактировал
                                    </th>
                                    <td>
                                        <%#Eval("EditorLastName")%>
                                        <%#Eval("EditorFirstName")%>
                                        <%#Eval("EditorPatronymicName")%>
                                        <%# String.IsNullOrEmpty(Eval("EditorLogin").ToString()) ? "" : ("(" + Eval("EditorLogin") +")")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left">
                                        IP-адрес редактора
                                    </td>
                                    <td class="left">
                                        <%# Convert.ToBoolean(Eval("IsVpnEditorIp")) ? "VPN&nbsp;" : String.Empty%><%#Eval("EditorIp")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="box-submit">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left">
                                        Логин/E-mail
                                    </td>
                                    <td class="text">
                                        <%#Eval("Login")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left">
                                        Состояние
                                    </td>
                                    <td class="text">
                                        <b>
                                            <%# UserAccountExtentions.GetUserAccountStatusName(UserAccount.ConvertStatusCode(Eval("Status").ToString()))%>
                                        </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="box-submit">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left">
                                        <span>Полное наименование организации</span>
                                    </td>
                                    <td class="text">
                                        <%#Eval("OrganizationName") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left">
                                        Субъект Российской Федерации,<br />
                                        на территории которого находится организация
                                    </td>
                                    <td class="text">
                                        <%#Eval("OrganizationRegionName") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left">
                                        Учредитель
                                    </td>
                                    <td class="text">
                                        <%#Eval("OrganizationFounderName") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left">
                                        Адрес
                                    </td>
                                    <td class="text">
                                        <%#Eval("OrganizationAddress") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left">
                                        Ф. И. О. руководителя организации
                                    </td>
                                    <td class="text">
                                        <%#Eval("OrganizationChiefName") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left">
                                        Факс
                                    </td>
                                    <td class="text">
                                        <%#Eval("OrganizationFax") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left">
                                        Телефон руководителя организации
                                    </td>
                                    <td class="text">
                                        <%#Eval("OrganizationPhone") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left">
                                        Ф. И. О. лица, ответственного за работу с
                                        <%= GeneralSystemManager.GetSystemName(2)%>
                                    </td>
                                    <td class="text">
                                        <%#Eval("LastName")%>
                                        <%#Eval("FirstName")%>
                                        <%#Eval("PatronymicName")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left">
                                        Телефон лица, ответственного за работу с
                                        <%= GeneralSystemManager.GetSystemName(2)%>
                                    </td>
                                    <td class="text">
                                        <%#Eval("Phone") %>
                                    </td>
                                </tr>
                                <%--
                <tr>
                    <td class="left">E-mail лица, ответственного за работу с <%= GeneralSystemManager.GetSystemName(2)%></td>
                    <td class="text"><%#Eval("Email") %></td>
                </tr>
                                --%>
                                <tr>
                                    <td class="left">
                                        Тип ОУ
                                    </td>
                                    <td class="text">
                                        <%# Convert.IsDBNull(Eval("EducationInstitutionTypeName")) ? "Не задан" : Eval("EducationInstitutionTypeName") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left">
                                        Список постоянных внешних IP-адресов компьютеров, с которых будет<br />
                                        осуществляться доступ к
                                        <%= GeneralSystemManager.GetSystemName(2)%>
                                    </td>
                                    <td class="text">
                                        <%# Eval("IpAddresses").ToString().Replace(",", "<br />")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="box-submit" style="border-bottom-color: #fff;">
                                        <br />
                                    </td>
                                </tr>
                            </table>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        По Ващему запросу ничего не найдено
                    </AlternatingItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        </form>
    </div>
    <asp:SqlDataSource runat="server" ID="dsHistoryVersion" ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="GetUserAccountLog" CancelSelectOnNullParameter="false" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="login" Type="String" QueryStringField="login" />
            <asp:QueryStringParameter Name="versionId" Type="String" QueryStringField="version" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
