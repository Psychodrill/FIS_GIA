<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" 
    Inherits="Fbs.Web.Administration.Accounts.Departments.List" 
    MasterPageFile="~/Common/Templates/Administration.master" %>
<%@ OutputCache VaryByParam="none" Duration="1" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Controls" Assembly="FbsWebUI" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Fbs.Core" %>
<%@ Import Namespace="Fbs.Web" %>
<%@ Import Namespace="System.Data" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHead">

    <script src="/Common/Scripts/Filter.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphContent">

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
        <tr><td class="left">Логин</td>
            <td><input name="login" class="txt" value="<%= Request.QueryString["login"] %>" /></td></tr>
        <tr><td class="left">Организация&nbsp;&nbsp;</td>
            <td><input name="organizationName" class="txt txt2" 
                value="<%= Request.QueryString["organizationName"] %>" /></td></tr>
        <tr><td class="left">E-mail</td>
            <td><input name="email" class="txt" value="<%= Request.QueryString["email"] %>" /></td></tr>
        <tr><td class="left2">Статус</td>
            <td><select name="status">
                <option value="">&lt;Все&gt;</option> 
                <option value="registration" <%= SelectValue("status", "registration") %>>На регистрации</option> 
                <option value="revision" <%= SelectValue("value", "revision") %>>На доработке</option> 
                <option value="consideration" <%= SelectValue("status", "consideration") %>>На согласовании</option> 
                <option value="activated" <%= SelectValue("status", "activated") %>>Действующий</option> 
                <option value="deactivated" <%= SelectValue("status", "deactivated") %>>Отключенный</option> 
            </select></td>
        </tr>
       <tr><td class="left2">Тип</td>
            <td>
            <select name="eitype" class="sel small">
                <option value="">&lt;Все&gt;</option>
                <option value="0" <%= SelectValue("eitype", "0")%>>&lt;Пустое&gt;</option>
                <option value="6" <%= SelectValue("eitype", "6")%>>Учредитель</option>
            </select>
            </td>
        </tr>
        <tr><td class="left">Оператор</td>
            <td>
                <input name="operatorName" class="txt" value="<%= Request.QueryString["operatorName"] %>" />
            </td>
        </tr>
        <tr><td class="left2">Комментарии оператора</td>
            <td>
            <select name="hasComments" class="sel small">
                <option value="0">&lt;Не важно&gt;</option>
                <option value="1" <%= SelectValue("hasComments", "1")%>>Нет</option>
                <option value="2" <%= SelectValue("hasComments", "2")%>>Есть</option>
            </select>
            </td>
        </tr>
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

        function resetButtonClick() {
            document.getElementById("login").value = "";
            document.getElementById("organizationName").value = "";
            document.getElementById("email").value = "";
            document.getElementById("status").value = "";
            document.getElementById("eitype").value = "";
            document.getElementById("hasEtalonOrg").value = "";
            document.getElementById("operatorName").value = "";
            document.getElementById("hasComments").value = "0";
            return false;
        }

    -->
    </script>

    <fbs:Pager_Simple runat="server" id="tablePager_top"></fbs:Pager_Simple>
    <asp:DataGrid runat="server" id="dgUserList"
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
                <div><fbs:SortRef_prefix ID="SortRef_prefix1" Prefix="../../../Common/Images/" runat="server" SortExpr="login" SortExprText="Логин" /></div>
            </HeaderTemplate>
            <ItemTemplate>
                <a href="/Administration/Accounts/Users/View.aspx?login=<%# 
                    HttpUtility.UrlEncode(Convert.ToString(Eval("Login"))) %>">
                    <%# Convert.ToString(Eval("Login")) %></a>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderStyle Width="30%" />
            <HeaderTemplate>
                <fbs:SortRef_prefix ID="SortRef_prefix2" Prefix="../../../Common/Images/" runat="server" SortExpr="organizationName" SortExprText="Организация" />
            </HeaderTemplate>
            <ItemTemplate>
                <%# (Eval("OrgId") != DBNull.Value)
                                        ? string.Format("<a href='../../Organizations/Administrators/OrgCard_Edit.aspx?OrgID={0}'>{1}</a>", Eval("OrgId"), Eval("OrganizationName"))
                    : Convert.ToString(Eval("OrganizationName"))%> 
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderStyle Width="15%" />
            <HeaderTemplate>
                <fbs:SortRef_prefix ID="SortRef_prefix3" Prefix="../../../Common/Images/" runat="server" SortExpr="lastName" SortExprText="Ответственное&nbsp;лицо" />
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("LastName") %> <%# Eval("FirstName") %> <%# Eval("PatronymicName") %><br/>
                <nobr><a href="mailto:<%# Eval("Email") %>"><%# Eval("Email") %></a></nobr>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn ItemStyle-Wrap="true">
            <HeaderStyle Width="15%"/>
            <HeaderTemplate>
                <div><fbs:SortRef_prefix ID="SortRef_prefix4" Prefix="../../../Common/Images/" runat="server" SortExpr="status" SortExprText="Статус" /></div>
            </HeaderTemplate>
            <ItemTemplate>
                <div style="white-space:normal;" title="<%# UserAccountExtentions.GetUserAccountStatusName(Eval("Status")) %>">
                <%# GetCustomStatus((Eval("Status").ToString()))%>
                </div>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn ItemStyle-Wrap="false">
            <HeaderStyle Width="5%"/>
            <HeaderTemplate>
                <div><fbs:SortRef_prefix ID="SortRef_prefix5" Prefix="../../../Common/Images/" runat="server" SortExpr="operatorName" SortExprText="Оператор" /></div>
            </HeaderTemplate>
            <ItemTemplate>
                <%#Eval("OperatorName")%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn ItemStyle-Wrap="false">
            <HeaderStyle Width="5%" CssClass="right-th"/>
            <HeaderTemplate>
                <div><fbs:SortRef_prefix ID="SortRef_prefix1" Prefix="../../../Common/Images/" runat="server" SortExpr="hasComments" SortExprText="Комментарии" /></div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("HasComments").ToString()=="2"?"Есть":"Нет" %>
            </ItemTemplate>
            </asp:TemplateColumn>

        </Columns>
    </asp:DataGrid>

    <web:NoRecordsText ID="NoRecordsText1" runat="server" ControlId="dgUserList">
        <Message><p class="notfound">Не найдено ни одного пользователя</p></Message>
    </web:NoRecordsText>

    <fbs:Pager_Simple runat="server" id="tablePager_bottom"></fbs:Pager_Simple>
