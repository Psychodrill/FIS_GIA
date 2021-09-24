<%@ Page Language="C#" MasterPageFile="~/Common/Templates/Administration.Master"
    AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Fbs.Web.Administration.Deliveries.List" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Controls" Assembly="FbsWebUI" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Fbs.Core" %>
<%@ Import Namespace="Fbs.Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">

    <script type="text/javascript" src="/Common/Scripts/CalendarPopup.js"></script>

    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>

    <script type="text/javascript" src="/Common/Scripts/Filter.js"></script>

</asp:Content>
<asp:Content ContentPlaceHolderID="cphActions" runat="server">
    <div class="h10">
    </div>
    <div class="border-block">
        <div class="tr">
            <div class="tt">
                <div>
                </div>
            </div>
        </div>
        <div class="content" id="JSPlaceHolder">
            <ul>
                <li><a href="Create.aspx?Type=Users" title="Cоздать рассылку">Cоздать рассылку пользователям</a></li>
                <li><a href="Create.aspx?Type=Organizations" title="Cоздать рассылку">Cоздать рассылку организациям</a></li>
            </ul>
            <div class="split">
            </div>
        </div>
        <div class="br">
            <div class="tt">
                <div>
                </div>
            </div>
        </div>
    </div>
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
            <div>
                <div class="h-line">
                    <div class="tl">
                        <div>
                        </div>
                    </div>
                    <div class="tr">
                        <div>
                            <ul>
                                <li><a href="Create.aspx" title="Cоздать документ">Cоздать рассылку</a></li>
                            </ul>
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
                            <td class="left2">
                                Заголовок
                            </td>
                            <td>
                                <input type="text" name="Title" id="TBTitle" value="<%= Request.QueryString["Title"] %>"
                                    class="txt txt2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="left">
                                Создана&nbsp;c
                            </td>
                            <td>
                                <input type="text" name="createDateFrom" id="TBCreateDateFrom" value="<%= Request.QueryString["createDateFrom"] %>"
                                    class="txt date" />
                                <img src="/Common/Images/ico-datepicker-fbs.gif" id="BtCreateDateFrom" alt="выбрать дату"
                                    onclick='return PickDate("TBCreateDateFrom", "BtCreateDateFrom", "CalendarContainer");' />
                            </td>
                        </tr>
                        <tr>
                            <td class="left">
                                Создана&nbsp;по
                            </td>
                            <td>
                                <input type="text" name="createDateTo" id="TBCreateDateTo" value="<%= Request.QueryString["createDateTo"] %>"
                                    class="txt date" />
                                <img src="/Common/Images/ico-datepicker-fbs.gif" id="BtCreateDateTo" alt="выбрать дату"
                                    onclick='return PickDate("TBCreateDateTo", "BtCreateDateTo", "CalendarContainer");' />
                            </td>
                        </tr>
                        <tr>
                            <td class="left">
                                Рассылка&nbsp;c
                            </td>
                            <td>
                                <input type="text" name="deliveryDateFrom" id="TBDeliveryDateFrom" value="<%= Request.QueryString["deliveryDateFrom"] %>"
                                    class="txt date" />
                                <img src="/Common/Images/ico-datepicker-fbs.gif" id="BtDeliveryDateFrom" alt="выбрать дату"
                                    onclick='return PickDate("TBDeliveryDateFrom", "BtDeliveryDateFrom", "CalendarContainer");' />
                            </td>
                        </tr>
                        <tr>
                            <td class="left">
                                Рассылка&nbsp;по&nbsp;&nbsp;
                            </td>
                            <td>
                                <input type="text" name="deliveryDateTo" id="TBDeliveryDateTo" value="<%= Request.QueryString["deliveryDateTo"] %>"
                                    class="txt date" />
                                <img src="/Common/Images/ico-datepicker-fbs.gif" id="BtDeliveryDateTo" alt="выбрать дату"
                                    onclick='return PickDate("TBDeliveryDateTo", "BtDeliveryDateTo", "CalendarContainer");' />
                            </td>
                        </tr>
                        <tr>
                            <td class="left2">
                                Статус
                            </td>
                            <td>
                                <select name="Status" id="CBStatus" style="width: auto;">
                                    <option value="" >&lt;Все&gt;</option>
                                    <option value="1" <%= IsSelected("1") %> >Ожидает отправки</option>
                                    <option value="2" <%= IsSelected("2") %> >В процессе отправки</option>
                                    <option value="3" <%= IsSelected("3") %> >Отправлена</option>
                                    <option value="4" <%= IsSelected("4") %> >Отправлена с ошибками</option>
                                </select>
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
    </div>
    <!-- /filtr2 -->

    <script type="text/javascript">
    <!--

    initFilter();

    function resetButtonClick()
    {
        // очищу значения контролов
        document.getElementById("TBTitle").value = "";
        document.getElementById("TBCreateDateTo").value = "";
        document.getElementById("TBCreateDateFrom").value = "";
        document.getElementById("TBDeliveryDateTo").value = "";
        document.getElementById("TBDeliveryDateFrom").value = "";
        document.getElementById("CBStatus").value = "";
        // заблокирую вызов стандартного reset
        return false;
    }

    -->
    </script>

    <form id="Form1" runat="server">
    <table class="pager">
        <tr>
            <td>
                <web:DataSourcePager runat="server" ID="DataSourcePagerHead" DataSourceId="DSDeliveriesListCount"
                    StartRowIndexParamName="start" MaxRowCountParamName="count" HideDefaultTemplates="true"
                    AlwaysShow="true" DefaultMaxRowCount="20" DefaultMaxRowCountSource="Cookies">
                    <Header>
                        Рассылки #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                        <web:DataSourceMaxRowCount ID="DataSourceMaxRowCount1" runat="server" Variants="20,50,100"
                            DataSourcePagerId="DataSourcePagerHead">
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
                <web:DataSourcePager ID="DataSourcePager1" runat="server" DataSourceId="DataSourcePagerHead"
                    StartRowIndexParamName="start" MaxRowCountParamName="count">
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
    <asp:DataGrid runat="server" ID="DGDeliveriesList" DataSourceID="DSDeliveriesList"
        AutoGenerateColumns="false" EnableViewState="False" ShowHeader="True" GridLines="None"
        CssClass="table-th">
        <HeaderStyle CssClass="th" />
        <Columns>
            <asp:TemplateColumn>
                <HeaderStyle Width="1%" CssClass="left-th" />
                <HeaderTemplate>
                    <div>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox runat="server" ID="CBDeliveries" />
                    <asp:HiddenField runat="server" ID="HFDeliveriesId" Value='<%#Eval("Id")%>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="40%" />
                <HeaderTemplate>
                    <div>
                        <fbs:SortRef_Prefix ID="SortRef_prefix1" runat="server" SortExpr="Title" SortExprText="Заголовок"
                            Prefix="../../../Common/Images/" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div title='<%#Eval("Title") %>'>
                        <a href='<%#(User.IsInRole("ViewDeliveries") ? "Edit" : "View")%>.aspx?id=<%# Eval("Id") %>'>
                            <%# Eval("Title")%>
                        </a>
                    </div>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" />
                <HeaderTemplate>
                    <div>
                        <fbs:SortRef_Prefix ID="SortRef_prefix1" runat="server" SortExpr="CreateDate" SortExprText="Создана"
                            Prefix="../../../Common/Images/" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# ((DateTime)Eval("CreateDate")).ToShortDateString()%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" />
                <HeaderTemplate>
                    <div>
                        <fbs:SortRef_Prefix ID="SortRef_prefix1" runat="server" SortExpr="DeliveryDate" SortExprText="Дата&nbsp;рассылки"
                            Prefix="../../../Common/Images/" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# ((DateTime)Eval("DeliveryDate")).ToShortDateString()%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="30%" CssClass="right-th" />
                <HeaderTemplate>
                    <div>
                        <fbs:SortRef_Prefix ID="SortRef_prefix1" runat="server" SortExpr="Status" SortExprText="Статус"
                            Prefix="../../../Common/Images/" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("StatusName") %>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    <web:NoRecordsText ID="NoRecordsText1" runat="server" ControlId="DGDeliveriesList">
        <Message>
            <p class="notfound">
                Не найдено ни одной рассылки</p>
        </Message>
    </web:NoRecordsText>
    <span id="LinkContainer">
        <ul>
            <li>
                <asp:LinkButton runat="server" ID="BtDelete" Text="Удалить" OnClientClick="return confirm('Вы действительно хотите удалить выбранные записи?')"
                    OnClick="BtDelete_Click" /></li>
        </ul>
    </span>

    <script language="javascript" type="text/javascript">
        MoveControls("LinkContainer", "JSPlaceHolder");
    </script>

    <table class="pager">
        <tr>
            <td>
                <web:DataSourcePager ID="DataSourcePager2" runat="server" DataSourceId="DataSourcePagerHead"
                    StartRowIndexParamName="start" MaxRowCountParamName="count" HideDefaultTemplates="true"
                    AlwaysShow="true" DefaultMaxRowCount="20" DefaultMaxRowCountSource="Cookies">
                    <Header>
                        Рассылки #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                        <web:DataSourceMaxRowCount ID="DataSourceMaxRowCount2" runat="server" Variants="20,50,100"
                            DataSourcePagerId="DataSourcePagerHead">
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
                <web:DataSourcePager ID="DataSourcePager3" runat="server" DataSourceId="DataSourcePagerHead"
                    StartRowIndexParamName="start" MaxRowCountParamName="count">
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
    </form>
    <asp:SqlDataSource runat="server" ID="DSDeliveriesList" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="SearchDeliveries" CancelSelectOnNullParameter="false" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="Title" Type="String" QueryStringField="Title" />
            <asp:QueryStringParameter Name="Status" Type="Int16" QueryStringField="Status" />
            <asp:QueryStringParameter Name="createDateFrom" Type="DateTime" QueryStringField="createDateFrom" />
            <asp:QueryStringParameter Name="createDateTo" Type="DateTime" QueryStringField="createDateTo" />
            <asp:QueryStringParameter Name="deliveryDateFrom" Type="DateTime" QueryStringField="deliveryDateFrom" />
            <asp:QueryStringParameter Name="deliveryDateTo" Type="DateTime" QueryStringField="deliveryDateTo" />
            <asp:QueryStringParameter DefaultValue="0" Name="sortColumn" Type="String" QueryStringField="sort" />
            <asp:QueryStringParameter DefaultValue="0" Name="sortAsc" Type="Int16" QueryStringField="sortorder" />
            <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
            <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count"
                CheckParamName="count" CheckParamSource="Cookies" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource runat="server" ID="DSDeliveriesListCount" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="SearchDeliveries" CancelSelectOnNullParameter="false" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="Title" Type="String" QueryStringField="Title" />
            <asp:QueryStringParameter Name="Status" Type="Int16" QueryStringField="Status" />
            <asp:QueryStringParameter Name="createDateFrom" Type="DateTime" QueryStringField="createDateFrom" />
            <asp:QueryStringParameter Name="createDateTo" Type="DateTime" QueryStringField="createDateTo" />
            <asp:QueryStringParameter Name="deliveryDateFrom" Type="DateTime" QueryStringField="deliveryDateFrom" />
            <asp:QueryStringParameter Name="deliveryDateTo" Type="DateTime" QueryStringField="deliveryDateTo" />
            <asp:Parameter DefaultValue="true" Name="showCount" Type="Boolean" />
        </SelectParameters>
    </asp:SqlDataSource>
    <div id="CalendarContainer" style="position: absolute; visibility: hidden; background-color: white;
        z-index: 99;">
    </div>
