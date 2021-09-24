<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WildcardRequestResult.aspx.cs" 
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.WildcardRequestResult" 
    MasterPageFile="~/Common/Templates/Certificates.Master" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Import Namespace="Fbs.Web" %>

<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">

    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphCertificateContent" runat="server">
    <div class="notice" id="WildcardRequestResult">
        <div class="top"><div class="l"></div><div class="r"></div><div class="m"></div></div>
        <div class="cont">
        <div dir="ltr" class="hide" title="Свернуть" onclick="ToggleNoticeState(this);">x<span></span></div>
        <div class="txt-in">            
            <p>Если не удается найти результаты участника ЕГЭ или свидетельство аннулировано без объяснения причины, то обращайтесь за информацией в Региональный Центр Обработки Информации, находящийся на территории региона, в котором было выдано данное свидетельство.</p>
        </div>
        </div>
        <div class="bottom"><div class="l"></div><div class="r"></div><div class="m"></div></div>
    </div>

    <script type="text/javascript">
        InitNotice();
    </script>
                
    <form id="Form1" runat="server">

            <table class="pager">
            <tr><td>
    
                <web:DataSourcePager runat="server"
                    id="DataSourcePagerHead" 
                    DataSourceId="dsSearchCount"
	                StartRowIndexParamName="start" 
	                MaxRowCountParamName="count"
	                HideDefaultTemplates="true"
	                AlwaysShow="true"
	                DefaultMaxRowCount="20"
	                DefaultMaxRowCountSource="Cookies">
                    <Header>
                        Результат поиска #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
                        <web:DataSourceMaxRowCount ID="DataSourceMaxRowCount1" runat="server"
                            Variants="20,50,100"
                            DataSourcePagerId="DataSourcePagerHead">
                        <Header>Записей на странице: </Header>
                        <Footer>.</Footer>
                        <Separator>, </Separator>
                        <ActiveTemplate><span>#count#</span></ActiveTemplate> 
                        <InactiveTemplate><a href="/SetMaxRowCount.aspx?count=#count#" 
                            title="Отображать #count# записей на странице">#count#</a></InactiveTemplate> 
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

            </td></tr>
            </table>

    <style>
        #ResultContainer {overflow-x:scroll;}
        html:first-child #ResultContainer {overflow:scroll;} /* только для Opera */
        #ResultContainer td {white-space:nowrap;}
    </style>    
    
    <div id="ResultContainer" style="width:100%; height:auto;">
    
        <asp:DataGrid runat="server" id="dgSearch"
            DataSourceID="dsSearch"
            AutoGenerateColumns="false" 
            EnableViewState="false"
            ShowHeader="True" 
            GridLines="None"
            CssClass="table-th"
            oninit="dgSearch_Init" OnPreRender="dgSearch_PreRender">
            <HeaderStyle CssClass="th" />
            <Columns>
                <asp:TemplateColumn>
                <HeaderStyle Width="15%" CssClass="left-th" />
                <HeaderTemplate>
                    <div><nobr>Свидетельство</nobr></div>
                </HeaderTemplate>
                <ItemTemplate>
                   <%# this.GetCertificateLink(Container.DataItem) %>
                </ItemTemplate>
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                <HeaderStyle Width="10%" />
                <HeaderTemplate>
                    <div>ТН</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("TypographicNumber") %>
                </ItemTemplate>
                </asp:TemplateColumn>
        
                <asp:TemplateColumn>
                <HeaderStyle Width="10%" />
                <HeaderTemplate>
                    <div>Фамилия</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("LastName") %>
                </ItemTemplate>
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                <HeaderStyle Width="10%" />
                <HeaderTemplate>
                    <div>Имя</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("FirstName") %>
                </ItemTemplate>
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                <HeaderStyle Width="10%" />
                <HeaderTemplate>
                    <div>Отчество</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("PatronymicName")%>
                </ItemTemplate>
                </asp:TemplateColumn> 
            
                <asp:TemplateColumn>
                <HeaderStyle Width="10%"/>
                <HeaderTemplate>
                    <div>Документ</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <nobr>
                    <span title="Серия"><%# Eval("PassportSeria")%></span>
                    <span title="Номер"><%# Eval("PassportNumber")%></span>
                    </nobr>
                </ItemTemplate>
                </asp:TemplateColumn>

                <asp:TemplateColumn >
                <HeaderStyle Width="10%" />
                <HeaderTemplate>
                    <div>Регион</div>
                </HeaderTemplate>
                <ItemTemplate>
                   <%# Convert.ToString(Eval("RegionName")) %>
                </ItemTemplate>
                </asp:TemplateColumn>
            
                <asp:TemplateColumn >
                <HeaderStyle Width="10%" />
                <HeaderTemplate>
                    <div>Год</div>
                </HeaderTemplate>
                <ItemTemplate>
                   <%# Convert.ToString(Eval("Year")) %>
                </ItemTemplate>
                </asp:TemplateColumn>            

                <asp:TemplateColumn >
                <HeaderTemplate>
                    <div>Статус</div>
                </HeaderTemplate>
                <ItemTemplate>
                   <%# Convert.ToString(Eval("Status")) %>
                </ItemTemplate>
                </asp:TemplateColumn>     
            
                <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                <HeaderStyle CssClass="right-th" />
                <HeaderTemplate>
                    <div>Проверки<span style="color:Red">&nbsp;*</span></div>
                </HeaderTemplate>
                <ItemTemplate>
                   <%# Convert.IsDBNull(Eval("UniqueIHEaFCheck")) ? 0 : Eval("UniqueIHEaFCheck")%>
                </ItemTemplate>
                </asp:TemplateColumn>     
            
            </Columns>
        </asp:DataGrid>
    </div>
        <web:NoRecordsText ID="NoRecordsText1" runat="server" ControlId="dgSearch">
            <p align="center">
                <img src="/Common/Images/spacer.gif" width="32" height="31" style="background-image:url('/Common/Images/important.gif')"/>
                По Вашему запросу ничего не найдено</p>
        </web:NoRecordsText>
    
            <table class="pager">
            <tr><td>
    
                <web:DataSourcePager runat="server"
                    id="DataSourcePagerHead2" 
                    DataSourceId="dsSearchCount"
	                StartRowIndexParamName="start" 
	                MaxRowCountParamName="count"
	                HideDefaultTemplates="true"
	                AlwaysShow="true"
	                DefaultMaxRowCount="20"
	                DefaultMaxRowCountSource="Cookies">
                    <Header>
                        Результат поиска #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
                        <web:DataSourceMaxRowCount ID="DataSourceMaxRowCount2" runat="server"
                            Variants="20,50,100"
                            DataSourcePagerId="DataSourcePagerHead2">
                        <Header>Записей на странице: </Header>
                        <Footer>.</Footer>
                        <Separator>, </Separator>
                        <ActiveTemplate><span>#count#</span></ActiveTemplate> 
                        <InactiveTemplate><a href="/SetMaxRowCount.aspx?count=#count#" 
                            title="Отображать #count# записей на странице">#count#</a></InactiveTemplate> 
                        </web:DataSourceMaxRowCount>                 
                    </Header>
                </web:DataSourcePager>
        
            </td>    
            <td align="right">

                <web:DataSourcePager ID="DataSourcePager2" runat="server"
                    DataSourceId="DataSourcePagerHead2"
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

            </td></tr>
            </table>

        <asp:PlaceHolder ID="phUniqueChecks" runat="server" Visible="true">
            <span style="color:Red">&nbsp;*</span> Количество проверок указывает количество уникальных проверок свидетельства ВУЗами и их филиалами. Для пользователей ССУЗов данное поле носит справочный характер.
        </asp:PlaceHolder>
    
        <asp:SqlDataSource  runat="server" ID="dsSearch" 
            ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
            SelectCommand="SearchCommonNationalExamCertificateWildcard"  CancelSelectOnNullParameter="false"
            SelectCommandType="StoredProcedure" onselecting="dsSearch_Selecting"
            > 
            <SelectParameters>
                <fbs:CurrentUserParameter Name="login" Type="String" />
                <fbs:CurrentUserParameter Name="ip" Type="String" />
                <asp:QueryStringParameter Name="lastName" ConvertEmptyStringToNull="true" Type="String" QueryStringField="LastName" />
                <asp:QueryStringParameter Name="firstName" ConvertEmptyStringToNull="true" Type="String" QueryStringField="FirstName" />
                <asp:QueryStringParameter Name="patronymicName" ConvertEmptyStringToNull="true" Type="String" QueryStringField="PatronymicName" />
                <asp:QueryStringParameter Name="passportSeria" ConvertEmptyStringToNull="true" Type="String" QueryStringField="DocSeries" />
                <asp:QueryStringParameter Name="passportNumber" ConvertEmptyStringToNull="true" Type="String" QueryStringField="DocNumber" />
                <asp:QueryStringParameter Name="year" ConvertEmptyStringToNull="true" Type="String" QueryStringField="Year" />
                <asp:QueryStringParameter Name="typographicNumber" ConvertEmptyStringToNull="true" Type="String" QueryStringField="TypographicNumber" />
                <asp:QueryStringParameter Name="Number" ConvertEmptyStringToNull="true" Type="String" QueryStringField="Number" />
                <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
                <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count" CheckParamName="count" CheckParamSource="Cookies" />
                <asp:Parameter DefaultValue="false" Name="showCount" Type="Boolean" />            
            </SelectParameters>
        </asp:SqlDataSource>
    
         <asp:SqlDataSource  runat="server" ID="dsSearchCount" 
            ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
            SelectCommand="SearchCommonNationalExamCertificateWildcard"  CancelSelectOnNullParameter="false"
            SelectCommandType="StoredProcedure" onselecting="dsSearch_Selecting"
            > 
            <SelectParameters>
                <fbs:CurrentUserParameter Name="login" Type="String" />
                <fbs:CurrentUserParameter Name="ip" Type="String" />
                <asp:QueryStringParameter Name="lastName" ConvertEmptyStringToNull="true" Type="String" QueryStringField="LastName" />
                <asp:QueryStringParameter Name="firstName" ConvertEmptyStringToNull="true" Type="String" QueryStringField="FirstName" />
                <asp:QueryStringParameter Name="patronymicName" ConvertEmptyStringToNull="true" Type="String" QueryStringField="PatronymicName" />
                <asp:QueryStringParameter Name="passportSeria" ConvertEmptyStringToNull="true" Type="String" QueryStringField="DocSeries" />
                <asp:QueryStringParameter Name="passportNumber" ConvertEmptyStringToNull="true" Type="String" QueryStringField="DocNumber" />
                <asp:QueryStringParameter Name="year" ConvertEmptyStringToNull="true" Type="String" QueryStringField="Year" />
                <asp:QueryStringParameter Name="typographicNumber" ConvertEmptyStringToNull="true" Type="String" QueryStringField="TypographicNumber" />
                <asp:QueryStringParameter Name="Number" ConvertEmptyStringToNull="true" Type="String" QueryStringField="Number" />
                <asp:Parameter DefaultValue="true" Name="showCount" Type="Boolean" />            
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</asp:Content>
