<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchCheckResult.aspx.cs" 
    Inherits="Fbs.Web.Certificates.Entrants.BatchCheckResult" 
    MasterPageFile="~/Common/Templates/Certificates.Master" %>
    
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>

<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
<% if (!HasResults) {%>
    <meta http-equiv="refresh" content="3" />
<%} %>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphCertificateActions">
    <div class="h10"></div>
    <div class="border-block">
        <div class="tr"><div class="tt"><div></div></div></div>
        <div class="content">
        <ul>
            <li><a href="/Certificates/Entrants/BatchCheckResultExportCsv.aspx?id=<%= Request.QueryString["id"] %>" 
                title="Экспорт в CSV" class="gray">Экспорт в CSV</a></li>
        </ul>
        </div>
        <div class="br"><div class="tt"><div></div></div></div>
    </div>    
</asp:Content>

<asp:Content ContentPlaceHolderID="cphCertificateContent" runat="server">

    <web:DataSourcePager runat="server" 
        DataSourceId="dsResultsListCount"
	    StartRowIndexParamName="start" 
	    MaxRowCountParamName="count"
	    DefaultMaxRowCount="20">
        <Header>
            <table class="pager"><tr>
                <td>Результаты проверок #StartRowIndex#-#LastRowIndex# из #TotalCount#</td>
                <td align="right">
        </Header>
        <Footer></td></tr></table></Footer>
        <PrevGroupTemplate></PrevGroupTemplate>
        <NextGroupTemplate></NextGroupTemplate>
        <FirstPageTemplate><a href="#PageUrl#"><<</a>&nbsp;</FirstPageTemplate>
        <LastPageTemplate>&nbsp;<a href="#PageUrl#">>></a></LastPageTemplate>
        <ActivePrevPageTemplate><a href="#PageUrl#"><</a>&nbsp;</ActivePrevPageTemplate>
        <ActivePageTemplate><span>#PageNo#</span> </ActivePageTemplate>
        <ActiveNextPageTemplate>&nbsp;<a href="#PageUrl#">></a></ActiveNextPageTemplate>
    </web:DataSourcePager>
    
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
                Результаты проверок #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
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
    
    
    <div id="ResultContainer" style="height:auto;">
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
            <HeaderStyle CssClass="left-th" Width="35%" />
            <HeaderTemplate>
                <div><nobr>Образовательное учреждение</nobr></div>
            </HeaderTemplate>
            <ItemTemplate>
                 <%# Convert.ToBoolean(Eval("IsExist")) ?
                        (Convert.IsDBNull(Eval("OrganizationName")) ? "Не найдено" : Convert.ToString(Eval("OrganizationName"))) :
                        "<span style=\"color:Red\" title='Информации по поступлению не найдено'>Не&nbsp;найдено</span>" %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderTemplate>
                <div title="Свидетельство ЕГЭ">Cв-во&nbsp;ЕГЭ</div>
            </HeaderTemplate>
            <HeaderStyle Width="10%" />
            <ItemStyle HorizontalAlign="Center" />
            <ItemTemplate>
                <nobr>
                <%# String.Format("<a href=\"/Certificates/CommonNationalCertificates/CheckResult.aspx?number={0}&isOriginal=False\">{0}</a>", Eval("CertificateNumber")) %>
                </nobr>
            </ItemTemplate>
            </asp:TemplateColumn>
                
            <asp:TemplateColumn>
            <HeaderStyle />
            <HeaderTemplate>
                <div>Фамилия</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("LastName") %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderTemplate>Имя</HeaderTemplate>
            <HeaderStyle />
            <ItemTemplate>
                <%# Eval("FirstName") %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderStyle CssClass="right-th" />
            <HeaderTemplate><div>Отчество&nbsp;</div></HeaderTemplate>
            <HeaderStyle />
            <ItemTemplate>
                <%# Eval("PatronymicName") %>
            </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    </div>

    <web:NoRecordsText runat="server" ControlId="dgResultsList">
        <Message><style type="text/css">#ExportContainer, #ResultContainer {display:none;}</style> 
        <p class="notfound">Не обработано ни одной записи</p></Message>
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
                Результаты проверок #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
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
        SelectCommand="dbo.SearchEntrantCheck"  CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <fbs:CurrentUserParameter Name="login" Type="String" />
            <asp:QueryStringParameter Name="batchId" QueryStringField="id" Type="Int64" />
            <asp:QueryStringParameter DefaultValue="1" Name="startRowIndex" Type="String" QueryStringField="start" />
            <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count"
                CheckParamName="count" CheckParamSource="Cookies" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    <asp:SqlDataSource  runat="server" ID="dsResultsListCount" 
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="dbo.SearchEntrantCheck"  CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <fbs:CurrentUserParameter Name="login" Type="String" />
            <asp:QueryStringParameter Name="batchId" QueryStringField="id" Type="Int64" />
            <asp:Parameter Name="showCount" Type="Boolean" DefaultValue="True" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>