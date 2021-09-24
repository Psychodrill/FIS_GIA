<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Main.Master" AutoEventWireup="true"
    CodeBehind="SelectOrg.aspx.cs" Inherits="Esrp.Web.Registration_st1" %>

<%@ Import Namespace="Esrp.Core.Organizations" %>
<%@ Import Namespace="Esrp.Core.Systems" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Controls" Assembly="Esrp.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <script src="/Common/Scripts/Filter.js" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            var params = {
                changedEl: "select",
                visRows: 7
            }
            cuSel(params);
            ToggleSearchBy('<%=this.Request.QueryString["RBSearchBy"]  %>',0);
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
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="server">
    <% if (GeneralSystemManager.IsUserActivated(User.Identity.Name))
       { %>
    <div class="left_col">
        <% } %>
        <form id="Form1" method="get" >
        <div class="col_in">
            <div class="organization">
                <p class="title">
                    Выбор организации</p>
                <p>
                    <strong>Для выбора Вашей организации:</strong></p>
                <ol>
                    <li>Выберите поиск <strong>по названию, региону и типу</strong> или поиск по <strong>
                        ИНН</strong></li>
                    <li>В случае выбора поиска по названию, укажите регион, тип организации и часть наименования
                        Вашей организации</li>
                    <li>В случае выбора поиска по ИНН, введите ИНН Вашей организации</li>
                    <li>Нажмите кнопку <strong>«Найти»</strong></li>
                </ol>
                <p>
                    &nbsp;</p>
                <div class="search_organization">
                    <p class="name_block">
                        <strong>Поиск организации</strong></p>
                    <div class="search_organization_in">
                        <p>
                            Искать по</p>
                        <p>
                            <input type="radio" id="r1" name="RBSearchBy" value="OrgName" <%= IsRadioButtonSelected("OrgName") %>
                                onclick="ToggleSearchBy('OrgName',1);" />
                            <label for="r1">
                                Названию, региону и типу</label>
                        </p>
                        <p>
                            <input type="radio" id="r2" name="RBSearchBy" value="INN" <%= IsRadioButtonSelected("INN") %>
                                onclick="ToggleSearchBy('INN',1);" />
                            <label for="r2">
                                Номеру ИНН</label>
                        </p>
                        <p>
                            <input type="radio" id="r3" name="RBSearchBy" value="Dic" <%= IsRadioButtonSelected("Dic") %>
                                onclick="ToggleSearchBy('Dic',1);" />
                            <label for="r3">
                                Выбор из справочника</label>
                        </p>
                        <a name="filter"></a>
                        <table width="100%">
                            <tr>
                                <td colspan="3">
                                    <table class="filtr2-table" width="100%">
                                        <!-- Поиск по Названию, региону и типу -->
                                        <tr id="RegionRow">
                                            <td class="left">
                                                Регион
                                            </td>
                                            <td width="1">
                                                <select id="RegID" name="RegID" class="sel long">
                                                    <option value="" <%=SelectValue("RegID", "") %>>&nbsp;&lt;Все&gt;</option>
                                                    <asp:Repeater ID="RepeaterRegions" runat="server">
                                                        <ItemTemplate>
                                                            <option value="<%# Eval("Id") %>" <%#SelectValue("RegID", Convert.ToString(Eval("Id"))) %>>
                                                                <%# Eval("Name") %></option>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </select>
                                            </td>
                                        </tr>
                                        <tr id="OrgTypeRow">
                                            <td class="left">
                                                Тип
                                            </td>
                                            <td>
                                                <select id="OldTypeId" name="OldTypeId" class="sel">
                                                    <option value="" <%=SelectValue("OldTypeId", "") %>>&nbsp;&lt;Все&gt;&nbsp;</option>
                                                    <option value="1" <%=SelectValue("OldTypeId", "1") %>>ВУЗ государственный</option>
                                                    <option value="2" <%=SelectValue("OldTypeId", "2") %>>ВУЗ негосударственный</option>
                                                    <option value="3" <%=SelectValue("OldTypeId", "3") %>>ССУЗ государственный</option>
                                                    <option value="4" <%=SelectValue("OldTypeId", "4") %>>ССУЗ негосударственный</option>
                                                    <option value="5" <%=SelectValue("OldTypeId", "5") %>>РЦОИ</option>
                                                    <option value="6" <%=SelectValue("OldTypeId", "6") %>>Орган управления образованием</option>
                                                    <option value="8" <%=SelectValue("OldTypeId", "8") %>>Учредитель</option>
                                                    <option value="7" <%=SelectValue("OldTypeId", "7") %>>Другое</option>
                                                </select>
                                            </td>
                                        </tr>
                                        <tr id="OrgNameRow" >
                                            <td class="left">
                                                Название
                                            </td>
                                            <td>
                                                <input id="OrgName" name="OrgName" class="txt long" type="text" value="<%= Request.QueryString["OrgName"] %>" />
                                            </td>
                                        </tr>
                                        <tr id="OrgNameWarningRow" >
                                            <td>
                                            </td>
                                            <td>
                                                <div class="warning">
                                                    <p>
                                                        Поиск может осуществляться по ключевому слову из полного наименования организации<br />
                                                        Например: энергетический. Поиск по аббревиатуре организации не осуществляется.</p>
                                                </div>
                                            </td>
                                        </tr>
                                        <!-- Поиск по ИНН -->
                                        <tr id="INNRow">
                                            <td class="left">
                                                ИНН
                                            </td>
                                            <td width="1">
                                                <input type="text" id="INN" name="INN" class="txt" maxlength="10" value="<%= Request.QueryString["INN"] %>" />
                                            </td>
                                        </tr>
                                        <tr id="INNWarningRow">
                                            <td>
                                            </td>
                                            <td>
                                                <div class="warning">
                                                    <p>
                                                        ИНН должен состоять только из цифр и содержать ровно 10 знаков.</p>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="Buttons">
                                            <td colspan="3" class="cell-bt">
                                                <input type="hidden" id="ReturnPrmName" name="ReturnPrmName" value="<%= ReturnPrmName %>" />
                                                <input type="hidden" id="BackUrl" name="BackUrl" value="<%= BackUrl %>" />
                                                <input type="hidden" id="PageSize" name="PageSize" value="<%= Request.QueryString["PageSize"] %>" />
                                                <input type="submit" class="bt bt2" value="Найти" style="width: 120px" />&nbsp;
                                                <input type="button" class="un_dott" value="Очистить" onclick="return resetButtonClick();" />&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        </form>
    </div>
    
    <script language="javascript" type="text/javascript">

        function ToggleSearchBy(Mode,notfirst) {
            if (Mode == '')
                Mode = 'OrgName';
            if (Mode == "INN") {
                document.getElementById("INNRow").style.display = "";
                document.getElementById("INNWarningRow").style.display = "";
                document.getElementById("OrgNameRow").style.display = "none";
                document.getElementById("OrgNameWarningRow").style.display = "none";
                document.getElementById("OrgTypeRow").style.display = "none";
                document.getElementById("RegionRow").style.display = "none";
                document.getElementById("DicParams").style.display = "none";
                document.getElementById("DicParamsDiv").style.display = "none";
                document.getElementById("DicTree").style.display = "none";
                document.getElementById("Buttons").style.display = "";
            
                if(notfirst==1)
                 $(".pnlData").css("display", "none");
            }
            else if (Mode == "OrgName") {
                document.getElementById("INNRow").style.display = "none";
                document.getElementById("INNWarningRow").style.display = "none";
                document.getElementById("OrgNameRow").style.display = "";
                document.getElementById("OrgNameWarningRow").style.display = "";
                document.getElementById("OrgTypeRow").style.display = "";
                document.getElementById("RegionRow").style.display = "";
                document.getElementById("DicParams").style.display = "none";
                document.getElementById("DicParamsDiv").style.display = "none";
                document.getElementById("DicTree").style.display = "none";
                document.getElementById("Buttons").style.display = "";
                if (notfirst == 1)
                    $(".pnlData").css("display", "none");
            }
            else if (Mode == "Dic") {
                document.getElementById("INNRow").style.display = "none";
                document.getElementById("INNWarningRow").style.display = "none";
                document.getElementById("OrgNameRow").style.display = "none";
                document.getElementById("OrgNameWarningRow").style.display = "none";
                document.getElementById("OrgTypeRow").style.display = "none";
                document.getElementById("RegionRow").style.display = "none";
                document.getElementById("DicParams").style.display = "";
                document.getElementById("DicParamsDiv").style.display = "";
                document.getElementById("DicTree").style.display = "";
                document.getElementById("Buttons").style.display = "none";
                $(".pnlData").css("display", "none");
            }
        }
    </script>
    <form id="Form2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="search_organization" style="width: 720px!important;" id="DicParamsDiv">
        <table id="DicParams"  cellpadding="0" cellspacing="0"
            border="0">
            <tr>
                <td style="padding-right: 10px;">
                    Регион
                </td>
                <td width="1" style="padding-right: 10px;">
                    <asp:DropDownList CssClass="sel" ID="dlRegions" DataTextField="Name" DataValueField="Id"
                        runat="server" AutoPostBack="True" OnSelectedIndexChanged="dlRegions_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Тип
                </td>
                <td>
                    <asp:DropDownList CssClass="sel" ID="dlSchoolType" DataTextField="Name" DataValueField="Id"
                        runat="server" AutoPostBack="True" OnSelectedIndexChanged="dlRegions_SelectedIndexChanged1">
                    </asp:DropDownList>
                </td>
            </tr>           
        </table>
    </div>
    <table id="DicTree"  cellpadding="0" cellspacing="0" border="0"
        width="100%">
        <tr id="t">
            <td style="padding-left: 11px;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:PlaceHolder ID="phEmpty" runat="server">
                            <asp:Label ID="Label2" runat="server" Text="Для выбранного региона и типа ОУ нет данных"></asp:Label>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="phNote" runat="server">
                            <div class="warning">
                                <p>
                                    Для загрузки справочника ОУ необходимо выбрать регион и тип ОУ.</p>
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="phDicTree" runat="server">
                            <asp:Label ID="Label1" runat="server" Text="Справочник ОУ"></asp:Label>
                            <br />
                            <asp:TreeView ID="TreeView1" runat="server" ImageSet="Arrows" Width="600px" NodeWrap="True"
                                OnSelectedNodeChanged="TreeView1_SelectedNodeChanged">
                                <ParentNodeStyle Font-Bold="False" />
                                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                <SelectedNodeStyle Font-Underline="True" HorizontalPadding="0px" VerticalPadding="0px"
                                    ForeColor="#5555DD" />
                                <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                                    NodeSpacing="0px" VerticalPadding="0px" />
                            </asp:TreeView>
                        </asp:PlaceHolder>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <a name="result"></a>
    <asp:Panel runat="server" ID="pnlData" Visible="false" CssClass="pnlData">
        <table id="pnlTable" class="pnlTable">
            <tr>
                <td style="padding-bottom: 10px">
                    По Вашему запросу найдено <b>
                        <asp:Label runat="server" ID="txtInfo" /></b> организаций.
                </td>
            </tr>
        </table>
        <div class="main_table">
            <div class="sort table_header">
                <div class="sorted">
                    <esrp:Pager_Simple runat="server" ID="tablePager_top" PageSizes="5,10,20,50,100" PageSize="100">
                    </esrp:Pager_Simple>
                    <div class="clear">
                    </div>
                </div>
                <div class="sorted">
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
                        <HeaderStyle Width="60%" CssClass="left-th" />
                        <HeaderTemplate>
                            <div>
                                <esrp:SortRef_Prefix runat="server" SortExpr="FullName" SortExprText="Название" Prefix="../../../Common/Images/" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Convert.ToString(Eval("FullName"))%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
                        <HeaderStyle Width="5%" />
                        <HeaderTemplate>
                            <esrp:SortRef_Prefix runat="server" SortExpr="TypeName" SortExprText="Тип" Prefix="../../../Common/Images/" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("TypeName").ToString() %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
                        <HeaderStyle Width="5%" />
                        <HeaderTemplate>
                            <esrp:SortRef_Prefix ID="SortRef_Prefix1" runat="server" SortExpr="IsPrivate" SortExprText="Правовая&nbsp;форма"
                                Prefix="../../../Common/Images/" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# ((OrganizationDataAccessor.OPF) Convert.ToInt32(Eval("IsPrivate"))) == 
													OrganizationDataAccessor.OPF.Private ? "Негосударственный" : "государственный" %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
                        <HeaderStyle Width="20%" />
                        <HeaderTemplate>
                            <esrp:SortRef_Prefix runat="server" SortExpr="RegName" SortExprText="Регион" Prefix="../../../Common/Images/" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Convert.ToString(Eval("RegName"))%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
                        <HeaderStyle Width="15%" CssClass="right-th" />
                        <HeaderTemplate>
                            &nbsp;
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="button" style="width: 100px" value="Выбрать" onclick="window.location='<%= BackUrl_forUse%><%= ReturnPrmName %>=<%#Eval("OrgID")%>'; return false;" />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid>
            <web:NoRecordsText runat="server" ControlId="dgOrgList" ID="NoRecords">
                <Message>
                    <p class="notfound">
                        Не найдено ни одной организации</p>
                </Message>
            </web:NoRecordsText>
        <div class="sort table_footer">
            <div class="f_left">
            </div>
            <div class="sorted">
                        <esrp:Pager_Simple runat="server" ID="tablePager_bottom" PageSizes="5,10,20,50,100"
                            PageSize="100">
                        </esrp:Pager_Simple>
                        <div class="clear">
                        </div>
            </div>
        </div>
        </div>
    </asp:Panel>
    <asp:Label runat="server" ID="lblNoFilter" Visible="false" Style="color: DarkRed;
        font-weight: bold;">Не заданы параметры поиска!</asp:Label>
    </form>
    <script type="text/javascript">

        function goAfterCommit() {

            if (window.confirm('Вы действительно не нашли организацию и хотите ввести её?')) {
                window.location = '/Registration_st2.aspx?OrgID=0';
            }
            return false;
        }

        function resetButtonClick() {
            document.getElementById("RegID").value = "";
            document.getElementById("OldTypeId").value = "";
            document.getElementById("OrgName").value = "";
            document.getElementById("INN").value = "";
            $('#RegID').val('');
            $('#cuselFrame-RegID .cuselText').html('&lt;Все&gt;');
            $('#OldTypeId').val('');
            $('#cuselFrame-OldTypeId .cuselText').html('&lt;Все&gt;');
            return false;
        }

        function resetButtonClick1() {
            var tt = "<%=this.dlRegions.ClientID %>";
            var t = "#" + tt;
            $(t).val('');
            t = '#cuselFrame-' + tt + ' .cuselText';
            $(t).html('&lt;Выберите регион&gt;');

            tt = "<%=this.dlSchoolType.ClientID %>";
            t = "#" + tt;
            $(t).val('');
            t = '#cuselFrame-' + tt + ' .cuselText';
            $(t).html('&lt;Выберите тип&gt;');

            return false;
        }

    </script>
</asp:Content>
