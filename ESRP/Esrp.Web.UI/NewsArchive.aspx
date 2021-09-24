<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewsArchive.aspx.cs" Inherits="Esrp.Web.NewsArchive" MasterPageFile="~/Common/Templates/Main.Master" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Controls" Assembly="Esrp.Web.UI" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/CalendarPopup.js"> </script>
    <script type="text/javascript" src="/Common/Scripts/Utils.js"> </script>
    <script type="text/javascript" src="/Common/Scripts/Filter.js"> </script>
    <script type="text/javascript" src="/Common/Scripts/FixPng.js"> </script>

    <script type="text/javascript">
        jQuery(document).ready(function() {
            var params = {
                changedEl: 'select',
                visRows: 7,
                scrollArrows: true
            };
            cuSel(params);
        });
    </script>

</asp:Content>

<asp:Content ID="Content1" runat="server"  ContentPlaceHolderID="cphContentModalDialog">
    <div id="filter1" class="pop" style="display: none;">
        <div class="pop_bg"></div>
        <div class="pop_rel">
            <div class="block">
                <form action="" method="get">
                    <table width="100%">
                        <tr>
                            <td>Название</td>
                            <td width="1"><input name="Name" id="Name" type="text" value="<%= this.Request.QueryString["Name"] %>"/></td>
                        </tr>
                        <tr>
                            <td>Дата с</td>
                            <td nowrap>
                            <input name="from" style="width: 280px!important;" class="date"  id="from" type="text" value="<%= this.Request.QueryString["from"] %>"/>
                                
                        </tr>
                        <tr>
                            <td>Дата по</td>
                            <td nowrap><input name="to" style="width: 280px!important;" id="to" class="date" type="text" value="<%= this.Request.QueryString["to"] %>"/>
                                
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <input type="button" value="Отмена" class="closed un_dott" onclick=" showFilterDlg(false); "/> 
                                <input type="reset" value="Очистить" class="un_dott" onclick=" return resetButtonClick(); "/> 
                                <input type="submit" value="Установить" onclick=" showFilterDlg(false); "/>
                            </td>
                        </tr>
                    </table>
                </form>
            </div><!--block-->
        </div>
    </div>
    <script type="text/javascript">
        function resetButtonClick() {
            document.getElementById("Name").value = "";
            document.getElementById("from").value = "";
            document.getElementById("to").value = "";
            return false;
        }

    // Отображение диалога фильтрации

        function showFilterDlg(canShow) {
            var filter = document.getElementById('filter1');

            if (canShow)
                filter.style.display = '';
            else
                filter.style.display = 'none';
        }
    $(function () {
        $(".date").datepicker({showOn:'both',
            buttonImage: "/Common/Images/ico-datepicker-Esrp.gif",
            buttonImageOnly: true,
            changeMonth: true,
            changeYear: true,
            yearRange:'c-30:c+30'
        });
    });
</script>
</asp:Content>


<asp:Content ContentPlaceHolderID="cphContent" runat="server">

    <div class="col_in">
        <form runat="server">

            <div class="main_table">
                <div class="sort table_header">
                    <div class="f_left">
                    </div>
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
                                                 DataSourceId="dsNewsListCount"
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
                    <div class="sorted">
                        <span class="f_left">
                            <a href="javascript:void(0);" class="un_dott" onclick="showFilterDlg(true);">Фильтр</a>  
                            <%= this.FilterStatusString()%>
                        </span>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>

                <asp:DataGrid runat="server" 
                              ID="dgNewsList" 
                              DataSourceID="dsNewsList" 
                              AutoGenerateColumns="false"
                              EnableViewState="False" 
                              ShowHeader="True" 
                              GridLines="None" 
                              UseAccessibleHeader="true"
                              Width="100%"
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
                                <%#((DateTime)this.Eval("Date")).ToShortDateString()%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle Width="90%"/>
                            <HeaderTemplate>
                                <div>
                                    <esrp:sortref_prefix prefix="../Common/Images/" runat="server" sortexpr="Name"
                                                        sortexprtext="Название" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div title='<%#this.Eval("Description")%>'>
                                    <a href='News.aspx?id=<%#this.Eval("Id")%>'>
                                        <%#this.Eval("Name")%></a>
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

    <asp:SqlDataSource runat="server" ID="dsNewsList" ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
                       SelectCommand="SearchNews" CancelSelectOnNullParameter="false" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="Name" Type="String" QueryStringField="Name" />
            <asp:QueryStringParameter Name="dateFrom" Type="DateTime" QueryStringField="from" />
            <asp:QueryStringParameter Name="dateTo" Type="DateTime" QueryStringField="to" />
            <asp:QueryStringParameter DefaultValue="0" Name="sortColumn" Type="String" QueryStringField="sort" />
            <asp:QueryStringParameter DefaultValue="0" Name="sortAsc" Type="Int16" QueryStringField="sortorder" />
            <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
            <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count" CheckParamName="count" CheckParamSource="Cookies" />
            <asp:Parameter DefaultValue="1" Name="IsActive" Type="Byte" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource runat="server" ID="dsNewsListCount" ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
                       SelectCommand="SearchNews" CancelSelectOnNullParameter="false" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="Name" Type="String" QueryStringField="Name" />
            <asp:QueryStringParameter Name="dateFrom" Type="DateTime" QueryStringField="from" />
            <asp:QueryStringParameter Name="dateTo" Type="DateTime" QueryStringField="to" />
            <asp:Parameter DefaultValue="1" Name="IsActive" Type="Byte" />
            <asp:Parameter DefaultValue="true" Name="showCount" Type="Boolean" />
        </SelectParameters>
    </asp:SqlDataSource>
    <div id="CalendarContainer" style="position: absolute; visibility: hidden; background-color: white; z-index: 999;">
    </div>
</asp:Content>

<script runat="server" type="text/C#">
    // Формирование строки статуса фильтра
    private string FilterStatusString()
    {
        var filter = new ArrayList();

        if (!string.IsNullOrEmpty(this.Request.QueryString["name"]))
        {
            filter.Add(string.Format("Название “{0};”", this.Request.QueryString["name"]));
        }

        string from = this.Request.QueryString["from"];
        string to = this.Request.QueryString["to"];
        if (!string.IsNullOrEmpty(from) || !string.IsNullOrEmpty(this.Request.QueryString["to"]))
        {
            filter.Add(
                String.Format(
                    "Период {0};",
                    (string.IsNullOrEmpty(from) ? "" : "c " + from + " ") + (string.IsNullOrEmpty(to) ? "" : "до " + to)));
        }

        if (filter.Count == 0)
        {
            filter.Add("Не задан");
        }

        return string.Join(" ", ((string[])filter.ToArray(typeof(string))));
    }

    </script>