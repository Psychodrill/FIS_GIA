<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" 
    Inherits="Esrp.Web.Administration.Accounts.Auditors.List"
    MasterPageFile="~/Common/Templates/Administration.master" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Controls" Assembly="Esrp.Web.UI" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>

<asp:Content runat="server" ContentPlaceHolderID="cphHead">
    <script src="/Common/Scripts/Filter.js" type="text/javascript"></script>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphActions">
    <div class="h10"></div>
    <div class="border-block">
        <div class="tr"><div class="tt"><div></div></div></div>
        <div class="content">
            <% if (User.IsInRole("EditAuditorAccount"))
               { %>
                <ul>
                <li><a href="/Administration/Accounts/Auditors/Create.aspx"
                    title="Создать пользователя">Создать пользователя</a></li>
                </ul>
            <% } %>        
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
        onclick="showFilter(true);">Фильтр:</a> <%= FilterStatusString()%></p>    
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
        <tr><td class="left">Логин</td>
            <td><input name="login" class="txt" value="<%= Request.QueryString["login"] %>" /></td></tr>
        <tr><td class="left">Фамилия</td>
            <td><input name="lastName" class="txt txt2" 
                value="<%= Request.QueryString["lastName"] %>" /></td></tr>
        <tr><td class="left">E-mail</td>
            <td><input name="email" class="txt" 
                value="<%= Request.QueryString["email"] %>" /></td></tr>
        <tr><td class="left2">Состояние&nbsp;&nbsp;</td>
            <td><select name="isActive">
                <option value="">&lt;Все&gt;</option>  
                <option value="1" <%= SelectIsActive("1") %>><%= IntrantAccountExtentions.GetStateName(true)%></option> 
                <option value="0" <%= SelectIsActive("0") %>><%= IntrantAccountExtentions.GetStateName(false)%></option> 
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
        document.getElementById("login").value = "";
        document.getElementById("lastName").value = "";
        document.getElementById("isActive").value = "";
        document.getElementById("email").value = "";
        return false;
    }

    -->
    </script>
    
    <table class="pager">
    <tr><td>
    
        <web:DataSourcePager runat="server"
            id="DataSourcePagerHead" 
            DataSourceId="dsAuditorListCount"
	        StartRowIndexParamName="start" 
	        MaxRowCountParamName="count"
	        HideDefaultTemplates="true"
	        AlwaysShow="true"
	        DefaultMaxRowCount="20"
	        DefaultMaxRowCountSource="Cookies">
            <Header>
                Пользователи #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
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


    <asp:DataGrid runat="server" id="dgAuditorList"
        DataSourceID="dsAuditorList"
        AutoGenerateColumns="false" 
        EnableViewState="false"
        ShowHeader="True" 
        GridLines="None"
        CssClass="table-th">
        <HeaderStyle CssClass="th" />
        <Columns>

            <asp:TemplateColumn>
            <HeaderStyle Width="15%" CssClass="left-th" />
            <HeaderTemplate>
                <div><esrp:SortRef_prefix Prefix="../../../Common/Images/" runat="server" SortExpr="Login" SortExprText="Логин" /></div>
            </HeaderTemplate>
            <ItemTemplate>
                <a href="/Administration/Accounts/Auditors/<%# 
                    (User.IsInRole("EditAuditorAccount") ? "Edit.aspx" : "View.aspx") %>?login=<%# 
                    HttpUtility.UrlEncode(Convert.ToString(Eval("Login"))) %>">
                    <%# Convert.ToString(Eval("Login"))%></a>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderStyle Width="35%" />
            <HeaderTemplate>
                <esrp:SortRef_prefix Prefix="../../../Common/Images/" runat="server" SortExpr="Name" SortExprText="Ф. И. О. пользователя" />
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("LastName")%> <%# Eval("FirstName")%> <%# Eval("PatronymicName")%>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderStyle Width="15%" />
            <HeaderTemplate>
                <div><esrp:SortRef_prefix Prefix="../../../Common/Images/" ID="SortRef1" runat="server" SortExpr="email" SortExprText="E-mail" /></div>
            </HeaderTemplate>
            <ItemTemplate>
                 <nobr><a href="mailto:<%# Convert.ToString(Eval("email")) %>"><%# Convert.ToString(Eval("email")) %></a></nobr>
            </ItemTemplate>
            </asp:TemplateColumn>
            
             <asp:TemplateColumn ItemStyle-Wrap="false">
            <HeaderStyle Width="10%" CssClass="right-th" />
            <HeaderTemplate>
                <div><esrp:SortRef_prefix Prefix="../../../Common/Images/" ID="SortRef2" runat="server" SortExpr="IsActive" SortExprText="Состояние" /></div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# IntrantAccountExtentions.GetStateName(Convert.ToBoolean(Eval("IsActive")))%>
            </ItemTemplate>
            </asp:TemplateColumn>

        </Columns>
    </asp:DataGrid>

    <web:NoRecordsText runat="server" ControlId="dgAuditorList">
        <Message><p class="notfound">Не найдено ни одного пользователя</p></Message>
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
                Пользователи #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
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


    <asp:SqlDataSource runat="server" ID="dsAuditorList" 
        ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="SearchAccount" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <asp:Parameter DefaultValue="Auditor" Name="groupCode" Type="String" />
            <asp:QueryStringParameter Name="login" Type="String" QueryStringField="login" />
            <asp:QueryStringParameter Name="lastName" Type="String" QueryStringField="lastName" />
            <asp:QueryStringParameter Name="isActive" Type="String" QueryStringField="isActive" />
            <asp:QueryStringParameter Name="email" Type="String" QueryStringField="email" />
            <asp:QueryStringParameter DefaultValue="0" Name="sortColumn" Type="String" QueryStringField="sort" />
            <asp:QueryStringParameter DefaultValue="1" Name="sortAsc" Type="Int16" QueryStringField="sortOrder" />
            <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
            <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count"
                CheckParamName="count" CheckParamSource="Cookies" />
            <asp:Parameter DefaultValue="false" Name="showCount" Type="Boolean" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="dsAuditorListCount" 
        ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="SearchAccount" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <asp:Parameter DefaultValue="Auditor" Name="groupCode" Type="String" />
            <asp:QueryStringParameter Name="login" Type="String" QueryStringField="login" />
            <asp:QueryStringParameter Name="lastName" Type="String" QueryStringField="lastName" />
            <asp:QueryStringParameter Name="isActive" Type="String" QueryStringField="isActive" />
            <asp:QueryStringParameter Name="email" Type="String" QueryStringField="email" />
            <asp:Parameter DefaultValue="true" Name="showCount" Type="Boolean" />
        </SelectParameters>
    </asp:SqlDataSource>    
