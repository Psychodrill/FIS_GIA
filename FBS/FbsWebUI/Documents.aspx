<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Documents.aspx.cs" Inherits="Fbs.Web.Documents"
    MasterPageFile="~/Common/Templates/PublicDocuments.master" %>



<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Fbs.Core" %>
<%@ Import Namespace="Fbs.Web" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Controls" Assembly="FbsWebUI" %>
<asp:Content runat="server" ContentPlaceHolderID="cphHead">

    <script src="/Common/Scripts/Filter.js" type="text/javascript"></script>

    <script src="/Common/Scripts/FixPng.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphLeftMenu">
    <div class="title">
        <div>
            <h3>
                Документы
            </h3>
        </div>
    </div>
    <ul>
        <asp:Repeater runat="server" ID="RDocumentTypes" DataSourceID="DSDocumentTypes">
            <ItemTemplate>
                <li class="<%#GetCSSClassByQueryString("DocType",Eval("Code")) %>"><a href="/Documents.aspx?DocType=<%# Eval("Code") %>" title="<%#Eval("Name") %>"><%# Eval("Name")%></a></li></ItemTemplate>
        </asp:Repeater>
    </ul>
    <asp:SqlDataSource runat="server" ID="DSDocumentTypes" SelectCommand="SELECT * FROM Context WHERE (Code!='Instruction' AND Code!='ExcelReports' AND Code!='For2011')"
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>">
    </asp:SqlDataSource>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <% // Контролы фильтрации списка %>
    <%-- Убрал фильтр, ибо мало документов
    <!-- filtr -->
    <div class="filtr" id="filtr-layer" style="display:none;">
    <div class="bg-l"></div><div class="bg-r"></div>
    <div class="content-f">
    <div class="fleft">
    <p><a href="javascript:void(0);" title="Установить параметры фильтра" 
        onclick="showFilter(true);">Фильтр:</a> <%= FilterStatusString() %></p>    
    </div>
    <form action="" method="get">
    <input type="submit" class="reset-bt" value="" title="Сбросить фильтр" />
    </form>
    </div>
    </div>
    <!-- /filtr -->

    <!-- filtr2 -->
    <div class="to-be-defined" id="filtr2-layer">
    <div class="filtr2" id="filtr2">
    <div class="h-line"><div class="tl"><div></div></div><div class="tr"><div></div></div><div class="t"><div></div></div></div>
    <div class="content-f2">
    <form action="" method="get">
        <table class="filtr2-table">
        <tr><td class="left">Название&nbsp;&nbsp;</td>
            <td><input name="name" class="txt txt2" 
                value="<%= Request.QueryString["name"] %>" /></td></tr>
        <tr>
        <td colspan="2" class="cell-bt">
            <input type="reset" class="bt" value="Очистить"  onclick="return resetButtonClick();" />&nbsp;
            <input type="submit" class="bt bt2" onclick="showFilter(false);" value="Установить" />&nbsp;
            <input type="button" class="bt" id="btCancel" value="Отмена" onclick="showFilter(false);" style="display:none;"/></td>
        </tr></table>
    </form>
    </div>
    <div class="h-line"><div class="bl"><div></div></div><div class="br"><div></div></div><div class="b"><div></div></div></div>
    </div>
    </div>
    <!-- /filtr2 -->

    <script type="text/javascript">
    <!--

    initFilter();

    function resetButtonClick()
    {
        document.getElementById("name").value = "";
        return false;
    }

    -->
    </script>
 --%>
    <table class="pager">
        <tr>
            <td>
                <web:DataSourcePager runat="server" ID="DataSourcePagerHead" DataSourceId="dsDocumentListCount"
                    StartRowIndexParamName="start" MaxRowCountParamName="count" HideDefaultTemplates="true"
                    AlwaysShow="true" DefaultMaxRowCount="20" DefaultMaxRowCountSource="Cookies">
                    <Header>
                        Документы #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                        <web:DataSourceMaxRowCount runat="server" Variants="20,50,100" DataSourcePagerId="DataSourcePagerHead">
                            <Header>
                                Записей на странице:
                            </Header>
                            <Footer>
                                .</Footer>
                            <Separator>
                                ,
                            </Separator>
                            <ActiveTemplate>
                                <span>#count#</span></ActiveTemplate>
                            <InactiveTemplate>
                                <a href="/SetMaxRowCount.aspx?count=#count#" title="Отображать #count# записей на странице">
                                    #count#</a></InactiveTemplate>
                        </web:DataSourceMaxRowCount>
                    </Header>
                </web:DataSourcePager>
            </td>
            <td align="right">
                <web:DataSourcePager runat="server" DataSourceId="DataSourcePagerHead" StartRowIndexParamName="start"
                    MaxRowCountParamName="count">
                    <PrevGroupTemplate></PrevGroupTemplate>
                    <NextGroupTemplate></NextGroupTemplate>
                    <FirstPageTemplate><a href="#PageUrl#"><<</a>&nbsp;</FirstPageTemplate>
                    <LastPageTemplate>&nbsp;<a href="#PageUrl#">>></a></LastPageTemplate>
                    <ActivePrevPageTemplate><a href="#PageUrl#"><</a>&nbsp;</ActivePrevPageTemplate>
                    <ActivePageTemplate><span>#PageNo#</span> </ActivePageTemplate>
                    <ActiveNextPageTemplate>&nbsp;<a href="#PageUrl#">></a></ActiveNextPageTemplate>
                </web:DataSourcePager>
            </td>
        </tr>
    </table>
    <asp:DataGrid runat="server" Width="100%" ID="dgDocumentList" DataSourceID="dsDocumentList"
        AutoGenerateColumns="false" EnableViewState="false" ShowHeader="True" GridLines="None"
        CssClass="table-th">
        <HeaderStyle CssClass="th" />
        <Columns>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" CssClass="left-th" />
                <HeaderTemplate>
                     <div>
                         <fbs:sortref_prefix prefix="../Common/Images/" runat="server" sortexpr="Date"
                            sortexprtext="Дата" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# DateTime.Parse(Eval("ActivateDate").ToString()).ToShortDateString() %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="100%" CssClass="right-th" />
                <HeaderTemplate>
                     <div>
                         <fbs:sortref_prefix prefix="../Common/Images/" runat="server" sortexpr="Name"
                            sortexprtext="Название" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <a title='<%# Eval("Description") %>' href='<%# (!string.IsNullOrEmpty(Convert.ToString(Eval("RelativeUrl")))) ?
                        Eval("RelativeUrl") : string.Format("/Document.aspx?id={0}", Eval("Id")) %>'>
                        <%# Eval("Name")%>
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
    <table class="pager">
        <tr>
            <td>
                <web:DataSourcePager runat="server" DataSourceId="DataSourcePagerHead" StartRowIndexParamName="start"
                    MaxRowCountParamName="count" HideDefaultTemplates="true" AlwaysShow="true" DefaultMaxRowCount="20"
                    DefaultMaxRowCountSource="Cookies">
                    <Header>
                        Документы #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                        <web:DataSourceMaxRowCount runat="server" Variants="20,50,100" DataSourcePagerId="DataSourcePagerHead">
                            <Header>
                                Записей на странице:
                            </Header>
                            <Footer>
                                .</Footer>
                            <Separator>
                                ,
                            </Separator>
                            <ActiveTemplate>
                                <span>#count#</span></ActiveTemplate>
                            <InactiveTemplate>
                                <a href="/SetMaxRowCount.aspx?count=#count#" title="Отображать #count# записей на странице">
                                    #count#</a></InactiveTemplate>
                        </web:DataSourceMaxRowCount>
                    </Header>
                </web:DataSourcePager>
            </td>
            <td align="right">
                <web:DataSourcePager runat="server" DataSourceId="DataSourcePagerHead" StartRowIndexParamName="start"
                    MaxRowCountParamName="count">
			        <PrevGroupTemplate></PrevGroupTemplate>
                    <NextGroupTemplate></NextGroupTemplate>
                    <FirstPageTemplate><a href="#PageUrl#"><<</a>&nbsp;</FirstPageTemplate>
                    <LastPageTemplate>&nbsp;<a href="#PageUrl#">>></a></LastPageTemplate>
                    <ActivePrevPageTemplate><a href="#PageUrl#"><</a>&nbsp;</ActivePrevPageTemplate>
                    <ActivePageTemplate><span>#PageNo#</span> </ActivePageTemplate>
                    <ActiveNextPageTemplate>&nbsp;<a href="#PageUrl#">></a></ActiveNextPageTemplate>
                </web:DataSourcePager>
            </td>
        </tr>
    </table>
    <asp:SqlDataSource runat="server" ID="dsDocumentList" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="SearchDocument" CancelSelectOnNullParameter="False" SelectCommandType="StoredProcedure"
        ProviderName="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString.ProviderName %>">
        <SelectParameters>
            <asp:QueryStringParameter Name="Name" Type="String" QueryStringField="Name" />
            <asp:Parameter DefaultValue="true" Name="IsActive" Type="Boolean" />
            <asp:QueryStringParameter  DefaultValue="other" Name="contextCodes" Type="String" QueryStringField="DocType"/>
            <asp:QueryStringParameter DefaultValue="Date" Name="sortColumn" Type="String" QueryStringField="sort" />
            <asp:QueryStringParameter DefaultValue="0" Name="sortAsc" Type="Int16" QueryStringField="sortorder" />
            <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
            <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count"
                CheckParamName="count" CheckParamSource="Cookies" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource runat="server" ID="dsDocumentListCount" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="SearchDocument" CancelSelectOnNullParameter="false" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="name" Type="String" QueryStringField="name" />
            <asp:Parameter DefaultValue="true" Name="IsActive" Type="Boolean" />
            <asp:QueryStringParameter  DefaultValue="other" Name="contextCodes" Type="String" QueryStringField="DocType"/>
            <asp:Parameter DefaultValue="true" Name="showCount" Type="Boolean" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

<script runat="server" type="text/C#">

    // Формирование строки статуса фильтра
    string FilterStatusString()
    {
        ArrayList filter = new ArrayList();

        if (!string.IsNullOrEmpty(Request.QueryString["name"]))
            filter.Add(string.Format("Название “{0}”", Request.QueryString["name"]));

        if (filter.Count == 0)
            filter.Add("Не задан");

        return string.Join("; ", ((string[])filter.ToArray(typeof(string))));
    }     
    
</script>