</asp:Content>

<script runat="server" type="text/C#">
    
    string GetShortStatus(string sysStatus)
    {
        switch(sysStatus)
        {
            case "registration": return "На регистрации";
            case "consideration": return "На согласовании";
            case "revision": return "На доработке";
            case "activated": return "Действующий";
            case "deactivated": return "Отключенный";
            default: return "[Не определён]";
        }
    }

    string GetCustomStatus(string sysStatus)
    {
        switch (sysStatus)
        {
            case "registration": return "На регистрации: Шаг 2 из 3";
            case "consideration": return "На согласовании: Шаг 3 из 3";
            case "revision": return "На доработке: Шаг 3 из 3";
            case "activated": return "Действующий";
            case "deactivated": return "Отключенный";
            default: return "[Не определён]";
        }
    }

    // Формирование строки статуса фильтра
    string FilterStatusString()
    {
        ArrayList filter = new ArrayList();

        if (!string.IsNullOrEmpty(Request.QueryString["login"]))
            filter.Add(string.Format("Логин “{0}”", Request.QueryString["login"]));

        if (!string.IsNullOrEmpty(Request.QueryString["organizationName"]))
            filter.Add(string.Format("Организация “{0}”", Request.QueryString["organizationName"]));

        if (!string.IsNullOrEmpty(Request.QueryString["email"]))
            filter.Add(string.Format("E-mail “{0}”", Request.QueryString["email"]));

        if (!string.IsNullOrEmpty(Request.QueryString["byip"]))
            filter.Add(string.Format("Доступ “{0}”",
                GetAccessName(Request.QueryString["byip"])));

        if (!string.IsNullOrEmpty(Request.QueryString["status"]))
        {
            string status = GetShortStatus(Request.QueryString["status"]);
            
            /*switch(Request.QueryString["status"])
            {
                case "registration": status = "Шаг 2 из 3"; break;
                case "revision": status = "Шаг 3 из 3:Доработка"; break;
                case "consideration": status = "Шаг 3 из 3:Проверка"; break;
                default: status = UserAccountExtentions.GetUserAccountStatusName(Request.QueryString["status"]); break;
            }*/
            //string status=UserAccountExtentions.GetUserAccountStatusName(Request.QueryString["status"])
            filter.Add(string.Format("Статус “{0}”", status));
        }

        int eitype = 0;
        if (!string.IsNullOrEmpty(Request.QueryString["eitype"]) && int.TryParse(Request.QueryString["eitype"], out eitype))
        {
            if (eitype == 0)
                filter.Add(string.Format("Тип ОУ “{0}”", "Пустое"));
            else
                filter.Add(string.Format("Тип ОУ “{0}”",
                    Fbs.Core.CatalogElements.OrgTypeDataAccessor.GetName(eitype)));
        }
        
        if (!string.IsNullOrEmpty(Request.QueryString["operatorName"]))
            filter.Add(string.Format("Оператор “{0}”", Request.QueryString["operatorName"]));

        if (!string.IsNullOrEmpty(Request.QueryString["hasComments"]))
        {
            if (Request.QueryString["hasComments"] == "1")
                filter.Add("Нет комментариев оператора");
            if (Request.QueryString["hasComments"] == "2")
                filter.Add("Есть комментарии оператора");
        }
        
        if (filter.Count == 0)
            filter.Add("Не задан");

        return string.Join("; ", ((string[])filter.ToArray(typeof(string))));
    }
    
    string SelectStatus(string status)
    { 
        if (Request.QueryString["status"] == status)
            return "selected=\"selected\"";
        
        return string.Empty; 
    }

    string SelectValue(string paramName, string value)
    {
        if (string.Compare(Request.QueryString[paramName], value, true) == 0)
            return "selected=\"selected\"";

        return string.Empty;
    }

    string GetAccessName(object isFixedIp)
    {
        if (!Convert.ToBoolean(Convert.ToInt16(isFixedIp)))
            return "VPN";
        else
            return "IP";
    }
</script>
