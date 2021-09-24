<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Main.Master"
    AutoEventWireup="true" CodeBehind="OrgList.aspx.cs" Inherits="Esrp.Web.Administration.Organizations.UserDepartments.OrgList" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Controls" Assembly="Esrp.Web.UI" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <script src="/Common/Scripts/Filter.js" type="text/javascript"></script>

    <script type="text/javascript">
        jQuery(document).ready(function () {
            var params = {
                changedEl: '._change_select_page_size',                
                visRows: 7,
                scrollArrows: true
            }
            cuSel(params);
        });
        function changePageCountAll(value) {
            correctRedirectToURLfromJS('<%= this.GetCurrentUrl("pageSize") %>' + value);
            return true;
        }
        function changePageCountTop() {
            return changePageCountAll(document.getElementById('<%= tablePager_top.ClientID %>').value);
        }
        function changePageCountBottom() {
            return changePageCountAll(document.getElementById('<%= tablePager_bottom.ClientID %>').value);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContentModalDialog" runat="server">
    <div id="filter1" class="pop" style="display: none;">
        <div class="pop_bg">
        </div>
        <div class="pop_rel">
            <div class="block">
                <form action="" method="get">
                <table width="100%">
                    <tr>
                        <td>
                            Название
                        </td>
                        <td width="1">
                            <input name="OrgName" id="OrgName" type="text" value="<%=Request.QueryString["OrgName"]%>" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Тип
                        </td>
                        <td>
                            <select id="TypeId" name="TypeId" multiple="multiple" style="width: 320px!important;">
                                <option value="" <%=SelectValue("TypeId", "") %>>&lt;Все&gt;</option>
                                <asp:Repeater ID="RepeaterOrgTypes" runat="server">
                                    <ItemTemplate>
                                        <option value="<%# Eval("Id") %>" <%# SelectValue("TypeId", Convert.ToString(Eval("Id"))) %>>
                                            <%#Eval("Name")%></option>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Регион
                        </td>
                        <td>
                            <select id="RegID" name="RegID" multiple="multiple" style="width: 320px!important;">
                                <option value="" <%=SelectValue("RegID", "") %>>&lt;Все&gt;</option>
                                <asp:Repeater ID="RepeaterRegions" runat="server">
                                    <ItemTemplate>
                                        <option value="<%#Eval("Id")%>" <%#SelectValue("RegID", Convert.ToString(Eval("Id")))%>>
                                            <%# Eval("Name") %></option>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Пользователи
                        </td>
                        <td>
                            <select name="UserCount" id="UserCount" class="_change_select_page_size">
                                <option id="sel_1_0" value="0" <%=SelectValue("UserCount", "0")%>>&lt;Не важно&gt;</option>
                                <option id="sel_1_1" value="1" <%=SelectValue("UserCount", "1")%>>Нет</option>
                                <option id="sel_1_2" value="2" <%=SelectValue("UserCount", "2")%>>Есть</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Активированные пользователи
                        </td>
                        <td>
                            <select name="ActivatedUsers" id="ActivatedUsers" class="_change_select_page_size">
                                <option id="sel_2_0" value="0" <%=SelectValue("ActivatedUsers", "0")%>>&lt;Не важно&gt;</option>
                                <option id="sel_2_1" value="1" <%=SelectValue("ActivatedUsers", "1")%>>Нет</option>
                                <option id="sel_2_2" value="2" <%=SelectValue("ActivatedUsers", "2")%>>Есть</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <input type="button" value="Отмена" class="closed un_dott" onclick="showFilterDlg(false);" />
                            <input type="reset" value="Очистить" class="un_dott" onclick="resetButtonClick(); return false;" />
                            <input type="submit" value="Установить" onclick="showFilterDlg(false);" />
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
            document.getElementById("OrgName").value = "";
            document.getElementById("RegID").value = "";
            document.getElementById("TypeId").value = "";
            $('#UserCount').val('0');
            $('#cuselFrame-UserCount .cuselText').html('&lt;Не важно&gt;');
            document.getElementById("sel_1_0").className = "cuselActive";
            document.getElementById("sel_1_1").className = "";
            document.getElementById("sel_1_2").className = "";
            $('#ActivatedUsers').val('0');
            $('#cuselFrame-ActivatedUsers .cuselText').html('&lt;Не важно&gt;');
            document.getElementById("sel_2_0").className = "cuselActive";
            document.getElementById("sel_2_1").className = "";
            document.getElementById("sel_2_2").className = "";
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

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphSecondLevelMenu">
			<div class="bottom_line">
				<div class="max_width">
					<esrp:TopMenu ID="SecondLevelMenu1" runat="server" RootResourceKey="organizationUserDepartments" 
                        HeaderTemplate="<ul>" FooterTemplate="</ul>"/>
					<div class="clear"></div>
				</div>
			</div><!--bottom_line-->
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphContent" runat="server">
    <form id="Form1" runat="server">
    <input type="hidden" id="sort" name="sort" value="<%=Request.QueryString["sort"]%>" />
    <input type="hidden" id="sortorder" name="sortorder" value="<%=Request.QueryString["sortorder"]%>" />
    <div class="main_table">
        <div class="sort table_header">
            <div class="sorted">
                <esrp:Pager_Simple runat="server" ID="tablePager_top">
                </esrp:Pager_Simple>
                <div class="clear">
                </div>
            </div>
            <div class="sorted">
                <span>&nbsp;<a href="javascript:void(0);" class="un_dott" onclick="showFilterDlg(true);">Фильтр</a>
                    <%= FilterStatusString()%>
                </span>
                <div class="clear">
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
        <asp:DataGrid runat="server" ID="dgOrgList" AutoGenerateColumns="false" EnableViewState="false"
            ShowHeader="True" GridLines="None" CssClass="table-th">
            <HeaderStyle CssClass="th" />
            <Columns>
                <asp:TemplateColumn>
                    <HeaderStyle Width="40%" CssClass="left-th" />
                    <HeaderTemplate>
                        <div>
                            <esrp:SortRef_Prefix ID="SortRef_Prefix1" runat="server" SortExpr="FullName" SortExprText="Название"
                                Prefix="../../../Common/Images/" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <a href="/Administration/Organizations/UserDepartments/OrgCardInfo.aspx?OrgID=<%#Eval("OrgID")%>">
                                <%# Convert.ToString(Eval("FullName"))%></a>
                        </div>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <esrp:SortRef_Prefix ID="SortRef_Prefix2" runat="server" SortExpr="TypeName" SortExprText="Тип"
                            Prefix="../../../Common/Images/" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.ToString(Eval("TypeName"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <esrp:SortRef_Prefix ID="SortRef_Prefix3" runat="server" SortExpr="RegName" SortExprText="Регион"
                            Prefix="../../../Common/Images/" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.ToString(Eval("RegName"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <div>
                            <esrp:SortRef_Prefix ID="SortRef_Prefix4" runat="server" SortExpr="UserCount" SortExprText="Пользователей"
                                Prefix="../../../Common/Images/" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.ToString(Eval("UserCount"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle CssClass="right-th" />
                    <HeaderTemplate>
                        <div>
                            <esrp:SortRef_Prefix ID="SortRef_Prefix5" runat="server" SortExpr="ActivatedUsers"
                                SortExprText="Активированных&nbsp;пользователей" Prefix="../../../Common/Images/" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%#Convert.ToString(Eval("ActivatedUsers"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <web:NoRecordsText ID="NoRecordsText1" runat="server" ControlId="dgOrgList">
            <Message>
                <p class="notfound">
                    Не найдено ни одной организации</p>
            </Message>
        </web:NoRecordsText>
        <div class="sort table_footer">
            <div class="f_left">
            </div>
            <div class="sorted">
                    <esrp:Pager_Simple runat="server" ID="tablePager_bottom">
                    </esrp:Pager_Simple>
                    <div class="clear">
                </div>
            </div>
        </div>
    </div>
    </form>
</asp:Content>
