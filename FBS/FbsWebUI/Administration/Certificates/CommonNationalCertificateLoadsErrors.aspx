<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommonNationalCertificateLoadsErrors.aspx.cs" 
    Inherits="Fbs.Web.Administration.Certificates.CommonNationalCertificateLoadsErrors" 
    MasterPageFile="~/Common/Templates/Administration.master" %>
    
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Import Namespace="Fbs.Web" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContent" runat="server">
    <p id="ExportContainer">
        <a href="/Administration/Certificates/CommonNationalCertificateLoadsErrorsExport.aspx?id=<%= Request.QueryString["id"] %>">Экспортировать в Excel</a>
    </p>

    <table class="pager">
    <tr><td>
    
        <web:DataSourcePager runat="server"
            id="DataSourcePagerHead" 
            DataSourceId="dsResultsListCount"
	        StartRowIndexParamName="start" 
	        MaxRowCountParamName="count"
	        HideDefaultTemplates="true"
	        AlwaysShow="true"
	        DefaultMaxRowCount="20"
	        DefaultMaxRowCountSource="Cookies">
            <Header>
                Свидетельства #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
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
    
    <web:NoRecordsText runat="server" ControlId="dgResultsList">
        <Message><p class="notfound">Не найдено ни одной записи</p>
        <style type="text/css">#ExportContainer{display:none;}</style></Message> 
    </web:NoRecordsText>
    
    <asp:DataGrid runat="server" id="dgResultsList"
            DataSourceID="dsResultsList"
            AutoGenerateColumns="false" 
            EnableViewState="false"
            ShowHeader="True" 
            GridLines="None"
            CssClass="table-th">
        <HeaderStyle CssClass="th" />
        
        <Columns>
            <asp:TemplateColumn>
                <HeaderStyle CssClass="left-th" />
                <HeaderTemplate>
                    <div>Ошибка</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("Error") %> <%# GetRowIndex(Eval("RowIndex"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
                <HeaderStyle CssClass="right-th" Width="1%" />
                <HeaderTemplate>
                    <div></div>
                </HeaderTemplate>
                <ItemTemplate>
                </ItemTemplate>
            </asp:TemplateColumn>                                                         
        </Columns>
    </asp:DataGrid>
    
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
                Свидетельства #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
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

    <asp:SqlDataSource  runat="server" ID="dsResultsList" 
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="dbo.SearchCommonNationalExamCertificateLoadingTaskError"  CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <asp:QueryStringParameter Name="taskId" Type="Int64" QueryStringField="id" />
            <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
            <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count"
                CheckParamName="count" CheckParamSource="Cookies" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    <asp:SqlDataSource  runat="server" ID="dsResultsListCount" 
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="dbo.SearchCommonNationalExamCertificateLoadingTaskError"  CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <asp:QueryStringParameter Name="taskId" Type="Int64" QueryStringField="id" />
            <asp:Parameter Name="showCount" Type="Boolean" DefaultValue="True" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

<script runat="server">
    public string GetRowIndex(object index)
    {
        if (Convert.IsDBNull(index))
            return String.Empty;
        return String.Format("({0})", Convert.ToInt64(index));
    }
</script>