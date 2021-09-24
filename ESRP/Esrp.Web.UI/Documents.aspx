<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Documents.aspx.cs" Inherits="Esrp.Web.Documents"
         MasterPageFile="~/Common/Templates/Main.Master" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Controls" Assembly="Esrp.Web.UI" %>

<asp:Content runat="server" ContentPlaceHolderID="cphHead">
    <script src="/Common/Scripts/Filter.js" type="text/javascript"> </script>
    <script src="/Common/Scripts/FixPng.js" type="text/javascript"> </script>


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
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSecondLevelMenu">
    <div class="bottom_line">
        <div class="max_width">
            <esrp:TopMenu ID="SecondLevelMenu1" runat="server" RootResourceKey="Documents" HeaderTemplate="<ul>" 
                         FooterTemplate="</ul>"/>
            <div class="clear"></div>
        </div>
    </div><!--bottom_line-->
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">

<div class="col_in">
    <form id="Form1" runat="server">

    			<div class="main_table">
					<div class="sort table_header">
                        <div class="f_left">
						</div>
						<div class="sorted">
                            <span class="f_left">
                                </span>
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
                                    DataSourceId="dsDocumentListCount"
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
                                <web:DataSourcePager ID="DataSourcePager3" runat="server"
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
    <asp:DataGrid runat="server" 
        Width="100%" 
        ID="dgDocumentList" 
        DataSourceID="dsDocumentList"
        UseAccessibleHeader="true"
        AutoGenerateColumns="false" 
        EnableViewState="false" 
        ShowHeader="True" 
        GridLines="None"
        CssClass="table-th">
        <HeaderStyle CssClass="actions" />
        <Columns>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%"/>
                <HeaderTemplate>
                    <div>
                        <esrp:sortref_prefix prefix="../Common/Images/" runat="server" sortexpr="Date"
                                            sortexprtext="Дата" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%#DateTime.Parse(this.Eval("Date").ToString()).ToShortDateString()%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="100%"/>
                <HeaderTemplate>
                    <div>
                        <esrp:sortref_prefix prefix="../Common/Images/" runat="server" sortexpr="Name"
                                            sortexprtext="Название" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <a title='<%#this.Eval("Description")%>' href='<%#
                (!string.IsNullOrEmpty(Convert.ToString(this.Eval("RelativeUrl"))))
                    ? this.Eval("RelativeUrl")
                    : string.Format("/Document.aspx?id={0}", this.Eval("Id"))%>'>
                        <%#this.Eval("Name")%>
                    </a>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

    <web:NoRecordsText runat="server" ControlId="dgDocumentList">
        <Message>
            <p class="notfound">
                Не найдено ни одного документа</p>
        </Message>
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
    </form>
    </div>


    <asp:SqlDataSource runat="server" ID="dsDocumentList" ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="SearchDocument" CancelSelectOnNullParameter="False" SelectCommandType="StoredProcedure"
        ProviderName="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString.ProviderName %>">
        <SelectParameters>
            <asp:QueryStringParameter Name="Name" Type="String" QueryStringField="Name" />
            <asp:Parameter DefaultValue="true" Name="IsActive" Type="Boolean" />
            <asp:QueryStringParameter  DefaultValue="other" Name="contextCodes" Type="String" QueryStringField="DocType"/>
            <asp:QueryStringParameter DefaultValue="Date" Name="sortColumn" Type="String" QueryStringField="sort" />
            <asp:QueryStringParameter DefaultValue="0" Name="sortAsc" Type="Int16" QueryStringField="sortorder" />
            <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
            <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count" CheckParamName="count" CheckParamSource="Cookies" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="dsDocumentListCount" ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="SearchDocument" CancelSelectOnNullParameter="false" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="name" Type="String" QueryStringField="name" />
            <asp:Parameter DefaultValue="true" Name="IsActive" Type="Boolean" />
            <asp:QueryStringParameter  DefaultValue="other" Name="contextCodes" Type="String" QueryStringField="DocType"/>
            <asp:Parameter DefaultValue="true" Name="showCount" Type="Boolean" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>