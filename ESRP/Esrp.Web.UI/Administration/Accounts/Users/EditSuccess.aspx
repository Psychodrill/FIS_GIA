<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditSuccess.aspx.cs" 
    Inherits="Esrp.Web.Administration.Accounts.Users.EditSuccess" 
    MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<form id="Form1" runat="server">

<div class="left_col">
    <asp:ValidationSummary  CssClass="error_block" ID="ValidationSummary2" runat="server" DisplayMode="BulletList"
        EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />

                    <div class="col_in">
						<div class="statement edit">
							
							<p class="title">Логин/Е-mail&nbsp;<%= CurrentUser.Login %></p>
							<p class="back"><a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
							<p class="statement_menu">
								<a href="/Administration/Accounts/Users/Edit.aspx?Login=<%= Login %>"  
                                    class="active" title="Изменить"><span>Изменить</span></a>
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
                                <a href="#" title="Запросить смену пароля"><span>Запросить смену пароля</span></a>
                                <% } %> 
                                <a href="/Administration/Accounts/Users/ChangePassword.aspx?Login=<%= Login %>" 
                                    title="Изменить пароль" class="gray"><span>Изменить пароль</span></a>
                                <a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>" 
                                    title="История изменений" class="gray"><span>История изменений</span></a>
                                <a href="/Administration/Accounts/Users/AuthenticationHistory.aspx?Login=<%= Login %>" 
                                    title="История аутентификаци" class="gray"><span>История аутентификаций</span></a>                                
							</p>
							<div class="clear"></div>
							<div class="statement edit">
							    <p class="title">Пользователь “<%= CurrentUser.Login %>” изменен успешно.</p>
                            </div>
                            <div class="statement_in">
                                <p><a href='/Administration/Accounts/Users/RemindPassword.aspx?login=<%= CurrentUser.Login %>&UserKey=<%= GetUserKeyCode() %>'>Запросить смену пароля</a></p>
<%--
    <% if (User.IsInRole("ActivateDeactivateUsers")) 
   { %>
    <a href='/Administration/Accounts/Users/Activate.aspx?login=<%= CurrentUser.Login %>&UserKey=<%= GetUserKeyCode() %>'>Активировать</a>
    <% } %>
--%>
                                <br />
		                        <p><a runat="server" id="lnkContinueEdit">Продолжить редактирование</a></p>
                                <p><a href="/Administration/Accounts/Users/List<%= GetUserKeyCode() %>.aspx">Список пользователей</a></p>
                                <p><a href="/Administration/Accounts/Users/Create<%= GetUserKeyCode() %>.aspx">Создать пользователя</a></p> 
                            </div>
                        </div>
                    </div>
                </div>
            </form>
        
</asp:Content>
