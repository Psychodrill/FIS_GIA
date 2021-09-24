<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="History.aspx.cs" 
    Inherits="Esrp.Web.Administration.Accounts.Administrators.History"
    MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Import Namespace="Esrp.Web" %>

<asp:Content runat="server" ContentPlaceHolderID="cphActions">
    <div class="h10"></div>
    <div class="border-block">
        <div class="tr"><div class="tt"><div></div></div></div>
        <div class="content">
        <ul>
        <li><a href="/Administration/Accounts/Administrators/Edit.aspx?Login=<%= Request.QueryString["login"] %>"
            title="Редактирование" class="gray">Редактирование</a></li>
        </ul>
    </div>
    <div class="br"><div class="tt"><div></div></div></div>
    </div> 
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">

    <table class="pager">
    <tr><td>
    
        <web:DataSourcePager runat="server"
            id="DataSourcePagerHead" 
            DataSourceId="dsHistoryListCount"
	        StartRowIndexParamName="start" 
	        MaxRowCountParamName="count"
	        HideDefaultTemplates="true"
	        AlwaysShow="true"
	        DefaultMaxRowCount="20"
	        DefaultMaxRowCountSource="Cookies">
            <Header>
                Изменения #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
                <web:DataSourceMaxRowCount runat="server"
                    Variants="20,50,100"
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

    <asp:DataGrid runat="server" id="dgHistoryList"
        DataSourceID="dsHistoryList"
        AutoGenerateColumns="false" 
        EnableViewState="false"
        ShowHeader="True" 
        GridLines="None"
        CssClass="table-th">
        <HeaderStyle CssClass="th" />
        <Columns>

            <asp:TemplateColumn>
                <HeaderStyle CssClass="left-th" Width="10%" />
                <HeaderTemplate><div></div></HeaderTemplate>
                <ItemTemplate>
                    <a href='HistoryVersion.aspx?login=<%#Eval("Login")%>&version=<%#Eval("VersionId")%>'>
                        просмотр
                    </a>
                </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
                <HeaderStyle Width="5%" />
                <HeaderTemplate>
                    Версия
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("VersionId")%>
                </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
                <HeaderStyle Width="20%" />
                <HeaderTemplate>
                    Дата
                </HeaderTemplate>
                <ItemTemplate>
                    <%#(DateTime)Eval("UpdateDate") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
                <HeaderStyle Width="45%" />
                <HeaderTemplate>
                    Редактор
                </HeaderTemplate>
                <ItemTemplate>
                    <%# AccountExtentions.GetFullNameWithHint(Eval("EditorLastName"), 
                        Eval("EditorFirstName"), Eval("EditorPatronymicName"), 
                        Eval("EditorLogin"), Eval("EditorIp"),  Eval("IsVpnEditorIp"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
                <HeaderStyle CssClass="right-th" Width="15%" />
                <HeaderTemplate>
                <div>
                    Изменение
                </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# AccountExtentions.GetChangesDescription(Eval("IsEdit"), 
                        Eval("IsPasswordChange"), Eval("IsStatusChange")) %>
                </ItemTemplate>
            </asp:TemplateColumn>
        
        </Columns>
    </asp:DataGrid>

    <web:NoRecordsText runat="server" ControlId="dgHistoryList">
        <Message><p class="notfound">Изменений не было</p></Message>
    </web:NoRecordsText>

    <table class="pager">
    <tr><td>
    
        <web:DataSourcePager runat="server"
            DataSourceId="DataSourcePagerHead"
	        StartRowIndexParamName="start" 
	        MaxRowCountParamName="count"
	        HideDefaultTemplates="true"
	        AlwaysShow="true"
	        DefaultMaxRowCount="20"
	        DefaultMaxRowCountSource="Cookies">
            <Header>
                Изменения #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
                <web:DataSourceMaxRowCount runat="server"
                    Variants="20,50,100"
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

    <asp:SqlDataSource runat="server" ID="dsHistoryList" 
        ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="SearchAccountLog" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <asp:QueryStringParameter Name="login" Type="String" QueryStringField="login" />
            <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
            <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count"
                CheckParamName="count" CheckParamSource="Cookies" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="dsHistoryListCount" 
        ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="SearchAccountLog" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <asp:QueryStringParameter Name="login" Type="String" QueryStringField="login" />
            <asp:Parameter DefaultValue="true" Name="showCount" Type="Boolean" />
        </SelectParameters>
    </asp:SqlDataSource>    
</asp:Content>
