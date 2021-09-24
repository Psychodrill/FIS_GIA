<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePasswordSuccess.aspx.cs" 
    Inherits="Esrp.Web.Administration.Accounts.Users.ChangePasswordSuccess" 
    MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>
<%@ Import Namespace="Esrp.Core.Systems" %>

<asp:Content  runat="server" ContentPlaceHolderID="cphContent">
<div class="left_col">
    <form id="Form2" runat="server">
    <asp:ValidationSummary CssClass="error_block"  ID="ValidationSummary2" runat="server" DisplayMode="BulletList"
        EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />
                    <div class="col_in">
						<div class="statement edit">
							
							<p class="title">Логин/Е-mail&nbsp;<%= CurrentUser.Login %></p>
							<p class="back"><a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
							<p class="statement_menu">
                                <% if ("IS".Equals(Request.QueryString["UserKey"]) || "OU".Equals(Request.QueryString["UserKey"]))
                                   { %>
								<a href="/Administration/Accounts/Users/Edit<%= GetUserKeyCode() %>.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>"><span>Изменить</span></a>
                                <a href="/Administration/Accounts/Users/ChangePassword.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" 
                                    class="active" title="Изменить пароль"><span>Изменить пароль</span></a>
                                    <%if (!GeneralSystemManager.CanChangeRCModel(this.User.Identity.Name))
                                      { %> 
                                        <a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" title="История изменений" class="gray">
                                            <span>История изменений</span>
                                        </a>
                                        <a href="/Administration/Accounts/Users/AuthenticationHistory.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" title="История аутентификаци" class="gray">
                                            <span>История аутентификаций</span>
                                        </a>
                                        <a href="/Administration/Accounts/Users/AccountKeyList.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" title="Ключи доступа" class="gray">
                                            <span>Ключи доступа</span>
                                        </a>
                                    <%} %>
                                <% }
                                   else{ %>
								<a href="/Administration/Accounts/Users/Edit<%= GetUserKeyCode() %>.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>"
                                    class="gray" title="Изменить"><span>Изменить</span></a>
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

                                    <% if (CurrentOrgUser.status != UserAccount.UserAccountStatusEnum.Deactivated) { %>
                                        <a href="#" title="Запросить смену пароля"><span>Запросить смену пароля</span></a>
                                    <% } %>
                                        <a href="/Administration/Accounts/Users/ChangePassword.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" class="active" title="Изменить пароль">
                                            <span>Изменить пароль</span>
                                        </a>
                                    <%if (!GeneralSystemManager.CanChangeRCModel(this.User.Identity.Name))
                                      { %> 
                                        <a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" title="История изменений" class="gray">
                                            <span>История изменений</span>
                                        </a>
                                        <a href="/Administration/Accounts/Users/AuthenticationHistory.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" title="История аутентификаци" class="gray">
                                            <span>История аутентификаций</span>
                                        </a>
                                    <%} %>
                                <% } %>
							</p>
							<div class="clear"></div>
                            <div class="statement edit">
							    <p class="title">Пароль пользователя “<%= CurrentUser.Login %>” изменен успешно.</p>
                            </div>
                            <div class="statement_in">
                                <p>
                                <% if (CurrentUser.Status != UserAccount.UserAccountStatusEnum.Deactivated){%>
                                Уведомление отправлено на  e-mail.
                                <%} %>
                                </p>
                                <p><a href='/Administration/Accounts/Users/Edit<%= GetUserKeyCode() %>.aspx?login=<%= CurrentUser.Login %>'>Продолжить редактирование</a></p>
                                <p><a href="/Administration/Accounts/Users/List<%= GetUserKeyCode() %>.aspx">Список пользователей</a></p>
                                <p><a href="/Administration/Accounts/Users/Create<%= GetUserKeyCode() %>.aspx">Создать пользователя</a></p> 
                            </div>
                        </div>
                    </div>
                    </form>
                </div>  
</asp:Content>
