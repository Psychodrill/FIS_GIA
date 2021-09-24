<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeactivateSuccess.aspx.cs" 
    Inherits="Esrp.Web.Administration.Accounts.Users.DeactivateSuccess" 
    MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<div class="left_col">
    <form id="Form2" runat="server">
       <p style="color:Red;" runat="server" id="pErrorMessage"></p>
                    <div class="col_in">
						<div class="statement edit">
							
							<p class="title">Логин/Е-mail&nbsp;<%= CurrentOrgUser.login %></p>
							<p class="back"><a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
							<p class="statement_menu">
								<a href="/Administration/Accounts/Users/Edit<%= GetUserKeyCode() %>.aspx?Login=<%= Login %>"
                                    class="gray" title="Изменить"><span>Изменить</span></a>
                                <% if (User.IsInRole("ActivateDeactivateUsers")) 
                                   {
                                       if (CurrentOrgUser.CanBeActivated())
                                       { %>
                                            <a href="/Administration/Accounts/Users/Activate.aspx?Login=<%= Login %>" 
                                                title="Активировать" class="gray">Активировать</a> 
                                <%     }
                                       if (CurrentOrgUser.CanBeDeactivated())
                                       { %>    
                                            <a href="/Administration/Accounts/Users/Deactivate.aspx?Login=<%= Login %>" 
                                                title="Отключить" class="gray">Отключить</a>
                                <%     } 
                                   } %> 

                                <% if (CurrentOrgUser.status != UserAccount.UserAccountStatusEnum.Deactivated) { %>
                                <a href="/Administration/Accounts/Users/RemindPassword.aspx?Login=<%= Login %>" 
                                    title="Напомнить пароль" class="gray"><span>Напомнить пароль</span></a>
                                <% } %> 
                                <a href="/Administration/Accounts/Users/ChangePassword.aspx?Login=<%= Login %>"
                                    class="gray" title="Изменить пароль"><span>Изменить пароль</span></a>
                                <a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>" 
                                    title="История изменений" class="gray"><span>История изменений</span></a>
                                <a href="/Administration/Accounts/Users/AuthenticationHistory.aspx?Login=<%= Login %>" 
                                    title="История аутентификаци" class="gray"><span>История аутентификаций</span></a>
							</p>
							<div class="clear"></div>
                            <div class="statement edit">
							    <p class="title">Пользователь “<%=CurrentOrgUser.login%>” отключен успешно.</p>
                            </div>

                            <div class="statement_in">
                            <% if (CurrentOrgUser != null)
                               { %>
                                <p><a runat="server" id="lnkContinueEdit">Продолжить редактирование</a><br /></p>
                                <p><a href="/Administration/Accounts/Users/List<%= GetUserKeyCode() %>.aspx">Список пользователей</a></p>
                                <p><a href="/Administration/Accounts/Users/Create<%= GetUserKeyCode() %>.aspx">Создать пользователя</a></p> 
    
                           <%  }
                               else
                               { %>         
                                <p>Пользователь “<%=Login%>” не найден.</p>
                           <% } %>

                            </div>

                        </div>
                    </div>
                    
    </form>
</div>

</asp:Content>
