<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="For2011.aspx.cs" Inherits="Esrp.Web.For2011"
    MasterPageFile="~/Common/Templates/Regular.master" %>

<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Core.Systems" %>
<%@ Import Namespace="Esrp.Web" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Controls" Assembly="Esrp.Web.UI" %>
<asp:Content runat="server" ContentPlaceHolderID="cphHead">

    <script src="/Common/Scripts/Filter.js" type="text/javascript"></script>

    <script src="/Common/Scripts/FixPng.js" type="text/javascript"></script>

</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
  <div class="notice" id="CheckTitleNotice">
                        <div class="top">
                            <div class="l">
                            </div>
                            <div class="r">
                            </div>
                            <div class="m">
                            </div>
                        </div>
                        <div class="cont">
                            <div dir="ltr" class="hide" title="Свернуть" onclick="ToggleNoticeState(this);">
                                x<span></span></div>
                            <div class="txt-in">
                                <p>
                                   В текущем году проводится подготовка <%= GeneralSystemManager.GetSystemName(2)%> к эксплуатации в защищенном режиме для
    реализации требований Федерального закона Российской Федерации от 27 июля 2006 года
    № 152-ФЗ «О персональных данных» и Постановления Правительства РФ от 17 ноября 2007
    г. № 781 «Об утверждении Положения об обеспечении безопасности персональных данных
    при их обработке в информационных системах ПДн».</p><p>Для оптимизации к 2011 году технических
    решений по организации защищенного режима работы с <%= GeneralSystemManager.GetSystemName(2)%> <b>приглашаем принять участие
    в апробации доработанной версии системы, поддерживающей защищенную схему взаимодействия.</b>
    Апробация проводится в июле - августе 2010 года. </p><p>В рамках апробации планируется
    отладить обмен индивидуальными запросами и файлами импорта/экспорта в пакетном режиме
    по защищенной сети передачи данных, при этом для всех участников апробации сохраняется
    возможность работать с <%= GeneralSystemManager.GetSystemName(2)%> в стандартном режиме. </p><p>Консультации по телефону горячей
    линии <%= GeneralSystemManager.GetSystemName(2)%> (495) 989 16 29.
    <br />
    <br /></p>
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
    
    <table class="pager">
        <tr>
            <td>
                <web:DataSourcePager runat="server" ID="DataSourcePagerHead" DataSourceId="dsDocumentListCount"
                    StartRowIndexParamName="start" MaxRowCountParamName="count" HideDefaultTemplates="true"
                    AlwaysShow="true" DefaultMaxRowCount="20" DefaultMaxRowCountSource="Cookies">
                    <Header>
                        Документы #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                        <web:DataSourceMaxRowCount ID="DataSourceMaxRowCount1" runat="server" Variants="20,50,100"
                            DataSourcePagerId="DataSourcePagerHead">
                            <Header>
                                Записей на странице:
                            </Header>
                            <Footer>
                                .</Footer>
                            <Separator>
                                ,
                            </Separator>
                            <ActiveTemplate>
                                <span>#count#</span></ActiveTemplate>
                            <InactiveTemplate>
                                <a href="/SetMaxRowCount.aspx?count=#count#" title="Отображать #count# записей на странице">
                                    #count#</a></InactiveTemplate>
                        </web:DataSourceMaxRowCount>
                    </Header>
                </web:DataSourcePager>
            </td>
            <td align="right">
                <web:DataSourcePager ID="DataSourcePager1" runat="server" DataSourceId="DataSourcePagerHead"
                    StartRowIndexParamName="start" MaxRowCountParamName="count">
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
    <asp:DataGrid Width="100%" runat="server" ID="dgDocumentList" DataSourceID="dsDocumentList"
        AutoGenerateColumns="false" EnableViewState="false" ShowHeader="True" GridLines="None"
        CssClass="table-th">
        <HeaderStyle CssClass="th" />
        <Columns>
            <asp:TemplateColumn>
                <HeaderStyle Width="100%" CssClass="left-th" />
                <HeaderTemplate>
                <div>
                         <esrp:sortref_prefix ID="Sortref_prefix1" prefix="../Common/Images/" runat="server" sortexpr="Name"
                            sortexprtext="Название" />
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <a title='<%# Eval("Description") %>' href='<%# (!string.IsNullOrEmpty(Convert.ToString(Eval("RelativeUrl")))) ?
                            Eval("RelativeUrl") : string.Format("/Document.aspx?id={0}", Eval("Id")) %>'>
                        <%# Eval("Name")%>
                    </a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn ItemStyle-Wrap="false">
                <HeaderStyle CssClass="right-th" />
                <HeaderTemplate>
                    <div>
                    </div>
                </HeaderTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    <web:NoRecordsText ID="NoRecordsText1" runat="server" ControlId="dgDocumentList">
        <Message>
            <p class="notfound">
                Не найдено ни одного документа</p>
        </Message>
    </web:NoRecordsText>
    <table class="pager">
        <tr>
            <td>
                <web:DataSourcePager ID="DataSourcePager2" runat="server" DataSourceId="dsDocumentListCount"
                    StartRowIndexParamName="start" MaxRowCountParamName="count" HideDefaultTemplates="true"
                    AlwaysShow="true" DefaultMaxRowCount="20" DefaultMaxRowCountSource="Cookies">
                    <Header>
                        Документы #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                        <web:DataSourceMaxRowCount ID="DataSourceMaxRowCount2" runat="server" Variants="20,50,100"
                            DataSourcePagerId="DataSourcePagerHead">
                            <Header>
                                Записей на странице:
                            </Header>
                            <Footer>
                                .</Footer>
                            <Separator>
                                ,
                            </Separator>
                            <ActiveTemplate>
                                <span>#count#</span></ActiveTemplate>
                            <InactiveTemplate>
                                <a href="/SetMaxRowCount.aspx?count=#count#" title="Отображать #count# записей на странице">
                                    #count#</a></InactiveTemplate>
                        </web:DataSourceMaxRowCount>
                    </Header>
                </web:DataSourcePager>
            </td>
            <td align="right">
                <web:DataSourcePager ID="DataSourcePager3" runat="server" DataSourceId="DataSourcePagerHead"
                    StartRowIndexParamName="start" MaxRowCountParamName="count">
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
    <asp:SqlDataSource runat="server" ID="dsDocumentList" ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="SearchDocument" CancelSelectOnNullParameter="false" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="Name" Type="String" QueryStringField="Name" />
            <asp:Parameter DefaultValue="true" Name="IsActive" Type="Boolean" />
            <asp:Parameter DefaultValue="For2011" Name="contextCodes" Type="String" />
            <asp:QueryStringParameter DefaultValue="0" Name="sortColumn" Type="String" QueryStringField="sort" />
            <asp:QueryStringParameter DefaultValue="1" Name="sortAsc" Type="Int16" QueryStringField="sortorder" />
            <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
            <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count"
                CheckParamName="count" CheckParamSource="Cookies" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource runat="server" ID="dsDocumentListCount" ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="SearchDocument" CancelSelectOnNullParameter="false" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="name" Type="String" QueryStringField="name" />
            <asp:Parameter DefaultValue="true" Name="IsActive" Type="Boolean" />
            <asp:Parameter DefaultValue="For2011" Name="contextCodes" Type="String" />
            <asp:Parameter DefaultValue="true" Name="showCount" Type="Boolean" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
