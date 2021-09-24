<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExcelTemplates.aspx.cs"
    Inherits="Fbs.Web.Administration.Reports.ExcelTemplates" MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Fbs.Core" %>
<%@ Import Namespace="Fbs.Web" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <form id="Form1" runat="server">
    <table class="pager">
    <tr><td>
    
        <web:DataSourcePager runat="server"
            id="DataSourcePagerHead" 
            DataSourceId="dsDocumentListCount"
	        StartRowIndexParamName="start" 
	        MaxRowCountParamName="count"
	        HideDefaultTemplates="true"
	        AlwaysShow="true"
	        DefaultMaxRowCount="20"
	        DefaultMaxRowCountSource="Cookies">
            <Header>
                Документы #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
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
    
    <asp:DataGrid Width="100%" runat="server" ID="dgDocumentList"
        DataSourceID="dsDocumentList"
        AutoGenerateColumns="false" 
        EnableViewState="false"
        ShowHeader="True" 
        GridLines="None"
        CssClass="table-th">
        <HeaderStyle CssClass="th" />
        <Columns>
            <asp:TemplateColumn>
                <HeaderStyle Width="100%" CssClass="left-th" />
                <HeaderTemplate>
                    <div>
                        <web:SortRef runat="server" SortExpr="Name" SortExprText="Название" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <a  title='<%# Eval("Description") %>' href='<%# (!string.IsNullOrEmpty(Convert.ToString(Eval("RelativeUrl")))) ?
                            Eval("RelativeUrl") : string.Format("/Document.aspx?id={0}", Eval("Id")) %>'>
                        <%# Eval("Name")%>
                    </a>
                </ItemTemplate>

            </asp:TemplateColumn>

            <asp:TemplateColumn ItemStyle-Wrap="false">
            <HeaderStyle CssClass="right-th"/>
            <HeaderTemplate>
                <div></div>
            </HeaderTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    
    <web:NoRecordsText runat="server" ControlId="dgDocumentList">
        <Message><p class="notfound">Не найдено ни одного шаблона</p></Message>
    </web:NoRecordsText>
    
    <table class="pager">
    <tr><td>
    
        <web:DataSourcePager runat="server"
            DataSourceId="dsDocumentListCount"
	        StartRowIndexParamName="start" 
	        MaxRowCountParamName="count"
	        HideDefaultTemplates="true"
	        AlwaysShow="true"
	        DefaultMaxRowCount="20"
	        DefaultMaxRowCountSource="Cookies">
            <Header>
                Документы #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
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

    <asp:SqlDataSource runat="server" ID="dsDocumentList" 
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="SearchDocument" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <asp:QueryStringParameter Name="Name" Type="String" QueryStringField="Name" />
            <asp:Parameter DefaultValue="true" Name="IsActive" Type="Boolean" />
            <asp:Parameter DefaultValue="ExcelReports" Name="contextCodes" Type="String" />
            <asp:QueryStringParameter DefaultValue="0" Name="sortColumn" Type="String" QueryStringField="sort" />
            <asp:QueryStringParameter DefaultValue="1" Name="sortAsc" Type="Int16" QueryStringField="sortorder" />
            <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
            <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count"
                CheckParamName="count" CheckParamSource="Cookies" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="dsDocumentListCount" 
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="SearchDocument" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <asp:QueryStringParameter Name="name" Type="String" QueryStringField="name" />
            <asp:Parameter DefaultValue="true" Name="IsActive" Type="Boolean" />
            <asp:Parameter DefaultValue="ExcelReports" Name="contextCodes" Type="String" />
            <asp:Parameter DefaultValue="true" Name="showCount" Type="Boolean" />
        </SelectParameters>
    </asp:SqlDataSource>
    </form>
</asp:Content>
