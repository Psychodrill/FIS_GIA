<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" 
         Inherits="Esrp.Web.Administration.Accounts.Users.View"
         MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Import Namespace="Esrp.Web" %>
<%@ Import Namespace="Esrp.Core" %>

<%@ Register src="../../../Controls/OperatorPanel.ascx" tagname="OperatorPanel" tagprefix="uc1" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">

    <p style="color: Red;" runat="server" id="pErrorMessage"></p>
    <form id="Form2" runat="server">

        <div class="right_col">
            <div class="col_in">
                <% if (isUserAdminOrSupport())
                   {%>
                    <uc1:OperatorPanel ID="OperatorPanel" runat="server" />
                <%} %>
						
            </div>
        </div>

        <div class="left_col">
            <asp:ValidationSummary CssClass="error_block"  ID="ValidationSummary2" runat="server" DisplayMode="BulletList"
                                   EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />
            <div class="col_in">
                <div class="statement edit">
							
                    <p class="title">Логин/Е-mail&nbsp;<%= CurrentUser.login %></p>
                    <p class="back"><a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
                    <p class="statement_menu">
                        <a href="/Administration/Accounts/Users/Edit.aspx?Login=<%= Login %>"
                           class="gray"><span>Изменить</span></a>
                        <% if (User.IsInRole("ActivateDeactivateUsers"))
                           {
                               if (CurrentUser.CanBeActivated())
                               {%>
                                <a href="/Administration/Accounts/Users/Activate.aspx?Login=<%= Login %>" 
                                   title="Активировать" class="gray">Активировать</a> 
                            <% }
                               if (CurrentUser.CanBeDeactivated())
                               { %>    
                                <a href="/Administration/Accounts/Users/Deactivate.aspx?Login=<%= Login %>" 
                                   title="Отключить" class="gray">Отключить</a>
                        <% }
                           } %> 

                        <% if (CurrentUser.status != UserAccount.UserAccountStatusEnum.Deactivated)
                           { %>
                            <a href="/Administration/Accounts/Users/RemindPassword.aspx?Login=<%= Login %>" 
                               title="Запросить смену пароля" class="gray"><span>Запросить смену пароля</span></a>
                        <% } %>  
                        <a href="/Administration/Accounts/Users/ChangePassword.aspx?Login=<%= Login %>" 
                           title="Изменить пароль" class="gray"><span>Изменить пароль</span></a>
                        <a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>" 
                           title="История изменений" class="gray"><span>История изменений</span></a>
                        <a href="/Administration/Accounts/Users/AuthenticationHistory.aspx?Login=<%= Login %>" 
                           title="История аутентификаци" class="gray"><span>История аутентификаций</span></a>                                
                    </p>
                    <div class="clear"></div>
                    <div class="statement_table">

                        <table width="100%">
                            <tr>
                                <td class="left">Логин/E-mail</td>
                                <td class="text"><%= CurrentUser.login %></td>
                            </tr>
                            <tr>
                                <td class="left">Состояние</td>
                                <td class="text"><b><%= UserAccountExtentions.GetUserAccountNewStatusName(CurrentUser.status) %></b>
                                    <br /><%= UserAccountExtentions.GetAdministartionStatusDescription(CurrentUser)  %></td>
                            </tr>
                            <tr>
                                <td class="left">
                                    Доступ к системам
                                </td>
                                <td class="text">
                                    <asp:Label runat="server" ID="lblSystem" />
                                </td>
                            </tr>
                            <tr>
                                <td class="left">
                                    Группа
                                </td>
                                <td class="text">
                                    <asp:Label runat="server" ID="lblGroup" />
                                </td>
                            </tr>
                            <tr>
                                <td class="left">
                                    Ф.И.О. лица, ответственного за работу с системами
                                </td>
                                <td class="text">
                                    <%= CurrentUser.GetFullName() %>
                                </td>
                            </tr>
                            <tr>
                                <td class="left">
                                    Телефон лица, ответственного за работу с системами
                                </td>
                                <td class="text">
                                    <%= CurrentUser.phone %>
                                </td>
                            </tr>
                        <tr>
                            <th>
                                Должность лица, ответственного за работу с системами
                            </th>
                                <td class="text">
                                    <%= CurrentUser.position %>
                                </td>
                        </tr>
                            <tr>
                                <td class="left">
                                    Скан заявки на регистрацию
                                </td>
                                <td class="text">
                                    <%=
        (IsRegistrationDocumentExists
             ? String.Format(
                 "<a target=_blank href=\"/Profile/ConfirmedDocumentView.aspx?login={0}\" title=\"Просмотр заявки на регистрацию\">просмотр</a>",
                 HttpUtility.UrlEncode(CurrentUser.login)) : "не загружен") %>
                                </td>
                            </tr>
                            <tr>
                                <td class="left">
                                    Номер заявки на регистрацию №
                                </td>
                                <td class="text">
                                    <%=
        (CurrentUser.RequestedOrganization != null
             ? String.Format(
                 "<a href=\"/Administration/Requests/RequestForm.aspx?RequestID={0}\" title=\"Перейти на карточку заявки\">{0}</a>", CurrentUser.RequestedOrganization
                   .Id)
             : "отсутствует") %>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="box-submit">	<br />
                                </td>
                            </tr>            
                            <tr>
                                <td class="left"><span>Полное наименование организации<BR/>(без организационно-правовой формы)</span></td>
                                <td class="text">
                                    <% if (CurrentUser.RequestedOrganization.OrganizationId != null)
                                       {%>
                                        <a href="../../Organizations/Administrators/OrgCard_Edit.aspx?OrgID=<%= CurrentUser.RequestedOrganization.OrganizationId%>" ><%= CurrentUser.RequestedOrganization.FullName%></a>
                                    <%
                                }else{%><%= CurrentUser.RequestedOrganization.FullName %><%}%>
                                    <br/> <asp:Label runat="server" ID="lblEtalonOrgName" Style="white-space: normal;" />
                                </td>
                            </tr>
                            <tr>
                                <td class="left">Краткое наименование организации</td>
                                <td class="text"><%= CurrentUser.RequestedOrganization.ShortName %>
                                    <BR/> <asp:Label runat=server ID="Label1" Style="white-space: normal;" /></td>
                            </tr>
                            <tr>
                                <td class="left">Субъект Российской Федерации, на территории которого находится организация</td>
                                <td class="text"><%= CurrentUser.RequestedOrganization.Region.Name %>
                                    <BR/> <asp:Label runat=server ID="lblEtalonRegion" Style="white-space: normal;" /></td>
                            </tr>
                            <tr>
                                <td class="left">Тип ОУ</td>
                                <td class="text"><%=
        (CurrentUser.RequestedOrganization.OrgType.Id == null)
            ? "Не задан"
            : CurrentUser.RequestedOrganization.OrgType.Name %>
                                    <BR/> <asp:Label runat=server ID="lblEtalonOrgType" Style="white-space: normal;" /></td>
                            </tr>
                            <tr>
                                <td class="left">Вид ОУ</td>
                                <td class="text"><%= (CurrentUser.RequestedOrganization.Kind.Id == null)
                           ? "Не задан"
                           : CurrentUser.RequestedOrganization.Kind.Name %>
                                    <BR/> <asp:Label runat=server ID="Label2" Style="white-space: normal;" /></td>
                            </tr>
                            <tr>
                                <td class="left">Организационно-правовая форма ОУ</td>
                                <td class="text"><%= CurrentUser.RequestedOrganization.IsPrivate ?"Негосударственный":"Государственный" %>
                                    <BR/> <asp:Label runat=server ID="Label3" Style="white-space: normal;" /></td>
                            </tr>
                            <tr>
                                <td class="left">Является филиалом</td>
                                <td class="text"><%= CurrentUser.RequestedOrganization.IsFilial ? "Да" : "Нет" %>
                                    <BR/> <asp:Label runat=server ID="Label4" Style="white-space: normal;" /></td>
                            </tr>

                            <tr>
                                <td class="left">ОГРН</td>
                                <td class="text"><%= CurrentUser.RequestedOrganization.OGRN %>
                                    <BR/> <asp:Label runat=server ID="Label6" Style="white-space:normal;" /></td>
                            </tr>

                            <tr>
                                <td class="left">ИНН</td>
                                <td class="text"><%= CurrentUser.RequestedOrganization.INN %>
                                    <BR/> <asp:Label runat=server ID="Label5" Style="white-space:normal;" /></td>
                            </tr>

                            <tr>
                                <td class="left">КПП</td>
                                <td class="text">
                                    <%=CurrentUser.RequestedOrganization.KPP%>
                                    <br/><asp:Label runat="server" ID="lblEtalonKPP" Style="white-space:normal;" />
                                </td>
                            </tr>
                            <tr>
                                <td class="left">Учредитель (для&nbsp;ССУЗов&nbsp;и&nbsp;ВУЗов)</td>
                                <td class="text"><%= CurrentUser.RequestedOrganization.OwnerDepartment %>
                                    <BR/> <asp:Label runat=server ID="lblEtalonFounder" Style="white-space:normal;" /></td>
                            </tr>
                            <tr>
                                <td class="left">Должность руководителя организации</td>
                                <td class="text"><%= CurrentUser.RequestedOrganization.DirectorPosition %>
                                    <BR/> <asp:Label runat=server ID="Label7" Style="white-space:normal;" /></td>
                            </tr>
                            <tr>
                                <td class="left">Ф.И.О. руководителя организации</td>
                                <td class="text"><%= CurrentUser.RequestedOrganization.DirectorFullName %>
                                    <BR/> <asp:Label runat=server ID="lblEtalonChief" Style="white-space:normal;" /></td>
                            </tr>
                            <tr>
                                <td class="left">Фактический адрес</td>
                                <td class="text"><%= CurrentUser.RequestedOrganization.FactAddress %>
                                    <BR/> <asp:Label runat=server ID="Label8" Style="white-space:normal;" /></td>
                            </tr>
                            <tr>
                                <td class="left">Юридический адрес</td>
                                <td class="text"><%= CurrentUser.RequestedOrganization.LawAddress %>
                                    <BR/> <asp:Label runat=server ID="lblEtalonAddress" Style="white-space:normal;" /></td>
                            </tr>
                            <tr runat="server" id="trReceptionOnResultsCNE" Visible="false">
                                <td class="left">Прием по результатам ЕГЭ (для&nbsp;ССУЗов&nbsp;и&nbsp;ВУЗов)</td>
                                <td class="text">
                                    <asp:Label runat="server" ID="lblReceptionOnResultsCNE" />
                                    <BR/> 
                                    <asp:Label runat=server ID="lblEtalonReceptionOnResultsCNE" Style="white-space:normal;" />
                                </td>
                            </tr>
                            <tr>
                                <td class="left">Свидетельсво об аккредитации</td>
                                <td class="text"><%= CurrentUser.RequestedOrganization.AccreditationSertificate %>
                                    <BR/> <asp:Label runat=server ID="Label9" Style="white-space:normal;" /></td>
                            </tr>
                            <tr>
                                <td class="left">Код города</td>
                                <td class="text"><%= CurrentUser.RequestedOrganization.PhoneCityCode  %>
                                    <BR/> <asp:Label runat=server ID="Label10" Style="white-space:normal;" /></td>
                            </tr>
                            <tr>
                                <td class="left">Телефон</td>
                                <td class="text"><%= CurrentUser.RequestedOrganization.Phone %>
                                    <BR/> <asp:Label runat=server ID="lblEtalonOrgPhone" Style="white-space:normal;" /></td>
                            </tr>
                            <tr>
                                <td class="left">Факс</td>
                                <td class="text"><%= CurrentUser.RequestedOrganization.Fax %></td>
                            </tr>
                            <tr>
                                <td class="left">EMail</td>
                                <td class="text"><%= CurrentUser.RequestedOrganization.EMail  %></td>
                            </tr>
                            <tr>
                                <td class="left">Сайт</td>
                                <td class="text"><%= CurrentUser.RequestedOrganization.Site  %></td>
                            </tr>  
                        </table>          
                    </div>
                    <div class="statement_in" style="border-bottom-color:#fff;">                                            
                        <p><strong>Все пользователи эталонной организации</strong></p>
                        <p><div class="statement_table">
                               <asp:Repeater runat="server" ID="rptUsers">
                                   <HeaderTemplate>
                                       <table width="100%"  id="idTable">
                                       <tr class="th">
                                           <td class="left-th" width="25%"><div>Логин</div></td>
                                           <td width="45%"><div>ФИО</div></td>
                                           <td class="right-th" width="30%"><div>Статус</div></td>
                                       </tr>
                                   </HeaderTemplate>
                                   <ItemTemplate>
                                       <tr>
                                           <td>
                                               <%# (Eval("Login").ToString()==CurrentUser.login) ? Eval("Login") : string.Format("<a href='../../Accounts/Users/View.aspx?login={0}'>{0}</a>",Eval("Login"))%>
                                           </td>
                                           <td><%# Eval("FIO") %><BR/><nobr><a href="mailto:<%# Convert.ToString(Eval("email")) %>"><%# Convert.ToString(Eval("email")) %></a></nobr></td>
                                           <td><%# GetUserStatus((string)Eval("Status")) %></td>
                                       </tr>
                                   </ItemTemplate>
                                   <FooterTemplate>
                                   </table>
                                   </FooterTemplate>
                               </asp:Repeater>
                               <asp:Label ID="lblNoUsers" runat="server" Visible="false" Text="Нет пользователей" />
                           </div>
                        </p>

                    </div>

                </div>
            </div>

        </div>
        <% if (IsRegistrationDocumentExists)
           {%>
            <script type="text/javascript">
                window.open('/Profile/ConfirmedDocumentView.aspx?login=<%= CurrentUser.login %>', 'RegZajavView', 'top=0,left=0,width=600,height=560,status=no,toolbar=no,menubar=no,scrollbars=yes,resizable=yes');
            </script>
        <% }%>
    </form>
</asp:Content>
