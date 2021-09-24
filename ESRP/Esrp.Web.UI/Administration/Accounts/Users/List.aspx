<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Esrp.Web.Administration.Accounts.Users.List"
    MasterPageFile="~/Common/Templates/Administration.master" %>

<%@ OutputCache VaryByParam="none" Duration="1" %>
<%@ Import Namespace="Esrp.Web" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Controls" Assembly="Esrp.Web.UI" %>
<asp:Content runat="server" ContentPlaceHolderID="cphHead">
    <script src="/Common/Scripts/Filter.js" type="text/javascript"> </script>
    
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
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphContentModalDialog">
    <div id="filter1" class="pop" style="display: none;">
        <div class="pop_bg">
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
                            Организация&nbsp;&nbsp;
                        </td>
                        <td>
                            <input name="organizationName" id="forganizationName" type="text" value="<%=this.Request.QueryString["organizationName"]%>" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Статус
                        </td>
                        <td>
                            <select name="status" id="fstatus" multiple="multiple">
                                <option <%=this.SelectValue("status", "")%> value="">&lt;Все&gt;</option>
                                <option value="registration" <%=this.SelectValue("status", "registration")%>>На регистрации</option>
                                <option value="revision" <%=this.SelectValue("status", "revision")%>>На доработке</option>
                                <option value="consideration" <%=this.SelectValue("status", "consideration")%>>На согласовании</option>
                                <option value="activated" <%=this.SelectValue("status", "activated")%>>Действующий</option>
                                <option value="deactivated" <%=this.SelectValue("status", "deactivated")%>>Отключенный</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Тип
                        </td>
                        <td>
                            <select name="eitype" id="feitype" multiple="multiple">
                                <option <%=this.SelectValue("eitype", "")%> value="">&lt;Все&gt;</option>
                                <asp:Repeater runat="server" ID="rptEducationTypes">
                                    <ItemTemplate>
                                        <option value="<%#this.Eval("Id")%>" <%#this.SelectValue("eitype", Convert.ToString(this.Eval("Id")))%>>
                                            <%#this.Eval("Name")%></option>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Оператор
                        </td>
                        <td>
                            <input name="operatorName" type="text" id="foperatorName" value="<%=this.Request.QueryString["operatorName"]%>" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Комментарии оператора
                        </td>
                        <td>
                            <select name="hasComments" id="fhasComments" class="_change_select_page_size">
                                <option id="sel_1" value="0">&lt;Не важно&gt;</option>
                                <option id="sel_2" value="1" <%=this.SelectValue("hasComments", "1")%>>Нет</option>
                                <option id="sel_3" value="2" <%=this.SelectValue("hasComments", "2")%>>Есть</option>
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
            document.getElementById("flogin").value = "";
            document.getElementById("forganizationName").value = "";
            document.getElementById("fstatus").value = "";
            document.getElementById("feitype").value = "";
            document.getElementById("foperatorName").value = "";
            $('#fhasComments').val('0');
            $('#cuselFrame-fhasComments .cuselText').html('&lt;Не важно&gt;');
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
    <div class="main_table">
        <div class="sort table_header">
            <div class="f_left">
                <% if (User.IsInRole("EditUserAccount"))
                   { %>
                <a href="/Administration/Accounts/Users/Create.aspx" class="create_user">Создать пользователя</a>
                <% } %>
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
        <asp:DataGrid runat="server" ID="dgUserList" AutoGenerateColumns="false" EnableViewState="false"
            ShowHeader="True" GridLines="None" UseAccessibleHeader="true" Width="100%" CssClass="table-th">
            <HeaderStyle CssClass="actions" />
            <Columns>
                <asp:TemplateColumn>
                    <HeaderStyle Width="15%" />
                    <HeaderTemplate>
                        <div>
                            <esrp:SortRef_Prefix Prefix="../../../Common/Images/" runat="server" SortExpr="login"
                                SortExprText="Логин" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <a href="/Administration/Accounts/Users/View.aspx?login=<%#HttpUtility.UrlEncode(Convert.ToString(this.Eval("Login")))%>">
                            <%#Convert.ToString(this.Eval("Login"))%></a>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="30%" />
                    <HeaderTemplate>
                        <esrp:SortRef_Prefix Prefix="../../../Common/Images/" runat="server" SortExpr="organizationName"
                            SortExprText="Организация" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <%#
                (this.Eval("OrgId") != DBNull.Value)
                    ? string.Format(
                        "<a href='../../Organizations/Administrators/OrgCard_Edit.aspx?OrgID={0}'>{1}</a>",
                        this.Eval("OrgId"),
                        this.Eval("OrganizationName"))
                    : Convert.ToString(this.Eval("OrganizationName"))%>
                        </div>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="15%" />
                    <HeaderTemplate>
                        <esrp:SortRef_Prefix Prefix="../../../Common/Images/" runat="server" SortExpr="lastName"
                            SortExprText="Ответственное&nbsp;лицо" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%#this.Eval("LastName")%>
                        <%#this.Eval("FirstName")%>
                        <%#this.Eval("PatronymicName")%><br />
                        <nobr><a href="mailto:<%#this.Eval("Email")%>"><%#this.Eval("Email")%></a></nobr>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn ItemStyle-Wrap="true">
                    <HeaderStyle Width="15%" />
                    <HeaderTemplate>
                        <div>
                            <esrp:SortRef_Prefix Prefix="../../../Common/Images/" runat="server" SortExpr="status"
                                SortExprText="Статус" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div style="white-space: normal;" title="<%#UserAccountExtentions.GetUserAccountStatusName(this.Eval("Status"))%>">
                            <%#this.GetShortStatus((this.Eval("Status").ToString()))%>
                        </div>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn ItemStyle-Wrap="false">
                    <HeaderStyle Width="5%" />
                    <HeaderTemplate>
                        <div>
                            <esrp:SortRef_Prefix Prefix="../../../Common/Images/" runat="server" SortExpr="operatorName"
                                SortExprText="Оператор" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%#this.Eval("OperatorName")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn ItemStyle-Wrap="false">
                    <HeaderStyle Width="5%" />
                    <HeaderTemplate>
                        <div>
                            <esrp:SortRef_Prefix ID="SortRef_prefix1" Prefix="../../../Common/Images/" runat="server"
                                SortExpr="hasComments" SortExprText="Комментарии" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%#this.Eval("HasComments").ToString() == "2" ? "Есть" : "Нет"%>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <web:NoRecordsText runat="server" ControlId="dgUserList">
            <Message>
                <p class="notfound">
                    Не найдено ни одного пользователя</p>
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
</asp:Content>
