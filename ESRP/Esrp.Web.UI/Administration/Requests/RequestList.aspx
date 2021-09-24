<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Administration.Master"
    AutoEventWireup="true" CodeBehind="RequestList.aspx.cs" Inherits="Esrp.Web.Administration.Organizations.RequestList" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Controls" Assembly="Esrp.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <script src="/Common/Scripts/Filter.js" type="text/javascript"></script>
   
    <script type="text/javascript" src="/Common/Scripts/jquery-ui-1.8.18.min.js"></script>
    <script type="text/javascript" src="/Common/Scripts/jquery-bbq-plugin.js"></script>
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
<asp:Content ID="Content2" ContentPlaceHolderID="cphLeftMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphActions" runat="server">
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphContentModalDialog">
    <div id="filter1" class="pop" style="display: none;">
        <div class="pop_bg">
        </div>
        <div class="pop_rel">
            <div class="block">
                <form action="" method="get">
                <table width="100%">
                    <tr>
                        <td>
                            Год
                        </td>
                        <td width="1">
                            <select id="YearInRequests" name="YearInRequests" multiple="multiple">
                                <option value="" <%=SelectValue("YearInRequests", "")%>>&lt;Все&gt;</option>
                                <asp:Repeater ID="rYearInRequests" runat="server">
                                    <ItemTemplate>
                                        <option value="<%#Eval("Year")%>" <%#SelectValue("YearInRequests", Convert.ToString(Eval("Year")))%>>
                                            <%#Eval("Year")%></option>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Статус
                        </td>
                        <td>
                            <select id="StatusID" name="StatusId" multiple="multiple">
                                <option value="" <%=SelectValue("StatusId", "")%>>&lt;Все&gt;</option>
                                <asp:Repeater ID="RepeaterOrgTypes" runat="server">
                                    <ItemTemplate>
                                        <option value="<%# Eval("StatusID") %>" <%#SelectValue("StatusId", Convert.ToString(Eval("StatusID")))%>>
                                            <%# Eval("Name") %></option>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <input type="button" value="Отмена" class="closed un_dott" onclick="showFilterDlg(false);" />
                            <input type="reset" value="Очистить" class="un_dott" onclick="return resetButtonClick();" />
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
            document.getElementById("StatusID").value = "";
            document.getElementById("YearInRequests").value = "";
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
<asp:Content ID="Content4" ContentPlaceHolderID="cphContent" runat="server">
    <form runat="server">
    <input type="hidden" id="sort" name="sort" value="<%= Request.QueryString["sort"] %>" />
    <input type="hidden" id="sortorder" name="sortorder" value="<%= Request.QueryString["sortorder"] %>" />
    <div class="main_table">
        <div class="sort table_header">
            <div class="f_left">
            </div>
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
        <asp:DataGrid runat="server" ID="dgReqList" AutoGenerateColumns="false" EnableViewState="false"
            ShowHeader="True" GridLines="None" CssClass="table-th" UseAccessibleHeader="true"
            Width="100%">
            <HeaderStyle CssClass="actions" />
            <Columns>
                <asp:TemplateColumn>
                    <HeaderStyle Width="15%" />
                    <HeaderTemplate>
                        <div>
                            <esrp:SortRef_Prefix runat="server" SortExpr="Number" SortExprText="Номер заявления"
                                Prefix="../../../Common/Images/" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <a href="/Administration/Requests/RequestForm.aspx?RequestID=<%#Eval("Number")%>">
                            <%# Convert.ToString(Eval("Number"))%></a>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="30%" />
                    <HeaderTemplate>
                        <esrp:SortRef_Prefix runat="server" SortExpr="FullName" SortExprText="Организация"
                            Prefix="../../../Common/Images/" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <%#(Eval("OrganizationId") != DBNull.Value) ? 
			            string.Format("<a href='../Organizations/Administrators/OrgCard_Edit.aspx?OrgID={0}'>{1}</a>", Eval("OrganizationId"), Convert.ToString(Eval("FullName"))) : 
			            Convert.ToString(Eval("FullName"))
                            %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="15%" />
                    <HeaderTemplate>
                        <esrp:SortRef_Prefix runat="server" SortExpr="CreateDate" SortExprText="Дата подачи"
                            Prefix="../../../Common/Images/" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.ToString(Eval("CreateDate"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="15%" />
                    <HeaderTemplate>
                        <esrp:SortRef_Prefix runat="server" SortExpr="StatusName" SortExprText="Статус" Prefix="../../../Common/Images/" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.ToString(Eval("StatusName"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="15%" />
                    <HeaderTemplate>
                        <esrp:SortRef_Prefix runat="server" SortExpr="OperatorWhoSetComment" SortExprText="Оператор"
                            Prefix="../../../Common/Images/" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.ToString(Eval("OperatorWhoSetComment"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="10%" />
                    <HeaderTemplate>
                        <div>
                            <esrp:SortRef_Prefix runat="server" SortExpr="HasComment" SortExprText="Комментарий"
                                Prefix="../../../Common/Images/" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.ToBoolean(Eval("HasComment")) ? "Есть" : "Нет"%>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <web:NoRecordsText runat="server" ControlId="dgReqList">
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
