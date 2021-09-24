<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommonNationalCertificateLoads.aspx.cs" 
    Inherits="Fbs.Web.Administration.Certificates.CommonNationalCertificateLoads" 
    MasterPageFile="~/Common/Templates/Administration.master" %>
    
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Import Namespace="Fbs.Web" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <%--<meta http-equiv="refresh" content="5" />--%>
</asp:Content>


<asp:Content  ContentPlaceHolderID="cphContent" runat="server">
    
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
        <Message><p class="notfound">Не найдено ни одной записи</p></Message>
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
                    <div>Дата</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <%# Convert.ToDateTime(Eval("UpdateDate")) %>
                </ItemTemplate>
            </asp:TemplateColumn>
            
  
            <asp:TemplateColumn>
                <HeaderTemplate>
                   Файл
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("SourceBatchUrl") %>
                </ItemTemplate>
            </asp:TemplateColumn>   
            
            <asp:TemplateColumn>
                <HeaderStyle CssClass="right-th" Width="25%" />
                <HeaderTemplate>
                    <div>Состояние</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# GetState(Eval("IsProcess"), Eval("IsLoaded"), Eval("IsCorrect"), 
                        Eval("IsActive"), Eval("Id"), Eval("ErrorCount")) %>
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
        SelectCommand="dbo.SearchCommonNationalExamCertificateLoadingTask"  CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
            <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count"
                CheckParamName="count" CheckParamSource="Cookies" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    <asp:SqlDataSource  runat="server" ID="dsResultsListCount" 
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="dbo.SearchCommonNationalExamCertificateLoadingTask"  CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <asp:Parameter Name="showCount" Type="Boolean" DefaultValue="True" />
        </SelectParameters>
    </asp:SqlDataSource>
    
</asp:Content>

<script runat="server">
    
    public string GetState(object IsProcess, object IsLoaded, object IsCorrect, object IsActive,
        object Id, object ErrorCount)
    {
        if (!Convert.IsDBNull(IsProcess) && Convert.ToBoolean(IsProcess))
            return "Выполняется";

        if (!Convert.IsDBNull(IsLoaded) && Convert.ToBoolean(IsLoaded))
        {
            if (!Convert.IsDBNull(IsCorrect) && Convert.ToBoolean(IsCorrect))
                return "Выполнено";
            else
                // Выполнено с ошибками
                return String.Format("Выполнено (<a href=\"/Administration/Certificates/CommonNationalCertificateLoadsErrors.aspx?id={0}\">{1} ошибок</a>)",
                    Id, ErrorCount);
        }

        if (!Convert.IsDBNull(IsActive) && Convert.ToBoolean(IsActive))
            return String.Format("<a onclick=\"return confirm('Вы действительно хотите загрузить данные?')\" href=\"/Administration/Certificates/CommonNationalCertificateLoad.aspx?id={0}\">Выполнить</a>", Id);

        return String.Empty;
    }
    
</script>