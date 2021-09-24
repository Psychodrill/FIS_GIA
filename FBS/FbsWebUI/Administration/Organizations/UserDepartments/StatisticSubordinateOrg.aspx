<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Administration.Master"
    AutoEventWireup="true" CodeBehind="StatisticSubordinateOrg.aspx.cs" Inherits="Fbs.Web.Administration.Organizations.UserDepartments.StatisticSubordinateOrg" %>

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
    <form id="form1" runat="server">
    <asp:Label ID="LblOrgName" runat="server" Text="" Font-Bold="true"></asp:Label><br />
    <br />
    <asp:LinkButton ID="LBReport" runat="server" Text="Получить отчет со статистикой по подведомственным учреждениям"
        CommandArgument="ReportXMLSubordinateOrg" OnClick="Report_Click">
    </asp:LinkButton>
    <br />
    <br />
    </form>
    <% // Контролы фильтрации списка %>
    <!-- filtr -->
    <div class="filtr" id="filtr-layer" style="display: none;">
        <div class="bg-l">
        </div>
        <div class="bg-r">
        </div>
        <div class="content-f">
            <div class="fleft">
                <p>
                    <a href="javascript:void(0);" title="Установить параметры фильтра" onclick="showFilter(true);">
                        Фильтр:</a>
                    <%= FilterStatusString() %></p>
            </div>
            <form action="" method="get">
            <input type="submit" class="reset-bt" value="" title="Сбросить фильтр" />
            </form>
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
                            <input name="OrgName" class="txt txt2" value="<%= Request.QueryString["OrgName"] %>" />
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
                            <select id="RegID" name="RegionId" class="sel small">
                                <option value="0" <%= IsElementSelected(0,"RegionId") %>>&lt;Все&gt;</option>
                                <asp:Repeater ID="RepeaterRegions" runat="server">
                                    <ItemTemplate>
                                        <option value="<%# Eval("Id") %>" <%# IsElementSelected((int)Eval("Id"),"RegionId") %>>
                                            <%# Eval("Name") %></option>
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
                            <select name="CountUser" class="sel small">
                                <option value="0" <%= IsElementSelected(0,"CountUser") %>>&lt;Не важно&gt;</option>
                                <option value="1" <%= IsElementSelected(1,"CountUser") %>>Нет</option>
                                <option value="2" <%= IsElementSelected(2,"CountUser") %>>Есть</option>
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

        function resetButtonClick() {
            document.getElementById("OrgName").value = "";
            document.getElementById("RegID").value = "0";
            document.getElementById("TypeId").value = "0";
            document.getElementById("UserCount").value = "0";
            return false;
        }

    -->
    </script>

    <div style="width: 100%; overflow-x: hidden;">
        <fbs:Pager_Simple runat="server" ID="tablePager_top">
        </fbs:Pager_Simple>
        <asp:DataGrid runat="server" ID="dgStatSubordinateOrg" AutoGenerateColumns="false"
            EnableViewState="false" ShowHeader="True" GridLines="None" CssClass="table-th">
            <HeaderStyle CssClass="th" />
            <Columns>
                <asp:TemplateColumn>
                    <HeaderStyle Width="60%" CssClass="left-th" />
                    <HeaderTemplate>
                        <div>
                            <fbs:SortRef_Prefix ID="SortRef_Prefix1" runat="server" SortExpr="FullName" SortExprText="Название"
                                Prefix="../../../Common/Images/" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <a href="/Administration/Organizations/UserDepartments/OrgCardInfo.aspx?OrgID=<%#Eval("Id")%>">
                            <%# Convert.ToString(Eval("FullName"))%></a>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="5%" />
                    <HeaderTemplate>
                        <fbs:SortRef_Prefix ID="SortRef_Prefix2" runat="server" SortExpr="RegionName" SortExprText="Регион"
                            Prefix="../../../Common/Images/" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.ToString(Eval("RegionName"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="20%" />
                    <HeaderTemplate>
                        <fbs:SortRef_Prefix ID="SortRef_Prefix3" runat="server" SortExpr="AccreditationSertificate"
                            SortExprText="Аккредитация" Prefix="../../../Common/Images/" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.ToString(Eval("AccreditationSertificate"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="20%" />
                    <HeaderTemplate>
                        <fbs:SortRef_Prefix ID="SortRef_Prefix5" runat="server" SortExpr="DirectorFullName"
                            SortExprText="Руководитель" Prefix="../../../Common/Images/" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.ToString(Eval("DirectorFullName"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="15%" />
                    <HeaderTemplate>
                        <div>
                            <fbs:SortRef_Prefix ID="SortRef_Prefix4" runat="server" SortExpr="CountUser" SortExprText="<nobr>Польз-ей</nobr>"
                                Prefix="../../../Common/Images/" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.ToString(Eval("CountUser"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="15%" />
                    <HeaderTemplate>
                        <div>
                            <fbs:SortRef_Prefix ID="SortRef_Prefix6" runat="server" SortExpr="UserUpdateDate"
                                SortExprText="Активация" Prefix="../../../Common/Images/" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.ToString(Eval("UserUpdateDate", "{0:dd.MM.yyyy}"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="15%" CssClass="right-th" />
                    <HeaderTemplate>
                        <div>
                            <fbs:SortRef_Prefix ID="SortRef_Prefix7" runat="server" SortExpr="CountUniqueChecks"
                                SortExprText="Кол.&nbsp;проверок" Prefix="../../../Common/Images/" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.ToString(Eval("CountUniqueChecks"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <web:NoRecordsText ID="NoRecordsText1" runat="server" ControlId="dgStatSubordinateOrg">
            <Message>
                <p class="notfound">
                    Не найдено ни одной организации</p>
            </Message>
        </web:NoRecordsText>
        <fbs:Pager_Simple runat="server" ID="tablePager_bottom">
        </fbs:Pager_Simple>
    </div>
</asp:Content>
