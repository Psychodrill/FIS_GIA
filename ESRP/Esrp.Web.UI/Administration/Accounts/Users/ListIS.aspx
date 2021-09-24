<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListIS.aspx.cs" Inherits="Esrp.Web.Administration.Accounts.Users.ListIS"
    MasterPageFile="~/Common/Templates/Administration.master" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Controls" Assembly="Esrp.Web.UI" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>
<asp:Content runat="server" ContentPlaceHolderID="cphHead">
    <script src="/Common/Scripts/Filter.js" type="text/javascript"></script>
    
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
<asp:Content runat="server" ContentPlaceHolderID="cphContentModalDialog">
    <div id="filterDiv" class="pop" style="display: none;">
        <div class="pop_bg" onclick="return false;">
        </div>
        <div class="pop_rel">
            <div class="block">
                <form action="" method="get">
                <table width="100%">
                    <tr>
                        <td>
                            Логин или e-mail
                        </td>
                        <td width="1">
                            <input name="login" id="flogin" type="text" value="<%= Request.QueryString["login"] %>" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Фамилия
                        </td>
                        <td>
                            <input name="lastName" id="flastName" type="text" value="<%= Request.QueryString["lastName"] %>" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Состояние
                        </td>
                        <td>
                            <select name="isActive" id="fisActive">
                                <option id="sel_1" value="">&lt;Все&gt;</option>
                                <option id="sel_2" value="1" <%= SelectIsActive("1") %>>
                                    <%= IntrantAccountExtentions.GetStateName(true)%></option>
                                <option id="sel_3" value="0" <%= SelectIsActive("0") %>>
                                    <%= IntrantAccountExtentions.GetStateName(false)%></option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <input type="button" value="Отмена" class="closed un_dott" onclick="showFilterDlg(false);" />
                            <input type="reset" value="Очистить" class="un_dott" onclick="resetButtonClick(); return false;" />
                            <input type="submit" value="Установить" onclick="showFilterDlg(false); return true;" />
                        </td>
                    </tr>
                </table>
                </form>
            </div>
            <!--block-->
        </div>
    </div>
    <script type="text/javascript">
        function resetButtonClick() {
            document.getElementById("flogin").value = "";
            document.getElementById("flastName").value = "";
            $('#fisActive').val('');
            $('#cuselFrame-fisActive .cuselText').html('&lt;Все&gt;');
            document.getElementById("sel_1").className = "cuselActive";
            document.getElementById("sel_2").className = "";
            document.getElementById("sel_3").className = "";
            return false;
        }

        function closeDlg() {
            var dlgDiv = document.getElementById('filterDiv');
            dlgDiv.style.display = 'none';
        }

        function showDlg() {
            document.getElementById('filterDiv').style.display = '';
            document.getElementById('filterDiv').focus();
        }

        // Отображение диалога фильтрации
        function showFilterDlg(canShow) {
            var filter = document.getElementById('filterDiv');

            if (canShow)
                filter.style.display = '';
            else
                filter.style.display = 'none';
        }
    </script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <div class="main_table">
        <div class="sort table_header">
            <div class="f_left">
                <% if (User.IsInRole("EditUserISAccount"))
                   { %>
                <a href="/Administration/Accounts/Users/CreateIS.aspx" class="create_user">Создать пользователя</a>
                <% } %>
            </div>
            <div class="sorted">
                <web:DataSourcePager ID="DataSourcePager2" runat="server" DataSourceId="DataSourcePagerHead"
                    StartRowIndexParamName="start" MaxRowCountParamName="count" HideDefaultTemplates="true"
                    AlwaysShow="true">
                    <Header>
                        <div class="sort_page">
                            <select id="selectPageCount" onchange="changePageCount()">
                                <web:DataSourceMaxRowCount ID="DataSourceMaxRowCount1" runat="server" Variants="20,50,100"
                                    DataSourcePagerId="DataSourcePagerHead">
                                    <Footer>
                                    </Footer>
                                    <Separator>
                                    </Separator>
                                    <ActiveTemplate>
                                        <option selected="selected" value="#count#"><span>#count#</span></option>
                                    </ActiveTemplate>
                                    <InactiveTemplate>
                                        <option value="#count#"><span>#count#</span></option>
                                    </InactiveTemplate>
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
                    <web:DataSourcePager runat="server" ID="DataSourcePagerHead" DataSourceId="dsAdministratorListCount"
                        StartRowIndexParamName="start" MaxRowCountParamName="count" HideDefaultTemplates="true"
                        AlwaysShow="true" DefaultMaxRowCount="20" DefaultMaxRowCountSource="Cookies">
                        <Header>
                            Показано <span>#StartRowIndex#-#LastRowIndex#</span> из #TotalCount#.
                        </Header>
                    </web:DataSourcePager>
                </p>
                <p class="page_nav">
                    <web:DataSourcePager ID="DataSourcePager1" runat="server" DataSourceId="DataSourcePagerHead"
                        StartRowIndexParamName="start" AlwaysShow="false" MaxRowCountParamName="count">
                        <Header>
                            Страницы&nbsp;</Header>
                        <PrevGroupTemplate>
                        </PrevGroupTemplate>
                        <NextGroupTemplate>
                        </NextGroupTemplate>
                        <FirstPageTemplate>
                            <a href="#PageUrl#"><<</a>&nbsp;</FirstPageTemplate>
                        <LastPageTemplate>
                            &nbsp;<a href="#PageUrl#">>></a></LastPageTemplate>
                        <ActivePrevPageTemplate>
                            <a href="#PageUrl#"><</a>&nbsp;</ActivePrevPageTemplate>
                        <ActivePageTemplate>
                            <span>#PageNo#</span>
                        </ActivePageTemplate>
                        <ActiveNextPageTemplate>
                            &nbsp;<a href="#PageUrl#">></a></ActiveNextPageTemplate>
                    </web:DataSourcePager>
                </p>
                <div class="clear">
                </div>
            </div>
            <div class="sorted">
                <span class="f_left"><a href="javascript:void(0);" class="un_dott" onclick="showFilterDlg(true);">
                    Фильтр</a>
                    <%= FilterStatusString()%>
                </span>
                <div class="clear">
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
        <asp:DataGrid runat="server" ID="dgAdministratorList" DataSourceID="dsAdministratorList"
            AutoGenerateColumns="false" EnableViewState="false" ShowHeader="True" ShowFooter="false"
            GridLines="None" UseAccessibleHeader="true" Width="100%" CssClass="table-th">
            <HeaderStyle CssClass="actions" />
            <Columns>
                <asp:TemplateColumn>
                    <HeaderStyle Width="20%" />
                    <HeaderTemplate>
                        <div>
                            <esrp:SortRef_Prefix ID="SortRef_prefix2" Prefix="../../../Common/Images/" runat="server"
                                SortExpr="Login" SortExprText="Логин" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <a href="/Administration/Accounts/Users/<%# 
                        (User.IsInRole("EditUserISAccount") ? "EditIS.aspx" : "ViewIS.aspx") %>?login=<%# 
                        HttpUtility.UrlEncode(Convert.ToString(Eval("Login"))) %>">
                            <%# Convert.ToString(Eval("Login"))%></a>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="35%" />
                    <HeaderTemplate>
                        <esrp:SortRef_Prefix ID="SortRef_prefix3" Prefix="../../../Common/Images/" runat="server"
                            SortExpr="Name" SortExprText="Ф. И. О. пользователя" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("LastName")%>
                        <%# Eval("FirstName")%>
                        <%# Eval("PatronymicName")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="35%" />
                    <HeaderTemplate>
                        <esrp:SortRef_Prefix ID="SortRef_prefix1" Prefix="../../../Common/Images/" runat="server"
                            SortExpr="GroupName" SortExprText="Группа" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("GroupName")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn ItemStyle-Wrap="false">
                    <HeaderStyle Width="10%" />
                    <HeaderTemplate>
                        <div>
                            <esrp:SortRef_Prefix Prefix="../../../Common/Images/" ID="SortRef2" runat="server"
                                SortExpr="IsActive" SortExprText="Состояние" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# IntrantAccountExtentions.GetStateName(Convert.ToBoolean(Eval("IsActive"))) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <web:NoRecordsText ID="NoRecordsText1" runat="server" ControlId="dgAdministratorList">
            <Message>
                <p class="notfound">
                    Не найдено ни одного пользователя</p>
            </Message>
        </web:NoRecordsText>
        <div class="sort table_footer">
            <div class="f_left">
            </div>
            <div class="sorted">
                    <web:DataSourcePager ID="DataSourcePager5" runat="server" DataSourceId="DataSourcePagerHead"
                        StartRowIndexParamName="start" MaxRowCountParamName="count" HideDefaultTemplates="true"
                        AlwaysShow="true">
                        <Header>
                            <div class="sort_page">
                                <select id="selectPageCountBottom" onchange="changePageCount(this)">
                                    <web:DataSourceMaxRowCount ID="DataSourceMaxRowCount1" runat="server" Variants="20,50,100"
                                        DataSourcePagerId="DataSourcePagerHead">
                                        <Footer>
                                        </Footer>
                                        <Separator>
                                        </Separator>
                                        <ActiveTemplate>
                                            <option selected="selected" value="#count#"><span>#count#</span></option>
                                        </ActiveTemplate>
                                        <InactiveTemplate>
                                            <option value="#count#"><span>#count#</span></option>
                                        </InactiveTemplate>
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
                        <web:DataSourcePager runat="server" ID="DataSourcePager6" DataSourceId="DataSourcePagerHead"
                            StartRowIndexParamName="start" MaxRowCountParamName="count" HideDefaultTemplates="true"
                            AlwaysShow="true" DefaultMaxRowCount="20" DefaultMaxRowCountSource="Cookies">
                            <Header>
                                Показано <span>#StartRowIndex#-#LastRowIndex#</span> из #TotalCount#.
                            </Header>
                        </web:DataSourcePager>
                    </p>
                    <p class="page_nav">
                        <web:DataSourcePager ID="DataSourcePager7" runat="server" DataSourceId="DataSourcePagerHead"
                            StartRowIndexParamName="start" AlwaysShow="false" MaxRowCountParamName="count">
                            <Header>
                                Страницы&nbsp;</Header>
                            <PrevGroupTemplate>
                            </PrevGroupTemplate>
                            <NextGroupTemplate>
                            </NextGroupTemplate>
                            <FirstPageTemplate>
                                <a href="#PageUrl#"><<</a>&nbsp;</FirstPageTemplate>
                            <LastPageTemplate>
                                &nbsp;<a href="#PageUrl#">>></a></LastPageTemplate>
                            <ActivePrevPageTemplate>
                                <a href="#PageUrl#"><</a>&nbsp;</ActivePrevPageTemplate>
                            <ActivePageTemplate>
                                <span>#PageNo#</span>
                            </ActivePageTemplate>
                            <ActiveNextPageTemplate>
                                &nbsp;<a href="#PageUrl#">></a></ActiveNextPageTemplate>
                        </web:DataSourcePager>
                    </p>
                    <div class="clear">
                </div>
            </div>
        </div>
    </div>
    <asp:SqlDataSource runat="server" ID="dsAdministratorList" ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="SearchAccountIS" CancelSelectOnNullParameter="false" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter DefaultValue="false" Name="isAdmin" Type="Boolean" />
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
    <asp:SqlDataSource runat="server" ID="dsAdministratorListCount" ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="SearchAccountIS" CancelSelectOnNullParameter="false" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter DefaultValue="false" Name="isAdmin" Type="Boolean" />
            <asp:QueryStringParameter Name="login" Type="String" QueryStringField="login" />
            <asp:QueryStringParameter Name="lastName" Type="String" QueryStringField="lastName" />
            <asp:QueryStringParameter Name="isActive" Type="String" QueryStringField="isActive" />
            <asp:QueryStringParameter Name="email" Type="String" QueryStringField="Email" />
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
            filter.Add(string.Format("Фамилия “{0}”", Request.QueryString["lastName"]));

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
