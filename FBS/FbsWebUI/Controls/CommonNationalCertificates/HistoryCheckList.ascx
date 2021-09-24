<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HistoryCheckList.ascx.cs" Inherits="Fbs.Web.Controls.CommonNationalCertificates.HistoryCheckList" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="asp" Namespace="WebControls" Assembly="WebControls" %>

<%--История проверок (начало) --%>
<h1>История проверок</h1>

            <table class="pager">
            <tr><td>
    
                <web:DataSourcePager runat="server"
                    id="DataSourcePagerHead" 
                    DataSourceId="checkHistoryCountDataSource"
	                StartRowIndexParamName="start" 
	                MaxRowCountParamName="count"
	                HideDefaultTemplates="true"
	                AlwaysShow="true"
	                DefaultMaxRowCount="10"
	                DefaultMaxRowCountSource="Cookies">
                    <Header>
                        Результаты проверок #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
                        <web:DataSourceMaxRowCount runat="server"
                            Variants="10,20,50"
                            DataSourcePagerId="DataSourcePagerHead">
                        <Header>Записей на странице: </Header>
                        <Footer>.</Footer>
                        <Separator>, </Separator>
                        <ActiveTemplate><span>#count#</span></ActiveTemplate> 
                        <InactiveTemplate><a href="/SetMaxRowCount.aspx?count=#count#" 
                        title="Отображать #count# записей на странице">#count#</a></InactiveTemplate> 
                        </web:DataSourceMaxRowCount>                 
                    </Header>
                </web:DataSourcePager>
        
            </td>    
            <td align="right">

                <web:DataSourcePager runat="server"
                    DataSourceId="DataSourcePagerHead"
	                StartRowIndexParamName="start"
	                MaxRowCountParamName="count">
			        <PrevGroupTemplate></PrevGroupTemplate>
                    <NextGroupTemplate></NextGroupTemplate>
                    <FirstPageTemplate><a href="#PageUrl#"><<</a>&nbsp;</FirstPageTemplate>
                    <LastPageTemplate>&nbsp;<a href="#PageUrl#">>></a></LastPageTemplate>
                    <ActivePrevPageTemplate><a href="#PageUrl#"><</a>&nbsp;</ActivePrevPageTemplate>
                    <ActivePageTemplate><span>#PageNo#</span> </ActivePageTemplate>
                    <ActiveNextPageTemplate>&nbsp;<a href="#PageUrl#">></a></ActiveNextPageTemplate>
                </web:DataSourcePager>    

            </td></tr>
            </table>

        <asp:DataGrid runat="server" id="dgUserList"
            DataSourceID="checkHistoryDataSource"
            AutoGenerateColumns="false" 
            EnableViewState="false"
            ShowHeader="True" 
            GridLines="None"
            CssClass="table-th f600">
            <HeaderStyle CssClass="th" />
            <Columns>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Center" CssClass="left-th" />
                    <HeaderTemplate>
                        <div>Дата</div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.ToString(Eval("DateCreate"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>

                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <div>Пользователь</div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <% if (this.Page.User.IsInRole("EditAdministratorAccount"))
                           { %>
                            <a href="/Administration/Accounts/Users/View.aspx?login=<%# Eval("Login") %>" title="Посмотреть профиль"><%# Eval("Login") %></a>
                        <% } else { %>
                            <%# Eval("Login") %>
                        <% } %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Center"/>
                    <HeaderTemplate>
                        <div>Статус</div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%#Eval("Status") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Center" CssClass="right-th" />
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <div>Результат</div>
                    </HeaderTemplate>
                    <ItemTemplate> 
                        <asp:LinkButton  CommandName="RedirectToNote" CommandArgument='<%#Eval("Login")+","+Eval("LinkResult.Url") %>' runat="server"          
                            Text='<%# Eval("LinkResult.Text") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
            </Columns>
        </asp:DataGrid>

        <web:NoRecordsText ID="NoRecordsText1" runat="server" ControlId="dgUserList">
            <Message><div class="notfound w600">
                <img src="/Common/Images/spacer.gif" width="32" height="31" style="background-image:url('/Common/Images/important.gif')"/>
                Не найдено ни одной записи</div></Message>
        </web:NoRecordsText>
        
        <table class="pager">
        <tr><td>
    
            <web:DataSourcePager ID="DataSourcePager1" runat="server"
                DataSourceId="DataSourcePagerHead"
	            StartRowIndexParamName="start" 
	            MaxRowCountParamName="count"
	            HideDefaultTemplates="true"
	            AlwaysShow="true"
	            DefaultMaxRowCount="10"
	            DefaultMaxRowCountSource="Cookies">
                <Header>
                    Результаты проверок #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
                    <web:DataSourceMaxRowCount runat="server"
                        Variants="10,20,50"
                        DataSourcePagerId="DataSourcePagerHead">
                    <Header>Записей на странице: </Header>
                    <Footer>.</Footer>
                    <Separator>, </Separator>
                    <ActiveTemplate><span>#count#</span></ActiveTemplate> 
                    <InactiveTemplate><a href="/SetMaxRowCount.aspx?count=#count#" 
                        title="Отображать #count# записей на странице">#count#</a></InactiveTemplate> 
                    </web:DataSourceMaxRowCount>                 
                </Header>
            </web:DataSourcePager>
        
        </td>    
        <td align="right">

            <web:DataSourcePager runat="server"
                DataSourceId="DataSourcePagerHead"
    	        StartRowIndexParamName="start"
	            MaxRowCountParamName="count">
			    <PrevGroupTemplate></PrevGroupTemplate>
                <NextGroupTemplate></NextGroupTemplate>
                <FirstPageTemplate><a href="#PageUrl#"><<</a>&nbsp;</FirstPageTemplate>
                <LastPageTemplate>&nbsp;<a href="#PageUrl#">>></a></LastPageTemplate>
                <ActivePrevPageTemplate><a href="#PageUrl#"><</a>&nbsp;</ActivePrevPageTemplate>
                <ActivePageTemplate><span>#PageNo#</span> </ActivePageTemplate>
                <ActiveNextPageTemplate>&nbsp;<a href="#PageUrl#">></a></ActiveNextPageTemplate>
            </web:DataSourcePager>    

        </td></tr>
        </table>
    <%--История проверок (конец) --%>
    
    <asp:ObjectDataSource runat="server" ID="checkHistoryCountDataSource" DataObjectTypeName="System.Int32" TypeName="FbsServices.CNECService" SelectMethod="SelectCheckHystoryCount" OnSelecting="СheckHistoryCountDataSourceOnSelecting" />

    <asp:ObjectDataSource ID="checkHistoryDataSource" runat="server" DataObjectTypeName="FbsWebViewModel.CNEC.HistoryCheckCertificateView"
        TypeName="FbsServices.CNECService" SelectMethod="SelectCheckHystory" OnSelecting="СheckHistoryDataSourceOnSelecting" />