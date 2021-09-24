<%@ Page Language="C#" MasterPageFile="~/Common/Templates/Administration.Master"
    AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Esrp.Web.Administration.Deliveries.List" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Controls" Assembly="Esrp.Web.UI" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>
<asp:Content ContentPlaceHolderID="cphHead" runat="server">

    <script type="text/javascript" src="/Common/Scripts/CalendarPopup.js"></script>

    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>

    <script type="text/javascript" src="/Common/Scripts/Filter.js"></script>

    <script type="text/javascript">
    jQuery(document).ready(function(){
	    var params = {
	        changedEl: 'select',
	        visRows: 7,
	        scrollArrows: true
	    }
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
                            <td>Заголовок</td>
                            <td width="1" ><input type="text" name="Title" id="TBTitle" value="<%= Request.QueryString["Title"] %>"/></td>
                        </tr>
                        <tr>
                            <td>Создана&nbsp;c</td>
                            <td nowrap>
                            <input type="text" style="width:280px!important;" class="date" name="createDateFrom" id="TBCreateDateFrom" value="<%= Request.QueryString["createDateFrom"] %>"/>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>Создана&nbsp;по</td>
                            <td nowrap>
                                <input type="text" style="width:280px!important;"  class="date" name="createDateTo" id="TBCreateDateTo" value="<%= Request.QueryString["createDateTo"] %>"/>
                               
                            </td>
                        </tr>
                        <tr>
                            <td>Рассылка&nbsp;c</td>
                            <td nowrap>
                                <input type="text"  class="date" style="width:280px!important;" name="deliveryDateFrom" id="TBDeliveryDateFrom" value="<%= Request.QueryString["deliveryDateFrom"] %>"/>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>Рассылка&nbsp;по&nbsp;&nbsp;</td>
                            <td nowrap>
                                <input type="text"  class="date" style="width:280px!important;" name="deliveryDateTo" id="TBDeliveryDateTo" value="<%= Request.QueryString["deliveryDateTo"] %>"/>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>Статус</td>
                            <td>
                                <select name="Status" id="CBStatus" style="width: auto;">
                                    <option id="sel_0" value="" >&lt;Все&gt;</option>
                                    <option id="sel_1" value="1" <%= IsSelected("1") %> >Ожидает отправки</option>
                                    <option id="sel_2" value="2" <%= IsSelected("2") %> >В процессе отправки</option>
                                    <option id="sel_3" value="3" <%= IsSelected("3") %> >Отправлена</option>
                                    <option id="sel_4" value="4" <%= IsSelected("4") %> >Отправлена с ошибками</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td class="cell-bt">
                                <input type="button" value="Отмена" class="closed un_dott" onclick="showFilterDlg(false);"/> 
                                <input type="reset" value="Очистить" class="un_dott" onclick="resetButtonClick(); return false;"/> 
                                <input type="submit" value="Установить" onclick="showFilterDlg(false);"/>
                            </td>
                        </tr>
                    </table>
                    </form>
				</div><!--block-->
			</div>
		</div>
<script type="text/javascript">
    $(function () {
        $(".date").datepicker({ showOn: 'both',
            buttonImage: "/Common/Images/ico-datepicker-Esrp.gif",
            buttonImageOnly: true,
            changeMonth: true,
            changeYear: true,
            yearRange: 'c-30:c+30'
        });
    });
function resetButtonClick() {
    // очищу значения контролов
    document.getElementById("TBTitle").value = "";
    document.getElementById("TBCreateDateTo").value = "";
    document.getElementById("TBCreateDateFrom").value = "";
    document.getElementById("TBDeliveryDateTo").value = "";
    document.getElementById("TBDeliveryDateFrom").value = "";
    $('#CBStatus').val('');
    $('#cuselFrame-CBStatus .cuselText').html('&lt;Все&gt;');
    document.getElementById("sel_0").className = "cuselActive";
    document.getElementById("sel_1").className = "";
    document.getElementById("sel_2").className = "";
    document.getElementById("sel_3").className = "";
    document.getElementById("sel_4").className = "";
    return false;
}

// Отображение диалога фильтрации
function showFilterDlg(canShow)
{
    var filter = document.getElementById('filter1');

    if (canShow)
        filter.style.display = '';
    else
        filter.style.display = 'none';
}
</script>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContent" runat="server">

    <form id="Form1" runat="server">

    <div class="main_table">
		<div class="sort table_header">
            <div class="f_left">
                <a href="Create.aspx?Type=Users" title="Cоздать рассылку" class="create_user">Cоздать рассылку пользователям</a>&nbsp;
                <a href="Create.aspx?Type=Organizations" title="Cоздать рассылку" class="create_user">Cоздать рассылку организациям</a>
			</div>
			<div class="sorted">
                <web:DataSourcePager ID="DataSourcePager1" runat="server"
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
                        DataSourceId="DSDeliveriesListCount"
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
                    <web:DataSourcePager ID="DataSourcePager4" runat="server"
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
                <span>
                    &nbsp;<a href="javascript:void(0);" class="un_dott" onclick="showFilterDlg(true);">Фильтр</a>  
                    <%= FilterStatusString()%>
                </span>
                <div class="clear"></div>
            </div>
		</div>
		<div class="clear"></div>
        <asp:DataGrid runat="server" 
            ID="DGDeliveriesList" 
            DataSourceID="DSDeliveriesList"
            AutoGenerateColumns="false" 
            EnableViewState="False" 
            ShowHeader="True" 
            GridLines="None"
            ShowFooter="false" 
            UseAccessibleHeader="true"
            CssClass="table-th">
            <HeaderStyle CssClass="actions" />
            <Columns>
                <asp:TemplateColumn>
                    <HeaderStyle Width="1%"/>
                    <HeaderTemplate>
                        <div><input type="checkbox" onclick="sel();" /></div>
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
                            <esrp:SortRef_Prefix ID="SortRef_prefix1" runat="server" SortExpr="Title" SortExprText="Заголовок"
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
                            <esrp:SortRef_Prefix ID="SortRef_prefix1" runat="server" SortExpr="CreateDate" SortExprText="Создана"
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
                            <esrp:SortRef_Prefix ID="SortRef_prefix1" runat="server" SortExpr="DeliveryDate" SortExprText="Дата&nbsp;рассылки"
                                Prefix="../../../Common/Images/" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# ((DateTime)Eval("DeliveryDate")).ToShortDateString()%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="30%"/>
                    <HeaderTemplate>
                        <div>
                            <esrp:SortRef_Prefix ID="SortRef_prefix1" runat="server" SortExpr="Status" SortExprText="Статус"
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
        
        <div class="sort table_footer">
            <% if (DGDeliveriesList.Items.Count > 0)
               { %>
            <div class="f_left">
                <span class="line">
                    <img src="/Common/Images/bg/del.png" alt="" width="12" height="12" />
                    <span class="un_dott">
                        <asp:LinkButton runat="server" ID="LinkButton1" Text="Удалить выбранных" 
                            OnClientClick="return confirm('Вы действительно хотите удалить выбранные записи?')"
                            OnClick="BtDelete_Click" />
                    </span>
                </span>
            </div>
            <% }
               else
               { %>
            <div class="f_left">
            </div>
            <% } %>
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
    <asp:SqlDataSource runat="server" ID="DSDeliveriesList" ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
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
    <asp:SqlDataSource runat="server" ID="DSDeliveriesListCount" ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
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

