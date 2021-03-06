<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewsArchive.aspx.cs" Inherits="Fbs.Web.NewsArchive"
    MasterPageFile="~/Common/Templates/Regular.master" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Controls" Assembly="FbsWebUI" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Fbs.Core" %>
<%@ Import Namespace="Fbs.Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">

    <script type="text/javascript" src="/Common/Scripts/CalendarPopup.js"></script>

    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>

    <script type="text/javascript" src="/Common/Scripts/Filter.js"></script>

    <script type="text/javascript" src="/Common/Scripts/FixPng.js"></script>

</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
    <% // Контролы фильтрации списка %>
    <!-- filtr -->
    <div class="filtr" id="filtr-layer" style="display: none;">
        <div class="bg-l">
        </div>
        <div class="bg-r">
        </div>
        <div class="content-f">
            <div class="fleft">
                <p>
                    <a href="javascript:void(0);" title="Установить параметры фильтра" onclick="showFilter(true);">
                        Фильтр:</a>
                    <%= FilterStatusString() %></p>
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
            <div class="h-line">
                <div class="tl">
                    <div>
                    </div>
                </div>
                <div class="tr">
                    <div>
                    </div>
                </div>
                <div class="t">
                    <div>
                    </div>
                </div>
            </div>
            <div class="content-f2">
                <form action="" method="get">
                <table class="filtr2-table">
                    <tr>
                        <td class="left">
                            Название
                        </td>
                        <td>
                            <input type="text" name="Name" id="Name" value="<%= Request.QueryString["name"] %>"
                                class="txt txt2" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Дата с
                        </td>
                        <td>
                            <input type="text" name="from" id="from" value="<%= Request.QueryString["from"] %>"
                                class="txt date" />
                            <img src="/Common/Images/ico-datepicker-fbs.gif" id="btnDateFrom" alt="выбрать дату"
                                onclick='return PickDate("from", "btnDateFrom", "CalendarContainer");' />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Дата по
                        </td>
                        <td>
                            <input type="text" name="to" id="to" value="<%= Request.QueryString["to"] %>" class="txt date" />
                            <img src="/Common/Images/ico-datepicker-fbs.gif" id="btnDateTo" alt="выбрать дату"
                                onclick='return PickDate("to", "btnDateTo", "CalendarContainer");' />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="cell-bt">
                            <input type="reset" class="bt" value="Очистить" onclick="return resetButtonClick();" />&nbsp;
                            <input type="submit" class="bt bt2" onclick="showFilter(false);" value="Установить" />&nbsp;
                            <input type="button" class="bt" id="btCancel" value="Отмена" onclick="showFilter(false);"
                                style="display: none;" />
                        </td>
                    </tr>
                </table>
                </form>
            </div>
            <div class="h-line">
                <div class="bl">
                    <div>
                    </div>
                </div>
                <div class="br">
                    <div>
                    </div>
                </div>
                <div class="b">
                    <div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- /filtr2 -->

    <script type="text/javascript">
    <!--

    initFilter();

    function resetButtonClick()
    {
        // очищу значения контролов
        document.getElementById("Name").value = "";
        document.getElementById("isActive").value = "";
        document.getElementById("from").value = "";
        document.getElementById("to").value = "";
        // заблокирую вызов стандартного reset
        return false;
    }

    -->
    </script>

    <p>
    </p>
    <%--<form runat="server">--%>
    <table class="pager">
        <tr>
            <td>
                <web:DataSourcePager runat="server" ID="DataSourcePagerHead" DataSourceId="dsNewsListCount"
                    StartRowIndexParamName="start" MaxRowCountParamName="count" HideDefaultTemplates="true"
                    AlwaysShow="true" DefaultMaxRowCount="20" DefaultMaxRowCountSource="Cookies">
                    <Header>
                        Новости #StartRowIndex#-#LastRowIndex# из #TotalCount#.
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
    <asp:DataGrid runat="server" ID="dgNewsList" DataSourceID="dsNewsList" AutoGenerateColumns="false"
        EnableViewState="False" ShowHeader="True" GridLines="None" CssClass="table-th">
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
                    <%# ((DateTime)Eval("Date")).ToShortDateString() %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="85%" CssClass="right-th" />
                <HeaderTemplate>
                    <div>
                         <fbs:sortref_prefix prefix="../Common/Images/" runat="server" sortexpr="Name"
                            sortexprtext="Название" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div title='<%#Eval("Description") %>'>
                        <a href='News.aspx?id=<%# Eval("Id") %>'>
                            <%# Eval("Name")%></a>
                    </div>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    <web:NoRecordsText runat="server" ControlId="dgNewsList">
        <Message>
            <p class="notfound">
                Не найдено ни одной новости</p>
        </Message>
    </web:NoRecordsText>
    <%--</form>--%>
    <table class="pager">
        <tr>
            <td>
                <web:DataSourcePager runat="server" DataSourceId="DataSourcePagerHead" StartRowIndexParamName="start"
                    MaxRowCountParamName="count" HideDefaultTemplates="true" AlwaysShow="true" DefaultMaxRowCount="20"
                    DefaultMaxRowCountSource="Cookies">
                    <Header>
                        Новости #StartRowIndex#-#LastRowIndex# из #TotalCount#.
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
    <asp:SqlDataSource runat="server" ID="dsNewsList" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="SearchNews" CancelSelectOnNullParameter="false" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="Name" Type="String" QueryStringField="Name" />
            <asp:QueryStringParameter Name="dateFrom" Type="DateTime" QueryStringField="from" />
            <asp:QueryStringParameter Name="dateTo" Type="DateTime" QueryStringField="to" />
            <asp:QueryStringParameter DefaultValue="0" Name="sortColumn" Type="String" QueryStringField="sort" />
            <asp:QueryStringParameter DefaultValue="0" Name="sortAsc" Type="Int16" QueryStringField="sortorder" />
            <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
            <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count"
                CheckParamName="count" CheckParamSource="Cookies" />
            <asp:Parameter DefaultValue="1" Name="IsActive" Type="Byte" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource runat="server" ID="dsNewsListCount" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="SearchNews" CancelSelectOnNullParameter="false" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="Name" Type="String" QueryStringField="Name" />
            <asp:QueryStringParameter Name="dateFrom" Type="DateTime" QueryStringField="from" />
            <asp:QueryStringParameter Name="dateTo" Type="DateTime" QueryStringField="to" />
            <asp:Parameter DefaultValue="1" Name="IsActive" Type="Byte" />
            <asp:Parameter DefaultValue="true" Name="showCount" Type="Boolean" />
        </SelectParameters>
    </asp:SqlDataSource>
    <div id="CalendarContainer" style="position: absolute; visibility: hidden; background-color: white;
        z-index: 999;">
    </div>
</asp:Content>

<script runat="server" type="text/C#">
    // Формирование строки статуса фильтра
    string FilterStatusString()
    {
        ArrayList filter = new ArrayList();

        if (!string.IsNullOrEmpty(Request.QueryString["name"]))
            filter.Add(string.Format("Название “{0};”", Request.QueryString["name"]));

        string from = Request.QueryString["from"];
        string to = Request.QueryString["to"];
        if (!string.IsNullOrEmpty(from) || !string.IsNullOrEmpty(Request.QueryString["to"]))
            filter.Add(String.Format("Период {0};",
                (string.IsNullOrEmpty(from) ? "" : "c " + from + " ") +
                (string.IsNullOrEmpty(to) ? "" : "до " + to)));

        if (filter.Count == 0)
            filter.Add("Не задан");

        return string.Join(" ", ((string[])filter.ToArray(typeof(string))));
    } 
</script>

