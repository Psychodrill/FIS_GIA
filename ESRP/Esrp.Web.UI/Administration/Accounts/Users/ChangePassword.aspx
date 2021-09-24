<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" 
         Inherits="Esrp.Web.Administration.Accounts.Users.ChangePassword"
         MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Core.Systems" %>
<%@ Register Assembly="Esrp.Web.UI" Namespace="Esrp.Web.Controls" TagPrefix="esrp" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHead">
    <script src="/Common/Scripts/Confirmation.js" type="text/javascript"> </script>
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphContent">
    <div class="left_col">
        <form id="Form2" runat="server">
            <asp:ValidationSummary  CssClass="error_block" ID="ValidationSummary2" runat="server" DisplayMode="BulletList"
                                    EnableClientScript="true" HeaderText="<p>Произошли следующие ошибки:</p>" />

            <div class="col_in">
                <div class="statement edit">
							
                    <p class="title">Логин/Е-mail&nbsp;<%= CurrentUser.Login %></p>
                    <p class="back"><a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
                    <p class="statement_menu">
                        <% if ("IS".Equals(Request.QueryString["UserKey"]) || "OU".Equals(Request.QueryString["UserKey"]))
                           { %>
                            <a href="/Administration/Accounts/Users/Edit<%= GetUserKeyCode() %>.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>"><span>Изменить</span></a>
                            <a href="#" class="active" title="Изменить пароль"><span>Изменить пароль</span></a>
                            <%if (!GeneralSystemManager.CanChangeRCModel(this.User.Identity.Name))
                              { %>
                                <a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" 
                                   title="История изменений" class="gray"><span>История изменений</span></a>
                                <a href="/Administration/Accounts/Users/AuthenticationHistory.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" 
                                   title="История аутентификаци" class="gray"><span>История аутентификаций</span></a>
                                <a href="/Administration/Accounts/Users/AccountKeyList.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" 
                                   title="Ключи доступа" class="gray"><span>Ключи доступа</span></a>
                            <%} %>
                        <% }
                           else{ %>
                            <a href="/Administration/Accounts/Users/Edit<%= GetUserKeyCode() %>.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>"
                               class="gray" title="Изменить"><span>Изменить</span></a>
                            <% if (User.IsInRole("ActivateDeactivateUsers"))
                               {
                                   if (CurrentOrgUser.CanBeActivated())
                                   {%>
                                    <a href="/Administration/Accounts/Users/Activate.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" 
                                       title="Активировать" class="gray">Активировать</a> 
                                <% }
                                   if (CurrentOrgUser.CanBeDeactivated())
                                   { %>    
                                    <a href="/Administration/Accounts/Users/Deactivate.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" 
                                       title="Отключить" class="gray">Отключить</a>
                            <% }
                               } %> 

                            <% if (CurrentOrgUser.status != UserAccount.UserAccountStatusEnum.Deactivated) { %>
                                <a href="/Administration/Accounts/Users/RemindPassword.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" 
                                   title="Запросить смену пароля" class="gray"><span>Запросить смену пароля</span></a>
                            <% } %> 
                            <a href="#" class="active" title="Изменить пароль"><span>Изменить пароль</span></a>
                            <a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" 
                               title="История изменений" class="gray"><span>История изменений</span></a>
                            <a href="/Administration/Accounts/Users/AuthenticationHistory.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" 
                               title="История аутентификаци" class="gray"><span>История аутентификаций</span></a>
                        <% } %>
                    </p>
                    <div class="clear"></div>
                    <div class="statement_table">
                        <table width="100%">
                            <tr>
                                <th>Новый пароль</th>
                                <td width="1"><asp:TextBox runat="server" ID="txtPassword" TextMode="Password"/></td>
                            </tr>
        
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPassword" 
                                                        EnableClientScript="false" Display="None" 
                                                        ErrorMessage='Поле "Новый пароль" обязательно для заполнения' /> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPassword" ErrorMessage="Введите новый пароль" Text="*" Display="None" />
                                <esrp:MinLengthValidator MinLength="6"  runat="server" ID="rvPassword" ControlToValidate="txtPassword"  ErrorMessage="Длина пароля должна быть не менее 6 символов" Display="None" Text="*" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Text="*" ErrorMessage='Поле "Новый пароль" должно содержать цифры, строчные, заглавные буквы' ControlToValidate="txtPassword" ValidationExpression="^.*(?=.{6,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$" Display="None" />
                            <tr>
                                <th>Новый пароль еще раз</th>
                                <td><asp:TextBox runat="server" ID="txtPasswordAgain" TextMode="Password" /></td>
                            </tr>
        
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPasswordAgain" 
                                                        EnableClientScript="false" Display="None" 
                                                        ErrorMessage='Поле "Новый пароль еще раз" обязательно для заполнения' /> 

                            <asp:CompareValidator ID="CompareValidator1" runat="server" EnableClientScript="false" Display="None" 
                                                  ControlToCompare="txtPassword" ControlToValidate="txtPasswordAgain" 
                                                  ErrorMessage="Пароли не совпадают"/>
                                        
                            <tr>
                                <td colspan="2" style="border-bottom: 0px;">
                                    <asp:Button runat="server" ID="Button1" Text="Сохранить" OnClick="btnUpdate_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
    
        </form>
    </div>
</asp:Content>
