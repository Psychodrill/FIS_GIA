<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Administration.Master"
    AutoEventWireup="true" CodeBehind="OrgList.aspx.cs" Inherits="Fbs.Web.Administration.Organizations.OrgList" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Controls" Assembly="FbsWebUI" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Fbs.Core" %>
<%@ Import Namespace="Fbs.Web" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">

    <script src="/Common/Scripts/Filter.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphLeftMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphActions" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphContent" runat="server">
    <% // Контролы фильтрации списка %>
    <!-- filtr -->	
    <div class="ffilter-block" id="filtr-layer" style="display: none;">
        <div class="tr">
			    <div class="tt">
				    <div>
				    </div>
			    </div>
		</div>
        <div class="content">
            <a href="javascript:void(0);" title="Установить параметры фильтра" onclick="showFilter(true);">Фильтр:</a>&nbsp;<%=FilterStatusString() %>
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
    <!-- "OrgName", "RegName", "RegID", "OrgType", "OrgTypeID", "OrgStatus", "OrgStatusID" -->
    <div class="to-be-defined" id="filtr2-layer">
        <div class="filtr2" id="filtr2">
            <div class="h-line">
                <div class="tl">
                    <div>
                    </div>
                </div>
                <div class="tr">
                    <div>
                    </div>
                </div>
                <div class="t">
                    <div>
                    </div>
                </div>
            </div>
            <div class="content-f2">
                <form action="" method="get">
                <table class="filtr2-table">
                    <tr>
                        <td class="left">
                            Название
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <input name="OrgName" id="OrgName" class="txt txt2" value="<%= Request.QueryString["OrgName"] %>" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Тип
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <select id="TypeId" name="TypeId" multiple="multiple" >
                                <option value="" <%=SelectValue("TypeId", "") %> >&lt;Все&gt;</option>
                                <asp:Repeater ID="RepeaterOrgTypes" runat="server">
                                    <ItemTemplate>
                                        <option value="<%# Eval("Id") %>" <%# SelectValue("TypeId", Convert.ToString(Eval("Id"))) %> > <%#Eval("Name")%></option>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Регион
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <select id="RegID" name="RegID" multiple="multiple" >
                                <option value="" <%=SelectValue("RegID", "")%> >&lt;Все&gt;</option>
                                <asp:Repeater ID="RepeaterRegions" runat="server">
                                    <ItemTemplate>
                                        <option value="<%#Eval("Id")%>" <%#SelectValue("RegID", Convert.ToString(Eval("Id")))%> > <%# Eval("Name") %></option>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Пользователи
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <select name="UserCount" class="sel" id="UserCount" >
                                <option value="0" <%=SelectValue("UserCount", "0")%> >&lt;Не важно&gt;</option>
                                <option value="1" <%=SelectValue("UserCount", "1")%> >Нет</option>
                                <option value="2" <%=SelectValue("UserCount", "2")%> >Есть</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Активированные пользователи
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <select name="ActivatedUsers" id="ActivatedUsers" class="sel" >
                                <option value="0" <%=SelectValue("ActivatedUsers", "0")%> >&lt;Не важно&gt;</option>
                                <option value="1" <%=SelectValue("ActivatedUsers", "1")%> >Нет</option>
                                <option value="2" <%=SelectValue("ActivatedUsers", "2")%> >Есть</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="cell-bt">
                            <input type="reset" class="bt" value="Очистить" onclick="return resetButtonClick();" />&nbsp;
                            <input type="submit" class="bt bt2" onclick="showFilter(false);" value="Установить" />&nbsp;
                            <input type="button" class="bt" id="btCancel" value="Отмена" onclick="showFilter(false);"
                                style="display: none;" />
                        </td>
                    </tr>
                </table>
                </form>
            </div>
            <div class="h-line">
                <div class="bl">
                    <div>
                    </div>
                </div>
                <div class="br">
                    <div>
                    </div>
                </div>
                <div class="b">
                    <div>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <!-- /filtr2 -->

    <script type="text/javascript">
    <!--

    initFilter();

    function resetButtonClick()
    {
        document.getElementById("OrgName").value = "";
        document.getElementById("RegID").value = "";
        document.getElementById("TypeId").value = "";
        document.getElementById("UserCount").value = "0";
        document.getElementById("ActivatedUsers").value = "0";
        return false;
    }

    -->
    </script>

    <fbs:Pager_Simple runat="server" ID="tablePager_top">
    </fbs:Pager_Simple>
    <asp:DataGrid runat="server" ID="dgOrgList" AutoGenerateColumns="false" EnableViewState="false"
        ShowHeader="True" GridLines="None" CssClass="table-th">
        <HeaderStyle CssClass="th" />
        <Columns>
            <asp:TemplateColumn>
                <HeaderStyle Width="40%" CssClass="left-th" />
                <HeaderTemplate>
                    <div>
                        <fbs:SortRef_Prefix runat="server" SortExpr="FullName" SortExprText="Название" Prefix="../../../Common/Images/" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <a href="/Administration/Organizations/Administrators/OrgCard_Edit.aspx?OrgID=<%#Eval("OrgID")%>">
                        <%# Convert.ToString(Eval("FullName"))%></a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderTemplate>
                    <fbs:SortRef_Prefix runat="server" SortExpr="TypeName" SortExprText="Тип" Prefix="../../../Common/Images/" />
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Convert.ToString(Eval("TypeName"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderTemplate>
                    <fbs:SortRef_Prefix runat="server" SortExpr="RegName" SortExprText="Регион" Prefix="../../../Common/Images/" />
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Convert.ToString(Eval("RegName"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderTemplate>
                    <div>
                        <fbs:SortRef_Prefix runat="server" SortExpr="UserCount" SortExprText="Пользователей"
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
                        <fbs:SortRef_Prefix ID="SortRef_Prefix1" runat="server" SortExpr="ActivatedUsers" SortExprText="Активированных&nbsp;пользователей" Prefix="../../../Common/Images/" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Convert.ToString(Eval("ActivatedUsers"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    <web:NoRecordsText runat="server" ControlId="dgOrgList">
        <Message>
            <p class="notfound">
                Не найдено ни одной организации</p>
        </Message>
    </web:NoRecordsText>
    <fbs:Pager_Simple  runat="server" ID="tablePager_bottom">
    </fbs:Pager_Simple>
</asp:Content>
