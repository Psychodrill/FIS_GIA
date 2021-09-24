<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Loginless.Master"
    AutoEventWireup="true" CodeBehind="SelectOrg.aspx.cs" Inherits="Fbs.Web.Registration_st1" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Controls" Assembly="FbsWebUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">

    <script src="/Common/Scripts/Filter.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="server">
    <p class="l8">
        Для выбора Вашей организации:
        <ul>
            <li>Выберите поиск по <b>названию, региону и типу</b> или поиск по <b>ИНН</b></li>
            <li>В случае выбора поиска по названию, укажите регион, тип организации и часть наименования Вашей организации</li>
            <li>В случае выбора поиска по ИНН, введите ИНН Вашей организации</li>
            <li>Нажмите кнопку <b>"Найти"</b></li>
        </ul>
    </p>
    <a name="filter"></a>

    <style type="text/css">
        #DicParams td, #DicTree td
        {
            padding-left: 11px;
        }
        
        #t td 
        {
            margin: 0;
            padding: 0;
        }
        
        #t table
        {
            margin: 0;
            padding: 0;
            margin-top: 2px;
        }
    </style>

    <form id="Form1" method="get" action="#result">

    <table class="table-th">
        <tr class="th">
            <td class="left-th">
                <div>
                </div>
            </td>
            <td style="text-align: center" width="600px">
                Поиск организации
            </td>
            <td class="right-th">
                <div style='text-align: center;'>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table class="filtr2-table">
                    <tr>
                        <td rowspan="6">
                        </td>
                        <td class="left">
                            Искать&nbsp;по:
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td style="white-space: nowrap;">
                            <input type="radio" name="RBSearchBy" value="OrgName" <%= IsRadioButtonSelected("OrgName") %> onclick="ToggleSearchBy('OrgName');" />
                            &nbsp;Названию,&nbsp;региону&nbsp;и&nbsp;типу&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" name="RBSearchBy" value="INN" <%= IsRadioButtonSelected("INN") %> onclick="ToggleSearchBy('INN');" />
                            &nbsp;ИНН&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" name="RBSearchBy" value="Dic" <%= IsRadioButtonSelected("Dic") %> onclick="ToggleSearchBy('Dic');" />
                            &nbsp;Выбор из справочника
                        </td>
                        <td rowspan="6">
                        </td>
                    </tr>
                    
                    <!-- Поиск по Названию, региону и типу -->
                    
                    <tr id="RegionRow" <%= IsRowVisible("OrgNameRow") %>>
                        <td class="left" style="width: 100px">
                            Регион
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <select id="RegID" name="RegID" class="sel long">
                                <option value="0" <%= IsElementSelected(0,"RegID") %>>&nbsp;&lt;Все&gt;</option>
                                <asp:Repeater ID="RepeaterRegions" runat="server">
                                    <ItemTemplate>
                                        <option value="<%# Eval("Id") %>" <%# IsElementSelected((int)Eval("Id"),"RegID") %>>
                                            <%# Eval("Name") %></option>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </select>
                        </td>
                    </tr>
                    <tr id="OrgTypeRow" <%= IsRowVisible("OrgNameRow") %>>
                        <td class="left">
                            Тип
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <select id="OldTypeId" name="OldTypeId" class="sel">
                                <option value="0" <%= IsElementSelected(0,"OldTypeId") %>>&nbsp;&lt;Все&gt;&nbsp;</option>
                                <option value="1" <%= IsElementSelected(1,"OldTypeId") %>>ВУЗ государственный</option>
                                <option value="2" <%= IsElementSelected(2,"OldTypeId") %>>ВУЗ негосударственный</option>
                                <option value="3" <%= IsElementSelected(3,"OldTypeId") %>>ССУЗ государственный</option>
                                <option value="4" <%= IsElementSelected(4,"OldTypeId") %>>ССУЗ негосударственный</option>
                                <option value="5" <%= IsElementSelected(5,"OldTypeId") %>>РЦОИ</option>
                                <option value="6" <%= IsElementSelected(6,"OldTypeId") %>>Орган управления образованием</option>
                                <option value="8" <%= IsElementSelected(8,"OldTypeId") %>>Учредитель</option>
                                <option value="7" <%= IsElementSelected(7,"OldTypeId") %>>Другое</option>
                            </select>
                        </td>
                    </tr>
                    <tr id="OrgNameRow" <%= IsRowVisible("OrgNameRow") %>>
                        <td class="left">
                            Название
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <input id="OrgName" name="OrgName" class="txt long" type="text" value="<%= Request.QueryString["OrgName"] %>" />
                            <div class="notice">
                                <div class="top">
                                    <div class="l">
                                    </div>
                                    <div class="r">
                                    </div>
                                    <div class="m">
                                    </div>
                                </div>
                                <div class="cont" style="background: none;">
                                    <div class="">
                                        <p />
                                        <img src="/Common/Images/notice-block-notice.gif" width="22" height="22" alt="!"
                                            class="ico-notice" />
                                        Поиск может осуществляться по ключевому слову из полного наименования организации
                                        (Например:<b>энергетический</b>).
                                        <br />
                                        Поиск по аббревиатуре организации не осуществляется.<p />
                                    </div>
                                </div>
                                <div class="bottom">
                                    <div class="l">
                                    </div>
                                    <div class="r">
                                    </div>
                                    <div class="m">
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    

                    <!-- Поиск по ИНН -->
                    
                    <tr id="INNRow" <%= IsRowVisible("INNRow") %>>
                        <td class="left">
                            ИНН
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <input type="text" id="INN" name="INN" class="txt" maxlength="10" value="<%= Request.QueryString["INN"] %>" />
                            <div class="notice">
                                <div class="top">
                                    <div class="l">
                                    </div>
                                    <div class="r">
                                    </div>
                                    <div class="m">
                                    </div>
                                </div>
                                <div class="cont" style="background: none;">
                                    <div class="">
                                        <p />
                                        <img src="/Common/Images/notice-block-notice.gif" width="22" height="22" alt="!"
                                            class="ico-notice" />
                                        ИНН должен состоять только из цифр и содержать ровно 10 знаков.<p />
                                    </div>
                                </div>
                                <div class="bottom">
                                    <div class="l">
                                    </div>
                                    <div class="r">
                                    </div>
                                    <div class="m">
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    
                    <tr id="Buttons">
                        <td colspan="3" class="cell-bt">
                            <input type="hidden" id="BackUrl" name="BackUrl" value="<%= BackUrl %>" />
                            <input type="hidden" id="PageSize" name="PageSize" value="<%= Request.QueryString["PageSize"] %>" />
                            <input type="submit" class="bt bt2" value="Найти" style="width: 120px" />&nbsp;
                            <input type="button" class="bt bt2" value="Очистить" style="width: 120px" onclick="resetButtonClick();" />&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
    
    <script type="text/javascript" src="/Common/Scripts/jquery.min.js"></script>
    
    <script language="javascript" type="text/javascript">

        function ToggleSearchBy(Mode)
        {
            if (Mode == "INN") {
                document.getElementById("INNRow").style.display = "";
                document.getElementById("OrgNameRow").style.display = "none";
                document.getElementById("OrgTypeRow").style.display = "none";
                document.getElementById("RegionRow").style.display = "none";
                document.getElementById("DicParams").style.display = "none";
                document.getElementById("DicTree").style.display = "none";
                document.getElementById("Buttons").style.display = "";
                $(".pnlData").css("display", "none");
            }
            else if (Mode == "OrgName") {
                document.getElementById("INNRow").style.display = "none";
                document.getElementById("OrgNameRow").style.display = "";
                document.getElementById("OrgTypeRow").style.display = "";
                document.getElementById("RegionRow").style.display = "";
                document.getElementById("DicParams").style.display = "none";
                document.getElementById("DicTree").style.display = "none";
                document.getElementById("Buttons").style.display = "";
            }
            else if (Mode == "Dic") {
                document.getElementById("INNRow").style.display = "none";
                document.getElementById("OrgNameRow").style.display = "none";
                document.getElementById("OrgTypeRow").style.display = "none";
                document.getElementById("RegionRow").style.display = "none";
                document.getElementById("DicParams").style.display = "";
                document.getElementById("DicTree").style.display = "";
                document.getElementById("Buttons").style.display = "none";
                $(".pnlData").css("display", "none");
            }
        }
    </script>

    <form id="Form2" runat="server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <table id="DicParams" <%= IsRowVisible("Dic") %> cellpadding="0" cellspacing="0" border="0" style="width: 700px;">
        <tr>
            <td style="width: 60px; white-space: nowrap;">Регион</td>
            <td>
                <asp:DropDownList CssClass="sel long" ID="dlRegions" DataTextField="Name" 
                    DataValueField="Id" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="dlRegions_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
        
        <tr>
            <td style="width: 60px; white-space: nowrap;">Тип</td>
            <td>
                <asp:DropDownList CssClass="sel" ID="dlSchoolType" DataTextField="Name" 
                    DataValueField="Id" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="dlRegions_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
    </table>
        
    <table id="DicTree" <%= IsRowVisible("Dic") %> cellpadding="0" cellspacing="0" border="0" style="width: 700px;">
        <tr id="t">
            <td style="padding-left: 11px;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" > 
                    <ContentTemplate>

                        <asp:PlaceHolder ID="phEmpty" runat="server">
                            <asp:Label ID="Label2" runat="server" Text="Для выбранного региона и типа ОУ нет данных"></asp:Label>
                        </asp:PlaceHolder>
                    
                        <asp:PlaceHolder ID="phNote" runat="server">
                            <div class="notice">
                                <div class="top">
                                    <div class="l">
                                    </div>
                                    <div class="r">
                                    </div>
                                    <div class="m">
                                    </div>
                                </div>
                                <div class="cont" style="background: none;">
                                    <div class="">
                                        <p />
                                        <img src="/Common/Images/notice-block-notice.gif" width="22" height="22" alt="!"
                                            class="ico-notice" />
                                        Для загрузки справочника ОУ необходимо выбрать регион и тип ОУ.<p />
                                    </div>
                                </div>
                                <div class="bottom">
                                    <div class="l">
                                    </div>
                                    <div class="r">
                                    </div>
                                    <div class="m">
                                    </div>
                                </div>
                            </div>
                        </asp:PlaceHolder>

                        <asp:PlaceHolder ID="phDicTree" runat="server">
                            <asp:Label ID="Label1" runat="server" Text="Справочник ОУ"></asp:Label>
                            <br />
                            <asp:TreeView ID="TreeView1" runat="server" ImageSet="Arrows" Width="600px" 
                                NodeWrap="True" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged">
                                <ParentNodeStyle Font-Bold="False" />
                                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                <SelectedNodeStyle Font-Underline="True" HorizontalPadding="0px" 
                                    VerticalPadding="0px" ForeColor="#5555DD" />
                                <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" 
                                    HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px"/>
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
                <td style="padding-left: 100px">
                    По Вашему запросу найдено <b>
                        <asp:Label runat="server" ID="txtInfo" /></b> организаций.
                    <div class="notice">
                        <div class="top">
                            <div class="l">
                            </div>
                            <div class="r">
                            </div>
                            <div class="m">
                            </div>
                        </div>
                        <div class="cont" style="background: none;">
                            <div class="">
                                <p>
                                    <img src="/Common/Images/notice-block-notice.gif" width="22" height="22" alt="!"
                                        class="ico-notice" />
                                    Если Ваша организация отсутствует в списке:
                                    <ul>
                                        <li><a href='#filter'>Уточните или проверьте корректность запроса</a></li>
                                        <li>Перейдите на <a href='<%=BackUrl_forUse%><%= ReturnPrmName %>=0'>форму регистрации</a></li>
                                    </ul>
                                    <br />
                            </div>
                        </div>
                        <div class="bottom">
                            <div class="l">
                            </div>
                            <div class="r">
                            </div>
                            <div class="m">
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <fbs:Pager_Simple runat="server" ID="tablePager_top" PageSizes="5,10,20,50,100" PageSize="100">
        </fbs:Pager_Simple>
        <asp:DataGrid runat="server" ID="dgOrgList" AutoGenerateColumns="false" EnableViewState="false"
            ShowHeader="True" GridLines="None" CssClass="table-th">
            <HeaderStyle CssClass="th" />
            <Columns>
                <asp:TemplateColumn>
                    <HeaderStyle Width="60%" CssClass="left-th" />
                    <HeaderTemplate>
                        <div>
                            <fbs:SortRef_Prefix runat="server" SortExpr="FullName" SortExprText="Название" Prefix="../../../Common/Images/" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.ToString(Eval("FullName"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="5%" />
                    <HeaderTemplate>
                        <fbs:SortRef_Prefix runat="server" SortExpr="TypeName" SortExprText="Тип" Prefix="../../../Common/Images/" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("TypeName").ToString() %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="5%" />
                    <HeaderTemplate>
                        <fbs:SortRef_Prefix ID="SortRef_Prefix1" runat="server" SortExpr="IsPrivate" SortExprText="Правовая&nbsp;форма"
                            Prefix="../../../Common/Images/" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.ToBoolean(Eval("IsPrivate"))?"Негосударственный":"государственный"  %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="20%" />
                    <HeaderTemplate>
                        <fbs:SortRef_Prefix runat="server" SortExpr="RegName" SortExprText="Регион" Prefix="../../../Common/Images/" />
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
        <fbs:Pager_Simple runat="server" ID="tablePager_bottom" PageSizes="5,10,20,50,100"
            PageSize="100">
        </fbs:Pager_Simple>
    </asp:Panel>
    <asp:Label runat="server" ID="lblNoFilter" Visible="false" Style="color: DarkRed;
        font-weight: bold;">Не заданы параметры поиска!</asp:Label>
    </form>

    <script type="text/javascript">
    <!--
    function goAfterCommit() {

        if (window.confirm('Вы действительно не нашли организацию и хотите ввести её?')) {
            window.location = '/Registration_st2.aspx?OrgID=0';
        }
        return false;
    }


    function resetButtonClick()
    {
        document.getElementById("RegID").value = "0";
        document.getElementById("OldTypeId").value = "0";
        document.getElementById("OrgName").value = "";
        document.getElementById("INN").value = "";
        return false;
    }

    -->
    </script>

</asp:Content>
