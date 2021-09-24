<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountKeyEditSuccess.aspx.cs" 
    Inherits="Esrp.Web.Administration.Accounts.Users.AccountKeyEditSuccess" 
    MasterPageFile="~/Common/Templates/Administration.Master" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<div class="left_col">
    <form id="Form2" runat="server">
    <asp:ValidationSummary  CssClass="error_block" ID="ValidationSummary2" runat="server" DisplayMode="BulletList"
        EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />

                    <div class="col_in">
						<div class="statement edit">
							
							<p class="title">Логин/Е-mail&nbsp;<%= CurrentUserLogin %></p>
							<p class="back"><a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
							<p class="statement_menu">
								<a href="/Administration/Accounts/Users/Edit<%= GetUserKeyCode() %>.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>"
                                    title="Изменить" class="gray"><span>Изменить</span></a>
                                <a href="/Administration/Accounts/Users/ChangePassword.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" 
                                    class="gray" title="Изменить пароль"><span>Изменить пароль</span></a>
                                <a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" 
                                    title="История изменений" class="gray"><span>История изменений</span></a>
                                <a href="/Administration/Accounts/Users/AuthenticationHistory.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" 
                                    title="История аутентификаци" class="gray"><span>История аутентификаций</span></a>
                                <a href="/Administration/Accounts/Users/AccountKeyList.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" 
                                    title="Ключи доступа" class="active"><span>Ключи доступа</span></a>
							</p>
							<div class="clear"></div>
                            <div class="statement edit">
							    <p class="title">Ключ доступа “<%= AccountKeyCode %>” для пользователя “<%= Login %>” изменен успешно.</p>
                            </div>
                            <div class="statement_in">
                                <p><a href='/Administration/Accounts/Users/AccountKeyEdit.aspx?login=<%= Login %>&key=<%= AccountKeyCode %>&UserKey=<%= GetUserKeyCode() %>'>Продолжить редактирование ключа доступа</a></p>
                                <p><a href='/Administration/Accounts/Users/AccountKeyList.aspx?login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>'>Список ключей доступа пользователя</a></p>
                                <p><a href='/Administration/Accounts/Users/Edit<%= GetUserKeyCode() %>.aspx?login=<%= Login %>'>Продолжить редактирование пользователя</a></p>
                                <p><a href="/Administration/Accounts/Users/List<%= GetUserKeyCode() %>.aspx">Список пользователей</a></p>
                                <p><a href="/Administration/Accounts/Users/Create<%= GetUserKeyCode() %>.aspx">Создать пользователя</a></p> 
                            </div>
                        </div>
                    </div>
                    </form>
                </div>
</asp:Content>