</asp:Content>

<script runat="server" type="text/C#">

// Формирование строки статуса фильтра
string FilterStatusString()
{
    ArrayList filter = new ArrayList();

    if (!string.IsNullOrEmpty(Request.QueryString["login"]))
        filter.Add(string.Format("Логин “{0}”", Request.QueryString["login"]));

    if (!string.IsNullOrEmpty(Request.QueryString["lastName"]))
        filter.Add(string.Format("Фамилия “{0}”", Request.QueryString["lastNAme"]));

    if (!string.IsNullOrEmpty(Request.QueryString["isActive"]))
        filter.Add(string.Format("Состояние “{0}”",
            IntrantAccountExtentions.GetStateName(GetIsActive())));

    if (!string.IsNullOrEmpty(Request.QueryString["email"]))
        filter.Add(string.Format("Email “{0}”", Request.QueryString["email"]));

    if (filter.Count == 0)
        filter.Add("Не задан");

    return string.Join("; ", ((string[])filter.ToArray(typeof(string))));
}

string SelectIsActive(string isActive)
{
    if (Request.QueryString["isActive"] == isActive)
        return "selected=\"selected\"";

    return string.Empty;
}

bool? GetIsActive()
{
    switch (Request.QueryString["isActive"])
    {
        case "1":
            return true;
        case "0":
            return false;
        default:
            return null;
    }
}
    
</script>
