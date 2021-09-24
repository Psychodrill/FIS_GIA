<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" 
    Inherits="Esrp.Web.Administration.FAQ.List" 
    MasterPageFile="~/Common/Templates/Administration.master" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>

<asp:Content runat="server" ContentPlaceHolderID="cphHead" > 
    <script src="/Common/Scripts/Filter.js" type="text/javascript"></script>
    <script src="/Common/Scripts/Utils.js" type="text/javascript"></script>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphActions">
    <div class="h10"></div>
    <div class="border-block">
        <div class="tr"><div class="tt"><div></div></div></div>
        <div class="content" id="JSPlaceHolder">
            <ul>
            <li><a href="Create.aspx" title="Cоздать вопрос">Cоздать вопрос</a></li>
            </ul>
            <div class="split"></div>
        </div>
        <div class="br"><div class="tt"><div></div></div></div>
    </div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">

    <% // Контролы фильтрации списка %>
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
        <tr><td class="left">Вопрос</td>
            <td><input name="Name" id="Name" class="txt txt2" 
                value="<%= Request.QueryString["name"] %>" /></td></tr>
        <tr><td class="left2">Состояние&nbsp;&nbsp;</td>
            <td><select name="isActive" id="isActive" >
                <option value="">&lt;Все&gt;</option>  
                <option value="1" <%= SelectStatus("1") %>>Опубликованные</option> 
                <option value="0" <%= SelectStatus("0") %>>Неопубликованные</option> 
            </select></td>
        </tr><tr>
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
        AskedQuestion.getElementById("Name").value = "";
        AskedQuestion.getElementById("isActive").value = "";
        return false;
    }

    -->
    </script>

<form id="Form1" runat="server">

    <table class="pager">
    <tr><td>
    
        <web:DataSourcePager runat="server"
            id="DataSourcePagerHead" 
            DataSourceId="dsAskedQuestionListCount"
	        StartRowIndexParamName="start" 
	        MaxRowCountParamName="count"
	        HideDefaultTemplates="true"
	        AlwaysShow="true"
	        DefaultMaxRowCount="20"
	        DefaultMaxRowCountSource="Cookies">
            <Header>
                Вопросы #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
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

    <asp:DataGrid runat="server" id="dgAskedQuestionList"
        DataSourceID="dsAskedQuestionList"
        AutoGenerateColumns="false" 
        EnableViewState="false"
        ShowHeader="True" 
        GridLines="None"
        CssClass="table-th">
        <HeaderStyle CssClass="th" />
        <Columns>
            <asp:TemplateColumn>
            <HeaderStyle Width="1%" CssClass="left-th" />
            <HeaderTemplate>
                <div></div>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox  runat="server" ID="cbAskedQuestion" />
                <asp:HiddenField runat="server" ID="hfAskedQuestionId" Value='<%#Eval("Id")%>' />
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderStyle Width="75%" />
            <HeaderTemplate>
                <web:SortRef runat="server" SortExpr="Name" SortExprText="Вопрос" />
            </HeaderTemplate>
            <ItemTemplate>
                <div title='<%#Eval("Question") %>'>
                    <a href='<%#(User.IsInRole("ViewNews") ? "Edit" : "View")%>.aspx?id=<%# Eval("Id") %>'>
                        <%# Eval("Name")%>
                    </a>
                </div>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderStyle Width="20%" CssClass="right-th" />
            <HeaderTemplate>
                <div>
                    <web:SortRef runat="server" SortExpr="IsActive" SortExprText="Опубликован" />
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# ((bool)Eval("IsActive")) ? "Да" : "Нет"%>
            </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    
    <web:NoRecordsText  runat="server" ControlId="dgAskedQuestionList">
        <Message><p class="notfound">Не найдено ни одного вопроса</p></Message>
    </web:NoRecordsText>   
     
     <span id="LinkContainer">
        <ul>
        <li><asp:LinkButton runat="server" ID="btnDelete" onclick="btnDelete_Click" Text="Удалить" 
            OnClientClick="return confirm('Вы действительно хотите удалить выбранные записи?')" /></li>
        <li><asp:LinkButton runat="server" ID="btnAcivate" onclick="btnAcivate_Click"
            Text="Опубликовать" /></li>
        <li></li><asp:LinkButton runat="server" ID="btnDeactivate" onclick="btnDeactivate_Click"
            Text="Снять с публикации" /></li>
        </ul>
    </span>
    
    <script language="javascript">
        MoveControls("LinkContainer", "JSPlaceHolder");
    </script>
    
</form>
    
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
                Вопросы #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
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
    
    <asp:SqlDataSource runat="server" ID="dsAskedQuestionList" 
        ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="SearchAskedQuestion" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <asp:QueryStringParameter Name="name" Type="String" QueryStringField="Name" />
            <asp:QueryStringParameter Name="IsActive" Type="Byte" QueryStringField="IsActive" />
            <asp:QueryStringParameter DefaultValue="0" Name="sortColumn" Type="String" QueryStringField="sort" />
            <asp:QueryStringParameter DefaultValue="1" Name="sortAsc" Type="Int16" QueryStringField="sortorder" />
            <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
            <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count"
                CheckParamName="count" CheckParamSource="Cookies" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="dsAskedQuestionListCount" 
        ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="SearchAskedQuestion" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <asp:QueryStringParameter Name="name" Type="String" QueryStringField="name" />
            <asp:QueryStringParameter Name="IsActive" Type="Byte" QueryStringField="IsActive" />
            <asp:Parameter DefaultValue="true" Name="showCount" Type="Boolean" />
            <asp:Parameter DefaultValue="" Name="sortColumn" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

<script runat="server" type="text/C#">

    // Формирование строки статуса фильтра
    string FilterStatusString()
    {
        ArrayList filter = new ArrayList();

        if (!string.IsNullOrEmpty(Request.QueryString["Name"]))
            filter.Add(string.Format("Вопрос “{0};”", Request.QueryString["Name"]));

        if (!string.IsNullOrEmpty(Request.QueryString["isActive"]))
            filter.Add(string.Format("Состояние “{0};”",
                Request.QueryString["isActive"] == "1" ? "опубликованные" : "неопубликованные"));

        if (filter.Count == 0)
            filter.Add("Не задан");

        return string.Join(" ", ((string[])filter.ToArray(typeof(string))));
    } 
    
    string SelectStatus(string status)
    {
        if (Request.QueryString["IsActive"] == status)
            return "selected=\"selected\"";
        
        return string.Empty; 
    }

</script>

