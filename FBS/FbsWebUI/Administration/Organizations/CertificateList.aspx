<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Administration.Master"
         AutoEventWireup="true" CodeBehind="CertificateList.aspx.cs" Inherits="Fbs.Web.Administration.Organizations.CertificateList" %>
<%@ Import Namespace="Fbs.Web" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Controls" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <script src="/Common/Scripts/Filter.js" type="text/javascript"></script>    
    <script type="text/javascript" src="/Common/Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="/Common/Scripts/jquery-bbq-plugin.js"></script>
    <script type="text/javascript" src="/Common/Scripts/jquery-ui-1.8.18.min.js"></script>
    <script type="text/javascript" src="/Common/Scripts/jquery.form.js"></script>
    <script type="text/javascript" src="/Common/Scripts/CalendarPopup.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphLeftMenu" runat="server"></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphActions" runat="server"></asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphContent" runat="server">
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
        <input type="hidden" id="OrgId" name="OrgId" value="<%= Request.QueryString["OrgId"] %>"/>
        <input type="hidden" id="hiddenIsUniqueCheck" name="cbisUniqueCheck" value="<%= Request.QueryString["cbisUniqueCheck"] %>"/>
        <input type="hidden" id="sortorder" name="sortorder" value="<%= Request.QueryString["sortorder"] %>"/>
        <input type="hidden" id="sort" name="sort" value="<%= Request.QueryString["sort"] %>"/>
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
    <form action="" method="GET" id="myForm" >        
        <table class="filtr2-table">
        <tr><td class="left2">Тип проверки</td>
            <td><select name="typeCheck" id="typeCheck">
                <option value="">&lt;Все&gt;</option>
                <option value="Интерактивная" <%= SelectValue("typeCheck", "Интерактивная") %>>Интерактивная</option> 
                <option value="Пакетная" <%= SelectValue("typeCheck", "Пакетная") %>>Пакетная</option> 
            </select></td></tr>
        <% if (!Config.IsOpenFbs)
           {%>
        <tr>
            <td class="left2">Фамилия&nbsp;&nbsp;</td>
            <td><input name="family" id="family" class="txt txt2" 
                value="<%= Request.QueryString["family"] %>" /></td>
        </tr>
        <% } %>
        <tr>
            <td class="left2">С:&nbsp;&nbsp;</td>
            <td>
                <input type="text" name="dateS" id="TBDateS" value="<%= Request.QueryString["dateS"] %>" class="txt date" />
                <img src="/Common/Images/ico-datepicker-fbs.gif" id="BtDateS" alt="выбрать дату" onclick='return PickDate("TBDateS", "BtDateS", "CalendarContainer");' />
            </td>
        </tr>
        <tr>
            <td class="left2">По:&nbsp;&nbsp;</td>
            <td>
                <input type="text" name="dateF" id="TBDateF" value="<%= Request.QueryString["dateF"] %>" class="txt date" />
                <img src="/Common/Images/ico-datepicker-fbs.gif" id="BtDateF" alt="выбрать дату" onclick='return PickDate("TBDateF", "BtDateF", "CalendarContainer");' />
            </td>
        </tr>
        <tr>
        <td colspan="2" class="cell-bt">
            <input type="reset" class="bt" value="Очистить"  onclick="return resetButtonClick();" />&nbsp;
            <input type="button" class="bt bt2" value="Установить" onclick="setQueryParam()" />&nbsp;
            <input type="button" class="bt" id="btCancel" value="Отмена" onclick="showFilter(false);" style="display:none;"/></td>
        </tr></table>
    </form>
    </div>
    <div class="h-line"><div class="bl"><div></div></div><div class="br"><div></div></div><div class="b"><div></div></div></div>
    </div>
    </div>
    <!-- /filtr2 -->

    <script type="text/javascript">
        $(document).ready(function () {

            if (<%=this.cb%>) {
                $('#cbisUniqueCheck').attr("checked", 'checked');
            }
            
            $('#formCheckBox').change(function () {
                var qString = $("#formCheckBox").formSerialize();
                if (qString == "") { qString = "cbisUniqueCheck=";}
                $.deparam("cbisUniqueCheck", true);
                var url = $.param.querystring(window.location.href, qString);
                window.location.assign(url);
                return false;
            });
        });

        initFilter();

        function resetButtonClick() {
            document.getElementById("typeCheck").value = "";
            <% if (!Config.IsOpenFbs)
               {%>
            document.getElementById("family").value = "";
            <% } %>
            
            document.getElementById("TBDateS").value = "";
            document.getElementById("TBDateF").value = "";
            return false;
        }
        
        function setQueryParam() {
            var newParam = "typeCheck=" + $("#typeCheck option:selected").val() + "&" + "family=" + $('#family').val() + "&" + "dateS=" + $('#TBDateS').val() + "&" + "dateF=" + $('#TBDateF').val();
            $.deparam("typeCheck");
            $.deparam("family");
            $.deparam("dateS");
            $.deparam("dateF");
            var url = $.param.querystring(window.location.href, newParam);
            window.location.assign(url);
        }
    </script>

    <form action="" method="GET" id="formCheckBox" >
            <br />        
            <div class="form">
                <input type="checkbox" id="cbisUniqueCheck" name="cbisUniqueCheck" class="checkbox-box" value="true" />
                <label>Только уникальные проверки</label>
                <div class="SpanContainer"></div>
            </div>
    </form>

    <form runat="server">
            <style>
                #ResultContainer {overflow-x:auto;}
                html:first-child #ResultContainer {overflow:auto;} /* только для Opera */
                #ResultContainer td {white-space:nowrap;}
            </style>    
    
            <div id="ResultContainer" style="width:100%; height:auto; margin-bottom: 25px;">

                <asp:GridView GridLines="None" EnableViewState="false" ID="gvChecks" 
                              AllowSorting="true" AutoGenerateColumns="false" DataSourceID="historyDataSource" 
                              runat="server" CssClass="table-th" OnDataBound="gvChecks_OnDataBound" >
                    <HeaderStyle CssClass="th" />
                    <Columns>
                        <%-- Дата и время проверки --%>
                        <asp:TemplateField >
                            <HeaderStyle CssClass="left-th" HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <div>
                                    <fbs:SortRef_prefix ID="SortRef_prefix1" Prefix="../../../Common/Images/" runat="server" SortExpr="date" SortExprText="Дата&nbsp;и&nbsp;время" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#this.Eval("DateCheck")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- Тип проверки --%>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <fbs:SortRef_prefix ID="SortRef_prefix1" Prefix="../../../Common/Images/" runat="server" SortExpr="TypeCheck" SortExprText="Тип&nbsp;проверки" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#this.Eval("TypeCheck")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- Свидетельство --%>
                        <asp:TemplateField HeaderText="Свидетельство" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLink1" Visible='<%# this.Eval("Status").ToString()!= "Не найдено" %>' runat="server" NavigateUrl='<%#this.Eval("Certificate.Url")%>'          
                                               Text='<%#this.Eval("Certificate.Text")%>'></asp:HyperLink>
                                <asp:Label runat="server" Visible='<%# this.Eval("Status").ToString()== "Не найдено" %>' Text='<%#this.Eval("Certificate.Text")%>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- Типографический номер --%>
                        <asp:TemplateField HeaderText="ТН" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%#this.Eval("TypographicNumber")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- Фамилия --%>
                        <asp:TemplateField >
                            <HeaderStyle HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <fbs:SortRef_prefix ID="SortRef_prefix1" Prefix="../../../Common/Images/" runat="server" SortExpr="LastName" SortExprText="Фамилия" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#this.Eval("LastName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- Имя --%>
                        <asp:TemplateField HeaderText="Имя" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%#this.Eval("FirstName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- Отчество --%>
                        <asp:TemplateField HeaderText="Отчество" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%#this.Eval("PatronymicName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- Документ --%>
                        <asp:TemplateField HeaderText="Документ" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%#this.Eval("Document")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- Год --%>
                        <asp:TemplateField HeaderText="Год" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%#this.Eval("Year")%>
                            </ItemTemplate>
                        </asp:TemplateField>         
                        <%-- Статус --%>
                        <asp:TemplateField HeaderText="Статус" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%#this.Eval("Status")%>
                            </ItemTemplate>
                        </asp:TemplateField>  
                        <%-- Количество проверок --%>
                        <asp:TemplateField HeaderText="Пр." >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%#this.Eval("CountCheck")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"/>
                        </asp:TemplateField>  
                        <%-- Распечатать справку --%>
                        <asp:TemplateField>
                            <HeaderStyle CssClass="right-th" />
                            <HeaderTemplate>
                                <div>Распечатать&nbsp;справку&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbprint" runat="server" Visible='<%# this.Eval("Status").ToString()!= "Не найдено" || Convert.ToBoolean(ConfigurationManager.AppSettings["EnableNotFoundNote"])%>' CommandArgument='<%#this.Eval("NumberRow")%>' OnCommand="Print"><%#this.Eval("PrintNote.Text")%></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>                         
                    </Columns>
                </asp:GridView>
            </div>
            <table class="pager">
                <tr>
                    <td>    
                        <web:DataSourcePager runat="server"
                                             id="DataSourcePagerHead" 
                                             DataSourceId="odsCountCNECheckHystoryByOrgId"
                                             StartRowIndexParamName="start" 
                                             MaxRowCountParamName="count"
                                             HideDefaultTemplates="true"
                                             AlwaysShow="true"
                                             DefaultMaxRowCount="10"
                                             DefaultMaxRowCountSource="Cookies">
                            <Header>
                                Проверки #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
                            <web:DataSourceMaxRowCount ID="DataSourceMaxRowCount1" runat="server"
                                                           Variants="10,20,50,100"
                                                           DataSourcePagerId="DataSourcePagerHead">
                                <Header>Записей на странице: </Header>
                                <Footer>.</Footer>
                                <Separator>, </Separator>
                                <ActiveTemplate><span>#count#</span></ActiveTemplate> 
                                <InactiveTemplate><a href="/SetMaxRowCount.aspx?count=#count#"  title="Отображать #count# записей на странице">#count#</a></InactiveTemplate> 
                                </web:DataSourceMaxRowCount>                 
                            </Header>
                        </web:DataSourcePager>        
                    </td>    
                    <td align="right">
                        <web:DataSourcePager ID="DataSourcePager1" runat="server"
                                             DataSourceId="DataSourcePagerHead"
                                             StartRowIndexParamName="start"
                                             MaxRowCountParamName="count">
			                                <PrevGroupTemplate></PrevGroupTemplate>
                                            <NextGroupTemplate></NextGroupTemplate>
                                            <FirstPageTemplate><a href="#PageUrl#"><<</a>&nbsp;</FirstPageTemplate>
                                            <LastPageTemplate>&nbsp;<a href="#PageUrl#">>></a></LastPageTemplate>
                                            <ActivePrevPageTemplate><a href="#PageUrl#"><</a>&nbsp;</ActivePrevPageTemplate>
                                            <ActivePageTemplate><span>#PageNo#</span> </ActivePageTemplate>
                                            <ActiveNextPageTemplate>&nbsp;<a href="#PageUrl#">></a></ActiveNextPageTemplate>
                        </web:DataSourcePager>  
                    </td>
                </tr>
            </table>   
            <web:NoRecordsText runat="server" ControlId="gvChecks" ID="nrtHistory" >
                <p align="center">
                    <img src="/Common/Images/spacer.gif" width="32" height="31" style="background-image: url('/Common/Images/important.gif')" />
                    По Вашему запросу ничего не найдено
                </p>
            </web:NoRecordsText>     
            <asp:Button runat="server" ID="btnPrintAllNote" Text="Распечатать" ToolTip="Распечатать все справки со страницы" CssClass="button" OnClick="BtnPrintAllNoteClick" />

            <asp:ObjectDataSource ID="historyDataSource" runat="server" DataObjectTypeName="FbsWebViewModel.CNEC.HistoryCheckCertificateForOrganizationView"
                                  TypeName="FbsServices.CNECService" SelectMethod="SelectCNECCheckHystoryByOrgId" OnSelecting="HistoryDataSourceOnSelecting" OnSelected="HistoryDataSourceOnSelected" />
             
            <asp:ObjectDataSource runat="server" ID="odsCountCNECheckHystoryByOrgId" DataObjectTypeName="System.Int32" TypeName="FbsServices.CNECService" SelectMethod="CountCNECCheckHystoryByOrgId" OnSelecting="CountCNECCheckHystoryByOrgIdOnSelecting" />
    </form>

    <script runat="server" type="text/C#">

        // Формирование строки статуса фильтра
        string FilterStatusString()
        {
            var filter = new ArrayList();

            if (!Config.IsOpenFbs && !string.IsNullOrEmpty(Request.QueryString["family"]))
                filter.Add(string.Format("Фамилия “{0}”", Request.QueryString["family"]));

            if (!string.IsNullOrEmpty(Request.QueryString["typeCheck"]))
                filter.Add(string.Format("Тип проверки “{0}”", Request.QueryString["typeCheck"]));

            if (!string.IsNullOrEmpty(Request.QueryString["dateS"]))
                filter.Add(string.Format("С: “{0}”", Request.QueryString["dateS"]));

            if (!string.IsNullOrEmpty(Request.QueryString["dateF"]))
                filter.Add(string.Format("По: “{0}”", Request.QueryString["dateF"]));

            if (filter.Count == 0)
                filter.Add("Не задан");

            return string.Join("; ", ((string[])filter.ToArray(typeof(string))));
        }

        string SelectValue(string paramName, string value)
        {
            if (string.Compare(Request.QueryString[paramName], value, true) == 0)
                return "selected=\"selected\"";

            return string.Empty;
        }
    
    </script> 
    <div id="CalendarContainer" style="position: absolute; visibility: hidden; background-color: white;
        z-index: 99;">
    </div>
</asp:Content>