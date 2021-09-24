<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AskedQuestions.aspx.cs" Inherits="Fbs.Web.AskedQuestions"
    MasterPageFile="~/Common/Templates/Regular.master" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Fbs.Core" %>
<%@ Import Namespace="Fbs.Web" %>
    
<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/CalendarPopup.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Prototype.js"></script>
    <script type="text/javascript" src="/Common/Scripts/FAQ.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Filter.js" ></script>
    <script type="text/javascript" src="/Common/Scripts/FixPng.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphContent" runat="server">

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
        <tr><td class="left">Вопрос&nbsp;</td>
            <td><input name="Name" id="Name" class="txt txt2" 
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
    
    
    <!-- FAQ  -->
    <div class="to-be-defined" id="faq-layer"  style="display:none">
        <div class="filtr2" id="faq" style="position:absolute">
            <div class="h-line"><div class="tl"><div></div></div><div class="tr"><div></div></div><div class="t"><div></div></div></div>
            <div class="content-f2">
                <table class="filtr2-table" style="width: 100%;">
                    <tr><td colspan="2">
                        <div id="FaqContainer">
                            <p>
                                <b>Вопрос:</b><br />
                                <span id="QuestionContainer"></span>
                            </p><p>
                                <b>Ответ:</b><br />
                                <span id="AnswerContainer"></span>
                            </p>
                        </div>
                        <div id="FaqError" style="display:none; color:red;">Вопрос не найден</div>
                    </td></tr>
                    <tr><td colspan="2" class="cell-bt">
                        <input type="button" class="bt" value="Закрыть" onclick="showFAQ(false);" />
                    </td></tr>
                </table>
            </div>
            <div class="h-line"><div class="bl"><div></div></div><div class="br"><div></div></div><div class="b"><div></div></div></div>
        </div>
    </div>   
    <!-- /FAQ -->

    <script type="text/javascript">
    <!--

    initFilter();

    function resetButtonClick()
    {
        AskedQuestion.getElementById("Name").value = "";
        return false;
    }

    -->
    </script>

<form runat="server">
    
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
           <HeaderStyle Width="100%" CssClass="left-th" />
            <HeaderTemplate>
                <div>
                    <web:SortRef runat="server" SortExpr="Name" SortExprText="Вопрос" />
                </div> 
            </HeaderTemplate>
            <ItemTemplate>
                <a href='/AskedQuestion.aspx?id=<%# Eval("Id") %>' title='<%#Eval("Question") %>' 
                    onclick='return getFAQ(this)'>
                    <%# Eval("Name")%>
                </a>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderStyle Width="0" CssClass="right-th" />
            <HeaderTemplate><div></div></HeaderTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    
    <web:NoRecordsText runat="server" ControlId="dgAskedQuestionList">
        <Message><p class="notfound">Не найдено ни одного вопроса</p></Message>
    </web:NoRecordsText>   
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
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="SearchAskedQuestion" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <asp:QueryStringParameter Name="name" Type="String" QueryStringField="Name" />
            <asp:Parameter DefaultValue="1" Name="IsActive" Type="Byte" />
            <asp:QueryStringParameter DefaultValue="0" Name="sortColumn" Type="String" QueryStringField="sort" />
            <asp:QueryStringParameter DefaultValue="1" Name="sortAsc" Type="Int16" QueryStringField="sortorder" />
            <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
            <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count"
                CheckParamName="count" CheckParamSource="Cookies" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="dsAskedQuestionListCount" 
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="SearchAskedQuestion" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <asp:QueryStringParameter Name="name" Type="String" QueryStringField="name" />
            <asp:Parameter DefaultValue="1" Name="IsActive" Type="Byte" />
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

        if (filter.Count == 0)
            filter.Add("Не задан");

        return string.Join(" ", ((string[])filter.ToArray(typeof(string))));
    } 
</script>
