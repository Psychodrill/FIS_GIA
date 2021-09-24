<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuthenticationHistory.aspx.cs" 
    Inherits="Esrp.Web.Administration.Accounts.Users.AuthenticationHistory"
    MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Import Namespace="Esrp.Web" %>
<%@ Import Namespace="Esrp.Core" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHead">

<script type="text/javascript">
    jQuery(document).ready(function () {
        var params = {
            changedEl: 'select',
            visRows: 7,
            scrollArrows: true
        }
        cuSel(params);
    });
</script>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">

<div class="left_col">
    <form id="Form2" runat="server">
    <asp:ValidationSummary CssClass="error_block"  ID="ValidationSummary2" runat="server" DisplayMode="BulletList"
        EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />

                    <div class="col_in">
						<div class="statement edit">
							
							<p class="title">Логин/Е-mail&nbsp;<%= CurrentUserLogin %></p>
							<p class="back"><a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
							<p class="statement_menu">
                                <% if ("IS".Equals(Request.QueryString["UserKey"]) || "OU".Equals(Request.QueryString["UserKey"]))
                                   { %>
								<a href="/Administration/Accounts/Users/Edit<%= GetUserKeyCode() %>.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" 
                                    class="gray"><span>Изменить</span></a>
                                <a href="/Administration/Accounts/Users/ChangePassword.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" 
                                    title="" class="gray"><span>Изменить пароль</span></a>
                                <a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" 
                                    title="История изменений" class="gray"><span>История изменений</span></a>
                                <a href="#" title="История аутентификаци" class="active"><span>История аутентификаций</span></a>
                                <a href="/Administration/Accounts/Users/AccountKeyList.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" 
                                    title="Ключи доступа" class="gray"><span>Ключи доступа</span></a>
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
                                <a href="/Administration/Accounts/Users/RemindPassword.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" 
                                    title="Запросить смену пароля" class="gray"><span>Запросить смену пароля</span></a>
                                <% } %> 
                                <a href="/Administration/Accounts/Users/ChangePassword.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" 
                                    class="gray" title="Изменить пароль"><span>Изменить пароль</span></a>
                                <a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>&UserKey=<%= GetUserKeyCode() %>" 
                                    title="История изменений" class="gray"><span>История изменений</span></a>
                                <a href="#" title="История аутентификаци" class="active"><span>История аутентификаций</span></a>
                                <% } %>
							</p>
							<div class="clear"></div>
                        </div>
                <div class="main_table">
					<div class="sort table_header">
						<div class="sorted">
                            <web:DataSourcePager ID="DataSourcePager2" runat="server"
                                DataSourceId="DataSourcePagerHead"
	                            StartRowIndexParamName="start"
	                            MaxRowCountParamName="count"
                                HideDefaultTemplates="true"
                                AlwaysShow="true">
			                    <Header>
                                <div class="sort_page">
								    <select id="selectPageCount" onchange="changePageCount()">
                                    <web:DataSourceMaxRowCount ID="DataSourceMaxRowCount1" runat="server"
                                            Variants="20,50,100"                    
                                            DataSourcePagerId="DataSourcePagerHead">
                                        <Footer></Footer>
                                        <Separator></Separator>
                                        <ActiveTemplate><option selected="selected" value="#count#"><span>#count#</span></option></ActiveTemplate>                                             
                                        <InactiveTemplate><option value="#count#"><span>#count#</span></option></InactiveTemplate> 
                                        <%--InactiveTemplate><a href="/SetMaxRowCount.aspx?count=#count#" title="Отображать #count# записей на странице">#count#</a>
                                        </InactiveTemplate--%> 
                                    </web:DataSourceMaxRowCount>
                                    </select>
							    </div>	
                                </Header>
                            </web:DataSourcePager> 
                            <p class="rec">
                                Записей на странице:&nbsp;
                            </p>
							<p class="views">
                                <web:DataSourcePager runat="server"
                                    id="DataSourcePagerHead" 
                                    DataSourceId="dsAuthenticationHistoryListCount"
	                                StartRowIndexParamName="start" 
	                                MaxRowCountParamName="count"
	                                HideDefaultTemplates="true"
	                                AlwaysShow="true"
	                                DefaultMaxRowCount="20"
	                                DefaultMaxRowCountSource="Cookies">
                                    <Header>
                                        Показано <span>#StartRowIndex#-#LastRowIndex#</span> из #TotalCount#.                               
                                    </Header>
                                </web:DataSourcePager>
                            </p>
							<p class="page_nav">                            
                                <web:DataSourcePager ID="DataSourcePager1" runat="server"
                                    DataSourceId="DataSourcePagerHead"
	                                StartRowIndexParamName="start"
                                    AlwaysShow="false"
	                                MaxRowCountParamName="count">
                                    <Header>Страницы&nbsp;</Header>
			                        <PrevGroupTemplate></PrevGroupTemplate>
                                    <NextGroupTemplate></NextGroupTemplate>
                                    <FirstPageTemplate><a href="#PageUrl#"><<</a>&nbsp;</FirstPageTemplate>
                                    <LastPageTemplate>&nbsp;<a href="#PageUrl#">>></a></LastPageTemplate>
                                    <ActivePrevPageTemplate><a href="#PageUrl#"><</a>&nbsp;</ActivePrevPageTemplate>
                                    <ActivePageTemplate><span>#PageNo#</span> </ActivePageTemplate>
                                    <ActiveNextPageTemplate>&nbsp;<a href="#PageUrl#">></a></ActiveNextPageTemplate>
                                </web:DataSourcePager>  
                            </p>
						    <div class="clear"></div>
					    </div>
					</div>
					<div class="clear"></div>

    <asp:DataGrid runat="server" id="dgAuthenticationHistoryList"
        DataSourceID="dsAuthenticationHistoryList"
        AutoGenerateColumns="false" 
        EnableViewState="false"
        ShowHeader="True" 
        GridLines="None"
        UseAccessibleHeader="true"
        width="100%"
        CssClass="table-th">
        <HeaderStyle CssClass="actions" />
        <Columns>

            <asp:TemplateColumn>
                <HeaderStyle Width="20%" />
                <HeaderTemplate><div>Время</div></HeaderTemplate>
                <ItemTemplate>
                    <%# Convert.ToDateTime(Eval("Date")).ToString() %>
                </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" />
                <HeaderTemplate>
                    IP-адрес
                </HeaderTemplate>
                <ItemTemplate>
                    <%# AccountExtentions.GetIpFullName(Eval("Ip"), Eval("IsVpnIp")) %>
                </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
                <HeaderStyle Width="70%" />
                <HeaderTemplate><div>Ошибки</div></HeaderTemplate>
                <ItemTemplate>
                    <%# AccountExtentions.GetAuthenticationErrorsDescription(Eval("IsPasswordValid"), 
                        Eval("IsIpValid")) %>
                </ItemTemplate>
            </asp:TemplateColumn>
        
        </Columns>
    </asp:DataGrid>

    <web:NoRecordsText runat="server" ControlId="dgAuthenticationHistoryList">
        <Message><p class="notfound">Пользователь не входил в систему</p></Message>
    </web:NoRecordsText>
    
                <div class="sort table_footer">
                    <div class="f_left">
                    </div>
                    <div class="sorted">

                            <web:DataSourcePager ID="DataSourcePager5" runat="server"
                                DataSourceId="DataSourcePagerHead"
	                            StartRowIndexParamName="start"
	                            MaxRowCountParamName="count"
                                HideDefaultTemplates="true"
                                AlwaysShow="true">
			                    <Header>
                                <div class="sort_page">
								    <select id="selectPageCountBottom" onchange="changePageCount(this)">
                                    <web:DataSourceMaxRowCount ID="DataSourceMaxRowCount1" runat="server"
                                            Variants="20,50,100"                    
                                            DataSourcePagerId="DataSourcePagerHead">
                                        <Footer></Footer>
                                        <Separator></Separator>
                                        <ActiveTemplate><option selected="selected" value="#count#"><span>#count#</span></option></ActiveTemplate>                                             
                                        <InactiveTemplate><option value="#count#"><span>#count#</span></option></InactiveTemplate> 
                                        <%--InactiveTemplate><a href="/SetMaxRowCount.aspx?count=#count#" title="Отображать #count# записей на странице">#count#</a>
                                        </InactiveTemplate--%> 
                                    </web:DataSourceMaxRowCount>
                                    </select>
							    </div>	
                                </Header>
                            </web:DataSourcePager> 
                            <p class="rec">
                                Записей на странице:&nbsp;
                            </p>
							<p class="views">
                                <web:DataSourcePager runat="server"
                                    id="DataSourcePager6" 
                                    DataSourceId="DataSourcePagerHead"
	                                StartRowIndexParamName="start" 
	                                MaxRowCountParamName="count"
	                                HideDefaultTemplates="true"
	                                AlwaysShow="true"
	                                DefaultMaxRowCount="20"
	                                DefaultMaxRowCountSource="Cookies">
                                    <Header>
                                        Показано <span>#StartRowIndex#-#LastRowIndex#</span> из #TotalCount#.                               
                                    </Header>
                                </web:DataSourcePager>
                            </p>
							<p class="page_nav">                            
                                <web:DataSourcePager ID="DataSourcePager7" runat="server"
                                    DataSourceId="DataSourcePagerHead"
	                                StartRowIndexParamName="start"
                                    AlwaysShow="false"
	                                MaxRowCountParamName="count">
                                    <Header>Страницы&nbsp;</Header>
			                        <PrevGroupTemplate></PrevGroupTemplate>
                                    <NextGroupTemplate></NextGroupTemplate>
                                    <FirstPageTemplate><a href="#PageUrl#"><<</a>&nbsp;</FirstPageTemplate>
                                    <LastPageTemplate>&nbsp;<a href="#PageUrl#">>></a></LastPageTemplate>
                                    <ActivePrevPageTemplate><a href="#PageUrl#"><</a>&nbsp;</ActivePrevPageTemplate>
                                    <ActivePageTemplate><span>#PageNo#</span> </ActivePageTemplate>
                                    <ActiveNextPageTemplate>&nbsp;<a href="#PageUrl#">></a></ActiveNextPageTemplate>
                                </web:DataSourcePager>  
                            </p>
						    <div class="clear"></div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    </div>

    <asp:SqlDataSource runat="server" ID="dsAuthenticationHistoryList" 
        ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="SearchAccountAuthenticationLog" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <asp:QueryStringParameter Name="login" Type="String" QueryStringField="login" />
            <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
            <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count"
                CheckParamName="count" CheckParamSource="Cookies" />
            <asp:Parameter DefaultValue="false" Name="showCount" Type="Boolean" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="dsAuthenticationHistoryListCount" 
        ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="SearchAccountAuthenticationLog" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <asp:QueryStringParameter Name="login" Type="String" QueryStringField="login" />
            <asp:Parameter DefaultValue="true" Name="showCount" Type="Boolean" />
        </SelectParameters>
    </asp:SqlDataSource>    
</asp:Content>

