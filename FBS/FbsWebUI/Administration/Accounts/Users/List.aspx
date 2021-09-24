<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" 
    Inherits="Fbs.Web.Administration.Accounts.Users.List" 
    MasterPageFile="~/Common/Templates/Administration.master" %>
<%@ OutputCache VaryByParam="none" Duration="1" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Controls" Assembly="FbsWebUI" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Fbs.Core" %>
<%@ Import Namespace="Fbs.Web" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Linq" %>

<asp:Content runat="server" ContentPlaceHolderID="cphHead">
    <script src="/Common/Scripts/Filter.js" type="text/javascript"></script>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">

    <% // Контролы фильтрации списка %>
    <!-- filtr -->
    <div class="ffilter-block" id="filtr-layer" style="display:none;">
            <div class="tr">
			        <div class="tt">
				        <div>
				        </div>
			        </div>
		    </div>
            <div class="content">
                <a href="javascript:void(0);" title="Установить параметры фильтра" onclick="showFilter(true);">Фильтр:</a> &nbsp;<%=FilterStatusString() %>
                <form action="" method="get">
                <input type="hidden" id="sort" name="sort" value="<%= Request.QueryString["sort"] %>"/>
                <input type="hidden" id="sortorder" name="sortorder" value="<%= Request.QueryString["sortorder"] %>"/>
                <input type="submit" class="reset-bt" value="" title="Сбросить фильтр" />
                </form>          
            </div>


            <div class="br">
			    <div class="tt">
				    <div>
				    </div>
		    </div>
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
            <td><input name="login" id="login" class="txt txt2" value="<%= Request.QueryString["login"] %>" /></td></tr>
        <tr><td class="left">Организация&nbsp;&nbsp;</td>
            <td><input name="organizationName" id="organizationName" class="txt txt2" 
                value="<%= Request.QueryString["organizationName"] %>" /></td></tr>
        <tr><td class="left">E-mail</td>
            <td><input name="email" id="email" class="txt txt2" value="<%= Request.QueryString["email"] %>" /></td></tr>
         <tr><td class="left2">Статус</td>
            <td><select name="status" id="fstatus" multiple="multiple">
                <option <%=SelectValue("status", "") %> value="" >&lt;Все&gt;</option> 
                <option value="registration" <%= SelectValue("status", "registration") %>>На регистрации</option> 
                <option value="revision" <%= SelectValue("status", "revision") %>>На доработке</option> 
                <option value="consideration" <%= SelectValue("status", "consideration") %>>На согласовании</option> 
                <option value="activated" <%= SelectValue("status", "activated") %>>Действующий</option> 
                <option value="deactivated" <%= SelectValue("status", "deactivated") %>>Отключенный</option> 
            </select></td>
         </tr>
         <tr>
            <td class="left2">Тип</td>
            <td>
            <select name="eitype" id="feitype" multiple="multiple">
                <option <%=SelectValue("eitype", "") %> value="" >&lt;Все&gt;</option>
                <asp:Repeater runat="server" ID="rptEducationTypes">
                <ItemTemplate>
                    <option value="<%# Eval("Id") %>" <%# SelectValue("eitype", Convert.ToString(Eval("Id"))) %>><%# Eval("Name") %></option> 
                </ItemTemplate> 
                </asp:Repeater>
            </select>
            </td>
        </tr>
        <tr><td class="left2">Комментарии оператора</td>
            <td>
            <select name="hasComments" id="hasComments" class="sel">
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

    function resetButtonClick()
    {
        document.getElementById("login").value = "";
        document.getElementById("organizationName").value = "";
        document.getElementById("email").value = "";
        document.getElementById("fstatus").value = "";
        document.getElementById("feitype").value = "";
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
                <div><fbs:SortRef_prefix Prefix="../../../Common/Images/" runat="server" SortExpr="login" SortExprText="Логин" /></div>
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
                <fbs:SortRef_prefix Prefix="../../../Common/Images/" runat="server" SortExpr="organizationName" SortExprText="Организация" />
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
                <fbs:SortRef_prefix Prefix="../../../Common/Images/" runat="server" SortExpr="lastName" SortExprText="Ответственное&nbsp;лицо" />
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("LastName") %> <%# Eval("FirstName") %> <%# Eval("PatronymicName") %><br/>
                <nobr><a href="mailto:<%# Eval("Email") %>"><%# Eval("Email") %></a></nobr>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn ItemStyle-Wrap="true">
            <HeaderStyle Width="15%"/>
            <HeaderTemplate>
                <div><fbs:SortRef_prefix Prefix="../../../Common/Images/" runat="server" SortExpr="status" SortExprText="Статус" /></div>
            </HeaderTemplate>
            <ItemTemplate>
                <div style="white-space:normal;" title="<%# UserAccountExtentions.GetUserAccountStatusName(Eval("Status")) %>">
                 <%# GetShortStatus((Eval("Status").ToString()))%>
                </div>
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

    <web:NoRecordsText runat="server" ControlId="dgUserList">
        <Message><p class="notfound">Не найдено ни одного пользователя</p></Message>
    </web:NoRecordsText>

    <fbs:Pager_Simple runat="server" id="tablePager_bottom"></fbs:Pager_Simple>
</asp:Content>