</asp:Content>

<script runat="server" type="text/C#">
    
    string IsSelected(string id)
    {
        if (Request.QueryString["Status"] == id)
            return "selected=\"selected\"";
      
        return String.Empty;
    }

    // Формирование строки статуса фильтра
    string FilterStatusString()
    {
        ArrayList filter = new ArrayList();

        if (!string.IsNullOrEmpty(Request.QueryString["Title"]))
        {
            filter.Add(string.Format("Заголовок “{0}”;", Request.QueryString["Title"]));
        }

        string CreateDateFrom = Request.QueryString["createDateFrom"];
        string CreateDateTo = Request.QueryString["createDateTo"];
        if (!string.IsNullOrEmpty(CreateDateFrom) || !string.IsNullOrEmpty(CreateDateTo))
        {
            filter.Add(String.Format("Создана {0};",
                (string.IsNullOrEmpty(CreateDateFrom) ? "" : "c " + CreateDateFrom + " ") +
                (string.IsNullOrEmpty(CreateDateTo) ? "" : "до " + CreateDateTo)));
        }

        string DeliveryDateFrom = Request.QueryString["deliveryDateFrom"];
        string DeliveryDateTo = Request.QueryString["deliveryDateTo"];
        if (!string.IsNullOrEmpty(DeliveryDateFrom) || !string.IsNullOrEmpty(DeliveryDateTo))
        {
            filter.Add(String.Format("Дата отправки {0};",
                (string.IsNullOrEmpty(DeliveryDateFrom) ? "" : "c " + DeliveryDateFrom + " ") +
                (string.IsNullOrEmpty(DeliveryDateTo) ? "" : "до " + DeliveryDateTo)));
        }


         string StatusId = Request.QueryString["Status"];
        if (!string.IsNullOrEmpty(StatusId))
        {
            string StatusName;
            switch (StatusId)
            {
                case "1":
                    StatusName = "Ожидает отправки";
                    break;
                case "2":
                    StatusName = "В процессе отправки";
                    break;
                case "3":
                    StatusName = "Отправлена";
                    break;
                case "4":
                    StatusName = "Отправлена с ошибками";
                    break;
                default :
                    throw new Exception("Неизвестный статус");
                    
            }
            filter.Add(string.Format("Статус “{0};”", StatusName));
        }
        
        
        if (filter.Count == 0)
            filter.Add("Не задан");

        return string.Join(" ", ((string[])filter.ToArray(typeof(string))));
        return "";
    } 
</script>

