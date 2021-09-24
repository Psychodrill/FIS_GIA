<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" 
    Inherits="Esrp.Web.Administration.Documents.List" 
    MasterPageFile="~/Common/Templates/Administration.master" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Controls" Assembly="Esrp.Web.UI" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>

<asp:Content runat="server" ContentPlaceHolderID="cphHead" > 
    <script src="/Common/Scripts/Filter.js" type="text/javascript"></script>
    <script src="/Common/Scripts/Utils.js" type="text/javascript"></script>

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

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphThirdLevelMenu">
    <div class="third_line">
		<div class="max_width">
			<esrp:TopMenu ID="SecondLevelMenu1" runat="server" RootResourceKey="press-center" HeaderTemplate="<ul>" 
                    FooterTemplate="</ul>"/>
			<div class="clear"></div>
		</div>
	</div><!--bottom_line-->
</asp:Content>

<asp:Content ID="Content1" runat="server"  ContentPlaceHolderID="cphContentModalDialog">
		<div id="filter1" class="pop" style="display: none;">
			<div class="pop_bg"></div>
			<div class="pop_rel">
				<div class="block">
                <form action="" method="get">
					<table width="100%">
                        <tr>
                            <td class="left">Название</td>
                            <td width="1"><input type="text" name="name" id="fname" value="<%= Request.QueryString["name"] %>" /></td>
                        </tr>
                        <tr>
                            <td class="left2">Состояние&nbsp;&nbsp;</td>
                            <td><select name="isActive" id="fisActive" >
                                <option id="sel_1" value="">&lt;Все&gt;</option>  
                                <option id="sel_2" value="1" <%= SelectStatus("1") %>>Опубликованные</option> 
                                <option id="sel_3" value="0" <%= SelectStatus("0") %>>Неопубликованные</option> 
                            </select></td>
                        </tr>
						<tr>
							<td></td>
							<td>
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
    function resetButtonClick() {
        document.getElementById("fname").value = "";
        $('#fisActive').val('');
        $('#cuselFrame-fisActive .cuselText').html('&lt;Все&gt;');
        document.getElementById("sel_1").className = "cuselActive";
        document.getElementById("sel_2").className = "";
        document.getElementById("sel_3").className = "";
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
</script>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">


<form runat="server">

    			<div class="main_table">
					<div class="sort table_header">
                        <div class="f_left">
                            <a href="Create.aspx" title="Cоздать документ" class="create_user">Cоздать документ</a>
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
                                <%= FilterStatusString()%>
                            </span>
                            <div class="clear"></div>
                        </div>
					<div class="sorted">
                        <asp:LinkButton runat="server" ID="btnDelete" onclick="btnDelete_Click" Text="Удалить выбранные" 
                            OnClientClick="return confirm('Вы действительно хотите удалить выбранные записи?')" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton runat="server" ID="btnAcivate" onclick="btnAcivate_Click"
                            Text="Опубликовать выбранные" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton runat="server" ID="btnDeactivate" onclick="btnDeactivate_Click"
                            Text="Снять с публикации выбранные" />                      
                    </div>
                    </div>


                    <div class="clear"></div>

    <asp:DataGrid runat="server" id="dgDocumentList"
        DataSourceID="dsDocumentList"
        AutoGenerateColumns="false" 
        EnableViewState="false"
        ShowHeader="True" 
        GridLines="None"
        UseAccessibleHeader="true"
        Width="100%"
        CssClass="table-th">
        <HeaderStyle CssClass="actions" />
        <Columns>
            <asp:TemplateColumn>
            <HeaderStyle Width="1%" CssClass="left-th" />
            <HeaderTemplate>
                <div><input type="checkbox" onclick="sel();" /></div>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox  runat="server" ID="cbDocument" />
                <asp:HiddenField runat="server" ID="hfDocumentId" Value='<%#Eval("Id")%>' />
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderStyle Width="75%" />
            <HeaderTemplate>
                <div><esrp:SortRef_prefix runat="server" SortExpr="Name" SortExprText="Название" Prefix="../../../Common/Images/" /></div>
            </HeaderTemplate>
            <ItemTemplate>
                <div title='<%#Eval("Description") %>'>
                    <a href='<%#(User.IsInRole("ViewDocument") ? "Edit" : "View")%>.aspx?id=<%# Eval("Id") %>'>
                        <%# Eval("Name")%>
                    </a>
                </div>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderStyle Width="20%" CssClass="right-th" />
            <HeaderTemplate>
                <div><esrp:SortRef_prefix ID="SortRef_prefix1" runat="server" SortExpr="IsActive" SortExprText="Опубликован" Prefix="../../../Common/Images/" /></div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# ((bool)Eval("IsActive")) ? "Да" : "Нет"%>
            </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    
    <web:NoRecordsText runat="server" ControlId="dgDocumentList">
        <Message><p class="notfound">Не найдено ни одного документа</p></Message>
    </web:NoRecordsText>   
  
        <div class="sort table_footer">
            <% if (dgDocumentList.Items.Count > 0)
               { %>
               <div class="f_left">
                   <span class="line">
                        <img src="/Common/Images/bg/del.png" alt="" width="12" height="12" />
                        <span class="un_dott">
                            <asp:LinkButton runat="server" ID="LinkButton1" Text="Удалить выбранных" 
                                OnClientClick="if (confirm('Вы действительно хотите удалить выбранные записи?')) {$('#<%= btnDelete.ClientID %>').click();} else return false;"
                                OnClick="btnDelete_Click" />
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
    <asp:SqlDataSource runat="server" ID="dsDocumentList" 
        ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="SearchDocument" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <asp:QueryStringParameter Name="Name" Type="String" QueryStringField="Name" />
            <asp:QueryStringParameter Name="IsActive" Type="Byte" QueryStringField="IsActive" />
            <asp:QueryStringParameter DefaultValue="0" Name="sortColumn" Type="String" QueryStringField="sort" />
            <asp:QueryStringParameter DefaultValue="1" Name="sortAsc" Type="Int16" QueryStringField="sortorder" />
            <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
            <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count"
                CheckParamName="count" CheckParamSource="Cookies" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="dsDocumentListCount" 
        ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="SearchDocument" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <asp:QueryStringParameter Name="name" Type="String" QueryStringField="name" />
            <asp:QueryStringParameter Name="IsActive" Type="Byte" QueryStringField="IsActive" />
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
            filter.Add(string.Format("Название “{0}”; ", Request.QueryString["name"]));

        if (!string.IsNullOrEmpty(Request.QueryString["isActive"]))            
            filter.Add(string.Format("Состояние “{0}”; ", 
                Request.QueryString["isActive"] == "1" ? "опубликованные" : "неопубликованные"));

        if (filter.Count == 0)
            filter.Add("Не задан");

        return string.Join("; ", ((string[])filter.ToArray(typeof(string))));
    } 
    
    string SelectStatus(string status)
    {
        if (Request.QueryString["IsActive"] == status)
            return "selected=\"selected\"";
        
        return string.Empty; 
    }    
    
</script>  
