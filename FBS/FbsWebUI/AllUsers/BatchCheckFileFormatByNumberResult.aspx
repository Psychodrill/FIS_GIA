<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchCheckFileFormatByNumberResult.aspx.cs"
    Inherits="Fbs.Web.AllUsers.BatchCheckFileFormatByNumberResult" MasterPageFile="~/Common/Templates/TestBatchCheck.Master" %>

<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Fbs.Core" %>
<%@ Import Namespace="Fbs.Web" %>
<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphCertificateContent" runat="server">
    <div class="notice" id="BatchCheckResultSubjectNotice">
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
                    РЯ - русский язык; М - математика; Ф - физика; Х - химия; Б - биология; ИР - история
                    России; Г - география; АЯ - английский язык; НЯ - немецкий язык; ФЯ - французский
                    язык; О - обществознание; Л - литература; ИЯ - испанский язык; И - информатика.
                </p>
                <p>
                    Коды ошибок: С- превышено максимальное кол-во строк; П - неверное число полей, поля должны разделяться символом "%'; НС
                    - поле Номер свидетельства должно быть заполнено в формате xx-xxxxxxxxx-xx, где
                    x - цифра от 0 до 9; БП - поля баллов должны содержать не меньше двух числовых значений
                    от 0 до 100 (целые или дробные с одним десятичным знаком после запятой)
                </p>
                <p>
                    Если не удается найти результаты участника ЕГЭ или свидетельство аннулировано без
                    объяснения причины, то обращайтесь за информацией в Региональный Центр Обработки
                    Информации, находящийся на территории региона, в котором было выдано данное свидетельство.</p>
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
    <style>
        #ResultContainer
        {
            overflow-x: scroll;
        }
        html:first-child #ResultContainer
        {
            overflow: scroll;
        }
        /* только для Opera */
    </style>
    <form id="FormFileFormatExport" runat="server" enctype="multipart/form-data">
    <div id="ResultContainer" style="width: 100%; height: auto;">
              <table class="pager">
            <tr><td>
    
                    <web:DataSourcePager runat="server"
                                         id="DataSourcePagerHead" 
                                         DataSourceId="dsResultsListCount"
                                         StartRowIndexParamName="start" 
                                         MaxRowCountParamName="count"
                                         HideDefaultTemplates="true"
                                         AlwaysShow="true"
                                         DefaultMaxRowCount="10"
                                         DefaultMaxRowCountSource="Cookies">
                        <Header>
                            Результаты проверок #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
                            <web:DataSourceMaxRowCount runat="server"
                                                       Variants="10,20,50,100"
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

                    <web:DataSourcePager runat="server"
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
        <asp:DataGrid runat="server" ID="dgResultsList" 
            AutoGenerateColumns="false" EnableViewState="True" ShowHeader="True" GridLines="None"
            CssClass="table-th-border table-th" PagerStyle-Position="Top" PagerStyle-HorizontalAlign="Left"
            PagerStyle-Mode="NumericPages" PagerStyle-ForeColor="#0066FF" PagerStyle-PageButtonCount="10">
            <HeaderStyle CssClass="th" />
            <Columns>
                <asp:TemplateColumn>
                    <HeaderStyle CssClass="left-th" />
                    <HeaderTemplate>
                        <div title="Номер строки">
                            №</div>
                    </HeaderTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <%# HighlightValue(Eval("RowIndex"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <div title="Номер свидетельства">
                            Cв-во</div>
                    </HeaderTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Номер свидетельства"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <div title="Типографский номер">
                            ТН</div>
                    </HeaderTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Фамилия">Фамилия</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Имя">Имя</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Отчество">Отчество</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Серия паспорта">СП</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Номер паспорта">НП</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Регион">Регион</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Год выдачи свидетельства">Год</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Статус">Статус</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Русский язык">РЯ</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Русский язык"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по русскому языку">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Апелляция по русскому языку"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Математика">М</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Математика"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по математике">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Апелляция по математике"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Физика">Ф</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Физика"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по физике">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Апелляция по физике"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Химия">Х</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Химия"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по химии">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Апелляция по химии"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Биология">Б</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Биология"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по биологии">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Апелляция по биологии"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="История России">ИР</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("История России"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по истории России">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Апелляция по истории России"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="География">Г</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("География"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по географии">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Апелляция по географии"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Английский язык">АЯ</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Английский язык"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по английскому языку">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Апелляция по английскому языку"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Немецкий язык">НЯ</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Немецкий язык"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по немецкому языку">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Апелляция по немецкому языку"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Французский язык">ФЯ</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Французский язык"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по французскому языку">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Апелляция по французскому языку"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Обществознание">О</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Обществознание"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по обществознанию">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Апелляция по обществознанию"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Литература">Л</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Литература"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по литературе">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Апелляция по литературе"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Испанский язык">ИЯ</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Испанский язык"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по испанскому языку">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Апелляция по испанскому языку"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Информатика">И</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Информатика"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <div title="Апелляция по информатике">
                            Ап</div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Апелляция по информатике"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle CssClass="right-th" />
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <div title="Комментарий">
                            Комментарий</div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("Комментарий"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <web:NoRecordsText runat="server" ControlId="dgResultsList">
            <Message>
                <style type="text/css">
                    #ExportContainer, #ResultContainer
                    {
                        display: none;
                    }
                </style>
                <p class="notfound">
                    Не обработано ни одной записи</p>
            </Message>
        </web:NoRecordsText>
        
        <table class="pager">
            <tr><td>
    
                    <web:DataSourcePager ID="DataSourcePager1" runat="server"
                                         DataSourceId="DataSourcePagerHead"
                                         StartRowIndexParamName="start" 
                                         MaxRowCountParamName="count"
                                         HideDefaultTemplates="true"
                                         AlwaysShow="true"
                                         DefaultMaxRowCount="10"
                                         DefaultMaxRowCountSource="Cookies">
                        <Header>
                            Результаты проверок #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
                            <web:DataSourceMaxRowCount ID="DataSourceMaxRowCount2" runat="server"
                                                       Variants="10,20,50,100"
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

                    <web:DataSourcePager runat="server"
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
        
        <asp:SqlDataSource runat="server" ID="dsResultsListCount" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
            SelectCommand="*" >
            </asp:SqlDataSource>
        <script type="text/javascript">
            InitNotice();
        </script>
    </div>
    </form>
</asp:Content>
<script runat="server">
    public string HighlightValue(object valueObj)
    {
        string value = Convert.ToString(valueObj);
        if (value.Contains("[НЕВЕРЕН ФОРМАТ]"))
            return String.Format("<span \" style=\"color:Red\">{0}</span>",
                value);
        else
            return value;

    }
</script>
