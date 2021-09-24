<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Administration.Master"
         AutoEventWireup="true" CodeBehind="CertificateListCommon.aspx.cs" Inherits="Fbs.Web.Administration.Organizations.CertificateListCommon" %>
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
    
    
    <form runat="server">
            <div style="padding: 8px;">
                <asp:CheckBox runat="server" Text="Только уникальные проверки" AutoPostBack="True" OnCheckedChanged="OnUniqueCheckSelected" ID="uniqueChecks"/>
            </div>

            <style>
                #ResultContainer {overflow-x:auto;}
                html:first-child #ResultContainer {overflow:auto;} /* только для Opera */
                #ResultContainer td {white-space:nowrap;}
            </style>    
    
            <div id="ResultContainer" style="width:100%; height:auto; margin-bottom: 25px;">

                <asp:GridView GridLines="None" EnableViewState="false" ID="gvChecks" 
                              AllowSorting="False" AutoGenerateColumns="false" DataSourceID="historyDataSource" 
                              runat="server" CssClass="table-th">
                    <HeaderStyle CssClass="th" />
                    <Columns>
                        <%-- Дата и время проверки --%>
                        <asp:TemplateField>
                            <HeaderStyle CssClass="left-th" HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <div>
                                    Дата&nbsp;и&nbsp;время
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval("CreateDate") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- Тип проверки --%>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <div>Тип&nbsp;проверки</div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval("SenderTypeName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- Фамилия --%>
                        <asp:TemplateField HeaderText="Фамилия" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:HyperLink runat="server" NavigateUrl="<%# RenderDetailsLink(Container) %>" Text='<%# DataBinder.Eval(Container, "DataItem.Surname", "{0}") %>'></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- Имя --%>
                        <asp:TemplateField HeaderText="Имя" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="<%# RenderDetailsLink(Container) %>" Text='<%# DataBinder.Eval(Container, "DataItem.Name", "{0}") %>'></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- Отчество --%>
                        <asp:TemplateField HeaderText="Отчество" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="<%# RenderDetailsLink(Container) %>" Text='<%# DataBinder.Eval(Container, "DataItem.SecondName", "{0}") %>'></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- Документ --%>
                        <asp:TemplateField HeaderText="Документ" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# Eval("DocumentSeries", "{0}") %>&nbsp;&nbsp;<%# Eval("DocumentNumber", "{0}") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- Распечатать справку --%>
                        <asp:TemplateField>
                            <HeaderStyle CssClass="right-th" />
                            <HeaderTemplate>
                                <div>Распечатать&nbsp;справку&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HyperLink runat="server" Text="Распечатать справку" NavigateUrl="<%# RenderPrintLink(Container) %>" Target="_blank"></asp:HyperLink>
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
                                             DataSourceId="historyDataSourceCount"
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
            <asp:Panel runat="server" ID="printAllPanel">
                <span style="white-space: nowrap">
                    <asp:HyperLink runat="server" ID="lnkPrintAllHtml" Text="Распечатать все справки (одна html-страница)" NavigateUrl="#" Target="_blank"></asp:HyperLink> 
                    &nbsp;&nbsp;|&nbsp;&nbsp;    
                    <asp:HyperLink runat="server" ID="lnkPrintAllWord" Text="Распечатать все справки (документ Microsoft Word)" NavigateUrl="#" Target="_blank"></asp:HyperLink> 
                </span> 
            </asp:Panel>
            
            
            

            <asp:ObjectDataSource ID="historyDataSource" runat="server" DataObjectTypeName="System.Data.DataTable"
                                  TypeName="FbsServices.OrganizationCheckHistoryService" SelectMethod="GetPage" OnSelecting="OnSelectData" OnObjectCreating="OnCreateDataSource" />
             
            <asp:ObjectDataSource runat="server" ID="historyDataSourceCount" DataObjectTypeName="System.Int32" TypeName="FbsServices.OrganizationCheckHistoryService" SelectMethod="GetPageCount" OnObjectCreating="OnCreateDataSource" />
    </form>
</asp:Content>